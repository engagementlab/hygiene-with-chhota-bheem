using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{

	public Animator SettingsAnimator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SettingsButtonDown()
	{
		
	}

	public void OpenSettings()
	{
		SettingsAnimator.gameObject.SetActive(true);
		SettingsAnimator.Play("SettingsOpen");
	}
}
