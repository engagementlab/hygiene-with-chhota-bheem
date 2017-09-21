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

public class Waypoint : MonoBehaviour {

	public void AddWaypoint()
	{
		
		var parent = transform.parent.GetComponent<ArchetypeMove>();
		
		if(parent != null)
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
			if(transform.parent.parent.GetComponent<ArchetypeMove>() != null)
				transform.parent.parent.GetComponent<ArchetypeMove>().OnDrawGizmosSelected();
		}
		
	}
	#endif
	
}