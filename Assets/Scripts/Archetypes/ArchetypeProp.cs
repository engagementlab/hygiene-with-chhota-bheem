using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ArchetypeProp : MonoBehaviour {

	[HideInInspector]
	public PropImage Image;
	
	public enum PropType
	{
		Bush,
		ThornBush,
		EyesBush,
		Rock1,		
		Rock2,
		Rock3
	}
	public PropType Type;
	
	public float XLeftCount;
	public float XLeftLimit = -20;
	public float XRightCount;
	public float XRightLimit =  20;

	public float YBelowCount;
	public float YBelowLimit =  -20;
	public float YAboveLimit = 20;
	public float YAboveCount;

	public bool ApplyChange;
	public bool SaveChanges;

	private bool _isSelected;
	private PropType _currentImgType;

	readonly List<Object> _objectsToUndo = new List<Object>();

	private void Start()
	{
		if(Image == null)
			Image = GetComponent<PropImage>();
	}

	// Update is called once per frame
	private void Update()
	{
		#if UNITY_EDITOR
			
			if(Image == null)
				Image = GetComponent<PropImage>();
			
			// Change image
			if(Type != _currentImgType)
			{
				_currentImgType = Type;
	
				var imgName = Type.ToString().ToLower();
				var imgAssetAtPath = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/props/" + imgName + ".png");
				Image.texture = imgAssetAtPath;
			}
			
			if(_isSelected && Selection.activeTransform != transform)
			{
					
				if(!SaveChanges)
					RemoveAllTiles();
				
			}
	
			_isSelected = Selection.activeTransform == transform;
			
			if(_isSelected && ApplyChange)
				RemoveAllTiles();
			
		#endif
	}

	private void LateUpdate()
	{
		if(ApplyChange)
			AddTiles();
		
	}

	private void RemoveAllTiles()
	{
		
		foreach(GameObject tile in _objectsToUndo)
		{
			if(tile.tag == "Prop")
				DestroyImmediate(tile.gameObject);
		}
		_objectsToUndo.Clear();
		
	}

	private void AddTiles()
	{
		if(XRightCount == 0 && XLeftCount == 0 && YAboveCount == 0 && YBelowCount == 0) return;

		if(XRightCount > 0)
		{
			for(var i = 1; i < XRightCount; i++)
			{

				var propPrefab = AssetDatabase.LoadAssetAtPath<ArchetypeProp>("Assets/Prefabs/Archetypes/PropBush.prefab");
				var position = Vector3.zero;
				var prop = Instantiate(propPrefab, position, Quaternion.identity);

				prop.Type = Type;
				prop.transform.parent = transform;
				position.x = 5.33f * i;
				prop.transform.localPosition = position;
				prop.name = "PropBushRight_" + i;

				_objectsToUndo.Add(prop.gameObject);
				Undo.RegisterCreatedObjectUndo(prop, "Add Tiles");
			}
		}

		if(XLeftCount < 0)
		{
			for(var h = -1; h > XLeftCount; h--)
			{

				var propPrefab = AssetDatabase.LoadAssetAtPath<ArchetypeProp>("Assets/Prefabs/Archetypes/PropBush.prefab");
				var position = Vector3.zero;
				var prop = Instantiate(propPrefab, position, Quaternion.identity);

				prop.Type = Type;
				prop.transform.parent = transform;
				position.x = 5.33f * h;
				prop.transform.localPosition = position;
				prop.name = "PropBushLeft_" + Mathf.Abs(h);

				_objectsToUndo.Add(prop.gameObject);
				Undo.RegisterCreatedObjectUndo(prop, "Add Tiles");
			}
		}

		if(YAboveCount > 0)
		{
			for(var i = 1; i < Mathf.RoundToInt(YAboveCount); i++)
			{

				var propPrefab = AssetDatabase.LoadAssetAtPath<ArchetypeProp>("Assets/Prefabs/Archetypes/PropBush.prefab");
				var position = Vector3.zero;
				var prop = Instantiate(propPrefab, position, Quaternion.identity);

				prop.Type = Type;
				prop.transform.parent = transform;
				position.y = 5.33f * i;
				prop.transform.localPosition = position;
				prop.name = "PropBushAbove_" + i;

				_objectsToUndo.Add(prop.gameObject);
				Undo.RegisterCreatedObjectUndo(prop, "Add Tiles");
			}
		}

		if(YBelowCount < 0)
		{
			for(var i = 1; i > Mathf.RoundToInt(YBelowCount); i--)
			{

				var propPrefab = AssetDatabase.LoadAssetAtPath<ArchetypeProp>("Assets/Prefabs/Archetypes/PropBush.prefab");
				var position = Vector3.zero;
				var prop = Instantiate(propPrefab, position, Quaternion.identity);

				prop.Type = Type;
				prop.transform.parent = transform;
				position.y = 5.33f * i;
				prop.transform.localPosition = position;
				prop.name = "PropBushBelow_" + Mathf.Abs(i);

				_objectsToUndo.Add(prop.gameObject);
				Undo.RegisterCreatedObjectUndo(prop, "Add Tiles");
			}
		}		
		ApplyChange = false;
	}
}
