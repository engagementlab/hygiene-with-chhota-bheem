using UnityEngine;

public class BossIconEvent : GameEvent
{

	public readonly string iconType;
	public readonly GameObject obj;

	public BossIconEvent (string type, GameObject gameObj)
	{
		iconType = type;
		obj = gameObj;
	}
}
