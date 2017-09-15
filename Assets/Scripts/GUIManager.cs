using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
	private static GUIManager _instanceInternal;
	public static GUIManager Instance
	{
		get { return _instanceInternal ?? (_instanceInternal = new GUIManager()); }
	}
	
	private RectTransform inventoryUI;

	// Use this for initialization
	public void Initialiaze ()
	{

		inventoryUI = GameObject.Find("Inventory").GetComponent<RectTransform>();

	}

	public void ShowSpellComponent(SpellComponent component)
	{
		
		inventoryUI.Find(component.ToString()).GetComponent<Image>().enabled = true;
		
	}
}
