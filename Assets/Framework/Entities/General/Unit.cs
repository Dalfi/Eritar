﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Eritar.Framework.Entities.General
{
  public class Unit : WorldObject, IsConstructable, INeedResearch
  {
    private Dictionary<RawResource, int> _rawIngredients;
    private Dictionary<Item, int> _ingredients;

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

    public float TrainigTime
    {
      get { return _trainingTime; }
      set { _trainingTime = value; }
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

    public override void Update(float deltaTime)
    {
    }

    public static bool SaveUnit(Unit obj, string FilePath)
    {
      obj.PrepareSaveObject();

      XmlSerializer serializer = new XmlSerializer(typeof(Unit));
      TextWriter writer = new StreamWriter(FilePath);

      // Serializes the purchase order, and closes the TextWriter.
      serializer.Serialize(writer, obj);
      writer.Close();

      return true;
    }

    public static Unit LoadUnitFromXML(string FilePath)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Unit));
      serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
      serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

      // A FileStream is needed to read the XML document.
      FileStream fs = new FileStream(FilePath, FileMode.Open);

      Unit obj = (Unit)serializer.Deserialize(fs);

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
