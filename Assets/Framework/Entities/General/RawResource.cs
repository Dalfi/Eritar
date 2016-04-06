using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public enum GatherType
  {
    Cut,
    Mine,
    Harvest
  }

  public class RawResource : WorldObject
  {
    private float _harvestSpeed;
    private int _harvestAmount;
    private int _amountLeft;
    private GatherType _gatherType;

    public float HarvestSpeed
    {
      get { return _harvestSpeed; }
      set { _harvestSpeed = value; }
    }

    public int HarvestAmount
    {
      get { return _harvestAmount; }
      set { _harvestAmount = value; }
    }

    public int AmountLeft
    {
      get { return _amountLeft; }
      set { _amountLeft = value; }
    }

    public GatherType GatherType
    {
      get { return _gatherType; }
      set { _gatherType = value; }
    }

    public override void Update(float deltaTime)
    {

    }

    public static bool SaveRawResource(RawResource obj, string FilePath)
    {
      obj.PrepareSaveObject();

      XmlSerializer serializer = new XmlSerializer(typeof(RawResource));
      TextWriter writer = new StreamWriter(FilePath);

      // Serializes the purchase order, and closes the TextWriter.
      serializer.Serialize(writer, obj);
      writer.Close();

      return true;
    }

    public static RawResource LoadRawResourceFromXML(string FilePath)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(RawResource));
      serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
      serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

      // A FileStream is needed to read the XML document.
      FileStream fs = new FileStream(FilePath, FileMode.Open);

      RawResource obj = (RawResource)serializer.Deserialize(fs);

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
