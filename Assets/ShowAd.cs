using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShowAd : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{

		Debug.Log("ADS INIT");
	
		#if UNITY_IOS
			Advertisement.Initialize("2805293", false);
		Debug.Log("IOS");

		#endif
	
		#if UNITY_ANDROID
			Advertisement.Initialize("2805294", false);	
		#endif
	
		Debug.Log("Unity Ads initialized: " + Advertisement.isInitialized);
		Debug.Log("Unity Ads is supported: " + Advertisement.isSupported);
		Debug.Log("Unity Ads test mode enabled: " + Advertisement.debugMode);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
		public void ShowRewardedAd()
		{
			if (Advertisement.IsReady())
			{
				var options = new ShowOptions { resultCallback = HandleShowResult };
				Advertisement.Show();
			} else
			{
				Debug.LogError("Ads not ready: " + Advertisement.version);
			}
		}

		private void HandleShowResult(ShowResult result)
		{
			switch (result)
			{
				case ShowResult.Finished:
					Debug.Log("The ad was successfully shown.");
					//
					// YOUR CODE TO REWARD THE GAMER
					// Give coins etc.
					break;
				case ShowResult.Skipped:
					Debug.Log("The ad was skipped before reaching the end.");
					break;
				case ShowResult.Failed:
					Debug.LogError("The ad failed to be shown.");
					break;
			}
		}
	
}
