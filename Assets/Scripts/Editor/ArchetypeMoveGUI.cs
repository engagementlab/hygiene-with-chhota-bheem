using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArchetypeMove), true)]
public class ArchetypeMoveGUI : Editor
{
	
	string[] movementDirections = new string[]
	{
		"Down",
		"Up",
		"Left",
		"Right"
	};

	public override void OnInspectorGUI()
	{
  
		ArchetypeMove _archetype = (ArchetypeMove)target;
    
		// Draw the default inspector
		DrawDefaultInspector();
    
		GUILayout.Label ("Movement Direction:");

		_archetype._direction = EditorGUILayout.Popup(_archetype._direction, movementDirections);
    
		// Update the selected choice in the underlying object
		_archetype.movementDir = movementDirections[_archetype._direction].ToLower();
		
		if(GUILayout.Button("Add Waypoint"))
			_archetype.AddWaypoint();
    
		// Save the changes back to the object
		EditorUtility.SetDirty(target);

	}
	
	public virtual void OnSceneGUI()
	{
		
		List<Transform> waypointChildren = new List<Transform>();

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
				0.1f,
				Vector3.zero, 
				Handles.CylinderHandleCap);
		
			Undo.RecordObject(waypoint.transform, "Waypoint Position");
		}

	}
    
    
}