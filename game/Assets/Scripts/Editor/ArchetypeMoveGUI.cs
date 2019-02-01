/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeMoveGUI.cs
	Custom editor inspector for all objects using/inheriting ArchetypeMove component.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/Editor/ArchetypeMoveGUI.cs

	Created by Johnny Richardson.
==============

*/

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArchetypeMove), true)]
public class ArchetypeMoveGUI : Editor
{
	
	public override void OnInspectorGUI()
	{
		
		var _archetype = (ArchetypeMove)target;

		if(_archetype.transform.parent != null)
		{
			string helpTxt = "This archetype has a parent and will be affected by the parent's speed.";

			if(_archetype.HasWaypoints())
				helpTxt += "\nIt also has animation waypoints, and so will use movement direction of parent.";
			
			EditorGUILayout.HelpBox(helpTxt, MessageType.Info);
		}
		
		_archetype.MoveEnabled = EditorGUILayout.Toggle("Move Enabled", _archetype.MoveEnabled);
		_archetype.KillsPlayer = EditorGUILayout.Toggle("Kills Player", _archetype.KillsPlayer);
		
		_archetype.SpellRandom = EditorGUILayout.Toggle("Give Random Spell", _archetype.SpellRandom);
		if(!_archetype.SpellRandom)
			_archetype.SpellGiven = (Spells)EditorGUILayout.EnumPopup("Spell Given", _archetype.SpellGiven);
		
		if(_archetype.transform.parent != null)
			_archetype.UseParentSpeed = EditorGUILayout.Toggle("Use Parent's Speed", _archetype.UseParentSpeed);

		_archetype.LeaveParentInCamera = EditorGUILayout.Toggle("Leave Parent Once In View", _archetype.LeaveParentInCamera);
		
		if(!_archetype.MoveEnabled)
		{
			_archetype.MoveOnceInCamera = EditorGUILayout.Toggle("Move Once In View", _archetype.MoveOnceInCamera);
			if(_archetype.MoveOnceInCamera || _archetype.LeaveParentInCamera)
				_archetype.MoveDelay = EditorGUILayout.Slider("Move Delay", _archetype.MoveDelay, 0, 10);
		}

		if(!_archetype.UseParentSpeed || _archetype.transform.parent == null)
			_archetype.MoveSpeed = EditorGUILayout.Slider("Movement Speed", _archetype.MoveSpeed, 0, 10);
		
		// Player can kill bool
		if(target.GetType().ToString() != "ArchetypeBoss")
		{
			_archetype.PlayerCanKill = EditorGUILayout.Toggle("Player Can Kill/Remove", _archetype.PlayerCanKill);
			if(_archetype.PlayerCanKill)
				_archetype.HitPoints = EditorGUILayout.IntSlider("Hit Points", _archetype.HitPoints, 1, 10);
		}
		
		// Draw the default inspector
		DrawDefaultInspector();
		
		// Animation
		if(_archetype.HasWaypoints()) {
			
			GUILayout.BeginVertical("box");
			
			EditorGUILayout.HelpBox("This sets the duration of the journey between start and end waypoints, the type of animation, and speed scaling for upward/downward tween.", MessageType.None);
			_archetype.AnimationDuration = EditorGUILayout.Slider("Animation Duration", _archetype.AnimationDuration, 1, 10);
			_archetype.AnimationType = (ArchetypeMove.AnimType) EditorGUILayout.EnumPopup("Animation Type", _archetype.AnimationType);

			// Animation speed controls
			_archetype.AnimationUpwardSpeed = EditorGUILayout.Slider("Up Speed", _archetype.AnimationUpwardSpeed, .01f, 2);
			_archetype.AnimationDownwardSpeed = EditorGUILayout.Slider("Down Speed", _archetype.AnimationDownwardSpeed, .01f, 2);

			_archetype.RotateOnWaypoints = EditorGUILayout.ToggleLeft("Rotate Along Waypoints", _archetype.RotateOnWaypoints);

			if(_archetype.AnimationType == ArchetypeMove.AnimType.Once)
				_archetype.DestroyOnEnd = EditorGUILayout.ToggleLeft("Delete When Done", _archetype.DestroyOnEnd);
			else
				_archetype.DestroyOnEnd = false;
			
			GUILayout.EndVertical();
			
		}
		
		// Movement Direction
		if(_archetype.transform.parent == null || !_archetype.HasWaypoints())
			_archetype.MovementDir = (ArchetypeMove.Dirs) EditorGUILayout.EnumPopup("Movement Direction", _archetype.MovementDir);
	
		if(GUILayout.Button("Add Waypoint"))
			_archetype.AddWaypoint();

		// Save the changes back to the object
		EditorUtility.SetDirty(target);

	}
	
	public virtual void OnSceneGUI()
	{
		
		if(!SceneEditor.ShowGizmos || Application.isPlaying) return;
		
		var waypointChildren = new List<Transform>();

		foreach(Transform t in ((ArchetypeMove)target).transform)
		{
			if(t.tag == "Waypoint")
				waypointChildren.Add(t);
		}

		Handles.color = Color.cyan;
		foreach(var waypoint in waypointChildren)
		{
			waypoint.transform.position = Handles.FreeMoveHandle(waypoint.transform.position,
				Quaternion.identity,
				0.2f,
				Vector3.zero, 
				Handles.CylinderHandleCap);
		
			Undo.RecordObject(waypoint.transform, "Waypoint Position");
		}

	}
    
}
#endif