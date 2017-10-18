﻿/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017

==============
	ArchetypeBoss.cs
	Archetype object for bosses, inheriting logic of archetype move.

	Created by Erica Salling, Johnny Richardson.
==============

*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArchetypeBoss : ArchetypeMove
{
	public GameObject[] Projectiles;
	
	[Range(0, 40)]
	[Tooltip("Time before tiled background stops moving")]
	public int BackgroundDelay;
	public Movements MovementType;
	public float Health = 2f;
	public float SmoothTime = .1f;
	
	[Range(0, 20)]
	public float ProjectileInterval;
	[Range(0, 10)]
	public float ProjectileSpeed = 5f;
	public ShootModes ShootMode;

	private GameObject _player;
	private GameObject _parent;

	private float _playerPos;
	private Vector3 _wizardPos;
	private RawImage HealthFill;
	
	public enum Movements
	{
		Avoid, 
		Follow, 
		Dodge
	}
	
	public enum ShootModes
	{
		Down, 
		AtPlayer, 
		Random
	}

	private float _intervalTime;
	private Vector3 _velocity;
	private bool _wait = true;

	private void Awake()
	{
		base.Awake();
		
		_player = GameObject.FindWithTag("Player");
		_parent = GameObject.FindWithTag("Parent");
		HealthFill = transform.Find("HP/Fill").GetComponent<RawImage>();

	}

	// Update is called once per frame
	private void Update () {
		
		base.Update();

		if(!(MainCamera.WorldToViewportPoint(transform.position).y < .9f)) return;

		if(_wait)
		{
			StartCoroutine(Spawned());
			_wait = false;
		}

		BossMove();

		if(Projectiles == null || Projectiles.Length < 1) return;
		
		if(_intervalTime >= ProjectileInterval) {

			_intervalTime = 0;
			
			// Choose Random projectile from list
			var random = Random.Range(0, Projectiles.Length);
			
			var projectilePos = transform.position;
			projectilePos.z = 0;
			Vector2 dir;

			// Change heading of projectile based on mode
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

			var moveComponent = projectile.GetComponent<ArchetypeMove>();
			if (moveComponent != null)
			{
				moveComponent.MoveSpeed = ProjectileSpeed;
				moveComponent.KillsPlayer = true;
			}
		}
		else
			_intervalTime += Time.deltaTime;
		
	}

	private IEnumerator Spawned()
	{
		yield return new WaitForSeconds(BackgroundDelay);
		_parent.GetComponent<ArchetypeMove>().MoveEnabled = false;

	}

	private void BossMove()
	{
		float height = 2f * Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;

		// Check player & wizard position
		_playerPos = _player.transform.position.x;
		_wizardPos = gameObject.transform.position;

		var distance = Vector3.Distance(_wizardPos, new Vector3(0f, _wizardPos.y, _wizardPos.z));

		switch(MovementType)
		{
			case Movements.Avoid:
				
				// move wizard away from bounds & player
				if(_wizardPos.x <= _playerPos && _wizardPos.x >= _playerPos - 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					else
						_wizardPos = new Vector3(_wizardPos.x - 2.0f, _wizardPos.y, _wizardPos.z);

				}
				else if(_wizardPos.x >= _playerPos && _wizardPos.x <= _playerPos + 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					else
						_wizardPos = new Vector3(_wizardPos.x + 2.0f, _wizardPos.y, _wizardPos.z);
				}

				break;

			case Movements.Follow:

				// move wizard away from bounds & towards player
				if(_wizardPos.x <= _playerPos && _wizardPos.x <= _playerPos - 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
					{
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					} else
					{
						_wizardPos = new Vector3(_wizardPos.x + 2.0f, _wizardPos.y, _wizardPos.z);
					}

				} else if(_wizardPos.x >= _playerPos && _wizardPos.x >= _playerPos + 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
					{
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					} else
					{
						_wizardPos = new Vector3(_wizardPos.x - 2.0f, _wizardPos.y, _wizardPos.z);
					}
				}

				break;

			case Movements.Dodge:

				// move wizard away from bounds & towards player

				if(_wizardPos.x <= _playerPos && _wizardPos.x <= _playerPos - 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
					{
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					} else
					{
						_wizardPos = new Vector3(_wizardPos.x + 2.0f, _wizardPos.y, _wizardPos.z);
					}

				} else if(_wizardPos.x >= _playerPos && _wizardPos.x >= _playerPos + 1.5f)
				{
					// Move Wizard
					if(distance >= width / 2.5f)
					{
						_wizardPos = new Vector3(0, _wizardPos.y, _wizardPos.z);
					} else
					{
						_wizardPos = new Vector3(_wizardPos.x - 2.0f, _wizardPos.y, _wizardPos.z);
					}
				}

				break;
		}

		gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, _wizardPos, ref _velocity, SmoothTime);
	}


	public void OnTriggerEnter(Collider collider)
	{

		if(_wait) return;
		if(collider.gameObject.tag != "Bubble") return;

		Events.instance.Raise(new HitEvent(HitEvent.Type.Spawn, collider, collider.gameObject));

		Vector2 v = HealthFill.rectTransform.sizeDelta;
		v.x += .5f;
		HealthFill.rectTransform.sizeDelta = v;

		if(!(Mathf.Abs(v.x - Health) <= .1f)) return;

		// Destroy Wizard
		iTween.ScaleTo(gameObject, Vector3.zero, 1.0f);
		StartCoroutine(DestroyWizard());

		// You won the game
		Events.instance.Raise(new ScoreEvent(1, ScoreEvent.Type.Wizard));
		Events.instance.Raise(new DeathEvent(true));

	}


	private IEnumerator DestroyWizard()
	{

		yield return new WaitForSeconds(1);
		Destroy(gameObject);
		
	}

}
