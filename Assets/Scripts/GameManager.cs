﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.VR.WSA;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
	[CanBeNull]
	public GameObject VillagerPrefab;

	private AudioSource _audio;

	private float _deltaTime;
	private bool _playerHasTouched;
	private bool _touching = true;
	private bool _paused;

	private Dictionary<string, AudioClip> _loadedAudio;

	private void Awake()
	{
		
		GameObject gameUi = (GameObject) Instantiate(Resources.Load("GameUI"));
		gameUi.name = "GameUI";
		GUIManager.Instance.Initialize();

		_audio = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
		_loadedAudio = new Dictionary<string, AudioClip>();

		Instantiate(Resources.Load("EventSystem"));
		
		Events.instance.AddListener<SoundEvent> (OnSoundEvent);
		// Start level music
		OnSoundEvent(new SoundEvent("song_1_test", SoundEvent.SoundType.Music, null, .3f));
	}

	private void Update()
	{

		bool noInput;
		#if UNITY_EDITOR
			noInput = !Input.GetMouseButton(0);
		#else
			noInput = Input.touches.Length == 0;
		#endif
	
		if(!GameConfig.GameOver)
		{
			// Pause only if player has already touched at some point
			if(!noInput)
			{
				_playerHasTouched = true;
				GameConfig.GamePaused = false;
			}

			if(noInput && _playerHasTouched)
			{
				if(_touching && !_paused)
				{
					StartCoroutine(Pause());
				}

			} 
			else
			{
				if(!_touching && _paused)
				{
					StartCoroutine(UnPause());
				}
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
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
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

	}

	private void OnSoundEvent(SoundEvent e)
	{

		if(e.SoundFileName != null)
		{
			// If sound name provided and clip not loaded, load into dictionary
			if(!_loadedAudio.ContainsKey(e.SoundFileName))
				_loadedAudio.Add(e.SoundFileName, Resources.Load<AudioClip>("Audio/" + e.Type + "/" + e.SoundFileName));

			// Play loaded clip
			_audio.PlayOneShot(_loadedAudio[e.SoundFileName], e.SoundVolume);
		}
		// Otherwise, play provided clip
		else if(e.SoundClip != null) 
			_audio.PlayOneShot(e.SoundClip, e.SoundVolume);
		
	}

	public IEnumerator Pause()
	{
		_touching = false;
		GameConfig.GamePaused = true;
		
		GUIManager.Instance.ShowPause();
		yield return new WaitForSeconds(.4f);
		
		_paused = true;
	}

	public IEnumerator UnPause()
	{
		_touching = true;
		
		GUIManager.Instance.HidePause();
		yield return new WaitForSeconds(.5f);
		
		_paused = false;
		GameConfig.GamePaused = false;
	}

}
