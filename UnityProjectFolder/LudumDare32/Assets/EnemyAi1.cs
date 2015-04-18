using UnityEngine;
using System.Collections;

public class EnemyAi1 : MonoBehaviour
{
	public float speed;
	//speed of movement
	public float jumppower;
	//setting the basic jump

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

	}

	public void Update()
	{
		_controller.AddForce((Vector2.right * speed / 100));
		//setting movement 
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "col") 
		{
			_controller.AddForce((Vector3.left * speed / 100));

		}
	}
}

