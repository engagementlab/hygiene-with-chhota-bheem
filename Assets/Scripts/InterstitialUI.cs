using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class InterstitialUI : MonoBehaviour
{

	public GameObject PreviousScreen;
	public Button BackButton;
	
	private GameObject _interstitialsBack;
	private GameObject _background;
	private Image _interstitialScreen;
	private int _interstitialScreenCount;

	private GameObject _interstitialsParent;

	private Sprite[] _interstitialImages;

	private Button _nextButton;
	private Button _playButton;

	private bool _animating;

	private GameObject[] _steps;
	private GameObject _step;
	private GameObject[] _stepGroups;
	private GameObject _stepParent;
	private GameObject _stepsVillager;
	private GameObject[] _stepsSoap;
	
	private GameObject[] _stepTargets;
	private int _currentStep;
	
	public void OpenLevelInterstitial(int level)
	{
		GameConfig.CurrentLevel = level;
		
		_background = transform.Find("Background").gameObject;
		_interstitialScreen = _background.transform.Find("Image").gameObject.GetComponent<Image>();
		_interstitialScreenCount = 0;
		
		_nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
		_playButton = transform.Find("PlayButton").gameObject.GetComponent<Button>();
		_nextButton.gameObject.SetActive(true);
		_playButton.gameObject.SetActive(false);

		_stepGroups = new[]
		{
			transform.Find("Background/Image/StepsOne").gameObject, 
			transform.Find("Background/Image/StepsTwo").gameObject, 
			transform.Find("Background/Image/StepsThree").gameObject
		};

		foreach (GameObject group in _stepGroups)
		{
			group.SetActive(false);
		}
		
		switch (GameConfig.CurrentChapter)
		{
			case 0:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapOneInterstitials");
				
				_stepParent = _stepGroups[0];
				break;
				
			case 1:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapTwoInterstitials");
				
				_stepParent = _stepGroups[1];
				break;
				
			case 2:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapThreeInterstitials");
				
				_stepParent = _stepGroups[2];
				break;
		}
		
		_stepParent.SetActive(true);
		
		_stepsVillager = _stepParent.transform.Find("Villager").gameObject;
		
		_stepsSoap = new []
		{
			_stepParent.transform.Find("SoapBackground").gameObject, 
			_stepParent.transform.Find("Soap").gameObject
		};
		
		_interstitialScreen.sprite = _interstitialImages[_interstitialScreenCount];
		
		if(BackButton != null)
			iTween.ScaleTo(BackButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeInElastic));
		
		iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(0, 970, 0), "time", .01f, "islocal", true));
		gameObject.SetActive(true);
		iTween.MoveTo(PreviousScreen, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "PreviousFinished", "oncompletetarget", gameObject));
		
		SetContent();

	}

	private void SetContent()
	{
		_steps = GameObject.FindGameObjectsWithTag("Step");

		_stepTargets = GameObject.FindGameObjectsWithTag("StepTarget");

		foreach (GameObject step in _steps)
		{
			step.transform.localScale = Vector3.zero;
		}

		foreach (GameObject soap in _stepsSoap)
		{
			soap.transform.localScale = Vector3.zero;
		}
				
	}

	private void PreviousFinished()
	{
		iTween.MoveTo(gameObject, iTween.Hash("position", Vector3.zero, "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
	}

	public void NextInterstitial()
	{
		if (_animating || _interstitialScreenCount == _interstitialImages.Length - 1)
			return;

		_animating = true;
		
		iTween.MoveTo(_background, iTween.Hash("position", new Vector3(540, 0, 0), "time", .5f, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "InterstitialSwap", "oncompletetarget", gameObject));
		
		_interstitialScreenCount++;

	}

	private void InterstitialSwap()
	{
		
		_interstitialScreen.GetComponent<Image>().sprite = _interstitialImages[_interstitialScreenCount];

		if (_interstitialScreenCount == _interstitialImages.Length - 1) // Final screen 
		{
			// Show play button
			_playButton.gameObject.SetActive(true);
			_nextButton.gameObject.SetActive(false);
		}

		iTween.MoveTo(_background, iTween.Hash("position", new Vector3(0, 0, 0), "time", .5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "EndAnimation", "oncompletetarget", gameObject));
		
		if (_interstitialScreenCount == 2)
		{
			// Animate the Spell Steps
			StartCoroutine(IntersitialSpellSteps());
		}

		if (_interstitialScreenCount > 2)
		{
			_stepParent.SetActive(false);
		}
	}

	private void EndAnimation()
	{
		_animating = false;
	}

	IEnumerator IntersitialSpellSteps()
	{
		yield return new WaitForSeconds(0.5f);
		
		_nextButton.interactable = false;

		for (int i = 0; i < _steps.Length; i++)
		{
			_currentStep = i;
			
			iTween.ScaleTo(_steps[i], iTween.Hash("scale", new Vector3(2, 2, 2), "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
			
			yield return new WaitForSeconds(1f);

			if (GameConfig.CurrentChapter == 1)
			{
				// Scale Arrow & Hands
				GameObject hands = _steps[i].transform.Find("Wash").gameObject;
				GameObject arrow = _steps[i].transform.Find("Arrow").gameObject;
				_step = _steps[i].transform.Find("SpellStep").gameObject;
				
				iTween.ScaleTo(arrow, iTween.Hash("scale", Vector3.zero, "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
				iTween.ScaleTo(hands, iTween.Hash("scale", Vector3.zero, "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
				
				yield return new WaitForSeconds(0.5f);
								
				arrow.SetActive(false);
				hands.SetActive(false);
								
				// Fix Width of Parent
				iTween.ValueTo(_steps[i], iTween.Hash("from", _steps[i].GetComponent<RectTransform>().sizeDelta, "to", _step.GetComponent<RectTransform>().sizeDelta, "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "onupdate", "UpdateRectSize", "onupdatetarget", gameObject));				
				iTween.ValueTo(_step, iTween.Hash("from", _step.GetComponent<RectTransform>().anchoredPosition3D, "to", new Vector3(46, -42, 0), "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "onupdate", "UpdateRectShift", "onupdatetarget", gameObject));
				
				// Continue
				yield return new WaitForSeconds(0.5f);
				
			}
			
			Vector3 position = _stepTargets[i].GetComponent<RectTransform>().anchoredPosition3D;
			
			iTween.ValueTo(_steps[i], iTween.Hash("from", _steps[i].GetComponent<RectTransform>().anchoredPosition3D, "to", position, "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "onupdate", "UpdateRectSteps", "onupdatetarget", gameObject));
			
			yield return new WaitForSeconds(0.1f);
			
			iTween.ScaleTo(_steps[i], iTween.Hash("scale", Vector3.one, "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "oncomplete", "EndStep", "oncompletetarget", gameObject, "oncompleteparams", i));

			yield return new WaitForSeconds(0.5f);
		}
		
	}
	
	private void UpdateRectSize(Vector2 size)
	{
		_steps[_currentStep].GetComponent<RectTransform>().sizeDelta = size;
	}

	private void UpdateRectSteps(Vector3 position)
	{
		_steps[_currentStep].GetComponent<RectTransform>().anchoredPosition3D = position;
	}
	
	private void UpdateRectShift(Vector3 position)
	{
		_step.GetComponent<RectTransform>().anchoredPosition3D = position;	
	}

	private void UpdateAlpha(float alpha)
	{
		_stepsVillager.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
	}
	
	IEnumerator IntersitialVillagerSoap()
	{
		yield return new WaitForSeconds(0.5f);
		
		iTween.ValueTo(_stepsVillager, iTween.Hash("from", 0, "to", 1, "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.linear, "onupdate", "UpdateAlpha", "onupdatetarget", gameObject));

		yield return new WaitForSeconds(0.5f);
		
		iTween.ScaleTo(_stepsSoap[0], iTween.Hash("scale", Vector3.one, "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
		
		yield return new WaitForSeconds(0.5f);

		iTween.ScaleTo(_stepsSoap[1], iTween.Hash("scale", Vector3.one, "time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
		
		yield return new WaitForSeconds(0.5f);
		
		_nextButton.interactable = true;

	}

	private void ResetSteps()
	{
		_stepParent.SetActive(false);
	}

	private void EndStep(int count)
	{
		if (count == _steps.Length - 1)
		{
			StartCoroutine(IntersitialVillagerSoap());
		}
	}
	
}
