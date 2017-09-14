using System;
using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
  public class Inventory
  {

    public readonly List<SpellComponent> SpellComponentsNeeded = Enum.GetValues(typeof(SpellComponent)).Cast<SpellComponent>().ToList();
    private List<SpellComponent> SpellInventory;
    
    static Inventory _instanceInternal;
    public static Inventory instance
    {
      get { return _instanceInternal ?? (_instanceInternal = new Inventory()); }
    }

    public bool HasSpell(SpellComponent component)
    {
      return SpellInventory.Contains(component);
    }

    public void AddSpellComponent(SpellComponent component)
    {
      SpellComponentsNeeded.Remove(component);
      SpellInventory.Add(component);
    }
    
  }
}