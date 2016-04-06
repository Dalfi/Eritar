using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public class Building : WorldObject, IsConstructable, INeedResearch
  {
    private Dictionary<RawResource, int> _rawIngredients;
    private Dictionary<Item, int> _ingredients;
    private List<Unit> _units;
    private Queue<Unit> _buildQueue;

    private int _hitpoints;
    private float _buildingTime;

    public Dictionary<RawResource, int> RawIngredients
    {
      get { return _rawIngredients; }
      set { _rawIngredients = value; }
    }

    public Dictionary<Item, int> Ingredients
    {
      get { return _ingredients; }
      set { _ingredients = value; }
    }

    public List<Unit> Units
    {
      get { return _units; }
      set { _units = value; }
    }

    public Queue<Unit> BuildQueue
    {
      get { return _buildQueue; }
      protected set { _buildQueue = value; }
    }

    public int Hitpoints
    {
      get { return _hitpoints; }
      set { _hitpoints = value; }
    }

    public float BuildingTime
    {
      get { return _buildingTime; }
      set { _buildingTime = value; }
    }

    public bool IsInConstruction
    {
      get { return BuildingTime > 0; }
    }

    public bool IsNeededResearchCompleted
    {
      get
      {
        return this.Owner.CheckIfResearchIsResearched(NeededResearch);
      }
    }

    public List<string> NeededResearch { get; set; }

    public override void Update(float deltaTime)
    {
      if (IsInConstruction)
      {
        BuildingTime = BuildingTime - deltaTime;
      }
      else
      {
        if (BuildQueue.Count > 0)
        {

        }
      }
    }


    public static bool SaveBuilding(Building obj, string FilePath)
    {
      obj.PrepareSaveObject();

      XmlSerializer serializer = new XmlSerializer(typeof(Building));
      TextWriter writer = new StreamWriter(FilePath);

      // Serializes the purchase order, and closes the TextWriter.
      serializer.Serialize(writer, obj);
      writer.Close();

      return true;
    }

    public static Building LoadBuildingFromXML(string FilePath)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Building));
      serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
      serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

      // A FileStream is needed to read the XML document.
      FileStream fs = new FileStream(FilePath, FileMode.Open);

      Building obj = (Building)serializer.Deserialize(fs);

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
