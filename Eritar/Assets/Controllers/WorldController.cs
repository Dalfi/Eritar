using Eritar.Framework.World;
using UnityEngine;
using System.Collections;
using System;

public class WorldController : MonoBehaviour
{
  public static WorldController Instance { get; protected set; }

  public World world { get; protected set; }

  // Use this for initialization
  void OnEnable()
  {
    if (Instance != null)
    {
      Debug.LogError("There should never be two world controllers.");
    }

    Instance = this;

    //if (loadWorld)
    //{
    //  loadWorld = false;
    //  CreateWorldFromSaveFile();
    //}
    //else
    //{
      CreateEmptyWorld();
    //}
  }
  
  // Update is called once per frame
  void Update()
  {

  }

  /// <summary>
  /// Gets the tile at the unity-space coordinates
  /// </summary>
  /// <returns>The tile at world coordinate.</returns>
  /// <param name="coord">Unity World-Space coordinates.</param>
  public Tile GetTileAtWorldCoord(Vector3 coord)
  {
    int x = Mathf.FloorToInt(coord.x);
    int y = Mathf.FloorToInt(coord.y);

    return world.GetTileAt(x, y);
  }

  private void CreateEmptyWorld()
  {
    throw new NotImplementedException();
  }
}
