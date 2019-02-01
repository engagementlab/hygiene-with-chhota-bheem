using UnityEngine;

public class BubbleEvent : GameEvent {

	public readonly Collider collider;

	public BubbleEvent (Collider thisCollider) {
		collider = thisCollider;
	}
}
