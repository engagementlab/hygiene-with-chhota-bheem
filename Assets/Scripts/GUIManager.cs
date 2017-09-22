using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Unity.Linq;

public class GUIManager
{
	static GUIManager _instanceInternal;
	public static GUIManager Instance
	{
		get
		{
			if(_instanceInternal == null)
				_instanceInternal = new GUIManager();

			return _instanceInternal;
		}
	}
	
	private RectTransform inventoryUI;
	private GameObject powerUpText;
	private GameObject pauseUI;
	private Animator pauseAnimator;

	private Text fliesCount;
	private Text villagerCount;
	private Text score;

	// Use this for initialization
	public void Initialiaze ()
	{

		inventoryUI = GameObject.Find("UI/Inventory").GetComponent<RectTransform>();
		powerUpText = GameObject.Find("UI/PowerUpText");
		
		pauseUI = GameObject.Find("UI/PauseUI");
		pauseAnimator = pauseUI.GetComponent<Animator>();

		powerUpText.SetActive(false);

		fliesCount = GameObject.Find("UI/Score/FlyCount").GetComponent<Text>();
		villagerCount = GameObject.Find("UI/Score/VillagerCount").GetComponent<Text>();
		score = GameObject.Find("UI/Score/ScoreCount").GetComponent<Text>();
		
	}

	public void DisplayCurrentPowerUp(string powerUpName)
	{
		
		powerUpText.GetComponent<Text>().text = "Power up: " + powerUpName;
		powerUpText.SetActive(true);
		
	}
	
	public void HidePowerUp()
	{
		
		powerUpText.SetActive(false);
		
	}

	public void ShowSpellComponent(SpellComponent component)
	{
		
		var img = inventoryUI.Find(component.ToString()).GetComponent<Image>();
		img.enabled = true;
		
		Inventory.instance.AddSpellComponent(component);
		
	}

	public void EmptySpells()
	{
		
		var spellIcons = inventoryUI.gameObject.Children().OfComponent<Image>().ToArray();
		foreach(var spellIcon in spellIcons)
			spellIcon.enabled = false;
		
	}

	public void UpdateScore(float num, string type) {

		if (type == "Villager") {
			villagerCount.text = GameConfig.peopleSaved.ToString();
		} else if (type == "Fly") {
			fliesCount.text = GameConfig.fliesCaught.ToString();
		}

		score.text = (float.Parse(score.text) + num).ToString();

	}

	public void ShowPause()
	{
		pauseAnimator.Play("ShowPause");
	}

	public void HidePause()
	{
//		pauseUI.SetActive(true);
		pauseAnimator.Play("HidePause");
	}
}
