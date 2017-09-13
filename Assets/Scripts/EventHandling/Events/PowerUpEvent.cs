using DefaultNamespace;

public class PowerUpEvent : GameEvent {
	
	public readonly PowerUps powerType;

	public PowerUpEvent (PowerUps type) {

		powerType = type;
		
	}
}
