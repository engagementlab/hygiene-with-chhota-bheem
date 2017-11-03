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

      
    }
    
  }
}