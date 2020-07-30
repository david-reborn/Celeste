using UnityEngine;
using System.Collections;
using myd.celeste;

public class BackgroundTiles : Entity
{
    public TileGrid Tiles;

    public BackgroundTiles(Vector2 position, VirtualMap<char> data)
    {
        this.Position = position;
        //this.Tag = (int)Tags.Global;
        this.Tiles = Gfx.BGAutotiler.GenerateMap(data, false).TileGrid;
        //this.Tiles.VisualExtend = 1;
        //this.Add((Component)this.Tiles);
        //this.Depth = 10000;
    }

}
