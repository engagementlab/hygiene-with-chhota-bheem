using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System.Linq;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
	[CanBeNull]
	public GameObject VillagerPrefab;

	private float _deltaTime;
	private bool _touching;

	private void Awake()
	{
		GameObject gameUi = (GameObject) Instantiate(Resources.Load("GameUI"));
		gameUi.name = "GameUI";
		GUIManager.Instance.Initialiaze();

		Instantiate(Resources.Load("EventSystem"));
	}

	private void Update()
	{

		#if UNITY_ANDROID && !UNITY_EDITOR
		if(!Input.GetMouseButton(0))
		{
			if(_touching)
			{
				_touching = false;
				GUIManager.Instance.ShowPause();
			}

		} 
		else
		{
			if(!_touching)
			{
				_touching = true;
				GUIManager.Instance.HidePause();
			}
		}
		#endif
		
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

	
	public void LoadLevel(string level) {

		if (!System.String.IsNullOrEmpty(level)) 
			UnityEngine.SceneManagement.SceneManager.LoadScene(level);
		else 
			UnityEngine.SceneManagement.SceneManager.LoadScene(Application.loadedLevel);
    		
	}

}
