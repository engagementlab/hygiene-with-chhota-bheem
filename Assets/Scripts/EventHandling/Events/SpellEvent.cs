using DefaultNamespace;

public class SpellEvent : GameEvent {
	
	public readonly Spells powerType;

	public SpellEvent (Spells type) {

		powerType = type;
		
	}
}
