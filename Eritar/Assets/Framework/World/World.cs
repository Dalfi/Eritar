﻿
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


    }
  }
}
