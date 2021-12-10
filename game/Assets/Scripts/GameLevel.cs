using UnityEngine;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour
{
	
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
	private void Awake ()
	{
		if(GameConfig.GameSpeedModifier == 0)
			GameConfig.GameSpeedModifier = gameSpeed;
	}

	private void Start()
	{
		if(speedGainText == null) return;
		
		speedGainText.text = GameConfig.NumBubblesSpeedGained + "";
		speedGainSlider.value = GameConfig.NumBubblesSpeedGained;

		intervalText.text = GameConfig.NumBubblesInterval + "";
		intervalSlider.value = GameConfig.NumBubblesInterval;

		gameSpeedText.text = GameConfig.GameSpeedModifier + "";
		gameSpeedSlider.value = GameConfig.GameSpeedModifier;

		bubbleOffsetText.text = GameConfig.BubbleOffset + "";
		bubbleOffsetSlider.value = GameConfig.BubbleOffset;
	}

	public void AdjustOffset()
	{
		GameConfig.BubbleOffset = bubbleOffsetSlider.value;
		bubbleOffsetText.text = bubbleOffsetSlider.value + "";
	}

	public void AdjustSpeedGain()
	{
		GameConfig.NumBubblesSpeedGained = speedGainSlider.value;
		speedGainText.text = speedGainSlider.value + "";
	}

	public void AdjustInterval()
	{
		GameConfig.NumBubblesInterval = intervalSlider.value;
		intervalText.text = intervalSlider.value + "";
	}

	public void AdjustSpeed()
	{
		GameConfig.GameSpeedModifier = gameSpeedSlider.value;
		gameSpeedText.text = gameSpeedSlider.value + "";
	}
	
}
