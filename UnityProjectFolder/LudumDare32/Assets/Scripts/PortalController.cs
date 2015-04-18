using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour 
{
	public Transform portalExit;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Blob")
		{
			other.transform.position = portalExit.position;
		}
	}
}
