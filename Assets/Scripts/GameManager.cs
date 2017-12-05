﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Analytics;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
	[CanBeNull]
	public GameObject VillagerPrefab;

	private float _deltaTime;
	private bool _playerHasTouched;
	private bool _touching = true;
	private bool _paused;
	private bool _slowMo;

	private GameObject _player;

	private AudioControl AudioController;

	private void Awake()
	{
		
		GameObject gameUi = (GameObject) Instantiate(Resources.Load("GameUI"));
		gameUi.name = "GameUI";
		GUIManager.Instance.Initialize();

		GameConfig.InitializePrefs();
		GameConfig.GameOver = false;
		GameConfig.Reset();

		Instantiate(Resources.Load("EventSystem"));
		
		// Send Player Data to Analytics
		Analytics.CustomEvent("levelStart",
			new Dictionary<string, object>
				{{ "level", GameConfig.CurrentScene }, { "playCount", GameConfig.LevelPlayCount }}
		);

		AudioController = GameObject.Find("AudioController").GetComponent<AudioControl>();

	}

	private void Start()
	{
		
		// Start level music
		Events.instance.Raise(new SoundEvent("song_1_test", SoundEvent.SoundType.Music, null, .3f));

		_player = GameObject.FindWithTag("Player");

	}

	private void Update()
	{

		bool noInput;
		#if UNITY_EDITOR
			noInput = !Input.GetMouseButton(0);
		#else
			noInput = Input.touches.Length == 0;
		#endif
	
		if(!GameConfig.GameOver && !_player.GetComponent<ArchetypePlayer>().Killed)
		{
			// Pause only if player has already touched at some point, and not in slow-mo mode
			if(!noInput && !_slowMo)
			{
				_playerHasTouched = true;
				GameConfig.GamePaused = false;
				GameConfig.SlowMo = false;
			}

			if(noInput)
			{
				if(!_slowMo)
					SlowMo();
			}
		}
		
		_deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
		
	}

	private void OnGUI()
	{
		#if !UNITY_EDITOR
		if(!Debug.isDebugBuild) return;
		#endif
		
		int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = Color.white;
		float msec = _deltaTime * 1000.0f;
		float fps = 1.0f / _deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		
		GUI.Label(rect, text, style);
		if(GUI.Button(new Rect(0, 40, 100, 50), "Stress Test"))
		{
			for(var i = 0; i < 45; i++)
			{
				Instantiate(VillagerPrefab, new Vector3(Random.Range(-2, 2), Random.Range(0, 20), 0), Quaternion.identity);
			}
		}
		
		// God mode toggle
		GameConfig.GodMode = GUI.Toggle(new Rect(0, 100, 100, 50), GameConfig.GodMode, "God Mode");

	}

	// If ArchetypeMove enters cam, mark as in view (more reliable/efficient than asking each object to watch x/y pos) 
	private void OnTriggerEnter(Collider other)
	{
		var archetypeMove = other.GetComponent<ArchetypeMove>();
		if(archetypeMove != null) archetypeMove.IsInView = true;
	}

	

	private void SlowMo()
	{
		if(GameConfig.GameOver) return;
		GUIManager.Instance.ShowSloMo();
		GameConfig.SlowMo = true;
		
		_slowMo = true;
		Time.timeScale = 0;
	}

	public void HideSlowMo()
	{
		GUIManager.Instance.HideSloMo();
		GameConfig.SlowMo = false;
		
		_slowMo = false;
		Time.timeScale = 1;
	}

	public void Pause()
	{
		_touching = false;
		GameConfig.GamePaused = true;
		
		GUIManager.Instance.HideSloMo();	
		GUIManager.Instance.ShowPause();
		
		// iTween Not working - leaving for future debugging
//		iTween.AudioTo(AudioController.WhichMusicPlayer().gameObject, 
//			iTween.Hash(
//				"audiosource", AudioController.WhichMusicPlayer(), 
//				"volume", AudioController.WhichMusicPlayer().volume/2, 
//				"pitch", AudioController.WhichMusicPlayer().pitch/2,
//				"time", 1f
//			)
//		);
		

		AudioController.WhichMusicPlayer().volume = AudioController.WhichMusicPlayer().volume/2;
			
		_paused = true;
	}

	public IEnumerator UnPause()
	{
		_touching = true;
		
		GUIManager.Instance.ShowSloMo();
		GUIManager.Instance.HidePause();

		// iTween Not working - leaving for future debugging
//		iTween.AudioTo(AudioController.WhichMusicPlayer().gameObject, 
//			iTween.Hash(
//				"audiosource", AudioController.WhichMusicPlayer(), 
//				"volume", AudioController.WhichMusicPlayer().volume*2, 
//				"pitch", AudioController.WhichMusicPlayer().pitch*2,
//				"time", 1f
//			)
//		);
		
		AudioController.WhichMusicPlayer().volume = AudioController.WhichMusicPlayer().volume*2;
		
		_paused = false;
		GameConfig.GamePaused = false;
		
		yield return new WaitForSeconds(.3f);

	}

}
