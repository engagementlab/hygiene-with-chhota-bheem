using System.Collections;
using UnityEngine;

public class VillagerObject : ArchetypeMove
{

//	public RawImage healthFill;

	private Camera _mainCamera;
	private SpriteRenderer _villagerRenderer;
	private Sprite[] _spriteFrames;
	
	[HideInInspector]
	public bool Spawned = false;
	
	public int ExitSpeed = 1;
	
	// Rate for reducing particle cloud on hit
	private float _rate;
	
	// Rate for sprite sheet frame swapping
	private int _frame;

	private Particles _particles;
	private ParticleSystem _particleSystem;
	private ParticleSystem.MainModule _main;
	private ParticleSystem.EmissionModule _emission;

	private bool _particleReady;

	private Vector3 _toPosition;
	
	private IEnumerator RemoveVillager()
	{
				
		_particleSystem.Clear();
		_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		_emission.enabled = false;
		
		yield return new WaitForEndOfFrame();
		
		SpawnSpellComponent();

		var targetX = 500;
		var targetY = -100;

		if(gameObject.transform.position.x > 0)
		{
			targetX = -500;
			targetY = 100;
		}

		_toPosition = new Vector3(gameObject.transform.position.x + targetX, gameObject.transform.position.y + targetY, 0);
		
		var distance = Vector3.Distance(_toPosition, transform.position);
		
		iTween.Stop(gameObject);
		iTween.MoveTo(gameObject, iTween.Hash("position", _toPosition, "time", distance/ExitSpeed, "easetype", iTween.EaseType.linear, "oncomplete", "CompleteWalkOff"));

	}

	private void CompleteWalkOff()
	{
		Destroy(gameObject);
	}

	// Use this for initialization
	private void Awake () {
		
		base.Awake();
		
		_mainCamera = Camera.main;
		_villagerRenderer = GetComponent<SpriteRenderer>();

		_particleReady = GetComponent<Particles>() != null;

		if (GetComponent<ParticleSystem>() != null)
		{
			_particleSystem = GetComponent<ParticleSystem>();
			_main = _particleSystem.main;
			_emission = _particleSystem.emission;
		}
		
		if(_particleReady)
			_particles = GetComponent<Particles>();
		
	}

	private void Start()
	{
		
		base.Start();
		
		// Pick random villager spritesheet and set to first frame
		_spriteFrames = Resources.LoadAll<Sprite>("Villagers/"+UnityEngine.Random.Range(1, 4));
		_villagerRenderer.sprite = _spriteFrames[0];
		_frame = _spriteFrames.Length / HitPoints;

		if (_particleSystem != null && _particleReady)
			_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		
		// Check if this was spawned, if not, add it to total count
		if (!Spawned)
			GameConfig.Multiplier++;
		
	}

	// Update is called once per frame
	private void Update () {
		
		base.Update();
			
		if(_mainCamera.WorldToViewportPoint(transform.position).y < -.5f)
			Destroy(gameObject);

		if (_particleReady && !IsDestroyed)
		{
			if (_particleSystem.isStopped)
			{
				_particles.PlayParticles(true);
				SetParticleRate();
			}
		}

	}

	private void SetParticleRate()
	{
		_rate = (_main.startSize.constant - _particles.Smallest) / HitPoints;
	}

	private void OnTriggerEnter(Collider collider) {
		
		if (IsDestroyed)
			return;
		
		if(collider.gameObject.tag != "Bubble") return;
		
		if(_bubblesHit < HitPoints-1)
		{
			_bubblesHit += _playerScript.Strength;
			
			if (_particleReady)
				_particles.ParticleReduce(_rate);

			if(_bubblesHit < HitPoints)
			{
				Events.instance.Raise(SoundEvent.WithClip(_playerScript.VillagerHitSounds[_bubblesHit]));
				_villagerRenderer.sprite = _spriteFrames[_frame * _bubblesHit];
			}
			return;
		}
		
		_villagerRenderer.sprite = _spriteFrames[_spriteFrames.Length - 1];
		
		Events.instance.Raise(SoundEvent.WithClip(_playerScript.UnhypnotizeSound));
		Events.instance.Raise (new ScoreEvent(pointsWorth));

		StartCoroutine(RemoveVillager());

		IsDestroyed = true;
		GameConfig.VillagersSaved++;

	}

}