using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Analytics;

public class ArchetypePlayer : MonoBehaviour {

	public float SmoothTime = 0.1f;
	public float BubbleSpeed = 15;

	public int SpellStepCount;
	
	public GameObject Bubble;

	public int BubbleInitialStrength = 1;
	public float BubbleSpeedIncrease = 2f;
	public float BubbleSizeIncrease = 0.1f;
	public int BubbleStrengthIncrease = 1;

	public AudioClip[] BubbleSounds;
	public AudioClip GameEndSound;
	public AudioClip ObstacleSound;

	public bool WonGame;
	public bool Killed = false;

	[HideInInspector] 
	public int Strength;
	[HideInInspector]
	public bool PoweredUp;
	[HideInInspector]
	public Spells SpellsType;

	private GameObject _lastBubble;
	private Camera _mainCamera;
	
	private float _currentBadScore;
	private float _targetScore;
	private float _intervalTime;
	private float _bossSpawnDelta = 0;
	
	private bool _freeMovement = true;
	private bool _trailEnabled = true;
	private bool _mouseDrag;
	private bool _moveDelta;
	private bool _scatterShootOn;
		
	private int _scatterShoot;
	private int _speedShoot;
	private int _bigShoot;

	private Vector3 _velocity;
	private Vector3 _bubbleScale;
	private Vector3 _bubbleDefault;
	private SphereCollider _collider;
	private List<float> dirs;

	private Animator _playerAnimator;

	private Particles _particles;
	private GameObject _glow;

	/**************
		UNITY METHODS
	***************/
	private void Awake () {

		_mainCamera = Camera.main;

		Events.instance.AddListener<DeathEvent> (OnDeathEvent);
		Events.instance.AddListener<SpellEvent> (OnSpellEvent);
		Events.instance.AddListener<ScoreEvent> (OnScoreEvent);
		
		_bubbleScale = new Vector3(0.12F, 0.12F, 0.22F);
		_bubbleDefault = _bubbleScale;
		
		var currentRect = GetComponent<RectTransform>().position;
		currentRect.z = -.5f;
		GetComponent<RectTransform>().position = currentRect;

		Strength = BubbleInitialStrength;

		_playerAnimator = GetComponent<Animator>();

		_particles = gameObject.GetComponent<Particles>();

		_glow = transform.Find("Glow").gameObject;
		_glow.SetActive(false);

	}

	private void Update()
	{

		if (Killed)
			return;
		
		if(GameConfig.SlowMo || GameConfig.GamePaused)
		{
			if(GameConfig.GamePaused)
			{
				_playerAnimator.speed = 0;
//				_particles.ParticleSystem.Pause();
			}
			else
//				_particles.ParticleSystem.Play();
			
			return;
		}
		_playerAnimator.speed = 1;

		// Get input pos via mouse/touch
		#if UNITY_EDITOR
			var inputPosition = Input.mousePosition;
		#else
			var inputPosition = Input.GetTouch(0).position;
		#endif
				
		var targetPosition = new Vector3(_mainCamera.ScreenToWorldPoint(inputPosition).x, _mainCamera.ScreenToWorldPoint(inputPosition).y + GameConfig.BubbleOffset, -.5f);
		transform.position = Utilities.ClampToScreen(Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, SmoothTime), _mainCamera);

		if(_currentBadScore < _targetScore) 
			_currentBadScore += _targetScore/20;
		
