using UnityEditor;
using UnityEngine;

public class DebugWindow : EditorWindow
{
  private Texture2D godModeTexture;
  
  [MenuItem("Window/Scene Debugging")] 
  public static void  ShowWindow () {
    GetWindow(typeof(DebugWindow));
  }
  
  private void OnGUI()
  {
    GUILayout.Label ("Scene Debugging Mode", EditorStyles.boldLabel);
    
    SceneEditor.ShowGizmos = EditorGUILayout.Toggle ("Show Gizmos", SceneEditor.ShowGizmos);
    
    EditorPrefs.SetBool("GodMode", EditorGUILayout.Toggle ("God Mode", EditorPrefs.GetBool("GodMode")));
    
    if(godModeTexture == null)
      godModeTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/editor/GodMode.png");
  
    if(EditorPrefs.GetBool("GodMode"))
      GUI.DrawTexture(new Rect(0, 75, 110, 110), godModeTexture);
  }
}