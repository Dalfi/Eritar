using Eritar.Framework;
using Eritar.Framework.Entities.General;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Eritar
{
  public class BuildingController : MonoBehaviour
  {
    public static BuildingController Instance { get; protected set; }

    private Dictionary<Building, GameObject> buildingGameObjectMap;

    public List<GameObject> buildingPrefabs;

    private World world
    {
      get { return WorldController.Instance.world; }
    }

    // Use this for initialization
    void OnEnable()
    {
      if (Instance != null)
      {
        Debug.LogError("There should never be two building controllers.");
      }

      Instance = this;
    }

    // Use this for initialization
    void Start()
    {
      buildingGameObjectMap = new Dictionary<Building, GameObject>();

      // Register our callback so that our GameObject gets updated whenever
      // the tile's type changes.
      world.RegisterBuildingCreated(OnBuildingCreated);
      WorldController.Instance.RegisterSelectionChanged(OnSelectionChanged);
    }

    public void OnBuildingCreated(Building building)
    {
      GameObject prefab = GetPrefab(building.ObjectName);

      // Create a visual GameObject linked to this data.
      GameObject buil_go = (GameObject)Instantiate(prefab, building.Position, building.Rotation);
      buil_go.name = building.ObjectName + "_" + building.Position.x + "_" + building.Position.z;
      buil_go.transform.SetParent(this.transform, true);

      buildingGameObjectMap.Add(building, buil_go);

    }

    public void OnSelectionChanged(WorldObject wo)
    {
      if (wo != null && wo.GetType() != typeof(Building))
        return; // was not a building so don't matter for us here

      if(wo == null) //Deselecting
      {
        if (buildingGameObjectMap.Keys.Count((b) => b.IsSelected) <= 0)
          return;
        else
        {
          foreach (var item in buildingGameObjectMap.Where((b)=>b.Key.IsSelected))
          {
            item.Key.IsSelected = false;
            UpdateSelection(item.Value);
          }
        }
      }
      else
      {
        //Selection a building
        Building b = (Building)wo;
        b.IsSelected = true;
        UpdateSelection(buildingGameObjectMap[b]);        
      }
    }

    public GameObject GetPrefab(string tag)
    {
      List<GameObject> lst = buildingPrefabs.FindAll((go) => go.tag == tag);

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

    internal Building BuildingSelected (GameObject go)
    {
      if (buildingGameObjectMap.ContainsValue(go))
      {
        Building b = buildingGameObjectMap.FirstOrDefault((bgom) => bgom.Value == go).Key;

        if (b != null)
        {
          return b;
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
