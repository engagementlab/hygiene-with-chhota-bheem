using UnityEngine;

public class ScoreEvent : GameEvent {

	public readonly int scoreAmount;

	public ScoreEvent (int amount) {
		scoreAmount = amount;
	}
}
