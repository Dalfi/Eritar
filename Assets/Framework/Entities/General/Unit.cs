using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public class Unit : WorldObject, IsConstructable, INeedResearch
  {
    private List<Ingredient> _ingredients;

    private int _hitpoints;
    private int _armor;
    private int _range;
    private float _accuracy;
    private float _attackDamage;
    private float _attackSpeed;
    private float _trainingTime;
    private float _moveSpeed;
    private float _rotateSpeed;
    private float _buildingTime;

    public List<Ingredient> Ingredients
    {
      get { return _ingredients; }
      set { _ingredients = value; }
    }

    public int Hitpoints
    {
      get { return _hitpoints; }
      set { _hitpoints = value; }
    }

    public int Armor
    {
      get { return _armor; }
      set { _armor = value; }
    }

    public int Range
    {
      get { return _range; }
      set { _range = value; }
    }

    public float Accuracy
    {
      get { return _accuracy; }
      set { _accuracy = value; }
    }

    public float AttackDamage
    {
      get { return _attackDamage; }
      set { _attackDamage = value; }
    }

    public float AttackSpeed
    {
      get { return _attackSpeed; }
      set { _attackSpeed = value; }
    }

    public float MoveSpeed
    {
      get { return _moveSpeed; }
      set { _moveSpeed = value; }
    }

    public float RotateSpeed
    {
      get { return _rotateSpeed; }
      set { _rotateSpeed = value; }
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

    public List<string> NeededResearch { get; set; }

    public bool IsNeededResearchCompleted
    {
      get
      {
        return this.Owner.CheckIfResearchIsResearched(NeededResearch);
      }
    }

    public Unit()
    {
    }

    protected Unit(Unit u)
      :base()
    {
      this.ObjectName = u.ObjectName;
      this.ObjectGUID = u.ObjectGUID;
      this.ObjectDescription = u.ObjectDescription;
      this.IconFilePath = u.IconFilePath;
      this.Ingredients = u.Ingredients;
      this.Hitpoints = u.Hitpoints;
      this.BuildingTime = u.BuildingTime;
      this.Armor = u.Armor;
      this.Accuracy = u.Accuracy;
      this.MoveSpeed = u.MoveSpeed;
      this.RotateSpeed = u.RotateSpeed;
      this.Range = u.Range;
      this.AttackDamage = u.AttackDamage;
      this.AttackSpeed = u.AttackSpeed;
    }

    virtual public Unit Clone()
    {
      return new Unit(this);
    }

    public override void Update(float deltaTime)
    {
      if (IsInConstruction)
      {
        BuildingTime = BuildingTime - deltaTime;
      }
      else
      {

      }
    }

    public static bool SaveUnit(Unit obj, string FilePath)
    {
      obj.PrepareSaveObject();

      XmlSerializer serializer = new XmlSerializer(typeof(Unit));
      using (TextWriter writer = new StreamWriter(FilePath))
      {
        // Serializes the purchase order, and closes the TextWriter.
        serializer.Serialize(writer, obj);
        writer.Close();
      }

      return true;
    }

    public static Unit LoadUnitFromXML(string FilePath)
    {
      Unit obj;

      XmlSerializer serializer = new XmlSerializer(typeof(Unit));
      serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
      serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

      // A FileStream is needed to read the XML document.
      using (FileStream fs = new FileStream(FilePath, FileMode.Open))
      {
        obj = (Unit)serializer.Deserialize(fs);
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
