﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System.Linq;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
	[CanBeNull]
	public GameObject VillagerPrefab;

	private float deltaTime;
	private bool touching = false;

	private void Awake()
	{
		StartCoroutine(LocationTest());
	}

	private void Update()
	{

		#if UNITY_ANDROID && !UNITY_EDITOR
		if(!Input.GetMouseButton(0))
		{
			if(touching)
			{
				touching = false;
				GUIManager.Instance.ShowPause();
			}

		} 
		else
		{
			if(!touching)
			{
				touching = true;
				GUIManager.Instance.HidePause();
			}
		}
		#endif
		
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f; 
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
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
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

	IEnumerator LocationTest()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
        	var time = Input.location.lastData.timestamp;

			Analytics.CustomEvent("gameStart", new Dictionary<string, object>
			{
			   { "latitude", Input.location.lastData.latitude },
			   { "longitude", Input.location.lastData.longitude }, 
			   { "time", time }
			});
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    public void LoadLevel(string level) {

    	if (!System.String.IsNullOrEmpty(level)) 
			UnityEngine.SceneManagement.SceneManager.LoadScene(level);
    	else 
    		UnityEngine.SceneManagement.SceneManager.LoadScene(Application.loadedLevel);
    		
    	GUIManager.Instance.Initialiaze();

    }

}