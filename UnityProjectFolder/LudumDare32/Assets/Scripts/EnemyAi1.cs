using UnityEngine;
using System.Collections;

public class EnemyAi1 : MonoBehaviour
{
	public float speed;
	//speed of movement
	public float jumppower;
	//setting the basic jump
	public bool _left;

	private Rigidbody _controller;
	// the basic controller
	private Vector2 _direction;
	// the direction
	private Vector2 _startPosition;
	//remembering start positions

	public void Start()
	{
		_controller = gameObject.GetComponent <Rigidbody> ();
		//getting the rigidbody
		_direction = new Vector2(-1,0);
		//setting the direction
		_startPosition = transform.position;
		//finding the spawn point 
		_left = false;

	}

	public void Update()
	{
		if (_left == false) {
			_controller.AddForce ((Vector3.right * speed / 100));
		} else {
			_controller.AddForce ((Vector3.left * speed / 100));
		}
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.tag == "col") 
		{
			Debug.Log("collided with wall");
			_left = true;
		}
	}
}

