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

	public GameObject[] spellBars;
	public float spellSize;
	private GameObject bar;

	// Use this for initialization
	public void Initialiaze ()
	{ 
		inventoryUI = GameObject.Find("GameUI/SpellJuiceBars");
		
		spellBars = GameObject.FindGameObjectsWithTag("SpellBar");
		spellText = GameObject.Find("GameUI/SpellJuiceBars/SpellText");
		
		// pauseUI = GameObject.Find("GameUI/PauseUI");
		// pauseAnimator = pauseUI.GetComponent<Animator>();

		spellText.SetActive(false);

		fliesCount = GameObject.Find("GameUI/Score/FlyCount").GetComponent<Text>();
		villagerCount = GameObject.Find("GameUI/Score/VillagerCount").GetComponent<Text>();
		score = GameObject.Find("GameUI/Score/ScoreCount").GetComponent<Text>();

		for (int i = 0; i < spellBars.Length; i++)
		{
			spellBars[i].SetActive(false);
			var fill = spellBars[i].transform.Find("Background").GetComponent<RectTransform>();
			fill.sizeDelta = new Vector2( fill.sizeDelta.x, 0);

		}

		spellSize = spellBars[0].transform.Find("Background/Fill").GetComponent<RectTransform>().sizeDelta.y/5;

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

	public void NewSpell(GameObject spellBar)
	{
		bar = GameObject.FindGameObjectWithTag("SpellBar");
		
		if (bar != null)
			bar.SetActive(false);
		
		spellBar.SetActive(true);
	}

	public void AddSpellJuice(Spells type, GameObject fill)
	{
		Debug.Log("Adding juice for spell '" + type + "'");

		var spellFill = fill.GetComponent<RectTransform>();
		spellFill.sizeDelta = new Vector2( spellFill.sizeDelta.x, spellFill.sizeDelta.y + spellSize);
	}

	public void EmptySpells()
	{
		
		bar = GameObject.FindGameObjectWithTag("SpellBar");
		
		if (bar != null)
			bar.SetActive(false);
		
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
