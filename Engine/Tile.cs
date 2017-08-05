using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Tile
    {
        private int _TileX;
        private int _TileY;

        private bool _Solid = true;

        private int _Left;
        private int _Top;

        public Tile(int tileX, int tileY, bool solid, int top, int left)
        {
            _TileX = tileX;
            _TileY = tileY;

            _Solid = solid;

            _Left = left;
            _Top = top;
        }

        public int tileX
        {
            set { _TileX = value; }
            get { return _TileX; }

        }
        public int tileY
        {
            set { _TileY = value; }
            get { return _TileY; }

        }
        public bool solid
        {
            set { _Solid = value; }
            get { return _Solid; }

        }
        public int left
        {
            set { _Left = value; }
            get { return _Left; }

        }
        public int top
        {
            set { _Top = value; }
            get { return _Top; }
        }
    }
}
