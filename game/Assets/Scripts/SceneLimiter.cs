using UnityEngine;

public class SceneLimiter : MonoBehaviour {

	// Destroy any ArchetypeMove object leaving
	private void OnTriggerExit(Collider other)
	{
		var archetypeMove = other.GetComponent<ArchetypeMove>();
		archetypeMove?.DestroyObject();
	}
}
