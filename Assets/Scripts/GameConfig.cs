using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConfig : MonoBehaviour
{

	public static bool sandboxMode = true;

  public static float numBubblesToStart = 4;
  public static float numBubblesGained = 1;
	public static float numBubblesSpeedGained = .05f;
  public static float numBubblesInterval = .25f;
  public static float numBubblesFull = 20;

  public static float peopleSpeedStart = .2f;
  public static float peopleSpeedCurrent = 1;
  public static float peopleNumberPerMin = 10;
  public static float peopleAmountIncreaseFactor = 0;
	public static float peopleSpeedIncreaseFactor = 0;

  public static float wizardSpeedStart = 1;
  public static float wizardAmountIncreaseFactor = 0;
  public static float wizardsNumberPerMin = 5;
  public static float wizardSpeedIncreaseFactor = 1;
  public static float wizardChance = 0.45f;

  public static float fliesSpeedStart = 1;
  public static float fliesNumberStart = 0;
  public static float fliesNumberPerMin = 10;
  public static float fliesAmountIncreaseFactor = 0;
  public static float fliesSpeedIncreaseFactor = 1;
  
  public static float powerUpChance = 0.55f;
  public static float powerUpNumberPerMin = 10;
  
  public static float poopChance = 0.25f;
	public static float numPoopSpeed = .1f;
	public static float numPoopSize = 1;
	public static float numPoopPerMin = 5;

	public static bool gamePaused = true;
	public static bool speedUpToggle;
  public static bool increaseToggle;
  public static bool peopleInGame = true;
  public static bool wizardInGame;
  public static bool wizardFloatMovement;
  public static bool fliesInGame = true;
  public static bool powerUpsInGame;
  public static bool poopInGame;
  
  public static int powerUpsCount = 0;
  public static int fliesCaught = 0;
  public static int peopleSaved = 0;

	public static float gameSpeedModifier = 15;
	public static float bubbleOffset = 2;

	// Use this for initialization
	void Awake () {
		
		DontDestroyOnLoad(this);

	}

	public static void Reset() {
    
		numBubblesInterval = .25f;
		
    numBubblesToStart = 4;
    numBubblesGained = 1;
    numBubblesFull = 20;

    peopleSpeedStart = .2f;
    peopleSpeedCurrent = 1;
    peopleNumberPerMin = 10;
    peopleAmountIncreaseFactor = 0;
    peopleSpeedIncreaseFactor = 0;

		wizardSpeedStart = 1;
		wizardAmountIncreaseFactor = 0;
		wizardSpeedIncreaseFactor = 1;

		fliesSpeedStart = 1;
		fliesNumberPerMin = 5;
		fliesAmountIncreaseFactor = 0;
		fliesSpeedIncreaseFactor = 1;

		wizardChance = 0.15f;
		
		speedUpToggle = false;
		increaseToggle = false;
		peopleInGame = true;
		wizardInGame = false;
		fliesInGame = true;

		powerUpsCount = 0;

	}

}