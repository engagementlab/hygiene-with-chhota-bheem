using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameConfig : MonoBehaviour
{

	public static float NumBubblesSpeedGained = .05f;
  public static float NumBubblesInterval = .5f;
  
	public static bool GamePaused = true;
	public static bool SlowMo;
	public static bool GameOver;
	public static bool GodMode;

	public static int CurrentLevel;
	public static int CurrentChapter;
	public static int LevelPlayCount;
	public static string CurrentScene;
	public static Dictionary<string, int> DictWonCount = new Dictionary<string, int>();
	public static Dictionary<string, int> DictLostCount = new Dictionary<string, int>();

	[Range(0f, 1f)]
	public static float GlobalVolume;

	public static bool SoundOn;
	public static bool MusicOn;
  
	public static float GameSpeedModifier = 15;
	public static float BubbleOffset = 1.5f;

	public static int CurrentLanguage;
	public static int Score;
	public static int VillagersSaved;
	public static int Multiplier;

	public static bool GameWon;
	
	// PlayerPrefs Types List
	public static List<String> Types;

	// Use this for initialization
	private void Awake () {
		
		DontDestroyOnLoad(gameObject);

	}
	
	public static void Reset()
	{

		GameOver = false;
		GameWon = false;
		NumBubblesInterval = .5f;
		VillagersSaved = 0;
		Multiplier = 0;

	}

	public static void InitializePrefs()
	{
		if (PlayerPrefs.HasKey("sound"))
			SoundOn = PlayerPrefs.GetInt("sound") == 1;
		else
		{
			PlayerPrefs.SetInt("sound", 1);
			SoundOn = true;
		}

		if (PlayerPrefs.HasKey("music"))
			MusicOn = PlayerPrefs.GetInt("music") == 1;
		else
		{
			PlayerPrefs.SetInt("music", 1);
			MusicOn = true;
		}

		if (PlayerPrefs.HasKey("volume"))
			GlobalVolume = PlayerPrefs.GetFloat("volume");
		else
		{
			GlobalVolume = 1f;
			PlayerPrefs.SetFloat("volume", GlobalVolume);
		}

		if(PlayerPrefs.HasKey("language"))
			CurrentLanguage = PlayerPrefs.GetInt("language");
		else
			PlayerPrefs.SetInt("language", CurrentLanguage);

//		foreach (Levels level in _levels)
//		{
//			
//		}
		
	}
	
	public static void UpdatePrefs(string key, int? num = null, float? floater = null, [CanBeNull] string text = null)
	{
		if (!PlayerPrefs.HasKey(key))
			return;
		
		if (num != null)
			PrefInts(key, (int)num);
		else if (floater != null)
			PrefFloats(key, (float)floater);
		else if (text != null)
			PrefStrings(key, text);
	
	}

	public static void PrefInts(string key, int num)
	{
		if (!PlayerPrefs.HasKey(key))
			return;
		
		PlayerPrefs.SetInt(key, num);
	}
	
	public static void PrefFloats(string key, float num)
	{
		if (!PlayerPrefs.HasKey(key))
			return;
		
		PlayerPrefs.SetFloat(key, num);
	}
	
	public static void PrefStrings(string key, string text)
	{
		if (!PlayerPrefs.HasKey(key))
			return;
		
		PlayerPrefs.SetString(key, text);
	}
	
	public static void UpdateScore(int worth)
	{
		Score = Score + worth;
		GUIManager.Instance.UpdateScore(Score);

	}

	public static int StarCount()
	{
		int _stars;

		if (!GameWon)
			return 0;
		
		if (VillagersSaved > Multiplier)
			_stars = 3;
		
		else if (VillagersSaved > 0 && VillagersSaved <= Multiplier)
			_stars = (int)( ( (float)VillagersSaved / Multiplier) * 3 );
		
		else
			_stars = 0;
		
		return _stars;
	}

	public static void LoadLevel()
	{
		var baseName = BaseName();
		CurrentScene = baseName;
		LevelPlayCount = 1;
		
		if(!DictWonCount.ContainsKey(baseName))
			DictWonCount = new Dictionary<string, int>{{baseName, 0}};
		
		if(!DictLostCount.ContainsKey(baseName))
			DictLostCount = new Dictionary<string, int>{{baseName, 0}};
		
		UnityEngine.SceneManagement.SceneManager.LoadScene(baseName);
	}

	public static string BaseName()
	{
		
		var baseName = "Level";
		switch(CurrentChapter)
		{
			case 0:
				baseName += "1.";
				break;

			case 1:
				baseName += "2.";
				break;

			case 2:
				baseName += "3.";
				break;
		}
		
		baseName += CurrentLevel == 1 ? "2" : "1";

		return baseName;

	}

}
