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
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ArchetypeBoss : ArchetypeMove
{
	public GameObject[] Projectiles;

	public AudioClip BossMusic;
	public AudioClip BossDeathClip;
	
	[Range(0, 1000)]
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

	private GameObject _parent;

	private float _playerPos;
	private float _playerHits;
	private Vector3 _wizardPos;
	private Image HealthFill;
	
	private bool _lifeLossRunning;
	
	public enum Movements
	{
		AvoidPlayerPosition, 
		Follow, 
		DodgeProjectiles
	}
	
	public enum ShootModes
	{
		Down, 
		AtPlayer, 
		Random
	}

	private float _intervalTime;
	private float _startingHpWidth;
	private Vector3 _velocity;
	private bool _wait = true;
	private bool _musicPlayed;
	private int _playerStrength;

	private SpriteRenderer _sprite;
	
	private void Awake()
	{
		
		base.Awake();
		_parent = GameObject.FindWithTag("Parent");
		
		HealthFill = transform.Find("Canvas/HP BG/HP").GetComponent<Image>();
		_startingHpWidth = HealthFill.fillAmount;

		_sprite = GetComponent<SpriteRenderer>();

		_lifeLossRunning = false;
	}

	// Update is called once per frame
	private void Update () {
		
		// Sanity check
		base.Update();
		var position = MainCamera.WorldToViewportPoint(transform.position).y;

		if(position < 2 && !_musicPlayed)
		{
			// Fade in boss music	
			var fileName = "chapter_" + System.Convert.ToInt32(GameConfig.CurrentChapter + 1) + "_boss";
			Events.instance.Raise(new SoundEvent(fileName, SoundEvent.SoundType.Music, null, 1, 1, true));
			_musicPlayed = true;
		}
		
		// Do nothing before in view
		if(!(position < .87f)) return;
		// Paused/over?
		if (GameConfig.GamePaused || GameConfig.GameOver) return;
		
		if(MoveEnabled) MoveEnabled = false;
		
		// Put at root of scene so it stays still
		if(transform.parent != null)
			transform.parent = null;
		

		if(_wait)
		{
			StartCoroutine(Spawned());
			_wait = false;
		}

		// Movement pattern call
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
					var heading = _player.transform.position - transform.position;
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

			// Create projectile and give it a velocity
			var projectile = Instantiate(Projectiles[random], projectilePos, Quaternion.identity);
			projectile.GetComponent<Rigidbody>().velocity = dir * ProjectileSpeed;

			// Make projectile always kill player
			var moveComponent = projectile.GetComponent<ArchetypeMove>();
			if (moveComponent != null)
				moveComponent.KillsPlayer = true;
		
		}
		else
			_intervalTime += Time.deltaTime;
		
	}

	public void OnTriggerEnter(Collider collider)
	{

		if(_wait) return;
		
		base.OnTriggerEnter(collider);
		if(collider.gameObject.tag != "Bubble") return;

		collider.gameObject.GetComponent<SphereCollider>().enabled = false;

		Events.instance.Raise(new HitEvent(HitEvent.Type.Spawn, collider, collider.gameObject));
		Events.instance.Raise(SoundEvent.WithClip(_playerScript.FightSounds[Random.Range(0, _playerScript.FightSounds.Length-1)]));

		float amtHit = _startingHpWidth / (Health / _playerScript.Strength);
		
		HealthFill.fillAmount -= amtHit;
		_playerHits += _playerScript.Strength;
		
		// Adjust health bar and stop unless boss is dead
//		HealthFill.sizeDelta = v;
		if(!(Health - _playerHits <= .1f)) return;

		// Destroy Wizard
		StartCoroutine(DestroyWizard());

		

	}

	// Disable boss' prior parent from moving after given delay
	private IEnumerator Spawned()
	{
		yield return new WaitForSeconds(BackgroundDelay);
		_parent.GetComponent<ArchetypeMove>().MoveEnabled = false;

	}

	private void BossMove()
	{
	
		// Check player & wizard position
		_playerPos = _player.transform.position.x;
		_wizardPos = gameObject.transform.position;
		var currentSmoothTime = SmoothTime;

		switch(MovementType)
		{
			case Movements.AvoidPlayerPosition:

				// Gradually go to inverse of player x
				_wizardPos = new Vector3(-_playerPos, _wizardPos.y, _wizardPos.z);
				currentSmoothTime *= 4;				
				
				break;

			case Movements.Follow:

				// Follow player on x
				_wizardPos = new Vector3(_playerPos, _wizardPos.y, _wizardPos.z);
				currentSmoothTime *= 2;

				break;

			case Movements.DodgeProjectiles:

				// Move away from current nearest player projectile
				// Find closest bubbles
				Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2).ToList().Select(c => c.GetComponent<Collider>())
																																											 .Where(c => c.tag == "Bubble").OrderBy(c => c.transform.position.y).ToArray();
				if(hitColliders.Length == 0) return;
				var xPos = hitColliders[0].transform.position.x;
				var target = xPos - 3;
				if(xPos < 0)
					target = xPos + 3;
				
				_wizardPos = new Vector3(target, _wizardPos.y, _wizardPos.z);

				break;
		}

		gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, _wizardPos, ref _velocity, currentSmoothTime);
		
	}

	private IEnumerator DestroyWizard()
	{
		if(_lifeLossRunning)
			yield return false;
		
		_lifeLossRunning = true;
		GameConfig.GameOver = true;
		int times;
		
		Events.instance.Raise(new SoundEvent(null, SoundEvent.SoundType.SFX, BossDeathClip, 1, 1, true));
		
		for (times = 0; times < 8; times++)
		{
			_sprite.color = Color.red;

			yield return new WaitForSeconds(0.1f);
			_sprite.color = Color.clear;
			 		
			yield return new WaitForSeconds(0.1f);
			_sprite.color = Color.white;

			if(times == 7)
			{
				iTween.ScaleTo(gameObject, Vector3.zero, 1.0f);
				
				yield return new WaitForSeconds(1.0f);
				
				Destroy(gameObject);
				_lifeLossRunning = false;
				
				// You won the game
				Events.instance.Raise(new ScoreEvent(pointsWorth));
				Events.instance.Raise(new GameEndEvent(true));
			}

		}
		
		
	}

}
