using System.Linq;
using LetterboxCamera;
using UnityEngine;
using UnityEngine.UI;

public class InterstitialUI : MonoBehaviour
{

	public RectTransform MenuParent;
	public GameObject PreviousScreen;
	public Button BackButton;

	public CanvasGroup paddingLeft;
	public CanvasGroup paddingRight;
	public CanvasGroup paddingTop;
	public CanvasGroup paddingBottom;
	
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
	private bool _showBack;
	
	private GameObject _oldBack;

	public void OpenLevelInterstitial(int level, bool hideBack=false)
	{
	
		Vector3[] objectCorners = new Vector3[4];
		paddingRight.gameObject.GetComponent<RectTransform>().GetWorldCorners(objectCorners);
		// Show left padding only if right shows in screen width
		if(objectCorners[1].x > Screen.width)
			paddingLeft.gameObject.active = false;
		
		// Show top padding only if device has iPhoneX+ dimensions
		if(Screen.width >= 1125 && Screen.height >= 2436)
		{
			paddingTop.gameObject.active = true;
			paddingBottom.gameObject.active = true;
		}
		
		string chapterKey = "";
		
		GameConfig.CurrentLevel = level;
		_interstitialScreenCount = 0;
		_showBack = !hideBack;
		
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
				chapterKey = "One";
				break;
				
			case 1:
				chapterKey = "Two";
				break;
				
			case 2:
				chapterKey = "Three";
				break;
		}		
		_interstitialImages = Resources.LoadAll<Sprite>("Chap" + chapterKey + "Interstitials");
		
		// Localization switch (NOTE: could be handled much better)
		if(GameConfig.CurrentLanguage == 1)
		 _interstitialImages = _interstitialImages.Where(i=> i.name.IndexOf("-en-us") == -1).ToArray();
		else
			_interstitialImages = _interstitialImages.Where(i=> i.name.IndexOf("-in-ta") == -1).ToArray();
		
		_thisScreen.sprite = _interstitialImages[_interstitialScreenCount];
		_nextScreen.sprite = _interstitialImages[_interstitialScreenCount+1];
		_firstImgParent.GetComponent<CanvasGroup>().alpha = 0;
		_secondImgParent.GetComponent<CanvasGroup>().alpha = 0;

		gameObject.SetActive(true);
		
		iTween.MoveTo(PreviousScreen, iTween.Hash("position", new Vector3(Screen.width+(Screen.width / 2), 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "PreviousFinished", "oncompletetarget", gameObject));
		
	}

	public void OpenInterstitial(int level)
	{
		OpenLevelInterstitial(level);
	}

	private void FadePadding(float alpha)
	{	
		paddingLeft.alpha = alpha;
		paddingRight.alpha = alpha;
		paddingBottom.alpha = alpha;
		paddingTop.alpha = alpha;
	}

	private void PreviousFinished()
	{
		
		_nextButton.gameObject.SetActive(true);
		_nextButton.transform.localScale = Vector3.one;
		GetComponent<CanvasGroup>().alpha = 1;

		if(BackButton != null)
		{
			BackButton.gameObject.SetActive(_showBack);
			iTween.ScaleFrom(BackButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic));
		}

		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", .7f, "onupdate", "FadeFirstImage"));
		iTween.ScaleFrom(_nextButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic));
		
		// Fade in padding
		paddingLeft.alpha = 0;
		paddingRight.alpha = 0;
		paddingBottom.alpha = 0;
		paddingTop.alpha = 0;
		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 1, "onupdate", "FadePadding"));
		
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
			_nextButton.gameObject.SetActive(false);
			
			iTween.ScaleFrom(_playButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", .7f, "easetype", iTween.EaseType.easeOutElastic));
			
			return;
			
		}
		_nextScreen.sprite = _interstitialImages[_interstitialScreenCount+1];
	}
	
}
