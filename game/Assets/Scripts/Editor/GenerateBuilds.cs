/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017-2019

==============
	GenerateBuilds.cs
	Custom build menu and methods for setting player preferences.
	https://github.com/engagementgamelab/hygiene-with-chhota-bheem/blob/master/Assets/Scripts/Editor/GenerateBuilds.cs

	Created by Johnny Richardson.
==============

*/
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

class GenerateBuilds {

    // These are the scenes that the build server is going to use
    private static readonly string[] ScenesMobile = { "Test" };

    // Options for all builds
    private static BuildOptions _buildOptions = BuildOptions.Development | BuildOptions.AllowDebugging;

    // Scene directory
    private const string _scenePrefix = "Assets/Scenes/";

    private const string SceneAffix = ".unity";

    private const string AppName = "Hygiene With Chhota Bheem";
    private const string TargetDir = "bin";

    [MenuItem ("Build/Build Android")]
    private static void PerformAndroidBuild ()
    {
     
        GenericBuild("Android", BuildTarget.Android);
    }
    
    [MenuItem ("Build/Build Debug")]
    private static void MakeDebugBuilds()
    {
        _buildOptions = BuildOptions.None;
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "IS_DEBUG");

        PerformAndroidBuild();
    }

    [MenuItem ("Build/Build Mac OS X Universal")]
    private static void PerformMacOsxBuild ()
    {
        
        GenericBuild("Mac", BuildTarget.StandaloneOSX);
    }
    [MenuItem ("Build/Build PC")]
    private static void PerformPcBuild ()
    {
        
        GenericBuild("PC", BuildTarget.StandaloneWindows);
    }
    

    [MenuItem ("Build/Build All")]
    private static void MakeAllBuilds()
    {

        PerformMacOsxBuild();
        PerformPcBuild();
        PerformAndroidBuild();
    }

    [MenuItem ("Build/Build Desktop")]
    private static void MakeDesktopBuilds()
    {

        PerformMacOsxBuild();
        PerformPcBuild();
    }
    
    [MenuItem ("Build/Set Icons")]
    private static void SetIcons ()
    {
        SetIcons (BuildTarget.StandaloneOSX);
    }

    private static void SetIcons(BuildTarget buildTarget)
    {
        if (buildTarget == BuildTarget.Android) {
            Texture2D[] textures = new [] {
                (Texture2D)Resources.Load ("AppIcons/icon_logo_1024"),
                (Texture2D)Resources.Load ("AppIcons/icon_logo_512"),
                (Texture2D)Resources.Load ("AppIcons/icon_logo_256"),
                (Texture2D)Resources.Load ("AppIcons/icon_logo_128"),
                (Texture2D)Resources.Load ("AppIcons/icon_logo_48"),
                (Texture2D)Resources.Load ("AppIcons/icon_logo_32"),
                (Texture2D)Resources.Load ("AppIcons/icon_logo_16")
            };

            PlayerSettings.SetIconsForTargetGroup (BuildTargetGroup.Standalone, textures);
        }
    }

    private static string[] FindEnabledScenes(string platform) {

        string[] sceneList = ScenesMobile;

        List<string> editorScenes = new List<string>();

        foreach (string sceneName in sceneList)
            editorScenes.Add(_scenePrefix + sceneName + SceneAffix);

        return editorScenes.ToArray();

    }

    /// <summary>
    /// Generate a game binary using the given options.
    /// </summary>
    /// <param name="platform">The platform name, used as the name for the folder to contain the platform's binary.</param>
    /// <param name="buildTarget">The Unity build target.</param>
    private static void GenericBuild(string platform, BuildTarget buildTarget)
    {

        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

        // Disable all but 4x3
        PlayerSettings.SetAspectRatio(UnityEditor.AspectRatio.Aspect4by3, true);
        PlayerSettings.SetAspectRatio(UnityEditor.AspectRatio.Aspect16by10, false);
        PlayerSettings.SetAspectRatio(UnityEditor.AspectRatio.Aspect16by9, false);
        PlayerSettings.SetAspectRatio(UnityEditor.AspectRatio.Aspect5by4, false);
        PlayerSettings.SetAspectRatio(UnityEditor.AspectRatio.AspectOthers, false);

        // Disable splash and res dialog
        PlayerSettings.showUnitySplashScreen = false;
        PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;

        PlayerSettings.productName = AppName;
        PlayerSettings.companyName = "Engagement Lab at Emerson College";

        string name = AppName;

        if(platform == "Mac")
            name = AppName + ".app";
        else if(platform == "PC") 
            name = AppName + ".exe"; 
        else if(platform == "Android")
            name = AppName + ".apk";        

        BuildReport res = BuildPipeline.BuildPlayer(FindEnabledScenes(platform), TargetDir + "/" + platform + "/" + name, buildTarget, _buildOptions);

        if (res.summary.totalErrors > 0)
            throw new Exception("BuildPlayer failure: " + res);

        // Reset define symbols for all groups
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, null);

    }

}
#endif