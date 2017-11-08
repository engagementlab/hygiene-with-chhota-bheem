using UnityEngine;
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
	private bool _slowMo;

	private Dictionary<string, AudioClip> _loadedAudio;

	private void Awake()
	{
		
		GameObject gameUi = (GameObject) Instantiate(Resources.Load("GameUI"));
		gameUi.name = "GameUI";
		GUIManager.Instance.Initialize();

		Instantiate(Resources.Load("EventSystem"));
		
	}

	private void Start()
	{
		_audio = GetComponent<AudioSource>();
		if(_audio == null)
			_audio = gameObject.AddComponent<AudioSource>();
		
		_loadedAudio = new Dictionary<string, AudioClip>();
		
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

	private void OnSoundEvent(SoundEvent e)
	{
			
		// If this is SFX and the sound is OFF, stop here
		if (e.Type == SoundEvent.SoundType.SFX && !GameConfig.SoundOn)
			return;
		
		// If this is Music and the music is OFF, stop here
		if (e.Type == SoundEvent.SoundType.Music && !GameConfig.MusicOn)
			return;
		
		if(e.SoundFileName != null)
		{
			// If sound name provided and clip not loaded, load into dictionary
			if(!_loadedAudio.ContainsKey(e.SoundFileName))
				_loadedAudio.Add(e.SoundFileName, Resources.Load<AudioClip>("Audio/" + e.Type + "/" + e.SoundFileName));

			// Play loaded clip
			_audio.PlayOneShot(_loadedAudio[e.SoundFileName], e.SoundVolume * GameConfig.GlobalVolume);
		}
		// Otherwise, play provided clip
		else if(e.SoundClip != null) 
			_audio.PlayOneShot(e.SoundClip, e.SoundVolume);
		
	}

	private void SlowMo()
	{
		GUIManager.Instance.ShowSloMo();
		GameConfig.SlowMo = true;
		
		_slowMo = true;
		Time.timeScale = .1f;
	}

	public void HideSlowMo()
	{
		GUIManager.Instance.HideSloMo();
		GameConfig.SlowMo = false;
		
		_slowMo = false;
		Time.timeScale = 1f;
	}

	public void Pause()
	{
		_touching = false;
		GameConfig.GamePaused = true;
		
		GUIManager.Instance.HideSloMo();	
		GUIManager.Instance.ShowPause();
		
		_paused = true;
		Time.timeScale = 1;
	}

	public IEnumerator UnPause()
	{
		_touching = true;
		
		GUIManager.Instance.ShowSloMo();
		GUIManager.Instance.HidePause();
		yield return new WaitForSeconds(.3f);
		
		_paused = false;
		GameConfig.GamePaused = false;
		
		Time.timeScale = .1f;
	}

}
