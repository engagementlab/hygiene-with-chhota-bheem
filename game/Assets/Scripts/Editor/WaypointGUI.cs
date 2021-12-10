/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	Waypoint.cs
	Draw gizmo for Waypoint object.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/Waypoint.cs

	Created by Johnny Richardson.
==============

*/
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoint), true)]
public class WaypointGUI : Editor
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
		
		Waypoint _waypoint= (Waypoint)target;
    
		if(GUILayout.Button("Add Another Waypoint"))
			_waypoint.AddWaypoint();
    
		// Save the changes back to the object
		EditorUtility.SetDirty(target);

	}
	

	public virtual void OnSceneGUI()
	{
		
		if(!SceneEditor.ShowGizmos) return;
		
		Waypoint _waypoint= (Waypoint)target;	
		
		Handles.color = Color.yellow;

		_waypoint.transform.position = Handles.FreeMoveHandle(_waypoint.transform.position,
			Quaternion.identity,
			0.4f,
			Vector3.zero, 
			Handles.CylinderHandleCap);
		
		Undo.RecordObject(_waypoint.transform, "Waypoint Position");
		
	}
    
}