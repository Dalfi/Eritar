using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eritar.Framework.World
{
  public class Tile
  {
    private TileType _type = TileType.Dirt;

    public TileType Type
    {
      get { return _type; }
      set { _type = value; }
    }

    public World world { get; protected set; }

    public int X { get; protected set; }
    public int Y { get; protected set; }

    private Action<Tile> cbTileChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="Tile"/> class.
    /// </summary>
    /// <param name="world">The world in wich the tile exists</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public Tile(World world, int x, int y)
    {
      this.world = world;
      this.X = x;
      this.Y = y;
    }
    
    public void RegisterTileChanged(Action<Tile> cb)
    {
      cbTileChanged += cb;
    }

    public void UnRegisterTileChanged(Action<Tile> cb)
    {
      cbTileChanged -= cb;
    }


  }
}
