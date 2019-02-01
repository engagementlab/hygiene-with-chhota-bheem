public class LoadLevelEvent : GameEvent
{

	public readonly string level;

	public LoadLevelEvent(string levelName)
	{
		level = levelName;
	}
}
