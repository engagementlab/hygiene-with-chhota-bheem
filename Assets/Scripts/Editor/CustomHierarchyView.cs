using UnityEngine;
using UnityEditor;
using System.Text;

[InitializeOnLoad]
public class CustomHierarchyView  {

	/*private static StringBuilder sb = new StringBuilder ();

	static CustomHierarchyView() {
		EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
	}

	static void HierarchyWindowItemOnGUI (int instanceID, Rect selectionRect) {			
		GameObject gameObject = EditorUtility.InstanceIDToObject (instanceID) as GameObject;
		DisplayActiveAndPassiveCardCount (selectionRect, gameObject);
	}

	static void DisplayActiveAndPassiveCardCount (Rect selectionRect, GameObject gameObject)
	{
		Waypoint[] cards = gameObject.GetComponentsInChildren<Waypoint>(true);
		int total = cards.Length;
		if (total > 0 && gameObject.GetComponent<Waypoint> () == null) {
			Rect r = new Rect (selectionRect);
			r.x += r.width - 65;
			sb.Length = 0;
			sb.Append (total).Append ("()");
			GUI.Label (r, sb.ToString ());
		}
	}*/
}