/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	SceneEditor.cs
	Custom gizmos for editor scene view.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/SceneEditor.cs

	Created by Johnny Richardson.
==============

*/
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SceneEditor : MonoBehaviour {
	
	void OnDrawGizmos()
	{
		if(Application.isPlaying) return;
		
		/*
		 Draw outline around all current ArchetypeMove objects, allowing easy tracking of how many moving objects are in scene.
		*/
		
		// Get lists of ArchetypeMove transforms ordered by x/y pos
		var transformsY = FindObjectsOfType<ArchetypeMove>().Select(t => t.transform).OrderBy(t => t.position.y).ToArray();
		var transformsX = FindObjectsOfType<ArchetypeMove>().Select(t => t.transform).OrderBy(t => t.position.x).ToArray();
		
		var xPosFirst = transformsX.First().position;
		var xPosLast = transformsX.Last().position;
		var yPosFirst = transformsY.First().position;
		var yPosLast = transformsY.Last().position;
		
		var topLeftPos = new Vector3(xPosFirst.x, yPosLast.y);
		var topRightPos = new Vector3(xPosLast.x, yPosLast.y);
		var bottomLeftPos = new Vector3(xPosFirst.x, yPosFirst.y);
		var bottomRightPos = new Vector3(xPosLast.x, yPosFirst.y);
			
		// Draw outline
		Gizmos.color = Color.green;
		Gizmos.DrawLine(topLeftPos, topRightPos);
		Gizmos.DrawLine(topLeftPos, bottomLeftPos);
		Gizmos.DrawLine(bottomLeftPos, bottomRightPos);
		Gizmos.DrawLine(bottomRightPos, topRightPos);

		var topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
		var topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
		
		Handles.color = Color.white;
		Handles.DrawDottedLine(topRight, topRight + new Vector3(0, 600), 5);
		Handles.DrawDottedLine(topLeft, topLeft + new Vector3(0, 600), 5);
		
	}
}
#endif