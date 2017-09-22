using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Analytics;

public class ArchetypePlayer : MonoBehaviour {

	public float SmoothTime = 0.1f;
	public float BubbleSpeed = 15;
	
	public GameObject Bubble;
	public GameObject GameOverText;
	public GameObject GameWonText;

	public bool WonGame;

	private float _currentBadScore;
	private float _targetScore;
	private float _intervalTime;
	private float _bossSpawnDelta = 0;

	private GameObject _lastBubble;
	private Camera _mainCamera;
	private PowerUps _powerUpType;

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
		Events.instance.AddListener<PowerUpEvent> (OnPowerUpEvent);
		Events.instance.AddListener<ScoreEvent> (OnScoreEvent);

		
	}

	// Use this for initialization
	private void Start ()
	{

		GameOverText = GameObject.FindGameObjectWithTag("Game Over");
		GameOverText.SetActive(false);

		GameWonText = GameObject.FindGameObjectWithTag("Game Won");
		GameWonText.SetActive(false);

	}

	private void Update() {
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		if(Input.touches.Length == 0) return;
		#endif
		
		var targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y + GameConfig.bubbleOffset, Camera.main.nearClipPlane);
		transform.position = Utilities.ClampToScreen(Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, SmoothTime), _mainCamera);

	  if(_currentBadScore < _targetScore) {
	  	_currentBadScore += _targetScore/20;
	  }
		
		if(_intervalTime >= GameConfig.numBubblesInterval) {

			_intervalTime = 0;

			if(!_scatterShootOn)
			{
				var dir = new Vector2(0, 1);
				dir.Normalize();

				var projectile = Instantiate(Bubble, transform.position, Quaternion.identity);
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
		Events.instance.RemoveListener<PowerUpEvent> (OnPowerUpEvent);
//		Events.instance.RemoveListener<SpellComponentEvent> (OnSpellComponentEvent);

	}
	
	/**************
		CUSTOM METHODS
	***************/

  	private void OnScoreEvent(ScoreEvent e) {

		GUIManager.Instance.UpdateScore(e.scoreAmount, e.eventType.ToString());

	}
	
	private void OnPowerUpEvent(PowerUpEvent e)
	{
		_powerUpType = e.powerType;
		
		// What kinda power up? 
		switch(_powerUpType)
		{
			case PowerUps.SpeedShoot:
				// Speed up bubble rate
				StartCoroutine(PowerUpBubbleSpeed());
				break;
			case PowerUps.ScatterShoot:
				// Make those bubbles scatter
				StartCoroutine(PowerUpScatterShoot());
				break;
		}
	}
 
	private void OnDeathEvent(DeathEvent e)
	{
		WonGame = e.wonGame;

		gameObject.SetActive(false);

		if (WonGame)
			GameWonText.SetActive(true);
		else 
			GameOverText.SetActive(true);

		// Send Player Data to Analytics
		Analytics.CustomEvent("gameEnd", new Dictionary<string, object>
	    {
		    { "gameState", WonGame }, 
			{ "time", Time.timeSinceLevelLoad }
	    });
		
	}

	private static IEnumerator PowerUpBubbleSpeed()
	{
		GUIManager.Instance.DisplayCurrentPowerUp("Bubble Speedup");
		GameConfig.numBubblesInterval /= 2;
		
		yield return new WaitForSeconds(5);
		
		GameConfig.numBubblesInterval *= 2;
		GUIManager.Instance.HidePowerUp();
	}

	private IEnumerator PowerUpScatterShoot()
	{
		GUIManager.Instance.DisplayCurrentPowerUp("Scatter Shot");
		_scatterShootOn = true;
		
		yield return new WaitForSeconds(5);

		_scatterShootOn = false;
		GUIManager.Instance.HidePowerUp();
	}
  
}
