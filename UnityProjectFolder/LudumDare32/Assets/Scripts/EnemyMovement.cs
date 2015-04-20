using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour 
{

	public GameObject deathEffect;

	public enum EnemyType {Chasing, Jumping, Fleeing};
	public EnemyType enemyType;
	
	//List of player states
	public enum PlayerState {Idle, Running, Jumping, Falling, Eating, Damaged, Evolving};
	//Current state
	public PlayerState playerState;
	//Previous state
	private PlayerState _lastState;
	
	//How fast can we move?
	public float movementSpeed;
	//How strong is the gravity
	public float gravity;
	//Max jump height
	public float jumpHeight;
	
	//Can we move?
	public bool canMove;
	//Are we on the ground?
	public bool isGrounded;
	//Are we jumping?
	public bool canJump;
	//Are we running?
	public bool isRunning;
	
	//Particles to explode the player
	public Transform deadParticle;
	//Child object which holds our animations	
	public Transform playerAnimation;
	//The animator for our child object
	public Animator anim;
	
	//Have we reached a wall?
	public bool _atWall;
	//Are we jumping?
	private bool _isJumping;
	
	private bool hasLanded;
	//Are we accepting input?
	public bool canInput;
	
	//Is the player dead?
	private bool _isDead;
	//Current movement speed
	private float _currentSpeed;
	//Maximum movement speed
	private float _maxSpeed;
	//Direction we are facing (-1 left, 1 right)
	public float _faceDir;
	//The lowest we're currently allowed on the Y axis (used for slopes)
	private float _minY;
	//Length of our downward facing rays
	private float _dRay;
	//Our transforms x scale
	private float xScale;
	//Our transforms y scale
	private float yScale;
	//Check which way walls are when dashing
	public int _wallDir;
	
	private GameObject player;
	
	private BoxCollider2D _boxCollider;
	
	//Set stuff up at the start
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		
		_boxCollider = GetComponent<BoxCollider2D>();
		//Facing right
		_faceDir = 1;
		//Player state is idle
		playerState = PlayerState.Idle;
		anim = playerAnimation.GetComponent<Animator>();
		//anim.SetInteger("animState",0);
		//Previous state is also idle`
		_lastState = playerState;
		
		//Accept input
		canInput = true;
		//The player is alive!
		_isDead = false;
		
		//Get scales
		xScale = transform.localScale.x / 2f;
		yScale = transform.localScale.y /2f;
		_faceDir = Random.Range (0,2);
		if(_faceDir == 0)
			_faceDir = -1;
		StartRunning();
	}
	
	void Update()
	{	



		//Check to see if we've got horizontal input, or if we're dashing
		if(isRunning)
		{
			//This will determine the direction of horizontal raycasting
			float checkDir = _faceDir;
			if(enemyType == EnemyType.Fleeing)
			{
				RaycastHit2D foodCheck = Physics2D.Raycast (transform.position, Vector3.right * checkDir, 1.0f, 1 << 11);
				Debug.DrawRay(transform.position, Vector3.right * checkDir);
				
				if(foodCheck.collider != null && foodCheck.collider.gameObject.tag == "Food")
				{
					if(_faceDir == 1)
						_faceDir = -1;
					else
						_faceDir = 1;
				}
			}
			
			//We are not at a wall
			_atWall = false;
			//Raycast 5 rays horizontally
			RaycastHit2D hit1 = Physics2D.Raycast (new Vector3(transform.position.x,transform.position.y + (xScale/2f),0), Vector3.right * checkDir, 0.23f,1 << 8);
			RaycastHit2D hit2 = Physics2D.Raycast (new Vector3(transform.position.x,transform.position.y,0), Vector3.right * checkDir,xScale+0.03f, 1 << 8);
			RaycastHit2D hit3 = Physics2D.Raycast (new Vector3(transform.position.x,transform.position.y - (xScale/2f),0), Vector3.right * checkDir, 0.23f, 1 << 8);
			RaycastHit2D hit4 = Physics2D.Raycast (new Vector3(transform.position.x,transform.position.y - (xScale - 0.1f),0), Vector3.right * checkDir, xScale+0.03f, 1 << 8);
			RaycastHit2D hit5 = Physics2D.Raycast (new Vector3(transform.position.x,transform.position.y + (xScale - 0.1f),0), Vector3.right * checkDir, xScale+0.03f, 1 << 8);
			//Check to see if any of the rays have hit the environment
			if(hit1.collider != null ||
			   hit2.collider != null ||
			   hit3.collider != null ||
			   hit4.collider != null ||
			   hit5.collider != null)
			{
				
				_wallDir = (int)checkDir;
				_atWall = true;
				if(isRunning)
				{
					_faceDir *= -1;
				}
			}
		}
		else
			_atWall = false;
		
		//Scale our animation to be facing the right direction
		playerAnimation.transform.localScale = new Vector3(_faceDir,1,1);
		
		if(canInput)
		{
			float s = 0;
			if(isRunning)
				s = _faceDir;
			//damageSpeed doesn't do anything, so it's always 0 at the moment
			float cSpeed = s;
			//Clamp our 'cSpeed'
			if(cSpeed > 1)
				cSpeed = 1;
			//And times it by our movement speed
			_currentSpeed = movementSpeed * cSpeed;
		}
		
		//If we're at a wall we set our movement speed to 0
		if(_atWall)
			_currentSpeed = 0;
		
		//Create a float and set it to a stupidly lowf number
		float topRay = Mathf.NegativeInfinity;
		
		//If we're not jumping, and we haven't started a jump
		if(!_isJumping && !canJump)
		{
			//Cast two rays down
			RaycastHit2D hitD1 = Physics2D.Raycast (new Vector3 (transform.position.x + (xScale-0.1f), transform.position.y - (yScale - 0.1f), 0), -Vector3.up, _dRay,1 << 8);
			RaycastHit2D hitD2 = Physics2D.Raycast (new Vector3 (transform.position.x - (xScale-0.1f), transform.position.y - (yScale - 0.1f), 0), -Vector3.up, _dRay, 1 << 8);
			Debug.DrawRay(new Vector3(transform.position.x + (xScale-0.1f), transform.position.y - (yScale - 0.1f),0),-Vector3.up);
			//Check to see if either of the rays have hit
			if((hitD1.collider != null) || 
			   (hitD2.collider != null))
			{
				
				//Set the 'topRay' to where the first ray hit
				if(hitD1.collider != null)
					topRay = hitD1.point.y;
				//If the second ray hit a higher position, set 'topRay' to there instead
				if(hitD2.collider != null && hitD2.point.y > topRay)
					topRay = hitD2.point.y;
				//If we have set the 'topRay' this frame, our lowest Y position is set to that point
				if(topRay > Mathf.NegativeInfinity)
					_minY = topRay;
				//We are touching the ground
				isGrounded = true;			
			}
			else
			{
				//None of the rays hit, so we're not on the ground
				isGrounded = false;
			}
		}
		
		//If we press jump, and we're either grounded or allowed a second jump
		//		if(Input.GetButtonDown("Jump"))
		//		{	
		//			Jump();
		//		}
		
		//Set our player state and animation based on what we're currently doing
		if(isGrounded)
		{
			if(GetComponent<Rigidbody2D>().velocity.x == 0)
			{
				playerState = PlayerState.Idle;
//				anim.SetInteger("animState",0);
			}
			else
			{
				playerState = PlayerState.Running;
				anim.SetInteger("animState",1);
			}
			if(!hasLanded)
			{
				hasLanded = true;
				if(enemyType == EnemyType.Chasing)
				{
					ChasePlayer();
				}
				else
				{
					PickRandomDir();
				}
			}
		}
		else 
		{
			if(GetComponent<Rigidbody2D>().velocity.y > 0)
			{
				playerState = PlayerState.Jumping;
				anim.SetInteger("animState",2);
			}
			else
			{
				playerState = PlayerState.Falling;
				hasLanded = false;
				anim.SetInteger("animState",3);
			}
		}
		
		//Reset isJumping if we're not actually jumping
		if(playerState != PlayerState.Jumping)
			_isJumping = false;
		
		//Here's all the nasty stuff to handle ramps...
		
		//If we're jumping then our 'dRay' is 0 (dRay is the length of a downward raycast)
		if(playerState == PlayerState.Jumping)
		{
			_dRay = 0.0f;
		}
		//If we're noton the ground, make it a short ray, so that we can find the ground as we get close
		else if(!isGrounded)
		{
			_dRay = 0.2f;
		}
		//Otherwise we are on the ground, so we wan the the ray to be longer (this helps lock us to the ground)
		else
		{
			_dRay = 1.0f;
		}
		
		//Making the players head a trigger when falling. This stops him from getting stuck on walls
		if(playerState == PlayerState.Falling)
			_boxCollider.isTrigger = true;
		else
			_boxCollider.isTrigger = false;
		
		//If we're on the ground, not 1 unit above our rayhit, rayhit exists, and we're not jumping
		if(isGrounded && transform.position.y != _minY+yScale && _minY != Mathf.NegativeInfinity && _dRay > 0 && !_isJumping)
		{
			//Set our y position to be 1 unit above where we have hit
			transform.position = new Vector3(transform.position.x,_minY+yScale,0);
			//Set our vertical velocity to 0
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,0);
		}
	}
	
	//Here's all of our physics stuff - moving the player
	void FixedUpdate()
	{
		//If we're allowed to move and not at a wall then we will set our horizontal velocity based on input
		if (!_atWall)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2 (_currentSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}
		
		//If we're at a wall then we don't move horizontal
		if(_atWall)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0,GetComponent<Rigidbody2D>().velocity.y);
		}
		//If we can jump and were not dashing then push the player up!
		if(canJump)
		{
			canJump = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,jumpHeight);
		}
		//This handled our falling, and limits it so we don't fall too fast
		if(!isGrounded && (playerState == PlayerState.Falling || playerState == PlayerState.Jumping))
		{
			float fall = GetComponent<Rigidbody2D>().velocity.y - gravity;
			if(fall < -25)
				fall = -25;
			
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, fall);
		}
		
		//This locks our player to the ground - used for ramps (actually, I'm not sure if this even gets called...)
		if(isGrounded && transform.position.y != _minY+1.0f && _minY != Mathf.NegativeInfinity && _dRay > 0)
		{
			if(_currentSpeed == 0)
				GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			else
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,0);
		}
	}
	
	void Jump()
	{
		if(isGrounded && canInput)
		{
			//Player state is jumping
			playerState = PlayerState.Jumping;
			//anim.SetInteger("animState",2);
			//We are now jumoing
			_isJumping = true;
			//We are not on the ground
			isGrounded = false;
			//Distance for vertical rays is set to 0
			_dRay = 0;
			//We have started a jump!
			canJump = true;
		}
	}
	
	void ChasePlayer()
	{
		if(player != null)
		{
			float myY = Mathf.Round(transform.position.y * 2)/2;
			float playerY = Mathf.Round(player.GetComponent<PlayerController>().lastY * 2)/2; 
			if(myY == playerY)
			{
				if(player.transform.position.x < transform.position.x)
					_faceDir = -1;
				else
					_faceDir = 1;
			}
			else
			{
				PickRandomDir();
			}
		}
		else
		{
			PickRandomDir();
		}
	}
	
	void PickRandomDir()
	{
		_faceDir = Random.Range(0,2);
		if(_faceDir == 0)
			_faceDir = -1;
	}
	
	void StartRunning()
	{
		isRunning = true;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "JumpTrigger" && enemyType == EnemyType.Jumping)
		{	
			GameObject blob = GameObject.FindGameObjectWithTag("Blob");
			if(blob != null)
			{
				float blobY = Mathf.Round(blob.GetComponent<BlobMovement>().chaseY *2)/2;
				float myY = Mathf.Round (transform.position.y * 2)/2;
				if(myY < blobY)
				{
					JumpTrigger.DirectionLimit dir = other.GetComponent<JumpTrigger>().directionLimit;
					if(dir == JumpTrigger.DirectionLimit.None || (dir == JumpTrigger.DirectionLimit.Left && _faceDir == -1) || (dir == JumpTrigger.DirectionLimit.Right && _faceDir == 1))
						Jump ();
				}
			}
		}
	}

	void OnDestroy(){

		Instantiate (deathEffect, transform.position, Quaternion.identity);
	}
}
