/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017-2019

==============
	LocalizedSpriteGUI.cs
	Custom editor inspector for localized sprite.

	Created by Johnny Richardson.
==============

*/

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizedSprite), true)]
public class LocalizedSpriteGUI : Editor
{
	public override void OnInspectorGUI()
	{
		
		LocalizedSprite sprite = (LocalizedSprite)target;

		sprite.LanguageShown = (LocalizedSprite.Languages)EditorGUILayout.EnumPopup("Language in Editor", sprite.LanguageShown);

		sprite.EnglishSprite = (Sprite)EditorGUILayout.ObjectField("English Sprite", sprite.EnglishSprite, typeof(Sprite), false);
		sprite.TamilSprite = (Sprite)EditorGUILayout.ObjectField("Tamil Sprite", sprite.TamilSprite, typeof(Sprite), false);
		sprite.HindiSprite = (Sprite)EditorGUILayout.ObjectField("Hindi Sprite", sprite.HindiSprite, typeof(Sprite), false);


		if(sprite.LanguageShown == LocalizedSprite.Languages.Tamil)
			sprite.sprite = sprite.TamilSprite;
		else if(sprite.LanguageShown == LocalizedSprite.Languages.Hindi)
			sprite.sprite = sprite.HindiSprite;
		else
		{
		
			// Use English as default to show in editor
			sprite.sprite = sprite.EnglishSprite;

		}
		
	}
}

#endif