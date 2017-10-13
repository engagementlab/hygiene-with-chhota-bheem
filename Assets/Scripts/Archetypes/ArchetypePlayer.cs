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
	
	public GameObject Bubble;
	
	[HideInInspector]
	public bool PoweredUp;

	public bool WonGame;

	private float _currentBadScore;
	private float _targetScore;
	private float _intervalTime;
	private float _bossSpawnDelta = 0;

	private GameObject _lastBubble;
	private Camera _mainCamera;
	public Spells _spellsType;

	private bool _freeMovement = true;
	private bool _trailEnabled = true;
	private bool _mouseDrag;
	private bool _moveDelta;
	private bool _scatterShootOn;

	private Vector3 _velocity;

	/**************
		UNITY METHODS
	***************/
	private void Awake () {

		_mainCamera = Camera.main;

		Events.instance.AddListener<DeathEvent> (OnDeathEvent);
		Events.instance.AddListener<SpellEvent> (OnSpellEvent);
		Events.instance.AddListener<ScoreEvent> (OnScoreEvent);

		var currentRect = GetComponent<RectTransform>().position;
		currentRect.z = -.5f;
		GetComponent<RectTransform>().position = currentRect;
	}

	private void Update() {
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(Input.touches.Length == 0) return;
		#endif
				
		var targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y + GameConfig.bubbleOffset, -.5f);
		transform.position = Utilities.ClampToScreen(Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, SmoothTime), _mainCamera);

	    if(_currentBadScore < _targetScore) {
		  	_currentBadScore += _targetScore/20;
	    }
		
		if(_intervalTime >= GameConfig.numBubblesInterval) {

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
				// Spawn 3 bubbles in different directions for scatter shot
				var dirs = new[] {-.5f, 0, .5f};
				for(int bubIndex = 0; bubIndex < 3; bubIndex++)
				{
					var projectile = Instantiate(Bubble, transform.position, Quaternion.identity);
					projectile.GetComponent<Rigidbody>().velocity = new Vector2(dirs[bubIndex], 1) * BubbleSpeed;
				}	
				
				
			}

		}
		else
			_intervalTime += Time.deltaTime;
	  
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
		_spellsType = e.powerType;

		if (e.powerUp)
		{
			// Spell ON
			StartCoroutine(SpellComplete(_spellsType));

			switch(_spellsType)
			{
				case Spells.SpeedShoot:
					// Speed up bubble rate
					if (PowerInfinite)
					{
						GUIManager.Instance.DisplayCurrentSpell("Bubble Speedup");
						GameConfig.numBubblesInterval /= 2;
						PoweredUp = true;
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
						GUIManager.Instance.DisplayCurrentSpell("Scatter Shot");
						_scatterShootOn = true;
						PoweredUp = true;
					}
					else
					{
						StartCoroutine(SpellScatterShoot(PowerTime));
					}
					break;
			}
		}
		else
		{
			// Spell OFF
			
			switch(_spellsType)
			{
				case Spells.SpeedShoot:
					GUIManager.Instance.HideSpell();
					GameConfig.numBubblesInterval *= 2;
					PoweredUp = false;
				
					break;
				case Spells.ScatterShoot:
					
					GUIManager.Instance.HideSpell();
					_scatterShootOn = false;
					PoweredUp = false;
					break;
			}
		}
		

		
	}
	
	private IEnumerator SpellComplete(Spells spell)
	{
		var animations = 0;
				
		GUIManager.Instance._spellStepsUi.SetActive(true);

		foreach (GameObject group in GUIManager.Instance._spellSteps)
		{
			if (group.name == spell.ToString())
			{
				group.SetActive(true);
				GUIManager.Instance._spellStepsComponent = group.GetComponentsInChildren<Animator>();
					
				for (var i = 0; i <= GUIManager.Instance._spellStepsComponent.Length; i++)
				{
					GUIManager.Instance._spellStepsComponent[i].Play("SpellStep");
					yield return new WaitForSeconds(2);
					animations++;
			
					if (animations >= GUIManager.Instance._spellStepsComponent.Length)
					{
						group.SetActive(false);
						GUIManager.Instance._spellStepsUi.SetActive(false);
					}
				}
			}
		}

	}
 
	private void OnDeathEvent(DeathEvent e)
	{
		WonGame = e.wonGame;

		gameObject.SetActive(false);
/*		GameEndScreen.SetActive(true);

		if (WonGame)
			GameWonText.SetActive(true);
		else 
			GameOverText.SetActive(true);*/

		// Send Player Data to Analytics
		Analytics.CustomEvent("gameEnd",
			new Dictionary<string, object>
			{{ "gameState", WonGame }, { "time", Time.timeSinceLevelLoad }}
		);
		
	}

	private static IEnumerator SpellBubbleSpeed(int time)
	{
		GUIManager.Instance.DisplayCurrentSpell("Bubble Speedup");
		GameConfig.numBubblesInterval /= 2;
				
		yield return new WaitForSeconds(time);
		
		GameConfig.numBubblesInterval *= 2;
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
