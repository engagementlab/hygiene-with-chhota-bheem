using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Unity.Linq;
using JetBrains.Annotations;

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
	
	private GameObject inventoryUI;
	private GameObject spellText;
	private GameObject pauseUI;
	private Animator pauseAnimator;

	private Text fliesCount;
	private Text villagerCount;
	private Text score;

	// Use this for initialization
	public void Initialiaze ()
	{

		inventoryUI = GameObject.Find("GameUI/SpellJuiceBars");
		spellText = GameObject.Find("GameUI/SpellJuiceBars/SpellText");
		
		pauseUI = GameObject.Find("GameUI/PauseUI");
		pauseAnimator = pauseUI.GetComponent<Animator>();

		spellText.SetActive(false);

		fliesCount = GameObject.Find("GameUI/Score/FlyCount").GetComponent<Text>();
		villagerCount = GameObject.Find("GameUI/Score/VillagerCount").GetComponent<Text>();
		score = GameObject.Find("GameUI/Score/ScoreCount").GetComponent<Text>();
		
	}

	public void DisplayCurrentSpell(string spellName)
	{
		
		spellText.GetComponent<Text>().text = "Spell: " + spellName;
		spellText.SetActive(true);
		
	}
	
	public void HideSpell()
	{
		
		spellText.SetActive(false);
		
	}

	public void AddSpellJuice(SpellComponent component)
	{
		
		
		
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
