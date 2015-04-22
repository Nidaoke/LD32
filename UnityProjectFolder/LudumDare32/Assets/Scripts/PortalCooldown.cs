using UnityEngine;
using System.Collections;
//To keep creatures that are too large from getting stuck in portals -Adam

public class PortalCooldown : MonoBehaviour 
{
	 public float mPortalCooldown = 0f;

	
	// Update is called once per frame
	void Update () 
	{
		if(mPortalCooldown > 0f)
		{
			mPortalCooldown -= Time.deltaTime;
		}
	}
}
