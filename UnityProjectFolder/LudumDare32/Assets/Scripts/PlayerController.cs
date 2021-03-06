﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	[SerializeField] private AudioClip mPlayerDeathSound;

//	public int animStateShow;

	public bool movedRight = true;

	public int feedAmount = 0;

	public int feedMax;

	public bool canFeed = true;

	public GameObject blob;

	//List of player states
	public enum PlayerState {Idle, Running, Jumping, Falling, Eating, Dead};
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
	
	public float playerSlide;
	
	//Can we move?
	public bool canMove;
	//Are we on the ground?
	public bool isGrounded;
	//Are we jumping?
	public bool canJump;
	
	//Max health                    Will we have Health or just death???
	public int maxHealth;
	//Current health value
	public int currentHealth;
	
	//Particles to explode the player
	public Transform deadParticle;
	//Child object which holds our animations	
//	public Transform playerAnimation;
	//The animator for our child object
	public Animator anim;
	
	//Have we cancelled our current jump?
	private bool _jumpCancelled;
	//Have we reached a wall?
	private bool _atWall;
	//Are we jumping?
	private bool _isJumping;
	//Are we accepting input?
	public bool canInput;
	
	//Is the player dead?
	private bool _isDead;
	//Current movement speed
	private float _currentSpeed;
	//Maximum movement speed
	private float _maxSpeed;
	//Direction we are facing (-1 left, 1 right)
	private float _faceDir;
	//The lowest we're currently allowed on the Y axis (used for slopes)
	private float _minY;
	//Length of our downward facing rays
	private float _dRay;
	//Our transforms x scale
	private float xScale;
	//Our transforms y scale
	private float yScale;
	//Check which way walls are when dashing
	private int _wallDir;
	
	public Transform food;
	
	public float lastY;
	private bool hasLanded;
	private bool isThrowing;
	
	private BoxCollider2D _boxCollider;
	
	private GameController gameController;
	private UIController uiController;
	
	//Set stuff up at the start
	void Start()
	{
		_boxCollider = GetComponent<BoxCollider2D>();
		//Facing right
		_faceDir = 1;
		//Player state is idle
		playerState = PlayerState.Idle;
//		anim = playerAnimation.GetComponent<Animator>();
		gameController = Camera.main.GetComponent<GameController>();
		uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
		anim.SetInteger("animState",0);
		//Previous state is also idle`
		_lastState = playerState;
		//Set health to max
		currentHealth = maxHealth;
		
		//Accept input
		canInput = true;
		//The player is alive!
		_isDead = false;
		
		xScale = transform.localScale.x / 2f;
		yScale = transform.localScale.y;
	}
	
	void Update()
	{	


		//Check to see if we've got horizontal input, or if we're dashing
		if ((((Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < -0.5f)) && canInput)) {
			//This will determine the direction of horizontal raycasting
			float checkDir = 0;

			checkDir = Input.GetAxisRaw ("Horizontal");
			_faceDir = checkDir;
			transform.localScale = new Vector3(checkDir, 1,1);

			//We are not at a wall
			_atWall = false;
			//Raycast 5 rays horizontally
			RaycastHit2D hit1 = Physics2D.Raycast (new Vector3 (transform.position.x, transform.position.y + (xScale / 2f), 0), Vector3.right * checkDir, 0.23f, 1 << 8);
			RaycastHit2D hit2 = Physics2D.Raycast (new Vector3 (transform.position.x, transform.position.y, 0), Vector3.right * checkDir, xScale + 0.03f, 1 << 8);
			RaycastHit2D hit3 = Physics2D.Raycast (new Vector3 (transform.position.x, transform.position.y - (xScale / 2f), 0), Vector3.right * checkDir, 0.23f, 1 << 8);
			RaycastHit2D hit4 = Physics2D.Raycast (new Vector3 (transform.position.x, transform.position.y - (xScale - 0.1f), 0), Vector3.right * checkDir, xScale + 0.03f, 1 << 8);
			RaycastHit2D hit5 = Physics2D.Raycast (new Vector3 (transform.position.x, transform.position.y + (xScale - 0.1f), 0), Vector3.right * checkDir, xScale + 0.03f, 1 << 8);
			//Check to see if any of the rays have hit the environment
			if (hit1.collider != null ||
				hit2.collider != null ||
				hit3.collider != null ||
				hit4.collider != null ||
				hit5.collider != null) {
				
				_wallDir = (int)checkDir;
				_atWall = true;
			}
		} else {
			_atWall = false;
		}
		
//		//Scale our animation to be facing the right direction
//		playerAnimation.transform.localScale = new Vector3 (_faceDir, 1, 1);
		
		if (canInput) {
			float s = _currentSpeed / movementSpeed;
			//Surely there's an easier way to clamp it to max/min
			if (Input.GetAxisRaw ("Horizontal") > 0.5f)
				s += playerSlide;
			else if (Input.GetAxisRaw ("Horizontal") < -0.5f)
				s -= playerSlide;
			else {
				if (s > 0.1f)
					s -= playerSlide;
				else if (s < -0.1f)
					s += playerSlide;
				else
					s = 0;
			}
			float cSpeed = s;
			//Clamp our 'cSpeed'
			if (cSpeed > 1f)
				cSpeed = 1f;
			if (cSpeed < -1f)
				cSpeed = -1f;
			//And times it by our movement speed
			_currentSpeed = movementSpeed * cSpeed;
		}
		
		//If we're at a wall we set our movement speed to 0
		if (_atWall)
			_currentSpeed = 0;
		
		//Create a float and set it to a stupidly lowf number
		float topRay = Mathf.NegativeInfinity;
		
		//If we're not jumping, and we haven't started a jump
		if (!_isJumping && !canJump) {
			//Cast two rays down
			RaycastHit2D hitD1 = Physics2D.Raycast (new Vector3 (transform.position.x + (xScale - 0.1f), transform.position.y - (yScale - 0.1f), 0), -Vector3.up, _dRay, 1 << 8);
			RaycastHit2D hitD2 = Physics2D.Raycast (new Vector3 (transform.position.x - (xScale - 0.1f), transform.position.y - (yScale - 0.1f), 0), -Vector3.up, _dRay, 1 << 8);
			Debug.DrawRay (new Vector3 (transform.position.x + (xScale - 0.1f), transform.position.y - (yScale - 0.1f), 0), -Vector3.up);
			//Check to see if either of the rays have hit
			if ((hitD1.collider != null) || 
				(hitD2.collider != null)) {
				
				//Set the 'topRay' to where the first ray hit
				if (hitD1.collider != null)
					topRay = hitD1.point.y;
				//If the second ray hit a higher position, set 'topRay' to there instead
				if (hitD2.collider != null && hitD2.point.y > topRay)
					topRay = hitD2.point.y;
				//If we have set the 'topRay' this frame, our lowest Y position is set to that point
				if (topRay > Mathf.NegativeInfinity)
					_minY = topRay;
				//We are touching the ground
				isGrounded = true;			
			} else {
				//None of the rays hit, so we're not on the ground
				isGrounded = false;
			}
		}
		
		//If we press jump, and we're grounded
		if (Input.GetButtonDown ("Jump") && canInput) 
		{	
			Jump ();
		}

		//Set our player state and animation based on what we're currently doing
		if(isGrounded)
		{
			if(GetComponent<Rigidbody2D>().velocity.x == 0)
			{
				playerState = PlayerState.Idle;
				anim.SetInteger("animState",0);
			}
			else
			{
				playerState = PlayerState.Running;
				anim.SetInteger("animState",1);
			}
			if(!hasLanded)
			{
				hasLanded = true;
				lastY = transform.position.y;
			}
		}
		else
		{
			if(GetComponent<Rigidbody2D>().velocity.y > 0)
			{
				playerState = PlayerState.Jumping;
				anim.SetInteger("animState",1);
			}
			else
			{
				playerState = PlayerState.Falling;
				hasLanded = false;

//				anim.SetInteger("animState",3);
			}
		}

//		if (playerState == PlayerState.Running) {
//			playerState = PlayerState.Running;
//			anim.SetInteger("animState", 1);
//		}
//				anim.SetInteger("animState",1);
//			}
//		}

		if (playerState == PlayerState.Running) 
		{
			playerState = PlayerState.Running;
			anim.SetInteger("animState", 1);
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


		//Moved throw into a single check. Get count of food, then throw if there's not too many
		if(Input.GetButtonDown("Throw") && canFeed && canInput && !PauseManager.isPaused)
		{
			feedAmount = GameObject.FindGameObjectsWithTag("Food").Length;
			if(feedAmount <= (feedMax - 1))//For some reason it keeps adding one to it, that's fine though.
			{
//				isThrowing = true;
//				StartCoroutine("ThrowCooldown");
				anim.SetInteger("animState", 5);
				//Time.timeScale = 0;
				Transform newFood = (Transform)Instantiate(food,transform.position,Quaternion.identity);
				newFood.GetComponent<ThrowFood>().Throw(750 *_faceDir, 750);
				Camera.main.GetComponent<CameraZed>().mLastFood = newFood.gameObject;
			}
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
			lastY = transform.position.y;
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
		if(isGrounded)
		{
			//Player state is jumping
			playerState = PlayerState.Jumping;

			anim.SetInteger("animState",1);
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
	
	void OnTriggerEnter2D(Collider2D other)
	{	
		if(other.gameObject.tag == "Enemy")// || other.gameObject.tag == "Blob")
		{
			Debug.Log("You have died");
			other.GetComponent<AudioSource>().PlayOneShot(mPlayerDeathSound);

			if(deadParticle != null)
				Instantiate(deadParticle, transform.position, Quaternion.identity);
			gameController.GameOver();
			Destroy(gameObject);
//			Destroy(blob);
		}

//		if (other.gameObject.tag == "Portal") {
//
//			Debug.Log("In Portal");
//
//			canFeed = false;
//		} else {
//
//			canFeed = true;
//		}
	}
	
//	IEnumerator ThrowCooldown()
//	{
//		yield return new WaitForSeconds(0.2f);
//		isThrowing = false;
//	}

//	void OnTriggerExit2D(Collider2D col) {

//		Debug.Log ("Exited Trigger");

//		if (col.gameObject.tag == "Portal") {
//
//			canFeed = true;
//		}
//	}
}
