/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017-2019

==============
	SceneLimiterGUI.cs
	SceneLimiter GUI.

	Created by Johnny Richardson.
==============

*/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneLimiter), true)]
public class SceneLimiterGUI : Editor
{
	
	private void OnSceneGUI()
	{
		BoxCollider collider = ((SceneLimiter)target).GetComponent<BoxCollider>();
		Vector3[] v = new[]
		{
			new Vector3(collider.bounds.min.x, collider.bounds.max.y, 0),
			new Vector3(collider.bounds.max.x, collider.bounds.max.y, 0),
			new Vector3(collider.bounds.max.x, collider.bounds.min.y, 0),
			new Vector3(collider.bounds.min.x, collider.bounds.min.y, 0)

		};
		Handles.DrawSolidRectangleWithOutline(v, new Color(.54f, .811f, 1, .1f), Color.yellow);
	}
	
}
#endif