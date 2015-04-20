using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour 
{
	public Transform portalExit;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Blob" || other.gameObject.tag == "Player"|| other.gameObject.tag == "Food")
		{
			other.transform.position = portalExit.position;
		}
	}
}
