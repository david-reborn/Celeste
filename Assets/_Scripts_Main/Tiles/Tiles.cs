﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace myd.celeste.demo
{
    public class Tiles
    {
        private byte[] adjacent = new byte[9];
        private Dictionary<char, Autotiler.TerrainType> lookup = new Dictionary<char, Autotiler.TerrainType>();

        public Tiles(string path)
        {
            Dictionary<char, XmlElement> dictionary = new Dictionary<char, XmlElement>();
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            foreach (XmlElement xml in doc.GetElementsByTagName("Tileset"))
            {
                char ch = xml.AttrChar("id");
                Tileset tileset = new Tileset(Gfx.Game["tilesets/" + xml.Attr("path")], 8, 8);
                Autotiler.TerrainType data = new Autotiler.TerrainType(ch);
                this.ReadInto(data, tileset, xml);
                if (xml.HasAttr("copy"))
                {
                    char key = xml.AttrChar("copy");
                    if (!dictionary.ContainsKey(key))
                        throw new Exception("Copied tilesets must be defined before the tilesets that copy them!");
                    this.ReadInto(data, tileset, dictionary[key]);
                }
                if (xml.HasAttr("ignores"))
                {
                    string str1 = xml.Attr("ignores");
                    char[] chArray = new char[1] { ',' };
                    foreach (string str2 in str1.Split(chArray))
                    {
                        if (str2.Length > 0)
                            data.Ignores.Add(str2[0]);
                    }
                }
                dictionary.Add(ch, xml);
                this.lookup.Add(ch, data);
            }
        }

        private void ReadInto(Autotiler.TerrainType data, Tileset tileset, XmlElement xml)
        {
            foreach (object obj in (XmlNode)xml)
            {
                if (!(obj is XmlComment))
                {
                    XmlElement xml1 = obj as XmlElement;
                    string str1 = xml1.Attr("mask");
                    Autotiler.Tiles tiles;
                    if (str1 == "center")
                        tiles = data.Center;
                    else if (str1 == "padding")
                    {
                        tiles = data.Padded;
                    }
                    else
                    {
                        Autotiler.Masked masked = new Autotiler.Masked();
                        tiles = masked.Tiles;
                        int index = 0;
                        int num = 0;
                        for (; index < str1.Length; ++index)
                        {
                            if (str1[index] == '0')
                                masked.Mask[num++] = (byte)0;
                            else if (str1[index] == '1')
                                masked.Mask[num++] = (byte)1;
                            else if (str1[index] == 'x' || str1[index] == 'X')
                                masked.Mask[num++] = (byte)2;
                        }
                        data.Masked.Add(masked);
                    }
                    string str2 = xml1.Attr("tiles");
                    char[] chArray1 = new char[1] { ';' };
                    foreach (string str3 in str2.Split(chArray1))
                    {
                        char[] chArray2 = new char[1] { ',' };
                        string[] strArray = str3.Split(chArray2);
                        int index1 = int.Parse(strArray[0]);
                        int index2 = int.Parse(strArray[1]);
                        MTexture mtexture = tileset[index1, index2];
                        tiles.Textures.Add(mtexture);
                    }
                    if (xml1.HasAttr("sprites"))
                    {
                        string str3 = xml1.Attr("sprites");
                        char[] chArray2 = new char[1] { ',' };
                        foreach (string str4 in str3.Split(chArray2))
                            tiles.OverlapSprites.Add(str4);
                        tiles.HasOverlays = true;
                    }
                }
            }
            data.Masked.Sort((Comparison<Autotiler.Masked>)((a, b) =>
            {
                int num1 = 0;
                int num2 = 0;
                for (int index = 0; index < 9; ++index)
                {
                    if (a.Mask[index] == (byte)2)
                        ++num1;
                    if (b.Mask[index] == (byte)2)
                        ++num2;
                }
                return num1 - num2;
            }));
        }

        public void GenerateTiles(Tilemap tilemap, VirtualMap<char> foregroundData, int startX, int startY, int tilesX, int tilesY, bool forceSolid, char forceID, Autotiler.Behaviour behaviour, ColliderGrid colliderGrid)
        {
            Rectangle forceFill = Rectangle.Empty;
            if (forceSolid)
                forceFill = new Rectangle(startX, startY, tilesX, tilesY);
            if (foregroundData != null)
            {
                for (int x1 = startX; x1 < startX + tilesX; x1 += 50)
                {
                    for (int y1 = startY; y1 < startY + tilesY; y1 += 50)
                    {
                        if (!foregroundData.AnyInSegmentAtTile(x1, y1))
                        {
                            y1 = y1 / 50 * 50;
                        }
                        else
                        {
                            int x2 = x1;
                            for (int index1 = Math.Min(x1 + 50, startX + tilesX); x2 < index1; ++x2)
                            {
                                int y2 = y1;
                                for (int index2 = Math.Min(y1 + 50, startY + tilesY); y2 < index2; ++y2)
                                {
                                    Autotiler.Tiles tiles = this.TileHandler(foregroundData, x2, y2, forceFill, forceID, behaviour);
                                    if (tiles != null)
                                    {
                                        SolidTile tile = ScriptableObject.CreateInstance<SolidTile>();
                                        tile.SetTile(tiles);
                                        tile.colliderType = colliderGrid[x2 - startX, y2 - startY]? Tile.ColliderType.Grid:Tile.ColliderType.None;
                                        tilemap.SetTile(new Vector3Int(x2 - startX, -(y2 - startY), 0), tile);
                                        //this.tilemap.SetTile(new Vector3Int(0, 0, 0), tile);
                                        //return;
                                    }
                                    //if (tiles != null)
                                    //{
                                    //    tileGrid.Tiles[x2 - startX, y2 - startY] = RandomUtil.Random.Choose<MTexture>(tiles.Textures);
                                    //    //if (tiles.HasOverlays)
                                    //    //    animatedTiles.Set(x2 - startX, y2 - startY, RandomUtil.Random.Choose<string>(tiles.OverlapSprites), 1f, 1f);
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int x = startX; x < startX + tilesX; ++x)
                {
                    for (int y = startY; y < startY + tilesY; ++y)
                    {
                        Autotiler.Tiles tiles = this.TileHandler((VirtualMap<char>)null, x, y, forceFill, forceID, behaviour);
                        if (tiles != null)
                        {
                            SolidTile tile = ScriptableObject.CreateInstance<SolidTile>();
                            tile.SetTile(tiles);
                            tile.colliderType = colliderGrid[x - startX, y - startY] ? Tile.ColliderType.Grid : Tile.ColliderType.None;
                            tilemap.SetTile(new Vector3Int(x - startX, -(y - startY), 0), tile);
                            //this.tilemap.SetTile(new Vector3Int(0, 0, 0), tile);
                            //return;
                        }
                        //if (tiles != null)
                        //{
                        //    tileGrid.Tiles[x - startX, y - startY] = RandomUtil.Random.Choose<MTexture>(tiles.Textures);
                        //    if (tiles.HasOverlays)
                        //        animatedTiles.Set(x - startX, y - startY, RandomUtil.Random.Choose<string>(tiles.OverlapSprites), 1f, 1f);
                        //}
                    }
                }
            }
        }

        private Autotiler.Tiles TileHandler(VirtualMap<char> mapData, int x, int y, Rectangle forceFill, char forceID, Autotiler.Behaviour behaviour)
        {
            char tile = this.GetTile(mapData, x, y, forceFill, forceID, behaviour);
            if (this.IsEmpty(tile))
                return null;
            Autotiler.TerrainType set = this.lookup[tile];
            bool flag1 = true;
            int num = 0;
            for (int index1 = -1; index1 < 2; ++index1)
            {
                for (int index2 = -1; index2 < 2; ++index2)
                {
                    bool flag2 = this.CheckTile(set, mapData, x + index2, y + index1, forceFill, behaviour);
                    if (!flag2 && behaviour.EdgesIgnoreOutOfLevel && !this.CheckForSameLevel(x, y, x + index2, y + index1))
                        flag2 = true;
                    this.adjacent[num++] = flag2 ? (byte)1 : (byte)0;
                    if (!flag2)
                        flag1 = false;
                }
            }
            if (flag1)
            {

                if ((behaviour.PaddingIgnoreOutOfLevel ? ((!this.CheckTile(set, mapData, x - 2, y, forceFill, behaviour) && this.CheckForSameLevel(x, y, x - 2, y)) || (!this.CheckTile(set, mapData, x + 2, y, forceFill, behaviour) && this.CheckForSameLevel(x, y, x + 2, y)) || (!this.CheckTile(set, mapData, x, y - 2, forceFill, behaviour) && this.CheckForSameLevel(x, y, x, y - 2)) || (!this.CheckTile(set, mapData, x, y + 2, forceFill, behaviour) && this.CheckForSameLevel(x, y, x, y + 2))) : (!this.CheckTile(set, mapData, x - 2, y, forceFill, behaviour) || !this.CheckTile(set, mapData, x + 2, y, forceFill, behaviour)) || (!this.CheckTile(set, mapData, x, y - 2, forceFill, behaviour)) || (!this.CheckTile(set, mapData, x, y + 2, forceFill, behaviour))))
                {
                    return this.lookup[tile].Padded;
                }
                else
                {
                    return this.lookup[tile].Center;
                }
            }
            //return  ? this.lookup[tile].Padded : this.lookup[tile].Center;
            for (int p = 0; p < set.Masked.Count; p++)
            {
                Autotiler.Masked masked = set.Masked[p];
                bool flag2 = true;
                for (int index = 0; index < 9 & flag2; ++index)
                {
                    if (masked.Mask[index] != (byte)2 && (int)masked.Mask[index] != (int)this.adjacent[index])
                        flag2 = false;
                }
                if (flag2)
                    return masked.Tiles;
            }
            return null;
        }

        private bool IsEmpty(char id)
        {
            return id == '0' || id == char.MinValue;
        }

        private bool CheckTile(Autotiler.TerrainType set, VirtualMap<char> mapData, int x, int y, Rectangle forceFill, Autotiler.Behaviour behaviour)
        {
            if (forceFill.Contains(x, y))
                return true;
            if (mapData == null)
                return behaviour.EdgesExtend;
            if (x < 0 || y < 0 || x >= mapData.Columns || y >= mapData.Rows)
            {
                if (!behaviour.EdgesExtend)
                    return false;
                char ch = mapData[Util.Clamp(x, 0, mapData.Columns - 1), Util.Clamp(y, 0, mapData.Rows - 1)];
                return !this.IsEmpty(ch) && !set.Ignore(ch);
            }
            char ch1 = mapData[x, y];
            return !this.IsEmpty(ch1) && !set.Ignore(ch1);
        }

        private char GetTile(VirtualMap<char> mapData, int x, int y, Rectangle forceFill, char forceID, Autotiler.Behaviour behaviour)
        {
            if (forceFill.Contains(x, y))
                return forceID;
            if (mapData == null)
                return !behaviour.EdgesExtend ? '0' : forceID;
            if (x >= 0 && y >= 0 && x < mapData.Columns && y < mapData.Rows)
                return mapData[x, y];
            if (!behaviour.EdgesExtend)
                return '0';
            int index1 = Util.Clamp(x, 0, mapData.Columns - 1);
            int index2 = Util.Clamp(y, 0, mapData.Rows - 1);
            return mapData[index1, index2];
        }

        private bool CheckForSameLevel(int x1, int y1, int x2, int y2)
        {
            return true;
        }
    }
}
