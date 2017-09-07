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
    
		// Draw the default inspector
		DrawDefaultInspector();

		if(_archetype.transform.parent != null)
			_archetype.UseParentSpeed = EditorGUILayout.Toggle("Use Parent's Speed", _archetype.UseParentSpeed);
		
		if(!_archetype.UseParentSpeed)
			_archetype.MoveSpeed = EditorGUILayout.Slider("Movement Speed", _archetype.MoveSpeed, 1, 10);

		if(_archetype.HasWaypoints() && _archetype.transform.parent != null)
			EditorGUILayout.HelpBox("Archetypes with parent and waypoints inherit movement direction of parent.", MessageType.Info);
		else
			_archetype.MovementDir = (ArchetypeMove.Dirs) EditorGUILayout.EnumPopup("Dir", _archetype.MovementDir);

		if(GUILayout.Button("Add Waypoint"))
			_archetype.AddWaypoint();

		// Save the changes back to the object
		EditorUtility.SetDirty(target);

	}
	
	public virtual void OnSceneGUI()
	{
		
		if(Application.isPlaying) return;
		
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