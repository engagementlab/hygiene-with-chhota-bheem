using UnityEngine;
using UnityEditor;
using System.Text;

[InitializeOnLoad]
public class CustomHierarchyView
{
	private static readonly Texture2D WaypointIcon;

	static CustomHierarchyView() {
	  WaypointIcon = AssetDatabase.LoadAssetAtPath ("Assets/Editor/waypoint-icon.png", typeof(Texture2D)) as Texture2D;
		EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGui;
	}

	private static void HierarchyWindowItemOnGui(int instanceID, Rect selectionRect) {			
		var gameObject = EditorUtility.InstanceIDToObject (instanceID) as GameObject;
		
		if(gameObject != null)
			DisplayItemGui(selectionRect, gameObject);
	}

	private static void DisplayItemGui(Rect selectionRect, GameObject gameObject)
	{
		
		var waypoint = gameObject.GetComponent<Waypoint>();
		if(waypoint == null) return;
		
		var r = new Rect (selectionRect);
		r.x += r.width - 15;
		GUI.Label(r, WaypointIcon);
		
	}
}