using UnityEngine;

public class WaypointParent : MonoBehaviour {

  #if UNITY_EDITOR
  private void OnDrawGizmosSelected()
  {
    if(transform.parent == null) return;
    
    var parent = transform.parent.GetComponent<ArchetypeMove>();
		
    if(parent != null)
      parent.OnDrawGizmosSelected();
		
  }
  #endif
  
}
