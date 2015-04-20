using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonSpriteToggle : MonoBehaviour 
{
	bool mOnDefault = true;
	[SerializeField] private Sprite mDefaultSprite;
	[SerializeField] private Sprite mAltSprite;


	public void ToggleSprite()
	{
		if(GetComponent<Image>() != null)
		{
			if(mOnDefault)
			{
				GetComponent<Image>().sprite = mAltSprite;
				mOnDefault = !mOnDefault;
			}
			else
			{
				GetComponent<Image>().sprite = mDefaultSprite;
				mOnDefault = !mOnDefault;
			}
		}
	}
}
