using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour 
{
	public GameObject highScoreText;
	private int highScore;
	
	void Start () 
	{
		highScore = PlayerPrefs.GetInt("highScore",0);
		highScoreText.GetComponent<Text>().text = "HIGH SCORE: " + highScore.ToString();
	}
	void Update () 
	{
		if(Input.GetButtonDown("Jump"))
		{
			Application.LoadLevel(1);
		}
	}
}
