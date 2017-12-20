using UnityEngine;
using UnityEngine.UI;

public class InterstitialUI : MonoBehaviour
{

	public GameObject PreviousScreen;
	public Button BackButton;
	
	private GameObject _interstitialsBack;
	private GameObject _firstImgParent;
	private GameObject _secondImgParent;
	private Image _thisScreen;
	private Image _nextScreen;
	private int _interstitialScreenCount;

	private GameObject _interstitialsParent;

	private Sprite[] _interstitialImages;

	private Button _nextButton;
	private Button _playButton;

	private bool _animating;
	
	private GameObject _oldBack;

	
	public void OpenLevelInterstitial(int level)
	{
		GameConfig.CurrentLevel = level;
		_interstitialScreenCount = 0;
		
		_firstImgParent = transform.Find("FirstImage").gameObject;
		_secondImgParent= transform.Find("SecondImage").gameObject;
		_thisScreen = _firstImgParent.transform.Find("Image").gameObject.GetComponent<Image>();
		_nextScreen = _secondImgParent.transform.Find("Image").gameObject.GetComponent<Image>();
		
		_nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
		_playButton = transform.Find("PlayButton").gameObject.GetComponent<Button>();
		
		// Defaults
		BackButton.gameObject.SetActive(false);
		_nextButton.gameObject.SetActive(false);
		_playButton.gameObject.SetActive(false);
		
		_oldBack = GameObject.Find("MenuUI/ChaptersLevels/Buttons/Back");
		if (_oldBack != null)
			_oldBack.gameObject.SetActive(false);
		
		switch (GameConfig.CurrentChapter)
		{
			case 0:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapOneInterstitials");
				break;
				
			case 1:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapTwoInterstitials");
				break;
				
			case 2:
				_interstitialImages = Resources.LoadAll<Sprite>("ChapThreeInterstitials");
				break;
		}
		
		_thisScreen.sprite = _interstitialImages[_interstitialScreenCount];
		_nextScreen.sprite = _interstitialImages[_interstitialScreenCount+1];
		_firstImgParent.GetComponent<CanvasGroup>().alpha = 0;
		_secondImgParent.GetComponent<CanvasGroup>().alpha = 0;

		gameObject.SetActive(true);
		iTween.MoveTo(PreviousScreen, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "PreviousFinished", "oncompletetarget", gameObject));
		
	}

	private void PreviousFinished()
	{
		
		_nextButton.gameObject.SetActive(true);
		_nextButton.transform.localScale = Vector3.one;
		GetComponent<CanvasGroup>().alpha = 1;

		if(BackButton != null)
		{
			BackButton.gameObject.SetActive(true);
			iTween.ScaleFrom(BackButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic));
		}

		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FadeFirstImage"));
		iTween.ScaleFrom(_nextButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "delay", 1, "easetype", iTween.EaseType.easeOutElastic));
		
	}

	public void NextInterstitial()
	{
		if (_animating || _interstitialScreenCount == _interstitialImages.Length - 1)
			return;
		
		_animating = true;
		
		_nextButton.interactable = false;
		_playButton.interactable = false;
		BackButton.interactable = false;
		
		_interstitialScreenCount++;
		iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", .7f, "onupdate", "FadeImage", "oncomplete", "EndAnimation", "oncompletetarget", gameObject));
	}
	
	private void FadeFirstImage(float alpha)
	{
		_firstImgParent.GetComponent<CanvasGroup>().alpha = alpha;
	}
	
	private void FadeImage(float alpha)
	{
		_firstImgParent.GetComponent<CanvasGroup>().alpha = alpha;
		_secondImgParent.GetComponent<CanvasGroup>().alpha = 1-alpha;
	}

	private void EndAnimation()
	{
		_animating = false;
		_nextButton.interactable = true;
		_playButton.interactable = true;
		BackButton.interactable = true;

		_thisScreen.sprite = _nextScreen.sprite;
		_firstImgParent.GetComponent<CanvasGroup>().alpha = 1;
		_secondImgParent.GetComponent<CanvasGroup>().alpha = 0;
		
		if (_interstitialScreenCount == _interstitialImages.Length - 1) // Final screen 
		{
			// Show play button
			_playButton.gameObject.SetActive(true);
			iTween.ScaleTo(_nextButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeInBack));
			iTween.ScaleFrom(_playButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "delay", 1, "easetype", iTween.EaseType.easeOutElastic));
			
			return;
			
		}
		_nextScreen.sprite = _interstitialImages[_interstitialScreenCount+1];
	}
	
}
