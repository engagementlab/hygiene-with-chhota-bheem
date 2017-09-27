using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace DefaultNamespace
{
  public class Inventory
  {

    public List<SpellComponent> SpellComponentsNeeded = Enum.GetValues(typeof(SpellComponent)).Cast<SpellComponent>().ToList();
    
    [NotNull]
    private readonly List<SpellComponent> SpellInventory = new List<SpellComponent>();
    
    static Inventory _instanceInternal;
    public static Inventory instance
    {
      get { return _instanceInternal ?? (_instanceInternal = new Inventory()); }
    }

    public bool HasSpell(SpellComponent component)
    {
      return SpellInventory.Contains(component);
    }
    
    public void ChangeSpell(SpellComponent component)
    {
      SpellInventory.Clear();
      SpellInventory.Add(component);
      
    }

    public void AddSpellComponent(SpellComponent component)
    {

      if(SpellInventory.Count == 2)
      {
        var powerUpGiven = Enum.GetValues(typeof(Spells)).Cast<Spells>().ToList()[UnityEngine.Random.Range(0, 2)];
        Events.instance.Raise(new SpellEvent(powerUpGiven));

        SpellComponentsNeeded = Enum.GetValues(typeof(SpellComponent)).Cast<SpellComponent>().ToList();
        SpellInventory.Clear();

        GUIManager.Instance.EmptySpells();
      } 
      else
      {    
        SpellComponentsNeeded.Remove(component);
        SpellInventory.Add(component);
      }
      
    }
    
  }
}