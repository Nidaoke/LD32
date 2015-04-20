using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour 
{
	public GameObject evolutionBar;
	public Sprite[] evolutionSprites;
	public int currentEvo;
	
	public Sprite[] petSprites;
	public GameObject petImage;
	
	public GameObject[] foodImages;
	
	void Start()
	{
		currentEvo = 0;
		SetImage();
	}
	
	void Update()
	{
		UpdateImages();
	}
	public void IncreaseEvolution()
	{
		++currentEvo;
		if(currentEvo == evolutionSprites.Length)
			currentEvo = 0;
		SetImage();
	}
	
	void SetImage()
	{
		evolutionBar.GetComponent<Image>().sprite = evolutionSprites[currentEvo];
		if(currentEvo == 0)
			evolutionBar.GetComponent<Image>().color = new Color(1,1,1,0);
		else
			evolutionBar.GetComponent<Image>().color = new Color(1,1,1,1);
	}
	
	public void SetPetImage(int image)
	{
		petImage.GetComponent<Image>().sprite = petSprites[image];
	}
	
	void UpdateImages()
	{
		GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
		for(int x = 0; x < 3; ++x)
		{
			if(x < food.Length)
			{
				foodImages[x].GetComponent<Image>().color = new Color(0.2f,0.2f,0.2f,1);
			}
			else
			{
				foodImages[x].GetComponent<Image>().color = new Color(1,1,1,1);
			}
		}
	}
}
