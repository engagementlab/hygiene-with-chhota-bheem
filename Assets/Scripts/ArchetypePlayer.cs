using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class ArchetypePlayer : MonoBehaviour {

	public float SmoothTime = 0.1f;
	public float BubbleSpeed = 15;
	
	public GameObject Bubble;
	public GameObject GameOverText;

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
//		Events.instance.AddListener<SpellComponentEvent> (OnSpellComponentEvent);

	}

	// Use this for initialization
	private void Start ()
	{

		GameOverText = GameObject.FindGameObjectWithTag("Game Over");
		GameOverText.SetActive(false);

	}

	private void Update() {
		
		var targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y + GameConfig.bubbleOffset, Camera.main.nearClipPlane);
		transform.position = ClampToScreen(Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, SmoothTime));

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
	private Vector3 ClampToScreen(Vector3 vector) {

		Vector3 pos = _mainCamera.WorldToViewportPoint(vector);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		pos.z = 0;

		Vector3 worldPos = _mainCamera.ViewportToWorldPoint(pos);
		// Debug.Log(worldPos.x);
		worldPos.x = Mathf.Clamp(worldPos.x, -6.9f, 6.9f);
		worldPos.z = 0;

		return worldPos;

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

	/*	private void OnSpellComponentEvent(SpellComponentEvent e)
	{
		if(!e.SpawnPickup)
			GUIManager.Instance.ShowSpellComponent(e.ComponentType);
	}
	*/
	private void OnDeathEvent(DeathEvent e)
	{
		WonGame = e.wonGame;
		
	}

	private static IEnumerator PowerUpBubbleSpeed()
	{
		GameConfig.numBubblesInterval /= 2;
		
		yield return new WaitForSeconds(5);
		
		GameConfig.numBubblesInterval *= 2;
	}

	private IEnumerator PowerUpScatterShoot()
	{
		_scatterShootOn = true;
		
		yield return new WaitForSeconds(5);

		_scatterShootOn = false;
	}
  
}
