using UnityEngine;

public class ScoreEvent : GameEvent {

	public readonly int scoreAmount;
	public readonly Type eventType;

	public enum Type {
		Fly,
		Snake, 
		Scorpion,
		Villager, 
		Wizard
	}

	public ScoreEvent (int amount, Type scoreType) {
		scoreAmount = amount;
		eventType = scoreType;
	}
}
