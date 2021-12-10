public class GameEndEvent : GameEvent
{

	public readonly bool wonGame;
	public readonly string killerName;

	public GameEndEvent(bool won, string killer=null)
	{
		wonGame = won;
		killerName = killer;
	}
}
