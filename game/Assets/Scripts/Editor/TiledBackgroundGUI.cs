/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	TiledBackgroundGUI.cs
	Inspector GUI for background.

	Created by Johnny Richardson.
==============

*/
using UnityEditor;

[CustomEditor(typeof(TiledBackground), true)]
public class TiledBackgroundGUI : Editor
{
  void OnEnable()
  {
    Tools.hidden = true;
  }
 
  void OnDisable()
  {
    Tools.hidden = false;
  }
	
  public override void OnInspectorGUI()
  {

	  TiledBackground background = (TiledBackground) target;	
	  background.BackgroundImg = (TiledBackground.BackgroundType) EditorGUILayout.EnumPopup("Background Type", background.BackgroundImg);
	  
    // Save the changes back to the object
    EditorUtility.SetDirty(target);

  }
	
}