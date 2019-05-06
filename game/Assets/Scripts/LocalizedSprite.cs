/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	LocalizedSprite.cs
	Class that inherits Image type and allows for localized sprite display based on language.

	Created by Johnny Richardson.
==============

*/
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LocalizedSprite : Image
{

	public Sprite EnglishSprite;
	public Sprite TamilSprite;
	public Sprite HindiSprite;
	
	// Use this for initialization
	private void Start ()
	{
		
		// Allows for multiple image sizes
		type = Type.Filled;
		fillMethod = FillMethod.Horizontal;
		preserveAspect = true;
		
		base.Start();
		
		UpdateSprite();

		Events.instance.AddListener<LanguageChangeEvent> (UpdateSprite);

	}

	private void OnDestroy()
	{
		Events.instance.RemoveListener<LanguageChangeEvent> (UpdateSprite);		
	}

	private void UpdateSprite(LanguageChangeEvent e=null)
	{
		if(TamilSprite == null)
		{
			sprite = EnglishSprite;
			return;
		}
		
		switch(GameConfig.CurrentLanguage)
		{
			
			case 0:

				sprite = EnglishSprite;
		
				break;
			
			case 1:

				sprite = TamilSprite;
		
				break;
			
			case 2:

				sprite = HindiSprite;

				break;
			
		}
		
	}
}
