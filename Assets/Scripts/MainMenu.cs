using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

	public RectTransform SignObject;
	public RectTransform LogoObject;
	public RectTransform PlayObject;
	public RectTransform SettingsObject;
	public RectTransform InfoObject;

	// Use this for initialization
	void Start()
	{

//		Hashtable baseParams = iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic);

		iTween.ScaleFrom(SignObject.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeInOutElastic));
		iTween.RotateFrom(SignObject.gameObject, iTween.Hash("z", 190, "time", 2, "easetype", iTween.EaseType.easeOutElastic, "delay", .5f));
		iTween.ScaleFrom(LogoObject.gameObject, iTween.Hash("scale", Vector3.zero, "time", 2, "easetype", iTween.EaseType.easeOutElastic, "delay", .7f));

		iTween.ScaleFrom(PlayObject.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.1f));
		iTween.ScaleFrom(SettingsObject.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.7f));
		iTween.ScaleFrom(InfoObject.gameObject, iTween.Hash("scale", Vector3.zero, "time", 1, "easetype", iTween.EaseType.easeOutElastic, "delay", 2.9f));

	}
}
