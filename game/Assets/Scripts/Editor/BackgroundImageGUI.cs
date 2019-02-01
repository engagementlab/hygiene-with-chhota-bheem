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

[CustomEditor(typeof(BackgroundImage), true)]
public class BackgroundImageGUI : Editor
{
  public override void OnInspectorGUI()
  {
	  EditorGUILayout.HelpBox("You cannot edit the background image manually. Please use the 'Background Type' dropdown on the Background object.", MessageType.Info);
			
    EditorUtility.SetDirty(target);

  }
	
}