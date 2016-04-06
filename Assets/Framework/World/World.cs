using Eritar.Framework.Entities.General;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Eritar.Framework
{
  public class World
  {
    public Tile[,] tiles;
    public List<WorldObject> objects;

    /// <summary>
    /// The tile width of the world.
    /// </summary>
    public int Width { get; protected set; }

    /// <summary>
    /// The tile height of the world
    /// </summary>
    public int Height { get; protected set; }

    public int TileSize { get; protected set; }

    Action<Tile> cbTileChanged;


    public World(int width, int height, int tileSize)
    {
      objects = new List<WorldObject>();

      if (tileSize != 1 && tileSize % 2 != 0)
      {
        Debug.LogError("There can only be an even tileSize!" + Environment.NewLine + "TileSize: " + tileSize.ToString());
      }
      else
      {
        SetupWorld(width, height, tileSize);
      }
    }

    private void SetupWorld(int width, int height, int tileSize)
    {
      Width = width;
      Height = height;
      TileSize = tileSize;

      tiles = new Tile[Width, Height];

      for (int x = 0; x < Width; x++)
      {
        for (int y = 0; y < Height; y++)
        {
          tiles[x, y] = new Tile(x, y);

          if (UnityEngine.Random.Range(0, 2) == 0)
          {
            tiles[x, y].TileType = TileType.Dirt;
          }
          else
          {
            tiles[x, y].TileType = TileType.Rock;
          }

          tiles[x, y].RegisterTileChangedCallback(OnTileChanged);
        }
      }
    }

    // Gets called whenever ANY tile changes
    void OnTileChanged(Tile t)
    {
      if (cbTileChanged == null)
        return;

      cbTileChanged(t);
    }

    public void RegisterTileChanged(Action<Tile> callbackfunc)
    {
      cbTileChanged += callbackfunc;
    }

    public void UnregisterTileChanged(Action<Tile> callbackfunc)
    {
      cbTileChanged -= callbackfunc;
    }

    /// <summary>
    /// Updates the world
    /// </summary>
    /// <param name="deltaTime">Time passed since the last frame</param>
    public void Update(float deltaTime)
    {
      foreach (WorldObject obj in objects)
      {
        obj.Update(deltaTime);
      }
    }

    /// <summary>
    /// Gets the tile data at x and y.
    /// </summary>
    /// <returns>The <see cref="Tile"/>.</returns>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public Tile GetTileAt(int x, int y)
    {
      //get the correct x and y for the tiles



      if (x >= Width || x < 0 || y >= Height || y < 0)
        return null;

      return tiles[x, y];
    }

    public float GetWidth()
    {
      return this.Width * this.TileSize;
    }

    public float GetHeight()
    {
      return this.Height * this.TileSize;
    }
  }
}
