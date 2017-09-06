using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public void AddWaypoint()
	{
		ArchetypeMove parent = transform.parent.GetComponent<ArchetypeMove>();
		
		if(parent != null)
			parent.AddWaypoint();
		
	}

#if UNITY_EDITOR
 
	void OnDrawGizmosSelected()
	{
		ArchetypeMove parent = transform.parent.GetComponent<ArchetypeMove>();
		
		if(parent != null)
			parent.OnDrawGizmosSelected();
		
	}
#endif
}
