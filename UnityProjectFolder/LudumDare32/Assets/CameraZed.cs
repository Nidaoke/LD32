using UnityEngine;
using System.Collections;

public class CameraZed : MonoBehaviour 
{

	public GameObject player;
	public GameObject mBlob;
	public GameObject mLastFood;
	Vector3 mCameraTarget;
	[SerializeField] private float mCameraBaseLerpSpeed = 0.05f;
	[SerializeField] private float mCameraLerpSpeed = 0.05f; //Dynamically adjusted based on how far the camera is from what it is looking at -Adam


	[SerializeField] private float mYPosMin = -1.75f;
	[SerializeField] private float mYPosMax = 9f;
	[SerializeField] private float mXPosMin = -12f;
	[SerializeField] private float mXPosMax = 12f;


	void Start()
	{
		if(player != null)
		{
			mCameraTarget = player.transform.position;
		}
		if(player != null)
		{
			if(player.GetComponent<PlayerController>().playerState == PlayerController.PlayerState.Idle)
			{
				mCameraTarget = player.transform.position;
			}
			else if(mBlob != null)
			{
				mCameraTarget = mBlob.transform.position;
			}
		}
	}

	void Update()
	{
		//Move towards the player ~Adam
		if(player != null)
		{
			if(mLastFood != null && !mLastFood.GetComponent<ThrowFood>().isGrounded)
			{
				mCameraTarget = mLastFood.transform.position;
				mCameraLerpSpeed = Mathf.Clamp(mCameraBaseLerpSpeed/5 + Vector2.Distance(transform.position, mCameraTarget)/100f, 0.01f, 0.5f);
			}
			else if(player.GetComponent<PlayerController>().playerState != PlayerController.PlayerState.Idle)
			{
				mCameraTarget = player.transform.position;
				mCameraLerpSpeed = mCameraBaseLerpSpeed;
			}
			else if(mBlob != null)
			{
				mCameraTarget = mBlob.transform.position;
				mCameraLerpSpeed = Mathf.Clamp(mCameraBaseLerpSpeed/5 + Vector2.Distance(transform.position, mCameraTarget)/20000f, 0.01f, 0.5f);
			}
		}

		transform.position = Vector3.Lerp(transform.position, new Vector3 (mCameraTarget.x, mCameraTarget.y, transform.position.z), mCameraLerpSpeed);

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
