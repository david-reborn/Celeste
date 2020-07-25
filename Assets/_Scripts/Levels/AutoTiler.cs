using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System;

namespace myd.celeste
{
    public class AutoTiler
    {
    //    private Dictionary<char, TerrainType> lookup = new Dictionary<char, TerrainType>();

    //    public AutoTiler(string xmlDescription)
    //    {
    //        XmlDocument document = new XmlDocument();
    //        document.LoadXml(xmlDescription);
    //        Dictionary<char, XmlElement> dictionary = new Dictionary<char, XmlElement>();

    //        foreach (object obj in document.GetElementsByTagName("Tileset"))
    //        {
    //            XmlElement xmlElement = (XmlElement)obj;
    //            char c = xmlElement.AttrChar("id");
    //            Sprite tileImages = Gameplay.GetImage("tilesets/" + xmlElement.Attr("path"));
    //            TileGrid tileset = new TileGrid(8, 8, tileImages.Width / 8, tileImages.Height / 8);
    //            tileset.Load(tileImages);
    //            TerrainType terrainType = new TerrainType(c);
    //            ReadInto(terrainType, tileset, xmlElement);

    //            if (xmlElement.HasAttr("copy"))
    //            {
    //                char key = xmlElement.AttrChar("copy");
    //                if (!dictionary.ContainsKey(key))
    //                {
    //                    throw new Exception("Copied tilesets must be defined before the tilesets that copy them!");
    //                }
    //                ReadInto(terrainType, tileset, dictionary[key]);
    //            }

    //            if (xmlElement.HasAttr("ignores"))
    //            {
    //                foreach (string text in xmlElement.Attr("ignores").Split(','))
    //                {
    //                    if (text.Length > 0)
    //                    {
    //                        terrainType.Ignores.Add(text[0]);
    //                    }
    //                }
    //            }

    //            dictionary.Add(c, xmlElement);
    //            lookup.Add(c, terrainType);
    //        }
    //    }

    //    private void ReadInto(TerrainType data, TileGrid tileset, XmlElement xml)
    //    {
    //        foreach (object obj in xml)
    //        {
    //            if (!(obj is XmlComment))
    //            {
    //                XmlElement xml2 = obj as XmlElement;
    //                string text = xml2.Attr("mask");
    //                Tiles tiles;
    //                if (text == "center")
    //                {
    //                    tiles = data.Center;
    //                }
    //                else if (text == "padding")
    //                {
    //                    tiles = data.Padded;
    //                }
    //                else
    //                {
    //                    Masked masked = new Masked();
    //                    tiles = masked.Tiles;
    //                    int i = 0;
    //                    int num = 0;
    //                    while (i < text.Length)
    //                    {
    //                        if (text[i] == '0')
    //                        {
    //                            masked.Mask[num++] = 0;
    //                        }
    //                        else if (text[i] == '1')
    //                        {
    //                            masked.Mask[num++] = 1;
    //                        }
    //                        else if (text[i] == 'x' || text[i] == 'X')
    //                        {
    //                            masked.Mask[num++] = 2;
    //                        }
    //                        i++;
    //                    }
    //                    data.Masked.Add(masked);
    //                }
    //                string[] array = xml2.Attr("tiles").Split(';');
    //                for (int j = 0; j < array.Length; j++)
    //                {
    //                    string[] array2 = array[j].Split(',');
    //                    int x = int.Parse(array2[0]);
    //                    int y = int.Parse(array2[1]);
    //                    Texture2D item = tileset.Tiles[x, y];
    //                    tiles.Textures.Add(item);
    //                }
    //                if (xml2.HasAttr("sprites"))
    //                {
    //                    foreach (string item2 in xml2.Attr("sprites").Split(','))
    //                    {
    //                        tiles.OverlapSprites.Add(item2);
    //                    }
    //                    tiles.HasOverlays = true;
    //                }
    //            }
    //        }
    //        data.Masked.Sort(delegate (Masked a, Masked b) {
    //            int num2 = 0;
    //            int num3 = 0;
    //            for (int k = 0; k < 9; k++)
    //            {
    //                if (a.Mask[k] == 2)
    //                {
    //                    num2++;
    //                }
    //                if (b.Mask[k] == 2)
    //                {
    //                    num3++;
    //                }
    //            }
    //            return num2 - num3;
    //        });
    //    }

    //    private class TerrainType
    //    {
    //        public char ID;
    //        public HashSet<char> Ignores = new HashSet<char>();
    //        public List<Masked> Masked = new List<Masked>();
    //        public Tiles Center = new Tiles();
    //        public Tiles Padded = new Tiles();

    //        public TerrainType(char id)
    //        {
    //            ID = id;
    //        }
    //        public bool Ignore(char c)
    //        {
    //            return ID != c && (Ignores.Contains(c) || Ignores.Contains('*'));
    //        }
    //    }
    //    private class Masked
    //    {
    //        public byte[] Mask = new byte[9];
    //        public Tiles Tiles = new Tiles();
    //    }
    //    private class Tiles
    //    {
    //        public List<Texture2D> Textures = new List<Texture2D>();
    //        public List<string> OverlapSprites = new List<string>();
    //        public bool HasOverlays;
    //    }
    //}

