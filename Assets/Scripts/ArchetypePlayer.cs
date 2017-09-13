using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArchetypePlayer : MonoBehaviour {

	public Image meterImage; 
	
	public float smoothTime = 0.1f;
	public float fillTime = 2; 

	public bool inBossBattle;
	
	public GameObject bubble;
	public float bubbleSpeed;
	
	private float intervalTime = 0;

	private GameObject lastBubble;
	public GameObject gameOverText;

	private Camera mainCamera;

	private List<GameObject> currentBubbles;
  // List<ArchetypeBubble> currentBubbleConfigs;

	private bool freeMovement = true;
	private bool trailEnabled = true;
	private bool mouseDrag = false;
	private bool moveLeft = false;
	private bool moveRight = false;
	private bool moveDelta = false;

	public bool wonGame;

	private Vector3 velocity = Vector3.zero;
	private Vector3 deltaMovement;

	private float currentBadScore;
	private float targetScore;

	private float bossSpawnDelta = 0;

	private PowerUps powerUpType;

	private Vector3 ClampToScreen(Vector3 vector) {

  	Vector3 pos = mainCamera.WorldToViewportPoint(vector);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		pos.z = 0;

		Vector3 worldPos = mainCamera.ViewportToWorldPoint(pos);
	  // Debug.Log(worldPos.x);
	  worldPos.x = Mathf.Clamp(worldPos.x, -6.9f, 6.9f);
		worldPos.z = 0;

  	return worldPos;

  }

	private void BubbleHitEvent(HitEvent e) {

		if(e.eventType == HitEvent.Type.Spawn) {
			SpawnHit(e.collider, e.bubble);
		} else {

			currentBubbles.Remove(e.bubble);

			Destroy(e.bubble);

		}

  }

	private void OnPowerUpEvent(PowerUpEvent e)
	{
		powerUpType = e.powerType;
		
  	 // What kinda power up? 
		switch(powerUpType)
		{
			case PowerUps.Matrix:
				// Slow down the whole world except the player
				break;
			case PowerUps.SpeedShoot:
				// Speed up bubble rate
				StartCoroutine(PowerUpBubbleSpeed());
				break;
			case PowerUps.ScatterShoot:
				// Make those bubbles scatter
				break;
		}
	}

	private static void SpawnHit(Collider collider, GameObject bubble=null) {

  }

	private void MovementToggle(bool value) {

  	freeMovement = value;

		if(!freeMovement) {
	    Vector3 lockedPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, 250, transform.position.z));
	    lockedPos.z = 0;

	    transform.position = lockedPos;
	  }

  }

	private void OnMovementEvent (MovementEvent e) {

  	mouseDrag = false;

		if(e.Direction == "left")
			moveLeft = !e.EndClick;
		else
			moveRight = !e.EndClick;
	
	}

	private void OnDeathEvent(DeathEvent e)
	{
		wonGame = e.wonGame;
		
	}

	private IEnumerator PowerUpBubbleSpeed()
	{
		GameConfig.numBubblesInterval /= 2;
		
		yield return new WaitForSeconds(5);
		
		GameConfig.numBubblesInterval *= 2;
	}

	private void Awake () {

		deltaMovement = transform.position;
		mainCamera = Camera.main;

		Events.instance.AddListener<MovementEvent> (OnMovementEvent);
		Events.instance.AddListener<PowerUpEvent> (OnPowerUpEvent);
		Events.instance.AddListener<DeathEvent> (OnDeathEvent);

	}

	// Use this for initialization
	private void Start ()
	{

//		Camera.main.orthographicSize = Screen.height / 2;

		currentBubbles = new List<GameObject>();
		// currentBubbleConfigs = new List<ArchetypeBubble>();

		gameOverText = GameObject.FindGameObjectWithTag("Game Over");
		// badScoreText = GameObject.Find("Bad Score").GetComponent<Text>();
		// goodScoreText = GameObject.Find("Good Score").GetComponent<Text>();
		gameOverText.SetActive(false);

		
		// goodScoreText.gameObject.SetActive(false);
		// badScoreText.gameObject.SetActive(false);
		
	}

	private void Update() {

  	Vector3 targetPosition;

		targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y + GameConfig.bubbleOffset, Camera.main.nearClipPlane);
		transform.position = ClampToScreen(Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime));
	
		if(moveLeft || moveRight) {
			transform.position = ClampToScreen(Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime));
			deltaMovement = transform.position;
		}
	  else if(!freeMovement && !mouseDrag)
	  	transform.position = Vector3.SmoothDamp(transform.position, deltaMovement, ref velocity, 0.2f);

	  if(currentBadScore < targetScore) {
	  	currentBadScore += targetScore/20;
	  }
		
		if(intervalTime >= GameConfig.numBubblesInterval) {


			intervalTime = 0;
			Vector2 dir = new Vector2(0, 1);
			
			dir.Normalize();
			
			GameObject projectile = Instantiate (bubble, transform.position, Quaternion.identity) as GameObject;
			projectile.GetComponent<Rigidbody> ().velocity = dir * bubbleSpeed; 
			
		}
		else
			intervalTime += Time.deltaTime;
	  
	}

	private void OnDestroy() {

		Events.instance.RemoveListener<MovementEvent> (OnMovementEvent);
		Events.instance.RemoveListener<HitEvent> (BubbleHitEvent);
		Events.instance.RemoveListener<DeathEvent> (OnDeathEvent);
		Events.instance.RemoveListener<PowerUpEvent> (OnPowerUpEvent);

	}

	private void OnMouseDown() {

		Behaviour h = (Behaviour)GetComponent("Halo");
		h.enabled = true;
  }

	private void OnMouseUp() {

		Behaviour h = (Behaviour)GetComponent("Halo");
		h.enabled = false;

  }

	private void OnMouseDrag() {

		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, freeMovement ? Input.mousePosition.y - 50 : 250, 0);
		Vector3 cursorPosition = mainCamera.ScreenToWorldPoint(cursorPoint);

		transform.position = ClampToScreen(cursorPosition);

	}

	private void OnTriggerStay(Collider other)
  {

	  if(other.gameObject.tag == "Spawner" && inBossBattle && meterImage != null) {
	  	if(meterImage.fillAmount < 1)
	  		meterImage.fillAmount += Time.deltaTime / fillTime;
				
	  	return;
	  }

  }

  
}
