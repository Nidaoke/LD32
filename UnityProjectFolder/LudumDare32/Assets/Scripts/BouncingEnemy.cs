using UnityEngine;
using System.Collections;

public class BouncingEnemy : MonoBehaviour 
{
	private float hSpeed;
	private float vSpeed;

	public GameObject deathEffect;
	
	void Start()
	{
		PickRandomDirections();
	}
	
	void Update()
	{
		if (hSpeed > 0) {

			transform.localScale = new Vector2(1, transform.localScale.y);
		}

		if (hSpeed < 0) {
			
			transform.localScale = new Vector2(-1, transform.localScale.y);
		}

		RaycastHit2D hitV1 = Physics2D.Raycast(new Vector3(transform.position.x + 0.4f,transform.position.y,0),Vector3.up * vSpeed,0.6f,1<<8);
		RaycastHit2D hitV2 = Physics2D.Raycast(new Vector3(transform.position.x,transform.position.y,0),Vector3.up * vSpeed,0.6f,1<<8);
		RaycastHit2D hitV3 = Physics2D.Raycast(new Vector3(transform.position.x - 0.4f,transform.position.y,0),Vector3.up * vSpeed,0.6f,1<<8);
		
		if(hitV1.collider != null ||
		   hitV2.collider != null ||
		   hitV3.collider != null)
		{
			vSpeed *= -1;
		}
		
		RaycastHit2D hitH1 = Physics2D.Raycast(new Vector3(transform.position.x,transform.position.y+0.4f,0),Vector3.right * vSpeed,0.6f,1<<8);
		RaycastHit2D hitH2 = Physics2D.Raycast(new Vector3(transform.position.x,transform.position.y,0),Vector3.right * vSpeed,0.6f,1<<8);
		RaycastHit2D hitH3 = Physics2D.Raycast(new Vector3(transform.position.x,transform.position.y-0.4f,0),Vector3.right * vSpeed,0.6f,1<<8);
		
		if(hitH1.collider != null ||
		   hitH2.collider != null ||
		   hitH3.collider != null)
		{
			hSpeed *= -1;
		}
		
		transform.Translate(Vector3.right * hSpeed * Time.deltaTime);
		transform.Translate(Vector3.up * vSpeed * Time.deltaTime);
	}
	
	void PickRandomDirections()
	{
		while(hSpeed <= 2 && hSpeed >= -2)
		{
			hSpeed = Random.Range (-7f,7f);
		}
		while(vSpeed <= 2 && vSpeed >= -2)
		{
			vSpeed = Random.Range (-7f,7f);
		}
	}

	public void SpawnDeathEffect()
	{
		if(deathEffect != null)
		{
			Debug.Log ("Bouncer Killed");
			Instantiate (deathEffect, transform.position, Quaternion.identity);
		}
	}
}
