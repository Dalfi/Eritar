using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public class Item : WorldObject
  {
    private List<Ingredient> _ingredients;

    private float _craftingSpeed;
    private float _craftingTime;

    public List<Ingredient> Ingredients
    {
      get { return _ingredients; }
      set { _ingredients = value; }
    }

    public float CraftingSpeed
    {
      get { return _craftingSpeed; }
      set { _craftingSpeed = value; }
    }

    public float CraftingTime
    {
      get { return _craftingTime; }
      set { _craftingTime = value; }
    }

    public override void Update(float deltaTime)
    {
    }

    public static bool SaveItem(Item obj, string FilePath)
    {
      obj.PrepareSaveObject();

      XmlSerializer serializer = new XmlSerializer(typeof(Item));

      using (TextWriter writer = new StreamWriter(FilePath))
      {
        // Serializes the purchase order, and closes the TextWriter.
        serializer.Serialize(writer, obj);
        writer.Close();
      }

      return true;
    }

    public static Item LoadItemFromXML(string FilePath)
    {
      Item obj;

      XmlSerializer serializer = new XmlSerializer(typeof(Item));
      serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
      serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

      // A FileStream is needed to read the XML document.
      using (FileStream fs = new FileStream(FilePath, FileMode.Open))
      {
        obj = (Item)serializer.Deserialize(fs);
      }

      return obj;
    }

    protected static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
    {
      Debug.LogError("Unknown Node:" + e.Name + "\t" + e.Text);
    }

    protected static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
    {
      System.Xml.XmlAttribute attr = e.Attr;
      Debug.LogError("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
    }
  }
}
