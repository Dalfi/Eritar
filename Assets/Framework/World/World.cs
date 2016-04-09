using Eritar.Framework.Entities.General;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Eritar.Framework
{
  public class World
  {
    public Tile[,] tiles;
    public List<Unit> Units { get; protected set; }
    public List<Building> Buildings { get; protected set; }
    public List<RawResource> RawResources { get; protected set; }
    public List<Item> Items { get; protected set; }

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
    Action<Building> cbBuildingCreated;
    Action<Unit> cbUnitCreated;

    public World(int width, int height, int tileSize)
    {
      Units = new List<Unit>();
      Buildings = new List<Building>();
      RawResources = new List<RawResource>();
      Items = new List<Item>();

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

    public void RegisterBuildingCreated(Action<Building> callbackfunc)
    {
      cbBuildingCreated += callbackfunc;
    }

    public void UnregisterBuildingCreated(Action<Building> callbackfunc)
    {
      cbBuildingCreated -= callbackfunc;
    }

    public void RegisterUnitCreated(Action<Unit> callbackfunc)
    {
      cbUnitCreated += callbackfunc;
    }

    public void UnregisterUnitCreated(Action<Unit> callbackfunc)
    {
      cbUnitCreated -= callbackfunc;
    }

    /// <summary>
    /// Updates the world
    /// </summary>
    /// <param name="deltaTime">Time passed since the last frame</param>
    public void Update(float deltaTime)
    {
      foreach (Unit obj in Units)
      {
        obj.Update(deltaTime);
      }
      foreach (Building obj in Buildings)
      {
        obj.Update(deltaTime);
      }
      foreach (RawResource obj in RawResources)
      {
        obj.Update(deltaTime);
      }
      foreach (Item obj in Items)
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
      if (x >= Width || x < 0 || y >= Height || y < 0)
        return null;

      return tiles[x, y];
    }

    public float GetCorrectWidth()
    {
      return this.Width * this.TileSize;
    }

    public float GetCorrectHeight()
    {
      return this.Height * this.TileSize;
    }

    public void CreateBuilding(Building buildingType, Player owner, Vector3 position, Quaternion rotation)
    {
      Building building = buildingType.Clone();

      building.Owner = owner;
      building.Position = position;
      building.Rotation = rotation;

      Buildings.Add(building);

      if (cbBuildingCreated != null)
        cbBuildingCreated(building);
    }

    public void CreateUnit(Unit unitType, Player owner, Vector3 spawnPosition, Quaternion spawnRotation)
    {
      Unit unit = unitType.Clone();

      unit.Owner = owner;
      unit.Position = spawnPosition;
      unit.Rotation = spawnRotation;

      Units.Add(unit);

      if (cbUnitCreated != null)
        cbUnitCreated(unit);
    }
  }
}
