using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public class Ingredient
  {
    private RawResource _RawResource;
    private Item _item;

    public RawResource RawResource
    {
      get { return _RawResource; }
      protected set { _RawResource = value; Changed(); }
    }

    public Item Item
    {
      get { return _item; }
      protected set { _item = value; Changed(); }
    }

    public int Amount { get; set; }

    public Ingredient()
    {
    }

    public Ingredient(Item item, int amount)
    {
      this.Amount = amount;
      this.Item = item;
    }

    public Ingredient(RawResource rawResource, int amount)
    {
      this.Amount = amount;
      this.RawResource = rawResource;
    }

    private void Changed()
    {
      if(this.RawResource != null && Item != null)
      {
        Debug.LogError("A ingredient can only be a rawresource or a item not both!");
      }
    }
  }
}
