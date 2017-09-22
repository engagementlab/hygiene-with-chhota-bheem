/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeMoveGUI.cs
	Custom editor inspector for all objects using/inheriting ArchetypeMove component.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/Editor/ArchetypeMoveGUI.cs

	Created by Johnny Richardson.
==============

*/

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArchetypeMove), true)]
public class ArchetypeMoveGUI : Editor
{
	
	public override void OnInspectorGUI()
	{
		
		if(Application.isPlaying) return;
  
		var _archetype = (ArchetypeMove)target;

		if(_archetype.transform.parent != null)
		{
			string helpTxt = "This archetype has a parent and will be affected by the parent's speed.";

			if(_archetype.HasWaypoints())
				helpTxt += "\nIt also has animation waypoints, and so will use movement direction of parent.";
			
			EditorGUILayout.HelpBox(helpTxt, MessageType.Info);
		}
    
		// Draw the default inspector
		DrawDefaultInspector();

		if(_archetype.transform.parent != null)
			_archetype.UseParentSpeed = EditorGUILayout.Toggle("Use Parent's Speed", _archetype.UseParentSpeed);
		
		if(!_archetype.UseParentSpeed || _archetype.transform.parent == null)
			_archetype.MoveSpeed = EditorGUILayout.Slider("Movement Speed", _archetype.MoveSpeed, 1, 10);
		
		// Animation
		if(_archetype.HasWaypoints()) {
			
			GUILayout.BeginVertical("box");
			
			EditorGUILayout.HelpBox("This sets the duration of the journey between start and end waypoints, the type of animation, and speed scaling for forward/backward tween.", MessageType.None);
			_archetype.AnimationDuration = EditorGUILayout.Slider("Animation Duration", _archetype.AnimationDuration, 1, 10);
			_archetype.AnimationType = (ArchetypeMove.AnimType) EditorGUILayout.EnumPopup("Animation Type", _archetype.AnimationType);

			// Animation speed controls
			_archetype.AnimationForwardSpeed = EditorGUILayout.Slider("Forward Speed", _archetype.AnimationForwardSpeed, 0, 2);
			_archetype.AnimationReverseSpeed = EditorGUILayout.Slider("Backward Speed", _archetype.AnimationReverseSpeed, 0, 2);
			
			GUILayout.EndVertical();
			
		}
		
		// Movement Direction
		if(_archetype.transform.parent == null || !_archetype.HasWaypoints())
			_archetype.MovementDir = (ArchetypeMove.Dirs) EditorGUILayout.EnumPopup("Movement Direction", _archetype.MovementDir);
		else
			EditorGUILayout.HelpBox("This object has a parent and waypoints; movement direction is not editable.", MessageType.Warning);

		if(GUILayout.Button("Add Waypoint"))
			_archetype.AddWaypoint();

		// Save the changes back to the object
		EditorUtility.SetDirty(target);

	}
	
	public virtual void OnSceneGUI()
	{
		
		if(!SceneEditor.ShowGizmos || Application.isPlaying) return;
		
		var waypointChildren = new List<Transform>();

		foreach(Transform t in ((ArchetypeMove)target).transform)
		{
			if(t.tag == "Waypoint")
				waypointChildren.Add(t);
		}

		Handles.color = Color.cyan;
		foreach(var waypoint in waypointChildren)
		{
			waypoint.transform.position = Handles.FreeMoveHandle(waypoint.transform.position,
				Quaternion.identity,
				0.2f,
				Vector3.zero, 
				Handles.CylinderHandleCap);
		
			Undo.RecordObject(waypoint.transform, "Waypoint Position");
		}

	}
    
}
#endif