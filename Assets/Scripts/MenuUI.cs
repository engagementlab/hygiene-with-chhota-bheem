using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MenuUI : MonoBehaviour
{

	public Animator SettingsAnimator;
	public AudioClip MenuMusic;
	private AudioSource _audio;
	
	// Use this for initialization
	void Start () {
	
		_audio = GetComponent<AudioSource>();
		// Start menu music
		_audio.PlayOneShot(MenuMusic);
		
	}

	public void SettingsButtonDown()
	{
		
	}

	public void Settings()
	{
		SettingsAnimator.gameObject.SetActive(true);
		SettingsAnimator.Play("SettingsOpen");
	}

	public void CloseMainMenu()
	{
		gameObject.GetComponent<Animator>().SetTrigger("MenuClose");
		
	}

	public void Screen(int num)
	{
		gameObject.GetComponent<Animator>().SetInteger("Screen", num);
	}

	public void Level(int num)
	{
		gameObject.GetComponent<Animator>().SetInteger("LevelSelected", num);
	}

	public void LevelSelectOpen()
	{
		Debug.Log("Triggering the level select!");
		
		gameObject.GetComponent<Animator>().SetTrigger("Levels");
	}
}
