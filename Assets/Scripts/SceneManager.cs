using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class SceneManager : MonoBehaviour
{
	
	private static GameObject camera;
	
	static SceneManager()
	{
		
		EditorApplication.hierarchyWindowItemOnGUI += LoadCamera;
//		EditorSceneManager.sceneSaved += OnSceneSaved;
	}
 
	static void OnSceneSaved(UnityEngine.SceneManagement.Scene scene)
	{
			DestroyImmediate(Camera.main.gameObject);
			EditorSceneManager.sceneSaved -= OnSceneSaved;
			EditorSceneManager.SaveScene(scene);
	}

	static void LoadCamera(int instanceID, Rect selectionRect) 
	{
					if(FindObjectOfType(typeof(Camera)))
				return;
			
			// Load main camera into scene
			camera = Resources.Load<GameObject>("SceneCamera");
			PrefabUtility.InstantiatePrefab(camera);
	}

	private static void DeleteCamera(Scene scene, bool removingScene)
	{
	}
	
}
