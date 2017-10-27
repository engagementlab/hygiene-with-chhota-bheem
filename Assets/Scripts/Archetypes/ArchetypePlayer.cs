using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Analytics;

public class ArchetypePlayer : MonoBehaviour {

	public float SmoothTime = 0.1f;
	public float BubbleSpeed = 15;

	public int PowerTime;
	public bool PowerInfinite;

	public int SpellStepCount;
	
	public GameObject Bubble;

	public int BubbleInitialStrength = 1;
	public float BubbleSpeedIncrease = 2f;
	public float BubbleSizeIncrease = 0.1f;
	public int BubbleStrengthIncrease = 1;
	
	[HideInInspector]
	public bool PoweredUp;

	public bool WonGame;
	
	private float _currentBadScore;
	private float _targetScore;
	private float _intervalTime;
	private float _bossSpawnDelta = 0;

	private GameObject _lastBubble;
	private Camera _mainCamera;
	
	[HideInInspector]
	public Spells SpellsType;

	private bool _freeMovement = true;
	private bool _trailEnabled = true;
	private bool _mouseDrag;
	private bool _moveDelta;
	private bool _scatterShootOn;

	[HideInInspector]
	public int Matrix = 0;

	
	private int _scatterShoot = 0;
	private int _speedShoot = 0;
	private int _bigShoot = 0;

	private Vector3 _velocity;
	
	private List<float> dirs;

	/**************
		UNITY METHODS
	***************/
	private void Awake () {

		_mainCamera = Camera.main;

		Events.instance.AddListener<DeathEvent> (OnDeathEvent);
		Events.instance.AddListener<SpellEvent> (OnSpellEvent);
		Events.instance.AddListener<ScoreEvent> (OnScoreEvent);
		
		Bubble.transform.localScale = new Vector3(0.12F, 0.12F, 0.22F);

		var currentRect = GetComponent<RectTransform>().position;
		currentRect.z = -.5f;
		GetComponent<RectTransform>().position = currentRect;
		
	}

