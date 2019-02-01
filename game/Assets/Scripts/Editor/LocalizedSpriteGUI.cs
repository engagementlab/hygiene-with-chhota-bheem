/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

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
		
		sprite.EnglishSprite = (Sprite)EditorGUILayout.ObjectField("English Sprite", sprite.EnglishSprite, typeof(Sprite), false);
		sprite.TamilSprite = (Sprite)EditorGUILayout.ObjectField("Tamil Sprite", sprite.TamilSprite, typeof(Sprite), false);
		
		// Use English as default to show in editor
		sprite.sprite = sprite.EnglishSprite;
		
	}
}

#endif