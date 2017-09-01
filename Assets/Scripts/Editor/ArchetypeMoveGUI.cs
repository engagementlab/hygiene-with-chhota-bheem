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
	
		Waypoint[] waypointChildren = ((ArchetypeMove)target).GetComponentsInChildren<Waypoint>();

		Handles.color = Color.cyan;
		foreach(var waypoint in waypointChildren)
		{
			waypoint.transform.position = Handles.FreeMoveHandle(waypoint.transform.position,
				Quaternion.identity,
				0.1f,
				Vector3.zero, 
				Handles.CylinderHandleCap);
		}

	}
    
    
}