/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017-2019

==============
	Waypoint.cs
	Draw gizmo for Waypoint object.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/Waypoint.cs

	Created by Johnny Richardson.
==============

*/
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public void AddWaypoint()
	{
		
		var parent = transform.parent.GetComponent<ArchetypeMove>();
		if(parent == null)
		{
			var waypointParent = transform.parent.GetComponent<WaypointParent>();
			waypointParent.AddWaypoint();
		}
		else
			parent.AddWaypoint();

	}

	#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if(!SceneEditor.ShowGizmos) return;
		
		var parent = transform.parent.GetComponent<ArchetypeMove>();

		if(parent != null)
			parent.OnDrawGizmosSelected();
		else
		{
			if(transform.parent.parent != null && transform.parent.parent.GetComponent<ArchetypeMove>() != null)
				transform.parent.parent.GetComponent<ArchetypeMove>().OnDrawGizmosSelected();
			else
			{
				if(transform.parent.GetComponent<WaypointParent>() != null)
					transform.parent.GetComponent<WaypointParent>().OnDrawGizmosSelected();
			}
		}
		
	}
	#endif
	
}