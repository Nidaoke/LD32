using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
	public GameObject highScoreText;
	public GameObject highScoreShadow;
	private int highScore;
	
	void Start () 
	{
		highScore = PlayerPrefs.GetInt("highScore",0);
		highScoreText.GetComponent<TextMesh>().text = "HIGH SCORE: " + highScore.ToString();
		highScoreShadow.GetComponent<TextMesh>().text = "HIGH SCORE: " + highScore.ToString();
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel(1);
		}
	}
}
