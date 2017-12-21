using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	public Slider slider;
	public RectTransform Logo;
	AsyncOperation sceneAO;
	
	// Use this for initialization
	void Start ()
	{

//		slider = transform.Find("Slider").GetComponent<Slider>();
		
		Events.instance.AddListener<LoadLevelEvent> (LoadLevel);
		
	}
	
	private void OnDestroy() {

		Events.instance.RemoveListener<LoadLevelEvent> (LoadLevel);

	}

	
	void LoadLevel(LoadLevelEvent e)
	{
		
		var baseName = "Level";
		switch(GameConfig.CurrentChapter)
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
		
		baseName += GameConfig.CurrentLevel == 1 ? "2" : "1";
		GameConfig.CurrentScene = baseName;
		GameConfig.LevelPlayCount = 1;
		
		if(!GameConfig.DictWonCount.ContainsKey(baseName))
			GameConfig.DictWonCount = new Dictionary<string, int>{{baseName, 0}};
		
		if(!GameConfig.DictLostCount.ContainsKey(baseName))
			GameConfig.DictLostCount = new Dictionary<string, int>{{baseName, 0}};
		
		StartCoroutine(LoadingSceneRealProgress(baseName));
	}
	
	IEnumerator LoadingSceneRealProgress(string sceneName)
	{
		GetComponent<Image>().enabled = true;
		slider.gameObject.SetActive(true);
		Logo.gameObject.SetActive(true);

		iTween.PunchRotation(Logo.gameObject, iTween.Hash("amount", new Vector3(0, 0, 7), "time", 1, "easetype", iTween.EaseType.easeInOutBounce));
		
		yield return new WaitForSeconds(1);
		
		sceneAO = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

		// disable scene activation while loading to prevent auto load
		sceneAO.allowSceneActivation = false;

		while (!sceneAO.isDone) {
			slider.value = sceneAO.progress;

			if (sceneAO.progress >= .9f) {
				slider.value = 1f;
				sceneAO.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