	private void Update() {
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(Input.touches.Length == 0) return;
		#endif
				
		var targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y + GameConfig.BubbleOffset, -.5f);
		transform.position = Utilities.ClampToScreen(Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, SmoothTime), _mainCamera);

	    if(_currentBadScore < _targetScore) {
		  	_currentBadScore += _targetScore/20;
	    }
		
		if(_intervalTime >= GameConfig.NumBubblesInterval) {

			_intervalTime = 0;
			
			var projectilePos = transform.position;
			projectilePos.z = 0;

			if(!_scatterShootOn)
			{
				var dir = new Vector2(0, 1);
				dir.Normalize();

				var projectile = Instantiate(Bubble, projectilePos, Quaternion.identity);
				projectile.GetComponent<Rigidbody>().velocity = dir * BubbleSpeed;
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
					projectile.GetComponent<Rigidbody>().velocity = new Vector2(dirs[bubIndex], 1).normalized * BubbleSpeed;
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

		GUIManager.Instance.UpdateScore(e.scoreAmount, e.eventType.ToString());

	}
	
	private void OnSpellEvent(SpellEvent e)
	{		
		SpellsType = e.powerType;

		if (e.powerUp)
		{
			// Spell ON
			StartCoroutine(SpellComplete(SpellsType));

			switch(SpellsType)
			{
				case Spells.SpeedShoot:
					// Speed up bubble rate
					if (PowerInfinite)
					{
						if (_speedShoot <= 0)
						{
							GUIManager.Instance.DisplayCurrentSpell("Bubble Speedup");
							GameConfig.NumBubblesInterval /= BubbleSpeedIncrease;
							PoweredUp = true;
						}
						else
						{
							GameConfig.NumBubblesInterval /= BubbleSpeedIncrease;
						}

						_speedShoot++;
					}
					else
					{
						StartCoroutine(SpellBubbleSpeed(PowerTime));
					}
				
					break;
				case Spells.ScatterShoot:
					// Make those bubbles scatter
					if (PowerInfinite)
					{
						if (_scatterShoot <= 0)
						{
							GUIManager.Instance.DisplayCurrentSpell("Scatter Shot");
							_scatterShootOn = true;
							PoweredUp = true;
						}
						
						_scatterShoot++;
					}
					else
					{
						StartCoroutine(SpellScatterShoot(PowerTime));
					}
					break;
					
				case Spells.BigShoot:

					// Make those bubbles bigger
					if (PowerInfinite)
					{
						if (_bigShoot <= 0)
						{
							GUIManager.Instance.DisplayCurrentSpell("Bigger Shoot");
							Bubble.transform.localScale += new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
							BubbleInitialStrength += BubbleStrengthIncrease;
							PoweredUp = true;
						}
						else
						{
							Bubble.transform.localScale += new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
						}
						
						_bigShoot++;
					}
					else
					{
						StartCoroutine(SpellBigShoot(PowerTime));
					}
					break;
					
				case Spells.Matrix:

					Matrix++;
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
						GUIManager.Instance.HideSpell();
						GameConfig.NumBubblesInterval *= BubbleSpeedIncrease;
						PoweredUp = false;
					}
					else
					{
						_speedShoot--;
						GameConfig.NumBubblesInterval *= BubbleSpeedIncrease;
					}
					
				
					break;
				case Spells.ScatterShoot:

					if (_scatterShoot <= 0)
					{
						GUIManager.Instance.HideSpell();
						_scatterShootOn = false;
						PoweredUp = false;
					}
					else
					{
						_scatterShoot--;
					}
					
					break;
					
				case Spells.BigShoot:

					if (_bigShoot <= 0)
					{
						GUIManager.Instance.HideSpell();
						Bubble.transform.localScale -= new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
						BubbleInitialStrength -= BubbleStrengthIncrease;
						PoweredUp = false;
					}
					else
					{
						_bigShoot--;
						BubbleInitialStrength -= BubbleStrengthIncrease;
						Bubble.transform.localScale -= new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
					}
					
					break;
			}
		}
		

		
	}
	
	private IEnumerator SpellComplete(Spells spell)
	{
		var animations = 0;

		GameConfig.GamePaused = true;
		GameConfig.GameSpeedModifier = 0;
		
		// TO DO - When assets are ready
				
//		GUIManager.Instance._spellStepsUi.SetActive(true);

//		foreach (GameObject group in GUIManager.Instance._spellSteps)
//		{
//			if (group.name == spell.ToString())
//			{
//				group.SetActive(true);
//				GUIManager.Instance._spellStepsComponent = group.GetComponentsInChildren<Animator>();
//					
//				for (var i = 0; i <= GUIManager.Instance._spellStepsComponent.Length-1; i++)
//				{
//					GUIManager.Instance._spellStepsComponent[i].Play("SpellStep");
//					animations++;
//			
//					if (animations >= GUIManager.Instance._spellStepsComponent.Length)
//					{
						yield return new WaitForSeconds(1);
//						group.SetActive(false);
//						GUIManager.Instance._spellStepsUi.SetActive(false);
						GameConfig.GamePaused = false;
//					}
//				}
//			}
//		}

	}
 
	private void OnDeathEvent(DeathEvent e)
	{
		WonGame = e.wonGame;

		gameObject.SetActive(false);
		GUIManager.Instance.GameEnd(WonGame);
		
		// Send Player Data to Analytics
		Analytics.CustomEvent("gameEnd",
			new Dictionary<string, object>
			{{ "gameState", WonGame }, { "time", Time.timeSinceLevelLoad }}
		);
		
	}
	
	private IEnumerator SpellBigShoot(int time)
	{
		GUIManager.Instance.DisplayCurrentSpell("Bubble Size Increase");
		Bubble.transform.localScale += new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
				
		yield return new WaitForSeconds(time);
		
		Bubble.transform.localScale -= new Vector3(BubbleSizeIncrease, BubbleSizeIncrease, 0);
		GUIManager.Instance.HideSpell();
	}
	
	private IEnumerator SpellBubbleSpeed(int time)
	{
		GUIManager.Instance.DisplayCurrentSpell("Bubble Speedup");
		GameConfig.NumBubblesInterval /= BubbleSpeedIncrease;
				
		yield return new WaitForSeconds(time);
		
		GameConfig.NumBubblesInterval *= BubbleSpeedIncrease;
		GUIManager.Instance.HideSpell();
	}

	private IEnumerator SpellScatterShoot(int time)
	{
		GUIManager.Instance.DisplayCurrentSpell("Scatter Shot");
		_scatterShootOn = true;
		
		yield return new WaitForSeconds(time);

		_scatterShootOn = false;
		GUIManager.Instance.HideSpell();
	}
  
}
