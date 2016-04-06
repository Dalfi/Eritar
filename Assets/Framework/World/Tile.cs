using System;

namespace Eritar.Framework
{
  public enum TileType:int
    {
        Dirt = 0,
        Rock = 1,
        Water = 99
    }
        
    public class Tile
    {
        private TileType _tileType = TileType.Dirt;

        public TileType TileType
        {
            get { return _tileType; }
            set
            {
                // Only set the type if it changed
                if (_tileType != value)
                {
                    _tileType = value;

                    if (cbTileChanged != null)
                        cbTileChanged(this); // Let things know that our type changed
                }
            }
        }

        public int X { get; protected set; }
        public int Y { get; protected set; }

        // The function we callback any time our tile's data changes
        private Action<Tile> cbTileChanged;

        public Tile (int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Register a function to be called back when our tile type changes.
        /// </summary>
        public void RegisterTileChangedCallback(Action<Tile> callback)
        {
            cbTileChanged += callback;
        }

        /// <summary>
        /// Unregister a callback.
        /// </summary>
        public void UnregisterTileChangedCallback(Action<Tile> callback)
        {
            cbTileChanged -= callback;
        }
    }
}
