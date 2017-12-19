using UnityEngine;
using UnityEngine.UI;

public class InterstitialUI : MonoBehaviour
{

	public GameObject PreviousScreen;
	public Button BackButton;
	public Button OldBack;
	
	private GameObject _interstitialsBack;
	private GameObject _background;
	private Image _interstitialScreen;
	private int _interstitialScreenCount;

	private GameObject _interstitialsParent;

	private Sprite[] _interstitialImages;

	private Button _nextButton;
	private Button _playButton;

	private bool _animating;
	
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
		OldBack.gameObject.SetActive(false);
		
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
		_interstitialScreen.sprite = _interstitialImages[_interstitialScreenCount];
		
		if(BackButton != null)
			iTween.ScaleTo(BackButton.gameObject, iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeInElastic));
		
		iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(0, 970, 0), "time", .01f, "islocal", true));
		gameObject.SetActive(true);
		iTween.MoveTo(PreviousScreen, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "PreviousFinished", "oncompletetarget", gameObject));
		
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
		
		_interstitialScreenCount++;
		iTween.MoveTo(_background, iTween.Hash("position", new Vector3(540, 0, 0), "time", .5f, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "InterstitialSwap", "oncompletetarget", gameObject));
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

	}

	private void EndAnimation()
	{
		_animating = false;
	}
	
}
