using UnityEngine;
using System.Collections;

public class CameraZed : MonoBehaviour {

	public GameObject player;

	[SerializeField] private float mCameraLerpSpeed = 0.05f;


	[SerializeField] private float mYPosMin = -1.75f;
	[SerializeField] private float mYPosMax = 9f;
	[SerializeField] private float mXPosMin = -12f;
	[SerializeField] private float mXPosMax = 12f;


	void Update()
	{
		//Move towards the player ~Adam

		transform.position = Vector3.Lerp(transform.position, new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z), mCameraLerpSpeed);

		//Keep camera in level bounds ~Adam
		//Y Min
		if(transform.position.y < mYPosMin)
		{
			transform.position = new Vector3(transform.position.x, mYPosMin, transform.position.z);
		}
		//Y Max
		if(transform.position.y > mYPosMax)
		{
			transform.position = new Vector3(transform.position.x, mYPosMax, transform.position.z);
		}
		//X Min
		if(transform.position.x < mXPosMin)
		{
			transform.position = new Vector3(mXPosMin, transform.position.y, transform.position.z);
		}
		//X Max
		if(transform.position.x > mXPosMax)
		{
			transform.position = new Vector3(mXPosMax, transform.position.y, transform.position.z);
		}




	}
}
