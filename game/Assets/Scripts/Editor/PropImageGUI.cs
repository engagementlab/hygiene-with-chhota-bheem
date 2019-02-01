/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	PropImageGUI.cs
	Inspector GUI for prop image.

	Created by Johnny Richardson.
==============

*/
using UnityEditor;

[CustomEditor(typeof(PropImage), true)]
public class PropImageGUI : Editor
{
  public override void OnInspectorGUI()
  {
    EditorGUILayout.HelpBox("You cannot edit the prop image manually. Please use the 'Prop Type' dropdown.", MessageType.Info);
			
    EditorUtility.SetDirty(target);

  }
	
}