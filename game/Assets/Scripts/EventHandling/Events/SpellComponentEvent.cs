using DefaultNamespace;

public class SpellComponentEvent : GameEvent {
	
	public readonly SpellComponent ComponentType;
	public readonly bool SpawnPickup;
	public readonly bool GiveToPlayer;
	

	public SpellComponentEvent (bool spawn)
	{

		SpawnPickup = spawn;

		if(spawn) return;
		
		//var neededCt = Inventory.instance.SpellComponentsNeeded.Count;
		
		//ComponentType = Inventory.instance.SpellComponentsNeeded[UnityEngine.Random.Range(0, neededCt)];
		GiveToPlayer = true;

	}
}
