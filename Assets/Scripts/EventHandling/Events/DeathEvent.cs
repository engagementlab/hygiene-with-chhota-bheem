using UnityEngine;

public class DeathEvent : GameEvent
{

	public readonly bool wonGame;

	public DeathEvent(bool won)
	{
		wonGame = won;
	}
}
