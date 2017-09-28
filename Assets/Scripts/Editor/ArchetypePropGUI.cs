using System.CodeDom;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArchetypeProp), true)]
public class ArchetypePropGUI : Editor
{

	public override void OnInspectorGUI()
	{

		var _archetype = (ArchetypeProp) target;
		float currentMax = _archetype.maxVal;
		float currentMin = _archetype.minVal;
		
		EditorGUILayout.MinMaxSlider(ref _archetype.minVal, ref _archetype.maxVal, _archetype.minLimit, _archetype.maxLimit);
		
		if(_archetype.maxVal != currentMax || _archetype.minVal != currentMin)
			EditorUtility.SetDirty(target);	
	}

//	private void OnSceneGUI()
//	{
//		Debug.Log("seelcted");
//		var _archetype = (ArchetypeProp) target;
//		_archetype.AddTiles();
//	}
}