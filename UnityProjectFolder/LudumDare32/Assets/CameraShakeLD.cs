using UnityEngine;
using System.Collections;

public class CameraShakeLD : MonoBehaviour {

	float strength;
	float mShakeTime = 0f;

	Vector3 mStartingPosition;

	void Start(){

		mStartingPosition = transform.position;
	}

	void Update(){

		if (mShakeTime > 0) {

			transform.position = mStartingPosition + (Random.insideUnitSphere * strength);

			mShakeTime -= Time.deltaTime;
		} else {

			mShakeTime = 0f;

			transform.position = mStartingPosition;
		}
	}

	public void ShakeCamera(float voidStrength, float voidDuration){

		strength = voidStrength;
		mShakeTime = voidDuration;
	}
}
