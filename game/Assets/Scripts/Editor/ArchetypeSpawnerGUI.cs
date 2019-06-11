/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017-2019

==============
	ArchetypeSpawnerGUI.cs
	Custom editor inspector for all objects using/inheriting ArchetypeSpawnerGUI component.

	Created by Johnny Richardson.
==============

*/

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ArchetypeSpawner), true)]
public class ArchetypeSpawnerGUI : Editor
{
	
	public override void OnInspectorGUI()
	{
		
		var _archetype = (ArchetypeSpawner)target;
    
		// Draw the default inspector
		DrawDefaultInspector();
		
		if(_archetype.MoveAfterSpawn)
			_archetype.UseSpawnerParent = EditorGUILayout.Toggle("Use Spawner's Parent", _archetype.UseSpawnerParent);

		if(_archetype.SpawnRepeating)
			_archetype.SpawnRepeatCount = EditorGUILayout.IntSlider("Repeat Count", _archetype.SpawnRepeatCount, 0, 50);
		
		_archetype.SpawnDelay = EditorGUILayout.Slider("Spawn Start Delay", _archetype.SpawnDelay, 0, 20);
		
		if(_archetype.SpawnedObjects == null)
			EditorGUILayout.HelpBox("Don't forget to assign a prefab!", MessageType.Error);

		// Save the changes back to the object
		EditorUtility.SetDirty(target);

	}
    
}
#endif