using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using myd.celeste;
using System;

public class Autotiler 
{

    public List<Rect> LevelBounds = new List<Rect>();
    private Dictionary<char, Autotiler.TerrainType> lookup = new Dictionary<char, Autotiler.TerrainType>();
    private byte[] adjacent = new byte[9];

    public Autotiler(string filename)
    {
        Dictionary<char, XmlElement> dictionary = new Dictionary<char, XmlElement>();
        foreach (XmlElement xml in Util.LoadContentXML(filename).GetElementsByTagName("Tileset"))
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

    private class TerrainType
    {
        public HashSet<char> Ignores = new HashSet<char>();
        public List<Autotiler.Masked> Masked = new List<Autotiler.Masked>();
        public Autotiler.Tiles Center = new Autotiler.Tiles();
        public Autotiler.Tiles Padded = new Autotiler.Tiles();
        public char ID;

        public TerrainType(char id)
        {
            this.ID = id;
        }

        public bool Ignore(char c)
        {
            return (int)this.ID != (int)c && (this.Ignores.Contains(c) || this.Ignores.Contains('*'));
        }
    }

    private class Masked
    {
        public byte[] Mask = new byte[9];
        public Autotiler.Tiles Tiles = new Autotiler.Tiles();
    }

    private class Tiles
    {
        public List<MTexture> Textures = new List<MTexture>();
        public List<string> OverlapSprites = new List<string>();
        public bool HasOverlays = false;
    }

    public struct Generated
    {
        public TileGrid TileGrid;
        public AnimatedTiles SpriteOverlay;
    }

    public struct Behaviour
    {
        public bool PaddingIgnoreOutOfLevel;
        public bool EdgesIgnoreOutOfLevel;
        public bool EdgesExtend;
    }
}
