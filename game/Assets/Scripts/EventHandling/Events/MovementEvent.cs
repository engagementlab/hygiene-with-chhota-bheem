
public class MovementEvent : GameEvent {

	public readonly string Direction;
	public readonly bool EndClick;

	public MovementEvent (string direction, bool end = false) {
		Direction = direction;
		EndClick = end;
	}
}
