using UnityEditor;
using UnityEngine;

public class DebugWindow : EditorWindow
{
  [MenuItem ("Window/Scene Debugging")]

  public static void  ShowWindow () {
    GetWindow(typeof(DebugWindow));
  }
  
  private void OnGUI()
  {
    GUILayout.Label ("Scene Debugging Mode", EditorStyles.boldLabel);
    
    SceneEditor.ShowGizmos = EditorGUILayout.Toggle ("Show Gizmos", SceneEditor.ShowGizmos);
  }
}