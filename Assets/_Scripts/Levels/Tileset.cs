﻿using UnityEngine;
using System.Collections;

public class Tileset 
{
    private MTexture[,] tiles;

    public Tileset(MTexture texture, int tileWidth, int tileHeight)
    {
        this.Texture = texture;
        this.TileWidth = tileWidth;
        this.TileHeight = this.TileHeight;
        this.tiles = new MTexture[this.Texture.Width / tileWidth, this.Texture.Height / tileHeight];
        for (int index1 = 0; index1 < this.Texture.Width / tileWidth; ++index1)
        {
            for (int index2 = 0; index2 < this.Texture.Height / tileHeight; ++index2)
                this.tiles[index1, index2] = new MTexture(this.Texture, index1 * tileWidth, index2 * tileHeight, tileWidth, tileHeight);
        }
    }

    public MTexture Texture { get; private set; }

    public int TileWidth { get; private set; }

    public int TileHeight { get; private set; }

    public MTexture this[int x, int y]
    {
        get
        {
            return this.tiles[x, y];
        }
    }

    public MTexture this[int index]
    {
        get
        {
            return index < 0 ? (MTexture)null : this.tiles[index % this.tiles.GetLength(0), index / this.tiles.GetLength(0)];
        }
    }
}
