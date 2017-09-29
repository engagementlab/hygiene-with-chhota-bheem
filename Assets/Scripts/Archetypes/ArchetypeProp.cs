using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ArchetypeProp : MonoBehaviour {

	
	public float minVal   = 0;
	public float minLimit = -20;
	public float maxVal   =  0;
	public float maxLimit =  20;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	private void Update()
	{
		if(Selection.activeTransform != transform) return;
		
		AddTiles();
	}

	private void AddTiles()
	{
		
		foreach(Transform tile in transform)
		{
			if(tile.tag == "Prop")
				DestroyImmediate(tile.gameObject);
		}

		if(maxVal == 0 && minVal == 0) return;

		for(var i = 0; i < maxVal; i++)
		{

			var propPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Archetypes/PropBush.prefab");
			var position = Vector3.zero;
			var prop = Instantiate(propPrefab, position, Quaternion.identity);

			prop.transform.parent = transform;
			position.x = i;
			prop.transform.localPosition = position;
//			Undo.RegisterCreatedObjectUndo(prop, "Waypoint Added");
		}

		for(var h = 0; h > minVal; h--)
		{

			var propPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Archetypes/PropBush.prefab");
			var position = Vector3.zero;
			var prop = Instantiate(propPrefab, position, Quaternion.identity);

			prop.transform.parent = transform;
			position.x = h;
			prop.transform.localPosition = position;
			Debug.Log(position);
//			Undo.RegisterCreatedObjectUndo(prop, "Waypoint Added");
		}

		// Find any current waypoint children
//		var localWaypoints = (from Transform tr in transform where tr.tag == "Waypoint" select tr.position).ToList();

		
	}
}
