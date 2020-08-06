using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myd.celeste.demo
{
    public class ColliderGrid
    {
        public VirtualMap<bool> data;
        public float CellWidth  { get; private set; }
        public float CellHeight { get; private set; }
        public ColliderGrid(int cellsX, int cellsY, float cellWidth, float cellHeight)
        {
            this.data = new VirtualMap<bool>(cellsX, cellsY, false);
            this.CellWidth = cellWidth;
            this.CellHeight = cellHeight;
        }

        public bool this[int x, int y]
        {
            get
            {
                return x >= 0 && y >= 0 && (x < this.CellsX && y < this.CellsY) && this.data[x, y];
            }
            set
            {
                this.data[x, y] = value;
            }
        }

        public int CellsX
        {
            get
            {
                return this.data.Columns;
            }
        }

        public int CellsY
        {
            get
            {
                return this.data.Rows;
            }
        }

    }
}
