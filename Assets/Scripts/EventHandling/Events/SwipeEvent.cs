using UnityEngine;

public class SwipeEvent : GameEvent {

	public readonly TKSwipeDirection dir;
	public readonly float velocity;

	public SwipeEvent (TKSwipeDirection thisDir, float vel) {
		dir = thisDir;
		velocity = vel;
	}
}
