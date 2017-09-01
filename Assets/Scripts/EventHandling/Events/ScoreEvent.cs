using UnityEngine;

public class ScoreEvent : GameEvent {

	public readonly float scoreAmount;
	public readonly Type eventType;

	public enum Type {
		Good,
		Bad
	}

	public ScoreEvent (float amount, Type scoreType) {
		scoreAmount = amount;
		eventType = scoreType;
	}
}
