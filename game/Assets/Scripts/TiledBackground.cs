/* 

Hygiene With Chhota Bheem
Created by Engagement Lab @ Emerson College, 2017-2019

==============
	TiledBackground.cs
	Tile and position background texture inside scene container canvas.

	Created by Johnny Richardson.
==============

*/

using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TiledBackground : MonoBehaviour {

	[HideInInspector]
	public BackgroundImage Image;
	
	public enum BackgroundType
	{
		Grass,
		Dirt,
		Rock
	}
	public BackgroundType BackgroundImg;
	
	private const float SquareSize = 8;
	private BackgroundType _currentImgType;
	private RectTransform _canvasRect;

	private void Start ()
	{

		_canvasRect = GetComponent<RectTransform>();
		
		var sizeY = _canvasRect.rect.height;
		var repY = sizeY / SquareSize;

		if(Image == null)
			Image = GetComponentInChildren<BackgroundImage>();
		
		var imgRect = Image.uvRect;
		imgRect.height = repY;
		Image.uvRect = imgRect;
		
	}
	
	#if UNITY_EDITOR
	private void Update()
	{

		if(Application.isPlaying) return;

		if(Image == null)
			Image = GetComponentInChildren<BackgroundImage>();

		// Change image
		if(BackgroundImg != _currentImgType)
		{
			_currentImgType = BackgroundImg;

			var imgName = BackgroundImg.ToString().ToLower();
			var imgAssetAtPath = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Sprites/backgrounds/" + imgName + "-bg.png");
			Image.texture = imgAssetAtPath;
		}

		if(Image == null) return;

		// Get lists of ArchetypeMove transforms ordered by x/y pos
		MonoBehaviour[] layoutObjects = FindObjectsOfType<ArchetypeMove>();
		MonoBehaviour[] props = FindObjectsOfType<ArchetypeProp>();
		ArrayUtility.AddRange(ref layoutObjects, props);
		
		var transformsY = layoutObjects.Select(t => t.transform).Where(t => t.gameObject.layer != 8).OrderBy(t => t.position.y).ToArray();
		var transformsX = layoutObjects.Select(t => t.transform).Where(t => t.gameObject.layer != 8).OrderBy(t => t.position.x).ToArray();
		
		if(transformsX.Length == 0 || transformsY.Length == 0) return;
		
		var xPosFirst = transformsX.First().position;
		var yPosLast = transformsY.Last().position;
		
		var topLeftPos = new Vector3(xPosFirst.x, yPosLast.y);

		// L/R game boundaries
		var rightBound = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)).x;
		var leftBound = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.nearClipPlane)).x;
		
		if(_canvasRect == null)
			_canvasRect = GetComponent<RectTransform>();
		
		// Size and position canvas of sprite
		_canvasRect.sizeDelta = new Vector2(rightBound - leftBound, topLeftPos.y + 6);

		var sizeY = _canvasRect.rect.height;
		var repY = sizeY / SquareSize;

		var imgRect = Image.uvRect;
		imgRect.height = repY;
		Image.uvRect = imgRect;
		
	}
	#endif
}