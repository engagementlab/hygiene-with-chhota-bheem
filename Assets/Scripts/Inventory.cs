using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace DefaultNamespace
{
  public class Inventory
  {

    public int count = 0;
    static Inventory _instanceInternal;
    public static Inventory instance
    {
      get { return _instanceInternal ?? (_instanceInternal = new Inventory()); }
    }

    

    public void AddSpellComponent(Spells spell)
    {

      if(count == 4)
      {
        // Reset Count
        count = 0;
        // New Spell Event
        Events.instance.Raise(new SpellEvent(spell));
        
        // Remove Spell JUICE UI
        GUIManager.Instance.EmptySpells();
      } 
      else
      {
        count++;
      }
      
    }
    
  }
}