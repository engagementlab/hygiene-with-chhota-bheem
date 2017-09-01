using UnityEngine;

public class HitEvent : GameEvent {

	public readonly Collider collider;
	public readonly GameObject bubble;
	public readonly Type eventType;

	public enum Type {
		Spawn,
		PowerUp
	}

	public HitEvent (Type hitType, Collider thisCollider, GameObject thisBubble) {

		eventType = hitType;
		collider = thisCollider;
		bubble = thisBubble;
		
	}
}