		if(_intervalTime >= GameConfig.NumBubblesInterval) {

			_intervalTime = 0;
			
			var projectilePos = transform.position;
			projectilePos.z = 0;

			if(!_scatterShootOn)
			{
				var dir = new Vector2(0, 1);
				dir.Normalize();

				var projectile = Instantiate(Bubble, projectilePos, Quaternion.identity);
				projectile.GetComponent<ArchetypeProjectile>().Initialize(_bubbleScale, dir * BubbleSpeed);
			} 
			else
			{
				dirs = new List<float>();
				// Spawn n bubbles in different directions for scatter shot
				ScatterDirs(_scatterShoot);
				int bubbleCount = (_scatterShoot * 2) + 1;
				for(int bubIndex = 0; bubIndex < bubbleCount; bubIndex++)
				{
					var projectile = Instantiate(Bubble, projectilePos, Quaternion.identity);
					projectile.GetComponent<ArchetypeProjectile>().Initialize(_bubbleScale, new Vector2(dirs[bubIndex], 1).normalized * BubbleSpeed);
				}	

			}

		}
		else
			_intervalTime += Time.deltaTime;
	  
	}

	private void ScatterDirs(float n)
	{
		float p = (2 * n) + 1; // number of breaks (bubbles)
		float c = 1 / (n + 1); // find the decimal breaks
		
		for (int i = 1; i <= p; i++)
		{
			float a = i * c;
			if (i == n + 1)
			{
				a = 0; // The middle is always zero
			} 
			else if (a == 1)
			{
				a = -c * n; 
			}
			else if (a > 1)
			{
				a = a - 1;
			}
			
			if (i > n + 1)
			{
				a = -a;
			} 
			dirs.Add(a);
			
		}
		
		
	}

	private void OnDestroy() {

		Events.instance.RemoveListener<DeathEvent> (OnDeathEvent);
		Events.instance.RemoveListener<SpellEvent> (OnSpellEvent);
		Events.instance.RemoveListener<ScoreEvent> (OnScoreEvent);

	}

	private void OnDisable()
	{
		
		Events.instance.RemoveListener<DeathEvent> (OnDeathEvent);
		Events.instance.RemoveListener<SpellEvent> (OnSpellEvent);
		Events.instance.RemoveListener<ScoreEvent> (OnScoreEvent);
		
	}
	

	private void OnEnable()
	{

		Events.instance.AddListener<DeathEvent> (OnDeathEvent);
		Events.instance.AddListener<SpellEvent> (OnSpellEvent);
		Events.instance.AddListener<ScoreEvent> (OnScoreEvent);
		
	}

	/**************
		CUSTOM METHODS
	***************/

  	private void OnScoreEvent(ScoreEvent e) {

		GameConfig.UpdateScore(e.scoreAmount);

	}
	
	private void OnSpellEvent(SpellEvent e)
	{		
		SpellsType = e.powerType;

		if (e.powerUp)
		{
			// Spell ON
			SpellComplete(SpellsType);

			switch(SpellsType)
			{
				case Spells.SpeedShoot:
					// Speed up bubble rate
					
					if (_speedShoot <= 0)
						PoweredUp = true;
					
					GameConfig.NumBubblesInterval /= BubbleSpeedIncrease;
					_speedShoot++;
					
					break;
				case Spells.ScatterShoot:
					// Make those bubbles scatter
					
					if (_scatterShoot <= 0)
					{
						_scatterShootOn = true;
						PoweredUp = true;
					}
					
					_scatterShoot++;
					
					break;
					
				case Spells.BigShoot:

					// Make those bubbles bigger
					if (_bigShoot <= 0)
					{
						PoweredUp = true;
					}
					
					_bubbleScale += new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
					Strength += BubbleStrengthIncrease;
					
					_bigShoot++;
					
					break;
			}
		}
		else
		{
			
			// Spell OFF
			switch(SpellsType)
			{
				case Spells.SpeedShoot:
					if (_speedShoot <= 0)
					{
						_particles.ParticleControl(false, SpellsType);
						PoweredUp = false;
					}
					else
						_speedShoot--;
					
					GameConfig.NumBubblesInterval *= BubbleSpeedIncrease;

					break;
				case Spells.ScatterShoot:

					if (_scatterShoot <= 0)
					{
						_scatterShootOn = false;
						PoweredUp = false;
						_particles.ParticleControl(false, SpellsType);
					}
					else
						_scatterShoot--;
					
					break;
					
				case Spells.BigShoot:

					if (_bigShoot <= 0)
					{
						PoweredUp = false;
						_particles.ParticleControl(false, SpellsType);
						_bubbleScale = _bubbleDefault;
						Strength = BubbleInitialStrength;
					}
					else
					{
						_bigShoot--;
						_bubbleScale -= new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
						Strength -= BubbleStrengthIncrease;
					}



					break;
			}
		}
		
	}

	
	private void SpellComplete(Spells spell)
	{
		var animations = 0;

		GameConfig.GamePaused = true;
		GameConfig.GameSpeedModifier = 0;
		
		// TO DO - Spell Steps - When assets are ready
		StartCoroutine(GUIManager.Instance.ShowSpellActivated());
		
		GameConfig.GamePaused = false;
		GameConfig.GameSpeedModifier = 15;
		
		// Send Particles
		GlowControl(true, SpellsType);

	}

	private void GlowControl(bool on, Spells type)
	{

		if (on)
		{
			switch (type)
			{
				case Spells.BigShoot:

					_glow.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 1f, 0.5f);
					break;
					
				case Spells.SpeedShoot: 
					_glow.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.5f);
					break;
					
				case Spells.ScatterShoot: 
					_glow.GetComponent<SpriteRenderer>().color = new Color(1f, 0.92f, 0.016f, 0.5f);
					break;
			}
			
			_glow.SetActive(true);
		}
		else
		{
			_glow.SetActive(false);
		}
		
	}
 
	private void OnDeathEvent(DeathEvent e)
	{
		WonGame = e.wonGame;

		gameObject.SetActive(false);
		GameConfig.GameOver = true;
		GameConfig.GameWon = WonGame;
		
		GUIManager.Instance.GameEnd(WonGame);
		
		// Send Player Data to Analytics
		Analytics.CustomEvent("gameEnd",
			new Dictionary<string, object>
			{{ "gameState", WonGame }, { "time", Time.timeSinceLevelLoad }}
		);
		
	}
	
  
}
