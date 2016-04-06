using Eritar.Framework.Entities.General;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class GameDataController : MonoBehaviour
{
  public static GameDataController Instance { get; protected set; }


  private List<RawResource> _rawResources;
  private List<Item> _items;
  private List<Unit> _units;
  private List<Building> _buildings;
  private List<Research> _Researchs;

  // Use this for initialization
  void Start()
  {
    LoadRawResources();
    LoadItems();
    LoadUnits();
    LoadBuildings();
  }

  // Use this for initialization
  void OnEnable()
  {
    if (Instance != null)
    {
      Debug.LogError("There should never be two gamedata controllers.");
    }

    Instance = this;
  }

  private void LoadRawResources()
  {
    _rawResources = new List<RawResource>();

    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\RawRessources";

    foreach (string file in Directory.GetFiles(s))
    {
      if (Path.GetExtension(file) != ".xml")
        continue;

      RawResource ressource = new RawResource();
      ressource = RawResource.LoadRawResourceFromXML(file);

      Debug.Log("Loaded Raw Ressource: " + ressource.ObjectName);

      _rawResources.Add(ressource);
    }
  }

  private void LoadItems()
  {
    _items = new List<Item>();

    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\Items";

    foreach (string file in Directory.GetFiles(s))
    {
      if (Path.GetExtension(file) != ".xml")
        continue;

      Item item = new Item();
      item = Item.LoadItemFromXML(file);

      Debug.Log("Loaded Item: " + item.ObjectName);

      _items.Add(item);
    }
  }

  private void LoadUnits()
  {
    _units = new List<Unit>();

    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\Units";

    foreach (string file in Directory.GetFiles(s))
    {
      if (Path.GetExtension(file) != ".xml")
        continue;

      Unit unit = new Unit();
      unit = Unit.LoadUnitFromXML(file);

      Debug.Log("Loaded Unit: " + unit.ObjectName);

      _units.Add(unit);
    }
  }

  private void LoadBuildings()
  {
    _buildings = new List<Building>();

    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\Buildings";

    foreach (string file in Directory.GetFiles(s))
    {
      if (Path.GetExtension(file) != ".xml")
        continue;

      Building building = new Building();
      building = Building.LoadBuildingFromXML(file);

      Debug.Log("Loaded Building: " + building.ObjectName);

      _buildings.Add(building);
    }
  }

  private void LoadResearch()
  {
    _Researchs = new List<Research>();

    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\Researchs";

    foreach (string file in Directory.GetFiles(s))
    {
      if (Path.GetExtension(file) != ".xml")
        continue;

      Research tech = new Research();
      tech = Research.LoadResearchFromXML(file);

      Debug.Log("Loaded Research: " + tech.ObjectName);

      _Researchs.Add(tech);
    }
  }

  private void SaveRawResources()
  {
    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\RawRessources";

    foreach (RawResource item in _rawResources)
    {
      string path = s + "\\" + item.ObjectName.Replace(' ', '_') + ".xml";
      RawResource.SaveRawResource(item, path);

      Debug.Log("Saved Raw Ressource: " + item.ObjectName);
    }
  }

  private void SaveItems()
  {
    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\Items";

    foreach (Item item in _items)
    {
      string path = s + "\\" + item.ObjectName.Replace(' ', '_') + ".xml";
      Item.SaveItem(item, path);

      Debug.Log("Saved Item: " + item.ObjectName);
    }
  }

  private void SaveUnits()
  {
    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\Units";

    foreach (Unit item in _units)
    {
      string path = s + "\\" + item.ObjectName.Replace(' ', '_') + ".xml";
      Unit.SaveUnit(item, path);

      Debug.Log("Saved Unit: " + item.ObjectName);
    }
  }

  private void SaveBuildings()
  {
    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\Buildings";

    foreach (Building item in _buildings)
    {
      string path = s + "\\" + item.ObjectName.Replace(' ', '_') + ".xml";
      Building.SaveBuilding(item, path);

      Debug.Log("Saved Building: " + item.ObjectName);
    }
  }

  private void SaveResearch()
  {
    string s = Directory.GetCurrentDirectory();

    s = s + "\\Assets\\Entities\\Researchs";

    foreach (Research item in _Researchs)
    {
      string path = s + "\\" + item.ObjectName.Replace(' ', '_') + ".xml";
      Research.SaveResearch(item, path);

      Debug.Log("Saved Research: " + item.ObjectName);
    }
  }
}
