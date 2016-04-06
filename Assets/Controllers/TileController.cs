using Eritar.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
  Dictionary<Tile, GameObject> tileGameObjectMap;

  World world { get { return WorldController.Instance.world; } }

  public Sprite Sprite_Tile_Dirt;
  public Sprite Sprite_Tile_Rock;

  // Use this for initialization
  void Start()
  {
    tileGameObjectMap = new Dictionary<Tile, GameObject>();

    GenerateTileGameObjects();

    world.RegisterTileChanged(OnTileChanged);
  }

  private void GenerateTileGameObjects()
  {
    for (int x = 0; x < world.Width; x++)
    {
      for (int y = 0; y < world.Height; y++)
      {
        Tile tile_data = world.GetTileAt(x, y);

        GameObject tile_go = new GameObject();

        tileGameObjectMap.Add(tile_data, tile_go);

        int posx = tile_data.X;
        int posy = tile_data.Y;

        //Correct the x and y for tilesizing
        if(world.TileSize != 1)
        {
          posx = posx * world.TileSize;
          posy = posy * world.TileSize;
        }

        tile_go.name = "Tile_" + x + "_" + y;
        tile_go.transform.position = new Vector3(posx, 0, posy);
        tile_go.transform.localScale = new Vector3(12.5f * world.TileSize, 12.5f * world.TileSize, 12.5f);
        tile_go.transform.rotation = Quaternion.Euler(270, 0, 0);
        tile_go.transform.SetParent(this.transform, true);

        // Add a Sprite Renderer
        // Add a default sprite for empty tiles.
        SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
        sr.sprite = GetTileSprite(tile_data);
        sr.sortingLayerName = "Tiles";
      }
    }
  }

  private Sprite GetTileSprite(Tile tile_data)
  {
    switch (tile_data.TileType)
    {
      case TileType.Rock:
        return Sprite_Tile_Rock;
      case TileType.Dirt:
        return Sprite_Tile_Dirt;
      case TileType.Water:
      default:
        Debug.LogError("GetTileSprite - Unrecognized tile type.");
        return Sprite_Tile_Dirt;
    }
  }

  void OnTileChanged(Tile tile_data)
  {
    if (tileGameObjectMap.ContainsKey(tile_data) == false)
    {
      Debug.LogError("tileGameObjectMap doesn't contain the tile_data -- did you forget to add the tile to the dictionary? Or maybe forget to unregister a callback?");
      return;
    }

    GameObject tile_go = tileGameObjectMap[tile_data];

    if (tile_go == null)
    {
      Debug.LogError("tileGameObjectMap's returned GameObject is null -- did you forget to add the tile to the dictionary? Or maybe forget to unregister a callback?");
      return;
    }

    Debug.Log("Changing Tiletype");


    // TODO: Tile aus Dictionary entfernen, GO zerstören und anschliessend neuerstellen!

    //if (tile_data.TileType == TileType.Floor)
    //{
    //    tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
    //}
    //else if (tile_data.TileType == TileType.Empty)
    //{
    //    tile_go.GetComponent<SpriteRenderer>().sprite = emptySprite;
    //}
    //else {
    //    Debug.LogError("OnTileTypeChanged - Unrecognized tile type.");
    //}

  }
}
