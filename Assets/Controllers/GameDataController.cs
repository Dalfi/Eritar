using Eritar.Framework.Entities.General;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

namespace Eritar
{
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
      LoadResearch();
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

    public RawResource GetRawResourceByName(string name)
    {
      return GetObjectByName<RawResource>(name, _rawResources);
    }

    public RawResource GetRawResourceByGUID(string guid)
    {
      return GetObjectByName<RawResource>(guid, _rawResources);
    }

    public Item GetItemByName(string name)
    {
      return GetObjectByName<Item>(name, _items);
    }

    public Item GetItemByGUID(string guid)
    {
      return GetObjectByName<Item>(guid, _items);
    }

    public Unit GetUnitByName(string name)
    {
      return GetObjectByName<Unit>(name, _units);
    }

    public Unit GetUnitByGUID(string guid)
    {
      return GetObjectByName<Unit>(guid, _units);
    }

    public Building GetBuildingByName(string name)
    {
      return GetObjectByName<Building>(name, _buildings);
    }

    public Building GetBuildingByGUID(string guid)
    {
      return GetObjectByName<Building>(guid, _buildings);
    }

    public Research GetResearchByName(string name)
    {
      return GetObjectByName<Research>(name, _Researchs);
    }

    public Research GetResearchByGUID(string guid)
    {
      return GetObjectByName<Research>(guid, _Researchs);
    }

    private T GetObjectByName<T>(string name, List<T> _objects) where T : WorldObject
    {
      List<T> lst = _objects.FindAll((o) => o.ObjectName.ToUpper() == name.ToUpper());

      if (lst.Count == 0)
      {
        Debug.LogWarning("There is no object with the name " + name);
        return default(T);
      }
      else if (lst.Count > 1)
      {
        Debug.LogWarning("There is more then one object with the name " + name);
        return default(T);
      }
      else
      {
        return lst[0];
      }
    }

    private T GetObjectByGUID<T>(string guid, List<T> _objects) where T : WorldObject
    {
      List<T> lst = _objects.FindAll((o) => o.ObjectGUID == guid);

      if (lst.Count == 0)
      {
        Debug.LogWarning("There is no object with the GUID " + guid);
        return default(T);
      }
      else if (lst.Count > 1)
      {
        Debug.LogWarning("There is more then one object with the GUID " + guid);
        return default(T);
      }
      else
      {
        return lst[0];
      }
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
}