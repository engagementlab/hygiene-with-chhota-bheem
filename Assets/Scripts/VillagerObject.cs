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
	
	private IEnumerator RemoveVillager()
	{
		yield return new WaitForEndOfFrame();
		
		SpawnSpellComponent();
		
		Destroy(gameObject);
	}

	// Use this for initialization
	private void Awake () {
		
		base.Awake();
		
		_mainCamera = Camera.main;
		_villagerRenderer = GetComponent<SpriteRenderer>();
		
		if (!_spawned)
			GameConfig.Multiplier++;

		if (gameObject.GetComponent<ParticleSystem>() == null)
			gameObject.AddComponent<ParticleSystem>();
		
		_particles = gameObject.GetComponent<ParticleSystem>();
		_main = _particles.main;

	}

	private void Start()
	{
		
		base.Start();
		
		// Pick random villager spritesheet and set to first frame
		_spriteFrames = Resources.LoadAll<Sprite>("Villagers/"+UnityEngine.Random.Range(1, 4));
		_villagerRenderer.sprite = _spriteFrames[0];
		
		_particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		
	}

	// Update is called once per frame
	private void Update () {
		
		base.Update();
			
		if(_mainCamera.WorldToViewportPoint(transform.position).y < -.5f)
			Destroy(gameObject);

		if (IsInView && gameObject.GetComponent<ParticleSystem>().isStopped)
		{
			gameObject.GetComponent<Particles>().PlayParticles(true);
			SetParticleRate();
		}

	}

	private void SetParticleRate()
	{
		_rate = (_main.startSize.constant - gameObject.GetComponent<Particles>().Smallest) / HitPoints;
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
			gameObject.GetComponent<Particles>().ParticleReduce(_rate);

			if(_bubblesHit < _spriteFrames.Length)
			{
				bool hiSound = Random.value > .5f;
				Events.instance.Raise(SoundEvent.WithClip(_playerScript.BubbleSounds[hiSound?0:1]));
				_villagerRenderer.sprite = _spriteFrames[_bubblesHit];
			}
			return;
		}
		
		iTween.ScaleTo(gameObject, Vector3.zero, 1f);
		Events.instance.Raise (new ScoreEvent(pointsWorth));

		StartCoroutine(RemoveVillager());

		IsDestroyed = true;
		GameConfig.VillagersSaved++;

	}

}