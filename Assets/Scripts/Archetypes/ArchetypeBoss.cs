/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeBoss.cs
	Archetype object that spawns other archetype once entering camera; inherits ArchetypeMove.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/Editor/ArchetypeSpawner.cs

	Created by Johnny Richardson.
==============

*/
using UnityEngine;

public class ArchetypeBoss : ArchetypeSpawner
{
	public GameObject[] projectiles;
	public float projectileInterval;
	public float projectileSpeed = 100f;
	public float SmoothTime = 0.1f;

	public int health;

	private GameObject _player;
	
	private float _intervalTime;
	private Camera _mainCamera;
	private Vector3 _velocity;

	private void Awake()
	{
		_mainCamera = Camera.main;
		_player = GameObject.FindGameObjectWithTag("Player");

		base.Awake();

	}

	// Update is called once per frame
	private void Update () {
		
//		var targetPosition = new Vector3(_player.transform.position.x, _player.transform.position.y, -.5f);
//		transform.position = Utilities.ClampToScreen(Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, SmoothTime), _mainCamera);
		base.Update();

		if(_intervalTime >= projectileInterval) {

			_intervalTime = 0;
			
			// Choose Random projectile from list
//			var random = Random.Range(0, projectiles.Length);
//			
//			var projectilePos = transform.position;
//			projectilePos.z = 0;
//			var dir = new Vector2(_player.transform.position.x, _player.transform.position.y);
//			dir.Normalize();
//
//			var projectile = Instantiate(projectiles[random], projectilePos, Quaternion.identity);
//			projectile.GetComponent<Rigidbody>().velocity = dir * projectileSpeed;

		}
		else
			_intervalTime += Time.deltaTime;
		
	}

	private void Projectile()
	{
	
		
		
	}
	
	
}
