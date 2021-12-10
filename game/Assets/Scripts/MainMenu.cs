﻿using System.Collections;
using System.Linq;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

	public RectTransform SignObject;
	public RectTransform LogoObject;
	public RectTransform PlayObject;
	public RectTransform SettingsObject;
	public RectTransform InfoObject;
	
	private GameObject[] _bubbles;

	void Awake()
	{
		
		SignObject.localScale = Vector3.zero;
		LogoObject.localScale = Vector3.zero;
		PlayObject.localScale = Vector3.zero;
		SettingsObject.localScale = Vector3.zero;
		InfoObject.localScale = Vector3.zero;
		
		_bubbles = GameObject.FindGameObjectsWithTag("GUIBubble");
		
	}
	
	// Use this for initialization
	void Start()
	{
		
		iTween.ScaleTo(SignObject.gameObject, iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeInOutElastic));
		iTween.RotateFrom(SignObject.gameObject, iTween.Hash("z", 190, "time", 2, "easetype", iTween.EaseType.easeOutElastic, "delay", .5f));
		iTween.ScaleTo(LogoObject.gameObject, iTween.Hash("scale", Vector3.one, "time", 2, "easetype", iTween.EaseType.easeOutElastic, "delay", .7f));

		iTween.ScaleTo(PlayObject.gameObject, iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.1f));
		iTween.ScaleTo(SettingsObject.gameObject, iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.7f));
		iTween.ScaleTo(InfoObject.gameObject, iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.9f));

		StartCoroutine(AnimateBubbles());
		
	}
	
	private IEnumerator AnimateBubbles()
	{
		
		System.Random rnd = new System.Random();
		_bubbles = _bubbles.OrderBy(x => rnd.Next()).ToArray();

		foreach(GameObject t in _bubbles)
			t.transform.localScale = Vector3.zero;

		yield return new WaitForSeconds(1.3f);
		
		for(var b = 0; b < _bubbles.Length; b++)
			iTween.ScaleTo(_bubbles[b], iTween.Hash("scale", Vector3.one, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", Random.Range(.3f, .5f) * b*.5f)); 
		
	}
}
