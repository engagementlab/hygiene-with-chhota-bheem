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

[ExecuteInEditMode]
public class SceneEditor : MonoBehaviour
{

	public static bool ShowGizmos = true;
	
	private Vector3[] _camBounds; 
	private Vector3[] _lowerCamBounds; 
	
	private Transform[] objSortedY;
	private Transform[] objSortedX;
	
	private Vector3 xPosFirst;
	private Vector3 xPosLast;
	private Vector3 yPosFirst;
	private Vector3 yPosLast;
	private Vector3 topLeftPos;
	private Vector3 topRightPos;
	private Vector3 bottomLeftPos;
	private Vector3 bottomRightPos;

	private Vector3 cameraTopRight;
	private Vector3 cameraTopLeft;
	
	private void Start()
	{
		
		// Get lists of ArchetypeMove transforms ordered by x/y pos
		objSortedY = FindObjectsOfType<ArchetypeMove>().Select(t => t.transform).Where(t => t.gameObject.layer != 8).OrderBy(t => t.position.y).ToArray();
		objSortedX = FindObjectsOfType<ArchetypeMove>().Select(t => t.transform).Where(t => t.gameObject.layer != 8).OrderBy(t => t.position.x).ToArray();
		

		xPosFirst = objSortedX.First().position;
		xPosLast = objSortedX.Last().position;
		yPosFirst = objSortedY.First().position;
		yPosLast = objSortedY.Last().position;
		
		topLeftPos = new Vector3(xPosFirst.x, yPosLast.y);
		topRightPos = new Vector3(xPosLast.x, yPosLast.y);
		bottomLeftPos = new Vector3(xPosFirst.x, yPosFirst.y);
		bottomRightPos = new Vector3(xPosLast.x, yPosFirst.y);
			
		cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
		cameraTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
		
		_camBounds = new[] {
			new Vector3(cameraTopLeft.x, 5.735f, cameraTopLeft.z), 
			new Vector3(cameraTopRight.x, 5.735f, cameraTopRight.z), 
			new Vector3(cameraTopRight.x, -5.735f, cameraTopLeft.z), 
			new Vector3(cameraTopLeft.x, -5.735f, cameraTopLeft.z), 
			new Vector3(cameraTopLeft.x, 5.735f, cameraTopLeft.z)		
		};
	}

	private void OnDrawGizmos()
	{
		if(Application.isPlaying) return;
		
		/*
		 Draw outline around all current ArchetypeMove objects, allowing easy tracking of how many moving objects are in scene.
		*/
		Gizmos.color = Color.green;
		Gizmos.DrawLine(topLeftPos, topRightPos);
		Gizmos.DrawLine(topLeftPos, bottomLeftPos);
		Gizmos.DrawLine(bottomLeftPos, bottomRightPos);
		Gizmos.DrawLine(bottomRightPos, topRightPos);

		// Draw L/R game boundaries
		Handles.color = Color.white;
		Handles.DrawDottedLine(cameraTopRight, cameraTopRight + new Vector3(0, 600), 5);
		Handles.DrawDottedLine(cameraTopLeft, cameraTopLeft + new Vector3(0, 600), 5);
		
		// Draw camera bounds
		Handles.color = new Color(1, .48f, 0.007f);
		Handles.DrawAAPolyLine(10, _camBounds);
		
		
	}
}
#endif