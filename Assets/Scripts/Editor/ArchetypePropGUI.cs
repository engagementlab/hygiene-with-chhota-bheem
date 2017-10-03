using System.CodeDom;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArchetypeProp), true)]
public class ArchetypePropGUI : Editor
{

	public override void OnInspectorGUI()
	{

		var _archetype = (ArchetypeProp) target;
		float currentXRightCount = _archetype.XRightCount;
		float currentXLeftCount = _archetype.XLeftCount;
		float currentYAboveCount = _archetype.YAboveCount;
		float currentYBelowCount = _archetype.YBelowCount;
		float currentSpacing = _archetype.PropSpacing;

		_archetype.Type = (ArchetypeProp.PropType) EditorGUILayout.EnumPopup("Prop Type", _archetype.Type);
		
		GUILayout.BeginVertical("box");
		
		EditorGUILayout.LabelField("Tiles to Left: " + (_archetype.XLeftCount > 0 ? 0 : Mathf.Abs(Mathf.RoundToInt(_archetype.XLeftCount))));
		EditorGUILayout.LabelField("Tiles to Right: " + (_archetype.XRightCount < 0 ? 0 : Mathf.RoundToInt(_archetype.XRightCount)));
		EditorGUILayout.MinMaxSlider(ref _archetype.XLeftCount, ref _archetype.XRightCount, _archetype.XLeftLimit, _archetype.XRightLimit);
		
		EditorGUILayout.LabelField("Tiles Above: " + (_archetype.YAboveCount < 0 ? 0 : Mathf.RoundToInt(_archetype.YAboveCount)));
		EditorGUILayout.LabelField("Tiles Below: " + (_archetype.YBelowCount > 0 ? 0 : Mathf.Abs(Mathf.RoundToInt(_archetype.YBelowCount))));
		EditorGUILayout.MinMaxSlider(ref _archetype.YBelowCount, ref _archetype.YAboveCount, _archetype.YBelowLimit, _archetype.YAboveLimit);
		
		EditorGUILayout.LabelField("Tile Spacing");
	  _archetype.PropSpacing = EditorGUILayout.Slider(_archetype.PropSpacing, 0, 20);

		GUILayout.EndVertical();
		
		bool apply = _archetype.XRightCount != currentXRightCount || _archetype.XLeftCount != currentXLeftCount 
		             || _archetype.YAboveCount != currentYAboveCount || _archetype.YBelowCount != currentYBelowCount
								 || _archetype.PropSpacing != currentSpacing;
		if(apply)
		{
			_archetype.ApplyChange = true;
	
			if(!_archetype.SaveChanges)
				EditorGUILayout.HelpBox("You will lose your changes if you don't save.", MessageType.Warning);
			
			EditorUtility.SetDirty(target);
		}
		

		if(GUILayout.Button("Save Changes"))
		{
			_archetype.SaveChanges = true;
			EditorUtility.SetDirty(target);
		}
		
	}
	
}