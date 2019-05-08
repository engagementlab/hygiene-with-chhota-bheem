/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017-2019

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
	
	private Transform[] _objSortedY;
	private Transform[] _objSortedX;
	
	private Vector3 _xPosFirst;
	private Vector3 _xPosLast;
	private Vector3 _yPosFirst;
	private Vector3 _yPosLast;
	private Vector3 _topLeftPos;
	private Vector3 _topRightPos;
	private Vector3 _bottomLeftPos;
	private Vector3 _bottomRightPos;

	private Vector3 _cameraTopRight;
	private Vector3 _cameraTopLeft;
	
	private void OnDrawGizmos()
	{
		if(Application.isPlaying) return;
		
		// Get lists of ArchetypeMove transforms ordered by x/y pos
		_objSortedY = FindObjectsOfType<ArchetypeMove>().Select(t => t.transform).Where(t => t.gameObject.layer != 8).OrderBy(t => t.position.y).ToArray();
		_objSortedX = FindObjectsOfType<ArchetypeMove>().Select(t => t.transform).Where(t => t.gameObject.layer != 8).OrderBy(t => t.position.x).ToArray();
		
		if(_objSortedX.Length == 0 || _objSortedY.Length == 0) return;
		
		_xPosFirst = _objSortedX.First().position;
		_xPosLast = _objSortedX.Last().position;
		_yPosFirst = _objSortedY.First().position;
		_yPosLast = _objSortedY.Last().position;
		
		_topLeftPos = new Vector3(_xPosFirst.x, _yPosLast.y);
		_topRightPos = new Vector3(_xPosLast.x, _yPosLast.y);
		_bottomLeftPos = new Vector3(_xPosFirst.x, _yPosFirst.y);
		_bottomRightPos = new Vector3(_xPosLast.x, _yPosFirst.y);
		
		/*
		 Draw outline around all current ArchetypeMove objects, allowing easy tracking of how many moving objects are in scene.
		*/
		Gizmos.color = Color.green;
		Gizmos.DrawLine(_topLeftPos, _topRightPos);
		Gizmos.DrawLine(_topLeftPos, _bottomLeftPos);
		Gizmos.DrawLine(_bottomLeftPos, _bottomRightPos);
		Gizmos.DrawLine(_bottomRightPos, _topRightPos);

		// Draw L/R game boundaries
		Handles.color = Color.white;
		Handles.DrawDottedLine(_cameraTopRight, _cameraTopRight + new Vector3(0, 600), 5);
		Handles.DrawDottedLine(_cameraTopLeft, _cameraTopLeft + new Vector3(0, 600), 5);
		
		_cameraTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
		_cameraTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane));
		_camBounds = new[] {
			new Vector3(_cameraTopLeft.x, 5.735f, _cameraTopLeft.z), 
			new Vector3(_cameraTopRight.x, 5.735f, _cameraTopRight.z), 
			new Vector3(_cameraTopRight.x, -5.735f, _cameraTopLeft.z), 
			new Vector3(_cameraTopLeft.x, -5.735f, _cameraTopLeft.z), 
			new Vector3(_cameraTopLeft.x, 5.735f, _cameraTopLeft.z)		
		};
		
		// Draw camera bounds
		Handles.color = new Color(1, .48f, 0.007f);
		Handles.DrawAAPolyLine(10, _camBounds);
		
	}
}
#endif