using Eritar.Framework;
using Eritar.Framework.Entities.General;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Eritar
{
  public class UnitController : MonoBehaviour
  {
    public static UnitController Instance { get; protected set; }

    private Dictionary<Unit, GameObject> unitGameObjectMap;
    private Dictionary<string, int> unitCount;

    public List<GameObject> unitPrefabs;

    private World world
    {
      get { return WorldController.Instance.world; }
    }

    // Use this for initialization
    void OnEnable()
    {
      if (Instance != null)
      {
        Debug.LogError("There should never be two unit controllers.");
      }

      Instance = this;
    }

    // Use this for initialization
    void Start()
    {
      unitGameObjectMap = new Dictionary<Unit, GameObject>();
      unitCount = new Dictionary<string, int>();

      // Register our callback so that our GameObject gets updated whenever
      // the tile's type changes.
      world.RegisterUnitCreated(OnUnitCreated);
      WorldController.Instance.RegisterSelectionChanged(OnSelectionChanged);
    }

    public void OnUnitCreated(Unit unit)
    {
      int count = 0;

      if (unitCount.ContainsKey(unit.ObjectName))
        count = unitCount[unit.ObjectName];
      else
        unitCount.Add(unit.ObjectName, 0);

      count++;

      GameObject prefab = GetPrefab(unit.ObjectName);

      // Create a visual GameObject linked to this data.
      GameObject buil_go = (GameObject)Instantiate(prefab, unit.Position, unit.Rotation);
      buil_go.name = unit.ObjectName + "_" + count;
      buil_go.transform.SetParent(this.transform, true);

      unitGameObjectMap.Add(unit, buil_go);

      unitCount[unit.ObjectName] = count;
    }

    public void OnSelectionChanged(WorldObject wo)
    {
      if (wo != null && wo.GetType() != typeof(Unit))
        return; // was not a unit so don't matter for us here

      if (wo == null) //Deselecting
      {
        if (unitGameObjectMap.Keys.Count((b) => b.IsSelected) <= 0)
          return;
        else
        {
          foreach (var item in unitGameObjectMap.Where((b) => b.Key.IsSelected))
          {
            item.Key.IsSelected = false;
            UpdateSelection(item.Value);
          }
        }
      }
      else
      {
        //Selection a building
        Unit u = (Unit)wo;
        u.IsSelected = true;
        UpdateSelection(unitGameObjectMap[u]);
      }
    }

    public GameObject GetPrefab(string tag)
    {
      List<GameObject> lst = unitPrefabs.FindAll((go) => go.tag == tag);

      if (lst.Count == 0)
      {
        Debug.LogWarning("There is no object with the tag " + tag);
        return null;
      }
      else if (lst.Count > 1)
      {
        Debug.LogWarning("There is more then one object with the tag " + tag);
        return null;
      }
      else
      {
        return lst[0];
      }
    }

    internal Unit UnitSelected(GameObject go)
    {
      if (unitGameObjectMap.ContainsValue(go))
      {
        Unit u = unitGameObjectMap.FirstOrDefault((bgom) => bgom.Value == go).Key;

        if (u != null)
        {
          return u;
        }
      }

      return null;
    }

    private void UpdateSelection(GameObject go)
    {
      Debug.Log("Selected: " + go.name);
    }
  }
}
