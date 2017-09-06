using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour
{

	public bool sandBoxMode;
	
	[Range(0, 30)]
	public float gameSpeed = 1;
	
	public Slider gameSpeedSlider;
	public Text gameSpeedText;
	
	public Slider bubbleOffsetSlider;
	public Text bubbleOffsetText;
	
	public Slider speedGainSlider;
	public Text speedGainText;
	
	public Slider intervalSlider;
	public Text intervalText;

	// Use this for initialization
	void Awake ()
	{
		GameConfig.sandboxMode = sandBoxMode;
		
		if(GameConfig.gameSpeedModifier == 0)
			GameConfig.gameSpeedModifier = gameSpeed;
	}

	void Start()
	{
		if(speedGainText == null) return;
		
		speedGainText.text = GameConfig.numBubblesSpeedGained + "";
		speedGainSlider.value = GameConfig.numBubblesSpeedGained;

		intervalText.text = GameConfig.numBubblesInterval + "";
		intervalSlider.value = GameConfig.numBubblesInterval;

		gameSpeedText.text = GameConfig.gameSpeedModifier + "";
		gameSpeedSlider.value = GameConfig.gameSpeedModifier;

		bubbleOffsetText.text = GameConfig.bubbleOffset + "";
		bubbleOffsetSlider.value = GameConfig.bubbleOffset;
	}

	public void AdjustOffset()
	{
		GameConfig.bubbleOffset = bubbleOffsetSlider.value;
		bubbleOffsetText.text = bubbleOffsetSlider.value + "";
	}

	public void AdjustSpeedGain()
	{
		GameConfig.numBubblesSpeedGained = speedGainSlider.value;
		speedGainText.text = speedGainSlider.value + "";
	}

	public void AdjustInterval()
	{
		GameConfig.numBubblesInterval = intervalSlider.value;
		intervalText.text = intervalSlider.value + "";
	}

	public void AdjustSpeed()
	{
		GameConfig.gameSpeedModifier = gameSpeedSlider.value;
		gameSpeedText.text = gameSpeedSlider.value + "";
	}
	
}
