﻿using myd.celeste;
using System;
using UnityEngine;

public class SolidTiles 
{
    public TileGrid Tiles;
    public AnimatedTiles AnimatedTiles;
    //public Grid Grid;
    private VirtualMap<char> tileTypes;

    private Vector2 position;
    public SolidTiles(Vector2 position, VirtualMap<char> data)
      //: base(position, 0.0f, 0.0f, true)
    {
        this.position = position;
        //this.Tag = (int)Tags.Global;
        //this.Depth = -10000;
        this.tileTypes = data;
        //this.EnableAssistModeChecks = false;
        //this.AllowStaticMovers = false;
        //this.Collider = (Collider)(this.Grid = new Grid(data.Columns, data.Rows, 8f, 8f));
        //for (int x = 0; x < data.Columns; x += 50)
        //{
        //    for (int y = 0; y < data.Rows; y += 50)
        //    {
        //        if (data.AnyInSegmentAtTile(x, y))
        //        {
        //            int index1 = x;
        //            for (int index2 = Math.Min(index1 + 50, data.Columns); index1 < index2; ++index1)
        //            {
        //                int index3 = y;
        //                for (int index4 = Math.Min(index3 + 50, data.Rows); index3 < index4; ++index3)
        //                {
        //                    if (data[index1, index3] != '0')
        //                        this.Grid[index1, index3] = true;
        //                }
        //            }
        //        }
        //    }
        //}
        Autotiler.Generated map = Gfx.FGAutotiler.GenerateMap(data, true);
        this.Tiles = map.TileGrid;
        this.Tiles.VisualExtend = 1;
        //this.Add((Component)this.Tiles);
        //this.Add((Component)(this.AnimatedTiles = map.SpriteOverlay));
    }

    //public override void Added(Scene scene)
    //{
    //    base.Added(scene);
    //    this.Tiles.ClipCamera = this.SceneAs<Level>().Camera;
    //    this.AnimatedTiles.ClipCamera = this.Tiles.ClipCamera;
    //}

    //private int CoreTileSurfaceIndex()
    //{
    //    Level scene = this.Scene as Level;
    //    if (scene.CoreMode == Session.CoreModes.Hot)
    //        return 37;
    //    return scene.CoreMode == Session.CoreModes.Cold ? 36 : 3;
    //}

    //private int SurfaceSoundIndexAt(Vector2 readPosition)
    //{
    //    int index1 = (int)((readPosition.X - (double)this.X) / 8.0);
    //    int index2 = (int)((readPosition.Y - (double)this.Y) / 8.0);
    //    if (index1 >= 0 && index2 >= 0 && (index1 < this.Grid.CellsX && index2 < this.Grid.CellsY))
    //    {
    //        char tileType = this.tileTypes[index1, index2];
    //        switch (tileType)
    //        {
    //            case '0':
    //                break;
    //            case 'k':
    //                return this.CoreTileSurfaceIndex();
    //            default:
    //                if (SurfaceIndex.TileToIndex.ContainsKey(tileType))
    //                    return SurfaceIndex.TileToIndex[tileType];
    //                break;
    //        }
    //    }
    //    return -1;
    //}

    //public override int GetWallSoundIndex(Player player, int side)
    //{
    //    int num = this.SurfaceSoundIndexAt(Vector2.op_Addition(player.Center, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.get_UnitX(), (float)side), 8f)));
    //    if (num < 0)
    //        num = this.SurfaceSoundIndexAt(Vector2.op_Addition(player.Center, new Vector2((float)(side * 8), -6f)));
    //    if (num < 0)
    //        num = this.SurfaceSoundIndexAt(Vector2.op_Addition(player.Center, new Vector2((float)(side * 8), 6f)));
    //    return num;
    //}

    //public override int GetStepSoundIndex(Entity entity)
    //{
    //    int num = this.SurfaceSoundIndexAt(Vector2.op_Addition(entity.BottomCenter, Vector2.op_Multiply(Vector2.get_UnitY(), 4f)));
    //    if (num == -1)
    //        num = this.SurfaceSoundIndexAt(Vector2.op_Addition(entity.BottomLeft, Vector2.op_Multiply(Vector2.get_UnitY(), 4f)));
    //    if (num == -1)
    //        num = this.SurfaceSoundIndexAt(Vector2.op_Addition(entity.BottomRight, Vector2.op_Multiply(Vector2.get_UnitY(), 4f)));
    //    return num;
    //}

    //public override int GetLandSoundIndex(Entity entity)
    //{
    //    int num = this.SurfaceSoundIndexAt(Vector2.op_Addition(entity.BottomCenter, Vector2.op_Multiply(Vector2.get_UnitY(), 4f)));
    //    if (num == -1)
    //        num = this.SurfaceSoundIndexAt(Vector2.op_Addition(entity.BottomLeft, Vector2.op_Multiply(Vector2.get_UnitY(), 4f)));
    //    if (num == -1)
    //        num = this.SurfaceSoundIndexAt(Vector2.op_Addition(entity.BottomRight, Vector2.op_Multiply(Vector2.get_UnitY(), 4f)));
    //    return num;
    //}
}
