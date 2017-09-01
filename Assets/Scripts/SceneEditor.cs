using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneEditor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnDrawGizmos()
	{
		
		Transform[] transforms = GameObject.FindObjectsOfType<ArchetypeMove>().Select(t => t.transform).OrderBy(t => t.position.y).ToArray();
		
		Vector3 topLeftPos = new Vector3(-3, transforms.Last().position.y, 0);
		Vector3 topRightPos = new Vector3(3, transforms.Last().position.y, 0);
		Vector3 bottomLeftPos = new Vector3(-3, transforms.First().position.y, 0);
		Vector3 bottomRightPos = new Vector3(3, transforms.First().position.y, 0);
	
		
		Gizmos.color = Color.green;
		Gizmos.DrawLine(topLeftPos, topRightPos);
		Gizmos.DrawLine(topLeftPos, bottomLeftPos);
		Gizmos.DrawLine(bottomLeftPos, bottomRightPos);
		Gizmos.DrawLine(bottomRightPos, topRightPos);
//		Gizmos.DrawLine(transforms[0].transform.position, transforms[1].transform.position);
		
		if (!mat) {
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}
	}
	
	public Material mat;
	void OnPostRender() {
		
	}
	
	
}
