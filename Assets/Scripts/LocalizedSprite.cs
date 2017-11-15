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

public class LocalizedSprite : Image
{

	public Sprite EnglishSprite;
	public Sprite TamilSprite;
	
	// Use this for initialization
	private void Start ()
	{
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
		sprite = GameConfig.CurrentLanguage == 1 ? TamilSprite : EnglishSprite;
	}
}
