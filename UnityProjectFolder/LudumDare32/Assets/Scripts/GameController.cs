using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public bool gameIsOver;
	public GameObject gameOverText;
	public GameObject highScoreText;
	public int score;
	
	public void GameOver()
	{
		if(!gameIsOver)
		{
			gameIsOver = true;
			StartCoroutine("EndGame");
		}
	}
	
	IEnumerator EndGame()
	{
		int highScore = PlayerPrefs.GetInt("highScore");
		if(score > highScore)
		{
			PlayerPrefs.SetInt("highScore",score);
			highScoreText.SetActive(true);
		}
		gameOverText.SetActive(true);
		yield return new WaitForSeconds(3.0f);
		Application.LoadLevel(0);
	}
}
