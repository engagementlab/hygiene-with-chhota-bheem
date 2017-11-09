using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

	public Animator SettingsAnimator;
	public AudioClip MenuMusic;
	private AudioSource _audio;

	public Sprite ToggleOn;
	public Sprite ToggleOff;

	private Toggle SoundToggle;
	private Toggle MusicToggle;
	
	// Use this for initialization
	void Start () {
	
		_audio = GetComponent<AudioSource>();
		// Start menu music
		_audio.PlayOneShot(MenuMusic);
		
		// Find Toggles
		SoundToggle = gameObject.transform.Find("Settings/Board/Sound/Toggle").GetComponent<Toggle>();
		MusicToggle = gameObject.transform.Find("Settings/Board/Music/Toggle").GetComponent<Toggle>();

		// Set Toggles to GameConfig settings
		SoundToggle.isOn = GameConfig.SoundOn;
		MusicToggle.isOn = GameConfig.MusicOn;
	}

	public void Screen(int num)
	{
		gameObject.GetComponent<Animator>().SetInteger("Screen", num);
	}

	public void Level(int num)
	{
		gameObject.GetComponent<Animator>().SetInteger("LevelSelected", num);
	}

	public void Sound()
	{
		GameConfig.SoundOn = !GameConfig.SoundOn;
		Debug.Log(GameConfig.SoundOn);
	}

	public void Music()
	{
		GameConfig.MusicOn = !GameConfig.MusicOn;

		Debug.Log(GameConfig.MusicOn);
	}
}
