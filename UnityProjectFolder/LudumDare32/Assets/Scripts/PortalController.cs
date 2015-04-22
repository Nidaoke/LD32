using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour 
{
	public Transform portalExit;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Blob" || other.gameObject.tag == "Player"|| other.gameObject.tag == "Food")
		{
			//To keep creatures that are too large from getting stuck in portals -Adam
			if(other.gameObject.GetComponent<PortalCooldown>()!= null)
			{
				if(other.gameObject.GetComponent<PortalCooldown>().mPortalCooldown <=0f)
				{
					other.transform.position = portalExit.position;
					other.gameObject.GetComponent<PortalCooldown>().mPortalCooldown = 0.5f;
				}
			}
			else
			{
				other.transform.position = portalExit.position;
			}
		}
	}
}
