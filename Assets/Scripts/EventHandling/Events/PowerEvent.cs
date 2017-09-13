using UnityEngine;

public class PowerEvent : GameEvent {

	public readonly Type powerType;

	public enum Type {
		Matrix,
		SpeedShoot,
		ScatterShoot
	}

	public PowerEvent (Type type) {

		powerType = type;
		
	}
}
