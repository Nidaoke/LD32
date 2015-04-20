using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour 
{

	//Made this static so that anyone can see when the game is paused
	public static bool isPaused = false;
	[SerializeField] GameObject mPausePanel;

	void Update()
	{

		if (Input.GetButtonDown("Quit")) 
		{
			Application.Quit();
		}
		if (Input.GetButtonDown("Pause")) 
		{
				if(!isPaused)
				{
					PauseGame();
				}
				else
				{
					UnpauseGame();
				}
		}
	}


	
	public void PauseGame()
	{
		isPaused = true;
		mPausePanel.SetActive(true);
		Time.timeScale = 0;
	}//END of PauseGame()
	
	public void UnpauseGame()
	{
		isPaused = false;
		mPausePanel.SetActive(false);
		Time.timeScale = 1;
	}//END of PauseGame()

	//Mute or UnMute the volume -Adam
	public void ToggleVolume()
	{
		if(AudioListener.volume != 0)
		{
			AudioListener.volume = 0;
		}
		else
		{
			AudioListener.volume = 1;
		}
	}//END of ToggleVolume()

	public void ReturnToMenu()
	{
		UnpauseGame();
		Application.LoadLevel(0);
		Destroy(gameObject);
	}//END of ReturnToMenu()
}
