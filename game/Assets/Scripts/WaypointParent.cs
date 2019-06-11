using UnityEngine;
using System.Linq;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class WaypointParent : MonoBehaviour {
	
  // Add gameobject of type Waypoint as child of this archetype; used in editor only
  public void AddWaypoint()
  {
		
	  #if UNITY_EDITOR
    var waypointPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Waypoint.prefab");
    var waypoint = Instantiate(waypointPrefab, Vector3.zero, Quaternion.identity);

    waypoint.transform.parent = transform;
		
    // Find any current waypoint children
    var localWaypoints = (from Transform tr in transform where tr.tag == "Waypoint" select tr.position).ToList();

    waypoint.name = "Waypoint_" + localWaypoints.Count;
    waypoint.transform.position = localWaypoints.Count > 1 ? localWaypoints[localWaypoints.Count - 2] : transform.position;
		 
    Selection.activeGameObject = waypoint;
		
    Undo.RegisterCreatedObjectUndo(waypoint, "Waypoint Added");
	  #endif
	}

	#if UNITY_EDITOR
  public void OnDrawGizmosSelected()
  {
	  if(transform.parent == null)
	  {
		  Utilities.DrawWaypoints(transform);
		  return;
	  }
    
    var parent = transform.parent.GetComponent<ArchetypeMove>();
    parent?.OnDrawGizmosSelected();

  }
	#endif
  
}
