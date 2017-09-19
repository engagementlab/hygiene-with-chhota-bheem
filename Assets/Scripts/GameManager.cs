using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject VillagerPrefab;
	private float deltaTime;
	private bool touching = false;

	private void Awake()
	{
		GUIManager.Instance.Initialiaze();
	}

	private void Update()
	{

		if(!Input.GetMouseButton(0))
		{
			if(touching)
			{
				touching = false;
				GUIManager.Instance.ShowPause();
			}

		} 
		else
		{
			if(!touching)
			{
				touching = true;
				GUIManager.Instance.HidePause();
			}
		}
		
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f; 
	}

	private void OnGUI()
	{
//		if(!Debug.isDebugBuild) return;
		
		int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		
		GUI.Label(rect, text, style);
		if(GUI.Button(new Rect(0, 40, 100, 50), "Stress Test"))
		{
			for(var i = 0; i < 45; i++)
			{
				Instantiate(VillagerPrefab, new Vector3(Random.Range(-2, 2), Random.Range(0, 20), 0), Quaternion.identity);
			}
		}
	}

}
