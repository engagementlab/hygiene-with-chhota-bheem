using UnityEngine;

public class GameConfig : MonoBehaviour
{

	public static float NumBubblesSpeedGained = .05f;
  public static float NumBubblesInterval = .5f;
  
	public static bool GamePaused = true;
	public static bool GameOver;
  
	public static float GameSpeedModifier = 15;
	public static float BubbleOffset = 2;

	public static int Score;
	public static int PossibleScore;

	// Use this for initialization
	private void Awake () {
		
		DontDestroyOnLoad(this);
		
	}

	public static void Reset() {
    
		NumBubblesInterval = .25f;
		Score = 0;

	}

	public static void UpdateScore(int worth)
	{
		Score = Score + worth;
		GUIManager.Instance.UpdateScore(Score);

	}

}