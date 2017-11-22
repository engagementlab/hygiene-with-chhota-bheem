using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class VillagerObject : ArchetypeMove
{

//	public RawImage healthFill;

	private Camera _mainCamera;
	private SpriteRenderer _villagerRenderer;
	private Sprite[] _spriteFrames;
	private bool _spawned;
	
	// Rate for reducing particle cloud on hit
	private float _rate;

	private ParticleSystem _particles;
	private ParticleSystem.MainModule _main;

	private bool _particleReady;
	private int _animSpeed = 1;

	private Vector3 _toPosition;
	
	private IEnumerator RemoveVillager()
	{
		yield return new WaitForEndOfFrame();
		
		SpawnSpellComponent();

		if (gameObject.transform.position.x > 0)
		{
			_toPosition = new Vector3(gameObject.transform.position.x + 500, gameObject.transform.position.y - 100, 0);
		}
		else
		{
			_toPosition = new Vector3(gameObject.transform.position.x - 500, gameObject.transform.position.y - 100, 0);

		}
		
		_particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		GetComponent<Particles>().PlayParticles(false);
		
		var distance = Vector3.Distance(_toPosition, transform.position);
		iTween.MoveTo(gameObject, iTween.Hash("position", _toPosition, "time", distance/_animSpeed, "easetype", iTween.EaseType.linear, "oncomplete", "CompleteWalkOff"));

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
		
		if (!_spawned)
			GameConfig.Multiplier++;
		
		_particleReady = GetComponent<Particles>() != null;

		if (gameObject.GetComponent<ParticleSystem>() != null)
		{
			_particles = gameObject.GetComponent<ParticleSystem>();
			_main = _particles.main;
		}

	}

	private void Start()
	{
		
		base.Start();
		
		// Pick random villager spritesheet and set to first frame
		_spriteFrames = Resources.LoadAll<Sprite>("Villagers/"+UnityEngine.Random.Range(1, 4));
		_villagerRenderer.sprite = _spriteFrames[0];
		
		if (GetComponent<ParticleSystem>() != null && _particleReady)
			_particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		
	}

	// Update is called once per frame
	private void Update () {
		
		base.Update();
			
		if(_mainCamera.WorldToViewportPoint(transform.position).y < -.5f)
			Destroy(gameObject);

		if (_particleReady)
		{
			if (_particles.isStopped)
			{
				GetComponent<Particles>().PlayParticles(true);
				SetParticleRate();
			}
		}

	}

	private void SetParticleRate()
	{
		_rate = (_main.startSize.constant - GetComponent<Particles>().Smallest) / HitPoints;
	}

	private void OnTriggerEnter(Collider collider) {
		
//		base.OnTriggerEnter(collider);
		if (IsDestroyed)
			return;
		
		if(collider.gameObject.tag != "Bubble") return;
		
		Destroy(collider.gameObject);
		if(_bubblesHit < HitPoints-1)
		{
			_bubblesHit += _playerScript.Strength;
			
			if (_particleReady)
				GetComponent<Particles>().ParticleReduce(_rate);

			if(_bubblesHit < _spriteFrames.Length)
			{
				bool hiSound = Random.value > .5f;
				Events.instance.Raise(SoundEvent.WithClip(_playerScript.BubbleSounds[hiSound?0:1]));
				_villagerRenderer.sprite = _spriteFrames[_bubblesHit];
			}
			return;
		}
		
		Events.instance.Raise (new ScoreEvent(pointsWorth));

		StartCoroutine(RemoveVillager());

		IsDestroyed = true;
		GameConfig.VillagersSaved++;

	}

}