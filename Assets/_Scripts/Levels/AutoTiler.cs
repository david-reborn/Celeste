﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System;

namespace myd.celeste
{
    public class AutoTiler
    {
        public List<Rect> LevelBounds = new List<Rect>();
        private Dictionary<char, TerrainType> lookup = new Dictionary<char, TerrainType>();
        private byte[] adjacent = new byte[9];
        public AutoTiler(string xmlDescription)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlDescription);
            Dictionary<char, XmlElement> dictionary = new Dictionary<char, XmlElement>();

            foreach (object obj in document.GetElementsByTagName("Tileset"))
            {
                XmlElement xmlElement = (XmlElement)obj;
                char c = xmlElement.AttrChar("id");
                Sprite tileImages = Gameplay.instance.GetImage("tilesets/" + xmlElement.Attr("path"));
                TileGrid tileset = new TileGrid(8, 8, Mathf.RoundToInt(tileImages.bounds.size.x) / 8, Mathf.RoundToInt(tileImages.bounds.size.y) / 8);
                tileset.Load(tileImages);
                TerrainType terrainType = new TerrainType(c);
                ReadInto(terrainType, tileset, xmlElement);

                if (xmlElement.HasAttr("copy"))
                {
                    char key = xmlElement.AttrChar("copy");
                    if (!dictionary.ContainsKey(key))
                    {
                        throw new Exception("Copied tilesets must be defined before the tilesets that copy them!");
                    }
                    ReadInto(terrainType, tileset, dictionary[key]);
                }

                if (xmlElement.HasAttr("ignores"))
                {
                    foreach (string text in xmlElement.Attr("ignores").Split(','))
                    {
                        if (text.Length > 0)
                        {
                            terrainType.Ignores.Add(text[0]);
                        }
                    }
                }

                dictionary.Add(c, xmlElement);
                lookup.Add(c, terrainType);
            }
        }

        private void ReadInto(TerrainType data, TileGrid tileset, XmlElement xml)
        {
            foreach (object obj in xml)
            {
                if (!(obj is XmlComment))
                {
                    XmlElement xml2 = obj as XmlElement;
                    string text = xml2.Attr("mask");
                    Tiles tiles;
                    if (text == "center")
                    {
                        tiles = data.Center;
                    }
                    else if (text == "padding")
                    {
                        tiles = data.Padded;
                    }
                    else
                    {
                        Masked masked = new Masked();
                        tiles = masked.Tiles;
                        int i = 0;
                        int num = 0;
                        while (i < text.Length)
                        {
                            if (text[i] == '0')
                            {
                                masked.Mask[num++] = 0;
                            }
                            else if (text[i] == '1')
                            {
                                masked.Mask[num++] = 1;
                            }
                            else if (text[i] == 'x' || text[i] == 'X')
                            {
                                masked.Mask[num++] = 2;
                            }
                            i++;
                        }
                        data.Masked.Add(masked);
                    }
                    string[] array = xml2.Attr("tiles").Split(';');
                    for (int j = 0; j < array.Length; j++)
                    {
                        string[] array2 = array[j].Split(',');
                        int x = int.Parse(array2[0]);
                        int y = int.Parse(array2[1]);
                        Sprite item = tileset.Tiles[x, y];
                        tiles.Textures.Add(item);
                    }
                    if (xml2.HasAttr("sprites"))
                    {
                        foreach (string item2 in xml2.Attr("sprites").Split(','))
                        {
                            tiles.OverlapSprites.Add(item2);
                        }
                        tiles.HasOverlays = true;
                    }
                }
            }
            data.Masked.Sort(delegate (Masked a, Masked b)
            {
                int num2 = 0;
                int num3 = 0;
                for (int k = 0; k < 9; k++)
                {
                    if (a.Mask[k] == 2)
                    {
                        num2++;
                    }
                    if (b.Mask[k] == 2)
                    {
                        num3++;
                    }
                }
                return num2 - num3;
            });
        }

        private class TerrainType
        {
            public char ID;
            public HashSet<char> Ignores = new HashSet<char>();
            public List<Masked> Masked = new List<Masked>();
            public Tiles Center = new Tiles();
            public Tiles Padded = new Tiles();

            public TerrainType(char id)
            {
                ID = id;
            }
            public bool Ignore(char c)
            {
                return ID != c && (Ignores.Contains(c) || Ignores.Contains('*'));
            }
        }
        private class Masked
        {
            public byte[] Mask = new byte[9];
            public Tiles Tiles = new Tiles();
        }
        private class Tiles
        {
            public List<Sprite> Textures = new List<Sprite>();
            public List<string> OverlapSprites = new List<string>();
            public bool HasOverlays;
        }

        public void SetLevelBounds(List<MapElement> levels)
        {
            LevelBounds.Clear();
            for (int i = 0; i < levels.Count; i++)
            {
                MapElement level = levels[i];
                int x = level.AttrInt("x", 0) / 8;
                int y = level.AttrInt("y", 0) / 8;
                int widthTiles = level.AttrInt("width", 0) / 8;
                int heightTiles = level.AttrInt("height", 0) / 8;

                LevelBounds.Add(new Rect(x, y, widthTiles, heightTiles));
            }
        }

        public TileGrid GenerateOverlay(char id, int x, int y, int tilesX, int tilesY, VirtualMap<char> mapData)
        {
            Behaviour behaviour = new Behaviour
            {
                EdgesExtend = true,
                EdgesIgnoreOutOfLevel = true,
                PaddingIgnoreOutOfLevel = true
            };
            return Generate(mapData, x, y, tilesX, tilesY, 1, id, behaviour);
        }

        //public Bitmap DisplayMap(MapElement level, List<Backdrop> backdrops, Rect bounds, bool before)
        //{
        //    if (Tiles.Columns == 0 || Tiles.Rows == 0) { return null; }

        //    Bitmap img = new Bitmap(Tiles.Columns * Width, Tiles.Rows * Height, PixelFormat.Format32bppArgb);
        //    using (Graphics g = Graphics.FromImage(img))
        //    {
        //        if (before)
        //        {
        //            using (SolidBrush brush = new SolidBrush(DefaultBackground))
        //            {
        //                g.FillRectangle(brush, 0, 0, img.Width, img.Height);
        //            }

        //            if (backdrops != null)
        //            {
        //                for (int i = 0; i < backdrops.Count; i++)
        //                {
        //                    Backdrop bd = backdrops[i];
        //                    if (level == null || bd.IsVisible(level.Attr("name")))
        //                    {
        //                        bd.Render(bounds, g);
        //                    }
        //                }
        //            }
        //        }

        //        for (int i = 0; i < Tiles.Columns; i++)
        //        {
        //            for (int j = 0; j < Tiles.Rows; j++)
        //            {
        //                Bitmap tile = Tiles[i, j];
        //                if (tile != null)
        //                {
        //                    g.DrawImage(tile, (float)i * Width, (float)j * Height);
        //                }
        //            }
        //        }

        //        if (!before && backdrops != null)
        //        {
        //            for (int i = 0; i < backdrops.Count; i++)
        //            {
        //                Backdrop bd = backdrops[i];
        //                if (level == null || bd.IsVisible(level.Attr("name")))
        //                {
        //                    bd.Render(bounds, g);
        //                }
        //            }
        //        }
        //    }
        //    return img;
        //}

        private TileGrid Generate(VirtualMap<char> mapData, int startX, int startY, int tilesX, int tilesY, int forceSolid, char forceID, Behaviour behaviour)
        {
            TileGrid tileGrid = new TileGrid(8, 8, tilesX, tilesY);
            Rect empty = new Rect();
            if (forceSolid != 0)
            {
                empty = new Rect(startX, startY, tilesX, tilesY);
            }
            if (mapData != null)
            {
                for (int i = startX; i < startX + tilesX; i += 50)
                {
                    for (int j = startY; j < startY + tilesY; j += 50)
                    {
                        if (!mapData.AnyInSegmentAtTile(i, j))
                        {
                            j = j / 50 * 50;
                        }
                        else
                        {
                            int k = i;
                            int num = Math.Min(i + 50, startX + tilesX);
                            while (k < num)
                            {
                                int l = j;
                                int num2 = Math.Min(j + 50, startY + tilesY);
                                while (l < num2)
                                {
                                    Tiles tiles = TileHandler(mapData, k, l, forceSolid, empty, forceID, behaviour);
                                    if (tiles != null)
                                    {
                                        tileGrid.Tiles[k - startX, l - startY] = Util.Random.Choose(tiles.Textures);
                                    }
                                    l++;
                                }
                                k++;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int m = startX; m < startX + tilesX; m++)
                {
                    for (int n = startY; n < startY + tilesY; n++)
                    {
                        Tiles tiles2 = TileHandler(null, m, n, forceSolid, empty, forceID, behaviour);
                        if (tiles2 != null)
                        {
                            tileGrid.Tiles[m - startX, n - startY] = Util.Random.Choose(tiles2.Textures);
                        }
                    }
                }
            }
            return tileGrid;
        }

        private Tiles TileHandler(VirtualMap<char> mapData, int x, int y, int forceSolid, Rect forceFill, char forceID, Behaviour behaviour)
        {
            char tile = GetTile(mapData, x, y, forceFill, forceID, behaviour);
            if (IsEmpty(tile))
            {
                return null;
            }
            TerrainType terrainType = lookup[tile];
            if (forceSolid == 1 && mapData == null)
            {
                return terrainType.Center;
            }
            bool flag = true;
            int num = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    bool flag2 = CheckTile(terrainType, mapData, x + j, y + i, forceFill, behaviour);
                    if (!flag2 && behaviour.EdgesIgnoreOutOfLevel && !CheckForSameLevel(x, y, x + j, y + i))
                    {
                        flag2 = true;
                    }
                    adjacent[num++] = (flag2 ? (byte)1 : (byte)0);
                    if (!flag2)
                    {
                        flag = false;
                    }
                }
            }
            if (!flag)
            {
                foreach (Masked masked in terrainType.Masked)
                {
                    bool flag3 = true;
                    int num2 = 0;
                    while (num2 < 9 && flag3)
                    {
                        if (masked.Mask[num2] != 2 && masked.Mask[num2] != adjacent[num2])
                        {
                            flag3 = false;
                        }
                        num2++;
                    }
                    if (flag3)
                    {
                        return masked.Tiles;
                    }
                }
                return null;
            }
            bool flag4;
            if (!behaviour.PaddingIgnoreOutOfLevel)
            {
                flag4 = (!CheckTile(terrainType, mapData, x - 2, y, forceFill, behaviour) || !CheckTile(terrainType, mapData, x + 2, y, forceFill, behaviour) || !CheckTile(terrainType, mapData, x, y - 2, forceFill, behaviour) || !CheckTile(terrainType, mapData, x, y + 2, forceFill, behaviour));
            }
            else
            {
                flag4 = ((!CheckTile(terrainType, mapData, x - 2, y, forceFill, behaviour) && CheckForSameLevel(x, y, x - 2, y)) || (!CheckTile(terrainType, mapData, x + 2, y, forceFill, behaviour) && CheckForSameLevel(x, y, x + 2, y)) || (!CheckTile(terrainType, mapData, x, y - 2, forceFill, behaviour) && CheckForSameLevel(x, y, x, y - 2)) || (!CheckTile(terrainType, mapData, x, y + 2, forceFill, behaviour) && CheckForSameLevel(x, y, x, y + 2)));
            }
            if (flag4)
            {
                return terrainType.Padded;
            }
            return terrainType.Center;
        }


        private bool CheckTile(TerrainType set, VirtualMap<char> mapData, int x, int y, Rect forceFill, Behaviour behaviour)
        {
            if (forceFill.Contains(new Vector2(x, y)))
            {
                return true;
            }
            if (mapData == null)
            {
                return behaviour.EdgesExtend;
            }
            if (x >= 0 && y >= 0 && x < mapData.Columns && y < mapData.Rows)
            {
                char c = mapData[x, y];
                return !IsEmpty(c) && !set.Ignore(c);
            }
            if (!behaviour.EdgesExtend)
            {
                return false;
            }
            char c2 = mapData[Util.Clamp(x, 0, mapData.Columns - 1), Util.Clamp(y, 0, mapData.Rows - 1)];
            return !IsEmpty(c2) && !set.Ignore(c2);
        }

        private char GetTile(VirtualMap<char> mapData, int x, int y, Rect forceFill, char forceID, Behaviour behaviour)
        {
            if (forceFill.Contains(new Vector2(x, y)))
            {
                return forceID;
            }
            if (mapData == null)
            {
                if (!behaviour.EdgesExtend)
                {
                    return '0';
                }
                return forceID;
            }
            else
            {
                if (x >= 0 && y >= 0 && x < mapData.Columns && y < mapData.Rows)
                {
                    return mapData[x, y];
                }
                if (!behaviour.EdgesExtend)
                {
                    return '0';
                }
                int x2 = Util.Clamp(x, 0, mapData.Columns - 1);
                int y2 = Util.Clamp(y, 0, mapData.Rows - 1);
                return mapData[x2, y2];
            }
        }

        private bool IsEmpty(char id)
        {
            return id == '0' || id == '\0';
        }

        private bool CheckForSameLevel(int x1, int y1, int x2, int y2)
        {
            return true;
            //foreach (Rectangle rectangle in LevelBounds) {
            //	if (rectangle.Contains(x1, y1) && rectangle.Contains(x2, y2)) {
            //		return true;
            //	}
            //}
            //return false;
        }

        public struct Behaviour
        {
            public bool PaddingIgnoreOutOfLevel;
            public bool EdgesIgnoreOutOfLevel;
            public bool EdgesExtend;
        }
    }

    public class TileGrid
    {
        public static Color DefaultBackground = Color.black;
        public int Width, Height;
        public VirtualMap<Sprite> Tiles;
        public TileGrid(int w, int h, int tilesX, int tilesY)
        {
            Width = w;
            Height = h;
            Tiles = new VirtualMap<Sprite>(tilesX, tilesY, null);
        }
        public void Load(Sprite images)
        {
            for (int i = 0; i < Tiles.Columns; i++)
            {
                for (int j = 0; j < Tiles.Rows; j++)
                {
                    Tiles[i, j] = Util.GetSubImage(images, new ExtSprite(i * Width, j * Height, Width, Height, 0, 0, Width, Height));
                }
            }
        }
        //public Texture2D DisplayMap(MapElement level, List<Backdrop> backdrops, Rectangle bounds, bool before)
        //{
        //    if (Tiles.Columns == 0 || Tiles.Rows == 0) { return null; }

        //    Texture2D img = new Texture2D(Tiles.Columns * Width, Tiles.Rows * Height, PixelFormat.Format32bppArgb);
        //    using (Graphics g = Graphics.FromImage(img))
        //    {
        //        if (before)
        //        {
        //            using (SolidBrush brush = new SolidBrush(DefaultBackground))
        //            {
        //                g.FillRectangle(brush, 0, 0, img.Width, img.Height);
        //            }

        //            if (backdrops != null)
        //            {
        //                for (int i = 0; i < backdrops.Count; i++)
        //                {
        //                    Backdrop bd = backdrops[i];
        //                    if (level == null || bd.IsVisible(level.Attr("name")))
        //                    {
        //                        bd.Render(bounds, g);
        //                    }
        //                }
        //            }
        //        }

        //        for (int i = 0; i < Tiles.Columns; i++)
        //        {
        //            for (int j = 0; j < Tiles.Rows; j++)
        //            {
        //                Texture2D tile = Tiles[i, j];
        //                if (tile != null)
        //                {
        //                    g.DrawImage(tile, (float)i * Width, (float)j * Height);
        //                }
        //            }
        //        }

        //        if (!before && backdrops != null)
        //        {
        //            for (int i = 0; i < backdrops.Count; i++)
        //            {
        //                Backdrop bd = backdrops[i];
        //                if (level == null || bd.IsVisible(level.Attr("name")))
        //                {
        //                    bd.Render(bounds, g);
        //                }
        //            }
        //        }
        //    }
        //    return img;
        //}
        //public Texture2D GenerateMap(VirtualMap<int> map)
        //{
        //    if (map.Columns == 0 || map.Rows == 0) { return null; }

        //    Texture2D img = new Texture2D(map.Columns * Width, map.Rows * Height, PixelFormat.Format32bppArgb);
        //    using (Graphics g = Graphics.FromImage(img))
        //    {
        //        for (int i = 0; i < map.Columns; i++)
        //        {
        //            for (int j = 0; j < map.Rows; j++)
        //            {
        //                Texture2D tile = this[map[i, j]];
        //                if (tile != null)
        //                {
        //                    g.DrawImage(tile, (float)i * Width, (float)j * Height);
        //                }
        //            }
        //        }
        //    }
        //    return img;
        //}
        //public void Overlay(MapElement level, string objects, int width, int height, TileGrid tileset)
        //{
        //    MapElement tileData = level.SelectFirst(objects);
        //    VirtualMap<int> map = ReadMapInt(tileData == null ? string.Empty : tileData.Attr("InnerText"), width, height);

        //    if (map.Columns == 0 || map.Rows == 0) { return; }

        //    for (int i = 0; i < map.Columns; i++)
        //    {
        //        for (int j = 0; j < map.Rows; j++)
        //        {
        //            Texture2D tile = tileset[map[i, j]];
        //            if (tile != null)
        //            {
        //                this[i, j] = tile;
        //            }
        //        }
        //    }
        //}
        private VirtualMap<int> ReadMapInt(string tiles, int width, int height)
        {
            VirtualMap<int> mapData = new VirtualMap<int>(width, height, -1);
            int length = tiles.Length;
            int i = 0;
            int col = 0, row = 0;
            StringBuilder sb = new StringBuilder();
            while (i < length)
            {
                char val = tiles[i++];
                if (char.IsNumber(val) || val == '-')
                {
                    sb.Append(val);
                }
                else if (val == ',')
                {
                    mapData[col++, row] = int.Parse(sb.ToString());
                    sb.Length = 0;
                }
                else if (val == '\r' || val == '\n')
                {
                    if (val == '\n')
                    {
                        if (sb.Length > 0)
                        {
                            mapData[col, row] = int.Parse(sb.ToString());
                            sb.Length = 0;
                        }
                        row++;
                        col = 0;
                    }
                    continue;
                }
            }
            return mapData;
        }

        public Sprite this[int x, int y]
        {
            get
            {
                return Tiles[x, y];
            }
            set
            {
                Tiles[x, y] = value;
            }
        }
        public Sprite this[int index]
        {
            get
            {
                if (index < 0)
                {
                    return null;
                }
                return Tiles[index % Tiles.Columns, index / Tiles.Columns];
            }
            set
            {
                if (index < 0)
                {
                    return;
                }
                Tiles[index % Tiles.Columns, index / Tiles.Columns] = value;
            }
        }
    }
}