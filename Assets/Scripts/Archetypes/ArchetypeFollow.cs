/*

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeFollow.cs
	Archetype class for objects that follow players.

	Created by Erica Salling, Johnny Richardson.
==============

*/

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ArchetypeFollow : ArchetypeMove
{
	[Range(1, 10)]
	public float FollowDistance = 2;
	[Range(1, 40)]	
	public float FollowDuration = 4;

	private Vector3 _playerPos;
	private Vector3 _thisPos;
	private Vector3 _velocity;
	private bool _chase = true;
	
	public void Awake() {

		base.Awake();
	}
	
	public void Update () {
		 
		// Do nothing outside camera
		if(!(MainCamera.WorldToViewportPoint(transform.position).y < 1)) return;

		if (_chase) {

			FollowDuration -= Time.deltaTime;

			if(FollowDuration < 0)
			{
				// Following done, animate normally
				_chase = false;
				AnimateOnlyInCamera = false;
				Animate();
				base.Update();
				
			} 
			else if(Player != null)
			{
				// Chase the Player 
				_playerPos = Player.transform.position;
				_thisPos = gameObject.transform.position;
				var distance = Vector3.Distance(_playerPos, _thisPos);

				// Divide distance by max value (10 is distance of 1) and then factor by current world distance, so we ramp smoothly
				gameObject.transform.position = Vector3.SmoothDamp(_thisPos, _playerPos, ref _velocity, FollowDistance/10*distance);

			}

		}

	}

}