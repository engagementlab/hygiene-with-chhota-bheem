public class SpellEvent : GameEvent {
	
	public readonly Spells powerType;
	public bool powerUp;

	public SpellEvent (Spells type, bool power) {

		powerType = type;
		powerUp = power;

	}
}
