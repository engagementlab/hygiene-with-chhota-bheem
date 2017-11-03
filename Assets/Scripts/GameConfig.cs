using UnityEngine;

public class GameConfig : MonoBehaviour
{

	public static float NumBubblesSpeedGained = .05f;
  public static float NumBubblesInterval = .5f;
  
	public static bool GamePaused = true;
	public static bool GameOver;
  
	public static float GameSpeedModifier = 15;
	public static float BubbleOffset = 2;

	public static int Score = 200;
	public static int VillagersSaved = 1;
	public static int Multiplier = 3;

	// Use this for initialization
	private void Awake () {
		
		DontDestroyOnLoad(this);
		
	}

	public static void Reset() {
    
		NumBubblesInterval = .25f;
		Score = 200;

	}

	public static void UpdateScore(int worth)
	{
		Score = Score + worth;
		GUIManager.Instance.UpdateScore(Score);

	}

	public static int StarCount()
	{
		int _stars;
		
		if (VillagersSaved > Multiplier)
		{
			_stars = 3;
		} 
		else if (VillagersSaved > 0 && VillagersSaved <= Multiplier)
		{
			_stars = (int)( ( (float)VillagersSaved / Multiplier) * 3 );
		}
		else 
		{
			_stars = 0;
		}
		
		return _stars;
	}

	public static int ScoreMultiplier()
	{
		Score *= VillagersSaved;
		return Score;
	}
	

}
