using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConfig : MonoBehaviour
{

	public static float NumBubblesSpeedGained = .05f;
  public static float NumBubblesInterval = .5f;
  
	public static bool GamePaused = true;
  
  public static int PowerUpsCount = 0;
  public static int FliesCaught = 0;
  public static int PeopleSaved = 0;

	public static float GameSpeedModifier = 15;
	public static float BubbleOffset = 2;

	// Use this for initialization
	private void Awake () {
		
		DontDestroyOnLoad(this);

	}

	public static void Reset() {
    
		NumBubblesInterval = .25f;
		
		PowerUpsCount = 0;

	}

}