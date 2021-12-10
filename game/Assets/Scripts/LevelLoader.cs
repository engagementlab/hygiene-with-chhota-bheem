using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	public Slider Slider;
	public RectTransform Logo;
	public Image Text;
	private AsyncOperation _sceneAo;
	
	// Use this for initialization
	private void Start ()
	{
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
		Slider.gameObject.SetActive(true);
		Logo.gameObject.SetActive(true);
		Text.gameObject.SetActive(true);

		iTween.ScaleFrom(Logo.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutBack));
		
		yield return new WaitForSeconds(1);
		
		_sceneAo = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

		// disable scene activation while loading to prevent auto load
		_sceneAo.allowSceneActivation = false;

		while (!_sceneAo.isDone) {
			Slider.value = _sceneAo.progress;

			if (_sceneAo.progress >= .9f) {
				Slider.value = 1f;
				_sceneAo.allowSceneActivation = true;
			}
			yield return null;
		}
		
	}
}
