using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArchetypePlayer : MonoBehaviour {

	public GameObject bubblePrefab;
	public Toggle toggleMovement;
	public Toggle toggleTrail;

	public Text badScoreText;
	public Text goodScoreText;

	public Image meterImage; 

	public float startingLifeAmount = 100.0f;
	public float movementSpeed = 5;
	public float smoothTime = 0.1f;
	public float bubbleFollowSpeed = .5f;
	public float fillTime = 2; 

	public bool inBossBattle;
	public bool hasBubbles;
	public bool shootingStaticMode;
	
  GameObject lastBubble;
	GameObject gameOverText;

	Camera mainCamera;

  List<GameObject> currentBubbles;
  // List<ArchetypeBubble> currentBubbleConfigs;

  bool freeMovement = true;
  bool trailEnabled = true;
  bool mouseDrag = false;
  bool moveLeft = false;
  bool moveRight = false;
  bool moveDelta = false;

	public bool wonGame;

	Vector3 velocity = Vector3.zero;
  Vector3 deltaMovement;

  float currentBadScore;
  float currentGoodScore;
  float targetScore;

  float bossSpawnDelta = 0;

  Vector3 ClampToScreen(Vector3 vector) {

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

  void AddBubble() {

  // 	if(!hasBubbles)
  // 		return;
		
		// // Transform target = (currentBubbleConfigs.Count > 0) ? currentBubbleConfigs[currentBubbleConfigs.Count-1].transform : transform;
		
		// lastBubble = Instantiate(bubblePrefab, new Vector3(0, -Screen.height/2, 0), Quaternion.identity);
		// // lastBubble.GetComponent<ArchetypeBubble>().target = target;

		// currentBubbles.Add(lastBubble);
		// // currentBubbleConfigs.Add(lastBubble.GetComponent<ArchetypeBubble>());

		// // bubbleFollowSpeed = Mathf.Clamp(bubbleFollowSpeed-.05f, .1f, .5f);

		// // foreach(ArchetypeBubble config in currentBubbleConfigs) 
		// // 	config.speed = bubbleFollowSpeed;

  }

	void BubbleHitEvent(HitEvent e) {

		Debug.Log(e);

		if(e.eventType == HitEvent.Type.Spawn) {
			SpawnHit(e.collider, e.bubble);
		} else {

			currentBubbles.Remove(e.bubble);

			Destroy(e.bubble);

		}

  }

  void SpawnHit(Collider collider, GameObject bubble=null) {

	  	if(collider.gameObject.GetComponent<VillagerObject>() != null)
	  		return;

	  	if(collider.gameObject.GetComponent<ArchetypeSpawner>() != null) {

	  		if(currentBubbles.Count == 0 && !wonGame) {
	  			gameObject.SetActive(false);
	  			gameOverText.SetActive(true);;

	  			// countText.text = "Power-ups captured: " + GameConfig.powerUpsCount;

	  			return;
	  		}

	  		if(bubble != null) {

		  		int indBubble = currentBubbles.IndexOf(bubble.gameObject);
		  		List<GameObject> bubblesRemove = currentBubbles.GetRange(indBubble, currentBubbles.Count-indBubble);

			  	foreach(GameObject thisBubble in bubblesRemove) {
			  		currentBubbles.Remove(thisBubble);
			  		// currentBubbleConfigs.Remove(thisBubble.GetComponent<ArchetypeBubble>());

						if(!inBossBattle)
				  		Destroy(thisBubble);
			  	}

		  		if(currentBubbles.Count > 0)
			  		lastBubble = currentBubbles[currentBubbles.Count-1];
			  	
			  	if(bubbleFollowSpeed+.05f <= 1)
						bubbleFollowSpeed += .05f;
				
				}
	  	
	  	}
			else {

				if(hasBubbles) {
					AddBubble();
				}
				else {

					if(currentBubbles.Count > 3) {
						
						List<GameObject> bubblesRemove;
		  			bubblesRemove = new List<GameObject>(new GameObject[] {bubble.gameObject});

				  	foreach(GameObject thisBubble in bubblesRemove) {
				  		currentBubbles.Remove(thisBubble);
				  		// currentBubbleConfigs.Remove(thisBubble.GetComponent<ArchetypeBubble>());

				  		Destroy(thisBubble);
				  	}

				  	Events.instance.Raise (new ScoreEvent(1, ScoreEvent.Type.Good));	
			  		Destroy(collider.gameObject);
			  		GameConfig.fliesCaught++;

				  }
				  else {

				  	Hashtable fadeOut = new Hashtable();
				  	Hashtable fadeIn = new Hashtable();

		        fadeOut.Add("amount", 0);
		        fadeOut.Add("time", .5f);
		        
		        fadeIn.Add("amount", 1);
		        fadeIn.Add("time", .5f);
		        fadeIn.Add("delay", .7f);

				  	iTween.ShakePosition(collider.gameObject, new Vector3(.1f, .1f, .1f), 1.5f);
				  	iTween.FadeTo(collider.gameObject, fadeOut);
				  	iTween.FadeTo(collider.gameObject, fadeIn);

				  }

				}

			}

		if(!inBossBattle)
			Destroy(collider.gameObject);
	

  }

  void MovementToggle(bool value) {

  	freeMovement = value;

		if(!freeMovement) {
	    Vector3 lockedPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width/2, 250, transform.position.z));
	    lockedPos.z = 0;

	    transform.position = lockedPos;
	  }

  }

  void TrailToggle(bool value) {

  	trailEnabled = value;

  	foreach(GameObject bubble in currentBubbles)
  		bubble.SetActive(!trailEnabled);

  }

	void OnMovementEvent (MovementEvent e) {

  	mouseDrag = false;

		if(e.Direction == "left")
			moveLeft = !e.EndClick;
		else
			moveRight = !e.EndClick;
	
	}

	void OnScoreEvent (ScoreEvent e) {

		// targetScore += e.scoreAmount;

		// GameConfig.powerUpsCount++;

		if(e.eventType == ScoreEvent.Type.Good) {
			currentGoodScore += e.scoreAmount;
//			goodScoreText.text = "Good Wizard: " + currentGoodScore;
		}
		else {
			currentBadScore += e.scoreAmount;
//			badScoreText.text = "Bad Wizard: " + currentBadScore;
		}


	}
	
	void OnDeathEvent(DeathEvent e)
	{
		wonGame = e.wonGame;
		
	}

	void Awake () {

		deltaMovement = transform.position;
		mainCamera = Camera.main;

		Events.instance.AddListener<MovementEvent> (OnMovementEvent);
		Events.instance.AddListener<HitEvent> (BubbleHitEvent);
		Events.instance.AddListener<DeathEvent> (OnDeathEvent);
		Events.instance.AddListener<ScoreEvent> (OnScoreEvent);

	}

	// Use this for initialization
	void Start ()
	{

//		Camera.main.orthographicSize = Screen.height / 2;

		currentBubbles = new List<GameObject>();
		// currentBubbleConfigs = new List<ArchetypeBubble>();

		// gameOverText = GameObject.Find("GameOver");
		// badScoreText = GameObject.Find("Bad Score").GetComponent<Text>();
		// goodScoreText = GameObject.Find("Good Score").GetComponent<Text>();
		// gameOverText.SetActive(false);

		if(hasBubbles)
		{
			for(int i = 0; i < GameConfig.numBubblesToStart; i++)
				AddBubble();
		}
		
		// goodScoreText.gameObject.SetActive(false);
		// badScoreText.gameObject.SetActive(false);
		
	}

	void Update() {

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
	  
	}

	void OnDestroy() {

		Events.instance.RemoveListener<MovementEvent> (OnMovementEvent);
		Events.instance.RemoveListener<HitEvent> (BubbleHitEvent);
		Events.instance.RemoveListener<ScoreEvent> (OnScoreEvent);
		Events.instance.RemoveListener<DeathEvent> (OnDeathEvent);

	}
	
  void OnMouseDown() {

		Behaviour h = (Behaviour)GetComponent("Halo");
		h.enabled = true;
  }
	
  void OnMouseUp() {

		Behaviour h = (Behaviour)GetComponent("Halo");
		h.enabled = false;

  }

	void OnMouseDrag() {

		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, freeMovement ? Input.mousePosition.y - 50 : 250, 0);
		Vector3 cursorPosition = mainCamera.ScreenToWorldPoint(cursorPoint);

		transform.position = ClampToScreen(cursorPosition);

	}

	void OnTriggerEnter(Collider collider)
  {

	  

	  // if(collider.gameObject.tag == "Spawner" && hasBubbles) {
	  // 	bossSpawnDelta = 0;

			// for(int i = 0; i < GameConfig.numBubblesGained; i++)
			// 	AddBubble();

	  // 	return;
	  // }

  	// if(collider.gameObject.tag != "Spawn" || collider.gameObject.tag != "Wizards")
  	// 	return;

  	// if(currentBubbles.Count > 0)
	  // 	SpawnHit(collider, currentBubbles[currentBubbles.Count-1]);
  	// else
	  // 	SpawnHit(collider);

  }

  void OnTriggerExit(Collider collider)
  {		

	  if(collider.gameObject.tag == "Spawner" && hasBubbles) {
	  	
			for(int i = 0; i < GameConfig.numBubblesGained; i++)
				AddBubble();
		  
	  }

  }

	void OnTriggerStay(Collider other)
  {

	  if(other.gameObject.tag == "Spawner" && inBossBattle && meterImage != null) {
	  	if(meterImage.fillAmount < 1)
	  		meterImage.fillAmount += Time.deltaTime / fillTime;
				
	  	return;
	  }

  }

  
}
