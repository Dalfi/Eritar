using Eritar.Framework;
using Eritar.Framework.Entities.General;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eritar
{
  public class WorldController : MonoBehaviour
  {
    public static WorldController Instance { get; protected set; }

    public World world { get; protected set; }

    private Action<WorldObject> cbSelectionChanged;

    // Use this for initialization
    void OnEnable()
    {
      if (Instance != null)
      {
        Debug.LogError("There should never be two world controllers.");
      }

      Instance = this;

      CreateEmptyWorld();
    }
    
    // Update is called once per frame
    void Update()
    {
      world.Update(Time.deltaTime);
    }

    /// <summary>
    /// ONLY FOR TESTING!
    /// </summary>
    void CreateEmptyWorld()
    {
      Debug.Log("Creating new empty world");
      // Create a world with Empty tiles
      world = new World(256, 256, 6);

      BoxCollider col = this.gameObject.AddComponent<BoxCollider>();
      col.size = new Vector3(world.GetCorrectWidth(), 1, world.GetCorrectHeight());
      col.center = new Vector3(world.GetCorrectWidth() / 2, 0, world.GetCorrectHeight() / 2);
      col.tag = "Ground";

      // Center the Camera
      Camera.main.transform.position = new Vector3(world.Width / 2, Camera.main.transform.position.y, world.Height / 2);
    }

    public bool WorldObjectSelected(GameObject go)
    {
      WorldObject WorldObject = null;

      WorldObject = BuildingController.Instance.BuildingSelected(go);

      if (WorldObject != null)
      {
        OnSelectionChanged(WorldObject); //Let things know that the Selection changed
        return true;
      }

      WorldObject = UnitController.Instance.UnitSelected(go);

      if (WorldObject != null)
      {
        OnSelectionChanged(WorldObject); //Let things know that the Selection changed
        return true;
      }

      return false;
    }

    public void ClearWorldObjectSelection()
    {
      Debug.Log("Objects Deselected");
      OnSelectionChanged(null); //Let things know that the Selection changed
    }

    // Gets called whenever the selection changes
    void OnSelectionChanged(WorldObject wo)
    {
      if (cbSelectionChanged == null)
        return;

      cbSelectionChanged(wo);
    }

    public void RegisterSelectionChanged(Action<WorldObject> callbackfunc)
    {
      cbSelectionChanged += callbackfunc;
    }

    public void UnregisterSelectionChanged(Action<WorldObject> callbackfunc)
    {
      cbSelectionChanged -= callbackfunc;
    }
  }
}