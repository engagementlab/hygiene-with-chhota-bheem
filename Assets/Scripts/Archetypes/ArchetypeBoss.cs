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
	public GameObject[] Projectiles;
	public float ProjectileInterval;
	public float ProjectileSpeed = 5f;
	public ShootModes ShootMode;

	public enum ShootModes
	{
		Down, 
		AtPlayer, 
		Random
		
	}

	private float _intervalTime;
	private Vector3 _velocity;

	private void Awake()
	{
		base.Awake();
	}

	// Update is called once per frame
	private void Update () {
		
		base.Update();

		if (!gameObject.GetComponent<ArchetypeWizard>().spawned || Projectiles != null || Projectiles.Length < 1) return;

		if(_intervalTime >= ProjectileInterval) {

			_intervalTime = 0;
			
			// Choose Random projectile from list
			var random = Random.Range(0, Projectiles.Length);
			
			var projectilePos = transform.position;
			projectilePos.z = 0;
			Vector2 dir;

			switch (ShootMode)
			{
					case ShootModes.Down:
						dir = new Vector2(0, -1);
						dir.Normalize();

						break;
						
					case ShootModes.AtPlayer:
						var heading = Player.transform.position - transform.position;
						var distance = heading.magnitude;
						dir = heading / distance;
						
						break;
						
					case ShootModes.Random:
						dir = new Vector2(0, -1);
						dir.Normalize();

						break;
						
						default:
							dir = new Vector2(-1, 0);
							break;
			}

			var projectile = Instantiate(Projectiles[random], projectilePos, Quaternion.identity);
			projectile.GetComponent<Rigidbody>().velocity = dir * ProjectileSpeed;

			if (projectile.GetComponent<ArchetypeMove>() != null)
			{
				projectile.GetComponent<ArchetypeMove>().MoveSpeed = ProjectileSpeed;
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
