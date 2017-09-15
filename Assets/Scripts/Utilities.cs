using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {


	public static Vector3 ClampToScreen(Vector3 vector, Camera camera) {

		Vector3 pos = camera.WorldToViewportPoint(vector);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		pos.z = 0;

		Vector3 worldPos = camera.ViewportToWorldPoint(pos);
		worldPos.x = Mathf.Clamp(worldPos.x, -6.9f, 6.9f);
		worldPos.z = 0;

		return worldPos;

	}
}
