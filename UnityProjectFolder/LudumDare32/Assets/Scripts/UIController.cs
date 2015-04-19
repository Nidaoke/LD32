using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour 
{
	public GameObject evolutionBar;
	public Sprite[] evolutionSprites;
	public int currentEvo;
	
	void Start()
	{
		currentEvo = 0;
		evolutionBar.GetComponent<SpriteRenderer>().sprite = evolutionSprites[currentEvo];
	}
	
	public void IncreaseEvolution()
	{
		++currentEvo;
		if(currentEvo == evolutionSprites.Length)
			currentEvo = 0;
		evolutionBar.GetComponent<SpriteRenderer>().sprite = evolutionSprites[currentEvo];
	}
}
