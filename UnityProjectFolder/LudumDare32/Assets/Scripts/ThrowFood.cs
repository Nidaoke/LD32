using UnityEngine;
using System.Collections;

public class ThrowFood : MonoBehaviour 
{
	public bool isGrounded;
	
	public GameObject currentPlatform;
	
	void Update()
	{
		if(!isGrounded)
		{
			RaycastHit2D hit1 = Physics2D.Raycast(new Vector3(transform.position.x + 0.2f,transform.position.y,0),-Vector3.up,0.5f,1<<8);
			RaycastHit2D hit2 = Physics2D.Raycast(transform.position,-Vector3.up,0.5f,1<<8);
			RaycastHit2D hit3 = Physics2D.Raycast(new Vector3(transform.position.x - 0.2f,transform.position.y,0),-Vector3.up,0.5f,1<<8);
			
			if(hit1.collider != null ||
				hit2.collider != null ||
				hit3.collider != null)
				{
					isGrounded = true;
					if(hit1.collider != null)
						currentPlatform = hit1.collider.gameObject;
					else if(hit2.collider != null)
						currentPlatform = hit2.collider.gameObject;
					else
						currentPlatform = hit3.collider.gameObject;
						
					GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
					
					CallPet();
				}
		}
	}
	
	public void Throw(float x, float y)
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2(x,y));
	}
	
	void CallPet()
	{
		GameObject pet = GameObject.FindGameObjectWithTag("Blob");
		if(pet != null)
			pet.GetComponent<BlobMovement>().FindFood();
		
	}
}
