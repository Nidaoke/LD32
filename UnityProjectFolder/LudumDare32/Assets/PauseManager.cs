using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {

	public bool isPaused = false;

	void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) {

			Application.Quit();
		}

		if (isPaused) {

			Time.timeScale = 0f;
		} else {

			Time.timeScale = 1f;
		}

		if (Input.GetKeyDown (KeyCode.P)) {

			if(Application.loadedLevel != 0){

				isPaused = !isPaused;
			}
		}
	}

	void OnGUI() {

		if (isPaused) {

			if (GUI.Button (new Rect ((Screen.width / 2) - (Screen.width / 20), (Screen.height / 3), Screen.width / 10, Screen.height / 10), "Continue")) {
				
				isPaused = false;
			}
			if (GUI.Button (new Rect ((Screen.width / 2) - (Screen.width / 20), (Screen.height / 2) - (Screen.height / 20), Screen.width / 10, Screen.height / 10), "Quit")) {
				
				isPaused = false;
				Application.Quit();
			}
			if (GUI.Button (new Rect ((Screen.width / 2) - (Screen.width / 20), (Screen.height / 1.6f) - (Screen.height / 20), Screen.width / 10, Screen.height / 10), "Main Menu")) {
				
				isPaused = false;
				Application.LoadLevel(0);
				Destroy(gameObject);
			}
		}
		
	}
}
