/*

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017-2019

==============
	ImportTextures.cs
	Unity texture asset import settings.
	
	Created by Johnny Richardson.
==============

*/
// Run only if inside editor
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TextureCompressionQuality = UnityEditor.TextureCompressionQuality;

public class TexturePostProcessor : AssetPostprocessor {

	// Texture import setting vars
	int _qualityBest = (int)TextureCompressionQuality.Best;
	int _qualityGood = (int)TextureCompressionQuality.Normal;
	int _qualityFast = (int)TextureCompressionQuality.Fast;
	
	// Set per-platform texture settings
	void OnPostprocessTexture(Texture2D texture) {
	
		TextureImporter importer = assetImporter as TextureImporter;

		importer.anisoLevel = 0;
		importer.isReadable = true;
		importer.mipmapEnabled = false;
		importer.filterMode = FilterMode.Trilinear;

		// Set texture settings
		importer.SetPlatformTextureSettings( "Android", 1024, TextureImporterFormat.DXT5, _qualityBest, false ); //ATC_RGBA8 \\ ETC2_RGBA8 || PVRTC_RGBA4

		Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
		if (asset)
			EditorUtility.SetDirty(asset);
		else 
			texture.anisoLevel = 0;

	}

}
#endif