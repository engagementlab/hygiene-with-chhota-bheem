/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeBoss.cs
	Archetype object for bosses, inheriting logic of spawner.

	Created by Erica Salling.
==============

*/
using UnityEngine;

public class ArchetypeBoss : ArchetypeSpawner
{
	public GameObject[] projectiles;
	public float projectileInterval;
	public float projectileSpeed = 5f;
	public ShootModes shootMode;

	public enum ShootModes
	{
		down, 
		atPlayer, 
		random
		
	}

	private GameObject _player;
	private float _intervalTime;
	private Vector3 _velocity;

	private void Awake()
	{
		base.Awake();
	}

	// Update is called once per frame
	private void Update () {
		
		base.Update();

		if (!gameObject.GetComponent<ArchetypeWizard>().spawned) return;

		if(_intervalTime >= projectileInterval) {

			_intervalTime = 0;
			
			// Choose Random projectile from list
			var random = Random.Range(0, projectiles.Length);
			
			var projectilePos = transform.position;
			projectilePos.z = 0;
			Vector2 dir;

			switch (shootMode)
			{
					case ShootModes.down:
						dir = new Vector2(0, -1);
						dir.Normalize();

						break;
						
					case ShootModes.atPlayer:
						var heading = _player.transform.position - transform.position;
						var distance = heading.magnitude;
						dir = heading / distance;
						
						break;
						
					case ShootModes.random:
						dir = new Vector2(0, -1);
						dir.Normalize();

						break;
						
						default:
							dir = new Vector2(-1, 0);
							break;
			}

			var projectile = Instantiate(projectiles[random], projectilePos, Quaternion.identity);
			projectile.GetComponent<Rigidbody>().velocity = dir * projectileSpeed;

			if (projectile.GetComponent<ArchetypeMove>() != null)
			{
				projectile.GetComponent<ArchetypeMove>().MoveSpeed = projectileSpeed;
				projectile.GetComponent<ArchetypeMove>().KillsPlayer = true;
			}
		}
		else
			_intervalTime += Time.deltaTime;
		
	}

	private void Projectile()
	{
	
		
		
	}
	
	
}
