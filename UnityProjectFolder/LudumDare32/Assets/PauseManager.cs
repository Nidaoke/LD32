using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {

	//Will input pause later, for now, just Quit


	void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) {

			Application.Quit();
		}
	}

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
