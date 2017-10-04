using UnityEngine;
using UnityEngine.UI;

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
	private int spellCount;
	private GameObject spellStepsUI;
	private GameObject[] spellSteps;
	private Animator[] spellStepsComponent;

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
		
		spellSize = spellBars[0].transform.Find("Background").GetComponent<RectTransform>().sizeDelta.y/5;

		for (int i = 0; i < spellBars.Length; i++)
		{
			spellBars[i].SetActive(false);
			
			var fill = spellBars[i].transform.Find("Background").GetComponent<RectTransform>();
			fill.sizeDelta = new Vector2( fill.sizeDelta.x, 0);

		}
		
		spellStepsUI = GameObject.Find("GameUI/SpellSteps");
		spellSteps = GameObject.FindGameObjectsWithTag("StepGroup");
		
		spellStepsUI.SetActive(false);
		spellCount = 0;
		
		foreach (GameObject group in spellSteps)
		{
			group.SetActive(false);
		}

	}
	
	private void SpellComplete(Spells type)
	{
		var animations = 0;
				
		spellStepsUI.SetActive(true);

		foreach (GameObject group in spellSteps)
		{
			if (group.name == type.ToString())
			{
				group.SetActive(true);
				spellStepsComponent = group.GetComponentsInChildren<Animator>();
						
				foreach (Animator step in spellStepsComponent)
				{
					step.Play("SpellStep");
					animations++;
			
					if (animations >= spellStepsComponent.Length)
					{
						Events.instance.Raise (new SpellEvent(type));
						group.SetActive(false);
						spellStepsUI.SetActive(false);
					}
				}
			}
		}		


				
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
		spellCount = 1;
	}

	public void AddSpellJuice(Spells type, GameObject fill)
	{
		Debug.Log("Adding juice for spell '" + type + "'");

		var spellFill = fill.GetComponent<RectTransform>();
		spellFill.sizeDelta = new Vector2( spellFill.sizeDelta.x, spellFill.sizeDelta.y + spellSize);
		spellCount++;

		if (spellCount == 5)
		{
			SpellComplete(type);
			EmptySpells();
		}

	}


	public void EmptySpells()
	{
		
		bar = GameObject.FindGameObjectWithTag("SpellBar");
				
		var fill = bar.transform.Find("Background").GetComponent<RectTransform>();
		fill.sizeDelta = new Vector2( fill.sizeDelta.x, 0);

		spellCount = 0;
		
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