    //public class TileGrid
    //{
    //    public static Color DefaultBackground = Color.black;
    //    public int Width, Height;
    //    public VirtualMap<Texture2D> Tiles;
    //    public TileGrid(int w, int h, int tilesX, int tilesY)
    //    {
    //        Width = w;
    //        Height = h;
    //        Tiles = new VirtualMap<Texture2D>(tilesX, tilesY, null);
    //    }
    //    public void Load(Texture2D images)
    //    {
    //        for (int i = 0; i < Tiles.Columns; i++)
    //        {
    //            for (int j = 0; j < Tiles.Rows; j++)
    //            {
    //                Tiles[i, j] = Util.GetSubImage(images, new ExtSprite(i * Width, j * Height, Width, Height, 0, 0, Width, Height));
    //            }
    //        }
    //    }
    //    public Texture2D DisplayMap(MapElement level, List<Backdrop> backdrops, Rectangle bounds, bool before)
    //    {
    //        if (Tiles.Columns == 0 || Tiles.Rows == 0) { return null; }

    //        Texture2D img = new Texture2D(Tiles.Columns * Width, Tiles.Rows * Height, PixelFormat.Format32bppArgb);
    //        using (Graphics g = Graphics.FromImage(img))
    //        {
    //            if (before)
    //            {
    //                using (SolidBrush brush = new SolidBrush(DefaultBackground))
    //                {
    //                    g.FillRectangle(brush, 0, 0, img.Width, img.Height);
    //                }

    //                if (backdrops != null)
    //                {
    //                    for (int i = 0; i < backdrops.Count; i++)
    //                    {
    //                        Backdrop bd = backdrops[i];
    //                        if (level == null || bd.IsVisible(level.Attr("name")))
    //                        {
    //                            bd.Render(bounds, g);
    //                        }
    //                    }
    //                }
    //            }

    //            for (int i = 0; i < Tiles.Columns; i++)
    //            {
    //                for (int j = 0; j < Tiles.Rows; j++)
    //                {
    //                    Texture2D tile = Tiles[i, j];
    //                    if (tile != null)
    //                    {
    //                        g.DrawImage(tile, (float)i * Width, (float)j * Height);
    //                    }
    //                }
    //            }

    //            if (!before && backdrops != null)
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
    //        return img;
    //    }
    //    public Texture2D GenerateMap(VirtualMap<int> map)
    //    {
    //        if (map.Columns == 0 || map.Rows == 0) { return null; }

    //        Texture2D img = new Texture2D(map.Columns * Width, map.Rows * Height, PixelFormat.Format32bppArgb);
    //        using (Graphics g = Graphics.FromImage(img))
    //        {
    //            for (int i = 0; i < map.Columns; i++)
    //            {
    //                for (int j = 0; j < map.Rows; j++)
    //                {
    //                    Texture2D tile = this[map[i, j]];
    //                    if (tile != null)
    //                    {
    //                        g.DrawImage(tile, (float)i * Width, (float)j * Height);
    //                    }
    //                }
    //            }
    //        }
    //        return img;
    //    }
    //    public void Overlay(MapElement level, string objects, int width, int height, TileGrid tileset)
    //    {
    //        MapElement tileData = level.SelectFirst(objects);
    //        VirtualMap<int> map = ReadMapInt(tileData == null ? string.Empty : tileData.Attr("InnerText"), width, height);

    //        if (map.Columns == 0 || map.Rows == 0) { return; }

    //        for (int i = 0; i < map.Columns; i++)
    //        {
    //            for (int j = 0; j < map.Rows; j++)
    //            {
    //                Texture2D tile = tileset[map[i, j]];
    //                if (tile != null)
    //                {
    //                    this[i, j] = tile;
    //                }
    //            }
    //        }
    //    }
    //    private VirtualMap<int> ReadMapInt(string tiles, int width, int height)
    //    {
    //        VirtualMap<int> mapData = new VirtualMap<int>(width, height, -1);
    //        int length = tiles.Length;
    //        int i = 0;
    //        int col = 0, row = 0;
    //        StringBuilder sb = new StringBuilder();
    //        while (i < length)
    //        {
    //            char val = tiles[i++];
    //            if (char.IsNumber(val) || val == '-')
    //            {
    //                sb.Append(val);
    //            }
    //            else if (val == ',')
    //            {
    //                mapData[col++, row] = int.Parse(sb.ToString());
    //                sb.Length = 0;
    //            }
    //            else if (val == '\r' || val == '\n')
    //            {
    //                if (val == '\n')
    //                {
    //                    if (sb.Length > 0)
    //                    {
    //                        mapData[col, row] = int.Parse(sb.ToString());
    //                        sb.Length = 0;
    //                    }
    //                    row++;
    //                    col = 0;
    //                }
    //                continue;
    //            }
    //        }
    //        return mapData;
    //    }
    //    public Texture2D this[int x, int y]
    //    {
    //        get
    //        {
    //            return Tiles[x, y];
    //        }
    //        set
    //        {
    //            Tiles[x, y] = value;
    //        }
    //    }
    //    public Texture2D this[int index]
    //    {
    //        get
    //        {
    //            if (index < 0)
    //            {
    //                return null;
    //            }
    //            return Tiles[index % Tiles.Columns, index / Tiles.Columns];
    //        }
    //        set
    //        {
    //            if (index < 0)
    //            {
    //                return;
    //            }
    //            Tiles[index % Tiles.Columns, index / Tiles.Columns] = value;
    //        }
    //    }
    }
}