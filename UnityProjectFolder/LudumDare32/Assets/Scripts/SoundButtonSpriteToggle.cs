using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundButtonSpriteToggle : MonoBehaviour 
{

	[SerializeField] private Sprite mDefaultSprite;
	[SerializeField] private Sprite mAltSprite;


	
	// Update is called once per frame
	void Update () 
	{
		if(GetComponent<Image>() != null)
		{
			if(AudioListener.volume == 0)
			{
				GetComponent<Image>().sprite = mAltSprite;
			}
			else
			{
				GetComponent<Image>().sprite = mDefaultSprite;
			}
		}

	}
}
