using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ArchetypePlayer : MonoBehaviour {

	public float SmoothTime = 0.1f;
	public float BubbleSpeed = 15;

	public int SpellStepCount;
	
	public GameObject Bubble;

	public int BubbleInitialStrength = 1;
	public int BubbleStrengthIncrease = 1;
	public float BubbleSpeedIncrease = 2f;
	public float BubbleSizeIncrease = 0.1f;

	public AudioClip[] BubblePopSounds;
	public AudioClip[] VillagerHitSounds;
	public AudioClip[] FightSounds;
	public AudioClip UnhypnotizeSound;
	public AudioClip GameEndSound;
	public AudioClip ObstacleSound;
	public AudioClip DeathClip;
	public AudioClip WinClip;


	public bool WonGame;
	public bool Killed;

	public float DieSpeed = 15f;

	[HideInInspector] 
	public int Strength;
	[HideInInspector]
	public Spells SpellsType;

	public bool PoweredUp => _powerUpState > 0;

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
	
	[HideInInspector]
	public bool LifeLossRunning;
		
	private int _scatterShoot;
	private int _speedShoot;
	private int _bigShoot;
	private int _powerUpState;
	
	private int _initialShootSpeed;

	private Vector3 _velocity;
	private Vector3 _bubbleScale;
	private Vector3 _bubbleDefault;
	private SphereCollider _collider;
	private List<float> _dirs;

	private Animator _playerAnimator;
	private SpriteRenderer _sprite;
	private Particles _particles;
	private GameObject _glow;

	private PowerUpUnderlay _underlay;

	private GameManager _gameManager;
	
	/**************
		UNITY METHODS
	***************/
	private void Awake () {

		_mainCamera = Camera.main;

		Events.instance.AddListener<GameEndEvent> (OnDeathEvent);
		Events.instance.AddListener<SpellEvent> (OnSpellEvent);
		Events.instance.AddListener<ScoreEvent> (OnScoreEvent);
		
		_bubbleScale = new Vector3(0.12F, 0.12F, 0.22F);
		_bubbleDefault = _bubbleScale;
		
		var currentRect = GetComponent<RectTransform>().position;
		currentRect.z = -.5f;
		GetComponent<RectTransform>().position = currentRect;

		Strength = BubbleInitialStrength;

		_playerAnimator = GetComponent<Animator>();
		_sprite = GetComponent<SpriteRenderer>();
		_particles = GetComponent<Particles>();
		
		_underlay = Instantiate(Resources.Load<PowerUpUnderlay>("PowerUpUnderlay"), Vector3.zero, Quaternion.identity);
		_underlay.transform.parent = transform;
		_underlay.transform.localPosition = Vector3.zero;

		_gameManager = Camera.main.GetComponent<GameManager>();
		
	}

	private void Update()
	{

		if (Killed || GameConfig.GameOver)
			return;
		
		if(GameConfig.SlowMo || GameConfig.GamePaused)
		{
			if(GameConfig.GamePaused)
				_playerAnimator.speed = 0;
			
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
				_dirs = new List<float>();
				// Spawn n bubbles in different directions for scatter shot
				ScatterDirs(_scatterShoot);
				int bubbleCount = (_scatterShoot * 2) + 1;
				for(int bubIndex = 0; bubIndex < bubbleCount; bubIndex++)
				{
					var projectile = Instantiate(Bubble, projectilePos, Quaternion.identity);
					projectile.GetComponent<ArchetypeProjectile>().Initialize(_bubbleScale, new Vector2(_dirs[bubIndex], 1).normalized * BubbleSpeed);
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
			_dirs.Add(a);
			
		}
		
		
	}

	private void OnDestroy() {

		Events.instance.RemoveListener<GameEndEvent> (OnDeathEvent);
		Events.instance.RemoveListener<SpellEvent> (OnSpellEvent);
		Events.instance.RemoveListener<ScoreEvent> (OnScoreEvent);

	}

	private void OnDisable()
	{
		
		Events.instance.RemoveListener<GameEndEvent> (OnDeathEvent);
		Events.instance.RemoveListener<SpellEvent> (OnSpellEvent);
		Events.instance.RemoveListener<ScoreEvent> (OnScoreEvent);
		
	}
	

	private void OnEnable()
	{

		Events.instance.AddListener<GameEndEvent> (OnDeathEvent);
		Events.instance.AddListener<SpellEvent> (OnSpellEvent);
		Events.instance.AddListener<ScoreEvent> (OnScoreEvent);
		
	}

	/**************
		CUSTOM METHODS
	***************/

	// For resetting at new level
	public void ResetLevel()
	{
		
		// Reset Spell counts
		_bigShoot = 0;
		_speedShoot = 0;
		_scatterShoot = 0;
		
		// Reset Shooting
		GameConfig.NumBubblesInterval = 0.5f;
		Strength = BubbleInitialStrength;
		
	}

	// Calls coroutine for player hit; allows caller to destroy immediately after 
	public void BeginPlayerHit(bool killed, string killerName=null)
	{
		StartCoroutine(PlayerHit(killed, killerName));
	}

	private IEnumerator PlayerHit(bool killed, string killerName=null)
	{
		if(!killed)
			_underlay.Subtract();

		if(LifeLossRunning)
			yield return false;
		
		LifeLossRunning = true;
		int times;
				
		// Flash player red
		for (times = 0; times < 4; times++)
		{
			_sprite.color = Color.red;

			yield return new WaitForSeconds(0.1f);
			_sprite.color = Color.clear;
			 		
			yield return new WaitForSeconds(0.1f);
			_sprite.color = Color.white;

			
			// Kill?
			if(times == 3)
			{
				StartCoroutine(PlayerLifeLoss(killed, killerName));
				LifeLossRunning = false;
			}

		}
				
	}
	
	private IEnumerator PlayerLifeLoss(bool die, string killerName=null)
	{
		
		// Do nothing if already dead
		if(Killed) yield return null;
		
		// Kill player
		if (die)
		{
			
			Killed = true;
			GameConfig.GameOver = true;

			if (!ReferenceEquals(transform.parent, null))
			{
				var toPosition = new Vector3(transform.position.x, transform.position.y - 500, 0);
				var distance = Vector3.Distance(toPosition, transform.position);
				
				iTween.Stop(gameObject);
				iTween.MoveTo(transform.parent.gameObject, iTween.Hash("position", toPosition, "time", distance/DieSpeed, "easetype", iTween.EaseType.linear));
			}

			Events.instance.Raise(SoundEvent.WithClip(ObstacleSound));

			yield return new WaitForSeconds(1f);

			Events.instance.Raise(new GameEndEvent(false, killerName));

		}
		else
		{
			OnSpellEvent(new SpellEvent(SpellsType, false));
			yield return new WaitForSeconds(.1f);	
		}
	}
	
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

					if(_speedShoot <= 0)
						_underlay.Setup(Spells.SpeedShoot);
					
					_underlay.Add();

					_powerUpState++;
					GameConfig.NumBubblesInterval /= BubbleSpeedIncrease;
					_speedShoot++;
					
					break;
				case Spells.ScatterShoot:
					// Make those bubbles scatter
					
					if (_scatterShoot <= 0)
					{
						_scatterShootOn = true;
						_underlay.Setup(Spells.ScatterShoot);
					}
					_underlay.Add();
					
					_powerUpState++;
					_scatterShoot++;
					
					break;
					
				case Spells.BigShoot:

					// Make those bubbles bigger
					if(_bigShoot <= 0)
						_underlay.Setup(Spells.BigShoot);
					
					_underlay.Add();
					
					_bubbleScale += new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
					Strength += BubbleStrengthIncrease;
					
					_powerUpState++;
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
					if(_speedShoot <= 1)
					{
						GameConfig.NumBubblesInterval = .5f;
						_speedShoot = 0;
					} else
					{
						GameConfig.NumBubblesInterval *= BubbleSpeedIncrease;
						_speedShoot--;
					}

					_powerUpState--;

					break;
				case Spells.ScatterShoot:

					if(_scatterShoot <= 1)
					{
						_scatterShootOn = false;
						_scatterShoot = 0;
					} else
						_scatterShoot--;

					_powerUpState--;

					break;

				case Spells.BigShoot:

					if(_bigShoot <= 1)
					{
						_bubbleScale = _bubbleDefault;
						Strength = BubbleInitialStrength;
						_bigShoot = 0;
					} else
					{
						_bigShoot--;
						_bubbleScale -= new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
						Strength -= BubbleStrengthIncrease;
					}

					_powerUpState--;

					break;
			}
		}

	}

	
	private void SpellComplete(Spells spell)
	{
		var animations = 0;

		GameConfig.GamePaused = true;
		GameConfig.GameSpeedModifier = 0;
		
		StartCoroutine(GUIManager.Instance.ShowSpellActivated());
		
		GameConfig.GamePaused = false;
		GameConfig.GameSpeedModifier = 15;

	}

	private void OnDeathEvent(GameEndEvent e)
	{

		WonGame = e.wonGame;
		
		if (WonGame)
			_gameManager.AudioController.Fade(WinClip);
		else 
			_gameManager.AudioController.Fade(DeathClip);


		gameObject.SetActive(false);
		GameConfig.GameWon = WonGame;
		
		_gameManager.AudioController.Fade(null);
		GUIManager.Instance.GameEnd(WonGame);
		
		// Send Player Data to Analytics
		Dictionary<string, object> analyticsData = new Dictionary<string, object>
		{
			{"finishedLevel", WonGame},
			{"time", Time.timeSinceLevelLoad},
			{"level", GameConfig.CurrentScene},
		};
		if(!WonGame)
			analyticsData["killerName"] = e.killerName;

		// Record win/loss
		if(GameConfig.CurrentScene != null)
		{

			if(WonGame)
			{
				if(!GameConfig.DictWonCount.ContainsKey(GameConfig.CurrentScene))
					GameConfig.DictWonCount[GameConfig.CurrentScene] = 0;
				else
					GameConfig.DictWonCount[GameConfig.CurrentScene]++;
			}
			/*else
			{
				if(GameConfig.DictLostCount.ContainsKey(GameConfig.CurrentScene))
					GameConfig.DictLostCount[GameConfig.CurrentScene]++;
				else
					GameConfig.DictLostCount[GameConfig.CurrentScene] = 0;
			}*/
			
			analyticsData["wonCount"] = GameConfig.DictWonCount[GameConfig.CurrentScene];
			analyticsData["lostCount"] = GameConfig.DictLostCount[GameConfig.CurrentScene];

		}
		
		Analytics.CustomEvent("levelEnd", analyticsData);
		ResetLevel();

	}
	
  
}
