using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

	public float PropSpacing = 5.33f;

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
			
		// Change image
		if(Type != _currentImgType)
			SetType(Type);
			
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

	private void SetType(PropType type)
	{
			
		if(Image == null)
			Image = GetComponent<PropImage>();

		Type = type;
		_currentImgType = type;
	
		var imgName = type.ToString().ToLower();
		var imgAssetAtPath = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/props/" + imgName + ".png");
		Image.texture = imgAssetAtPath;
		Image.enabled = true;

		if(transform.parent == null && transform.childCount > 0)
		{
			foreach(Transform t in transform)
			{
				if(t.tag == "Prop")
					t.GetComponent<ArchetypeProp>().SetType(type);
			}
		}

	}

	private void RemoveAllTiles()
	{
		Object[] undoCopy = new Object[_objectsToUndo.Count]; 
		_objectsToUndo.CopyTo(undoCopy);
		
		foreach(GameObject tile in undoCopy)
		{
			if(tile.gameObject != null)
			{
				_objectsToUndo.Remove(tile.gameObject);
				DestroyImmediate(tile.gameObject);
			}
		}
		
	}

	private void InstantiateTile(int index, string direction, bool xAxis=true)
	{
		
		var propPrefab = AssetDatabase.LoadAssetAtPath<ArchetypeProp>("Assets/Prefabs/Archetypes/Prop.prefab");
		var position = Vector3.zero;
		var prop = Instantiate(propPrefab, position, Quaternion.identity);

		prop.transform.parent = transform;
		prop.SetType(Type);
		
		if(xAxis)
			position.x = PropSpacing * index;
		else
			position.y = PropSpacing * index;
		
		prop.transform.localPosition = position;
		prop.name = "PropBush_" + direction + "-" + Mathf.Abs(index);

		_objectsToUndo.Add(prop.gameObject);
		Undo.RegisterCreatedObjectUndo(prop, "Add Tiles");
		
	}

	private void AddTiles()
	{
		
		if(XRightCount == 0 && XLeftCount == 0 && YAboveCount == 0 && YBelowCount == 0) return;

		if(XRightCount > 0)
		{
			for(var i = 1; i < XRightCount; i++)
				InstantiateTile(i, "Right");				
		}

		if(XLeftCount < 0)
		{
			for(var i = -1; i > XLeftCount; i--)
				InstantiateTile(i, "Left");				
				
		}

		if(YAboveCount > 0)
		{
			for(var i = 1; i < YAboveCount; i++)
				InstantiateTile(i, "Above", false);

		}
		if(YBelowCount < 0)
		{
			for(var i = 1; i > YBelowCount; i--)
				InstantiateTile(i, "Below", false);

		}		
		ApplyChange = false;
		
	}
	
}
