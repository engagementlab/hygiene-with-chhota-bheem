/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeSpawnerGUI.cs
	Custom editor inspector for all objects using/inheriting ArchetypeSpawnerGUI component.

	Created by Johnny Richardson.
==============

*/

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArchetypeSpawner), true)]
public class ArchetypeSpawnerGUI : Editor
{
	
	public override void OnInspectorGUI()
	{
		
		var _archetype = (ArchetypeSpawner)target;
    
		// Draw the default inspector
		DrawDefaultInspector();

		if(_archetype.SpawnRepeating)
			_archetype.SpawnRepeatCount = EditorGUILayout.IntSlider("Repeat Count", _archetype.SpawnRepeatCount, 0, 50);
		
		_archetype.SpawnDelay = EditorGUILayout.Slider("Spawn Delay", _archetype.SpawnDelay, 0, 20);

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