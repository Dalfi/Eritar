using UnityEngine;
using System;

namespace Eritar.Framework.World
{
  public class World
  {
    Tile[,] tiles;

    // The tile width of the world.
    public int Width { get; protected set; }

    // The tile height of the world
    public int Height { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="World"/> class.
    /// </summary>
    /// <param name="width">Width in tiles.</param>
    /// <param name="height">Height in tiles.</param>
    public World(int width, int height)
    {
      // Creates an empty world.
      SetupWorld(width, height);

    }

    private void SetupWorld(int width, int height)
    {
      Width = width;
      Height = height;

      for (int x = 0; x < Width; x++)
      {
        for (int y = 0; y < height; y++)
        {
          tiles[x, y] = new Tile(this, x, y);
          tiles[x, y].RegisterTileChanged(OnTileChanged);
        }
      }



      Debug.Log("World created with " + (Width * Height) + " tiles.");
    }

    /// <summary>
    /// Gets the tile data at x and y.
    /// </summary>
    /// <returns>The <see cref="Tile"/>.</returns>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public Tile GetTileAt(int x, int y)
    {
      if (x >= Width || x < 0 || y >= Height || y < 0)
      {
        //Debug.LogError("Tile ("+x+","+y+") is out of range.");
        return null;
      }
      return tiles[x, y];
    }

    private void OnTileChanged(Tile tile)
    {

    }
  }
}
