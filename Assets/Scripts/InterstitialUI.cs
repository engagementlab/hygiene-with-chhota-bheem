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

	private Sprite[] _interstitialImages;
	
	public void OpenLevelInterstitial(int level)
	{
		
//		_levelsOpen = false;
//		_interstitialsOpen = true;
		GameConfig.CurrentLevel = level;
		
		_background = transform.Find("Background").gameObject;
		_interstitialScreen = _background.transform.Find("Image").gameObject.GetComponent<Image>();
		_interstitialScreenCount = 0;
		
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
			iTween.ScaleTo(BackButton.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeInElastic));
	
		gameObject.SetActive(true);
		
		iTween.MoveTo(PreviousScreen, iTween.Hash("position", new Vector3(540, 0, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeInBack));
		iTween.MoveFrom(gameObject, iTween.Hash("position", new Vector3(0, 970, 0), "time", 1, "islocal", true, "easetype", iTween.EaseType.easeOutBack, "delay", 1));
		
	}

	public void NextInterstitial()
	{
		_interstitialScreenCount++;
		iTween.MoveTo(_background, iTween.Hash("position", new Vector3(540, 0, 0), "time", .5f, "islocal", true, "easetype", iTween.EaseType.easeInBack, "oncomplete", "InterstitialSwap", "oncompletetarget", gameObject));
	}

	private void InterstitialSwap()
	{
		
		_interstitialScreen.GetComponent<Image>().sprite = _interstitialImages[_interstitialScreenCount];

		if (_interstitialScreenCount == 3) // Final screen 
		{
			// Show play button
			transform.Find("PlayButton").gameObject.SetActive(true);
			transform.Find("NextButton").gameObject.SetActive(false);
		}

		iTween.MoveTo(_background, iTween.Hash("position", new Vector3(0, 0, 0), "time", .5f, "islocal", true, "easetype", iTween.EaseType.easeOutBack));
		
	}
	
}
