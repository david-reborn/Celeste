using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using myd.celeste;
using System.IO;

public class Atlas
{

    private Dictionary<string, MTexture> textures = new Dictionary<string, MTexture>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, List<MTexture>> orderedTexturesCache = new Dictionary<string, List<MTexture>>();
    private Dictionary<string, string> links = new Dictionary<string, string>();
    public List<Texture2D> Sources;

    public static Atlas FromAtlas(string path, Atlas.AtlasDataFormat format)
    {
        Atlas atlas = new Atlas();
        atlas.Sources = new List<Texture2D>();
        Atlas.ReadAtlasData(atlas, path, format);
        return atlas;
    }

    public MTexture this[string id]
    {
        get
        {
            return this.textures[id];
        }
        set
        {
            this.textures[id] = value;
        }
    }

    private static void ReadAtlasData(Atlas atlas, string path, Atlas.AtlasDataFormat format)
    {
        switch (format)
        {
            //case Atlas.AtlasDataFormat.TexturePacker_Sparrow:
            //    XmlElement xml1 = Util.LoadContentXML(path)["TextureAtlas"];
            //    string path2_1 = xml1.Attr("imagePath", "");
            //    VirtualTexture texture1 = VirtualContent.CreateTexture(Path.Combine(Path.GetDirectoryName(path), path2_1));
            //    MTexture parent1 = new MTexture(texture1);
            //    atlas.Sources.Add(texture1);
            //    IEnumerator enumerator1 = xml1.GetElementsByTagName("SubTexture").GetEnumerator();
            //    try
            //    {
            //        while (enumerator1.MoveNext())
            //        {
            //            XmlElement current = (XmlElement)enumerator1.Current;
            //            string atlasPath = current.Attr("name");
            //            Rectangle clipRect = current.Rect();
            //            atlas.textures[atlasPath] = !current.HasAttr("frameX") ? new MTexture(parent1, atlasPath, clipRect) : new MTexture(parent1, atlasPath, clipRect, new Vector2((float)-current.AttrInt("frameX"), (float)-current.AttrInt("frameY")), current.AttrInt("frameWidth"), current.AttrInt("frameHeight"));
            //        }
            //        break;
            //    }
            //    finally
            //    {
            //        if (enumerator1 is IDisposable disposable)
            //            disposable.Dispose();
            //    }
            //case Atlas.AtlasDataFormat.CrunchXml:
            //    IEnumerator enumerator2 = Calc.LoadContentXML(path)[nameof(atlas)].GetEnumerator();
            //    try
            //    {
            //        while (enumerator2.MoveNext())
            //        {
            //            XmlElement current = (XmlElement)enumerator2.Current;
            //            string str = current.Attr("n", "");
            //            VirtualTexture texture2 = VirtualContent.CreateTexture(Path.Combine(Path.GetDirectoryName(path), str + ".png"));
            //            MTexture parent2 = new MTexture(texture2);
            //            atlas.Sources.Add(texture2);
            //            foreach (XmlElement xml2 in (XmlNode)current)
            //            {
            //                string atlasPath = xml2.Attr("n");
            //                Rectangle clipRect = new Rectangle(xml2.AttrInt("x"), xml2.AttrInt("y"), xml2.AttrInt("w"), xml2.AttrInt("h"));
            //                atlas.textures[atlasPath] = !xml2.HasAttr("fx") ? new MTexture(parent2, atlasPath, clipRect) : new MTexture(parent2, atlasPath, clipRect, new Vector2((float)-xml2.AttrInt("fx"), (float)-xml2.AttrInt("fy")), xml2.AttrInt("fw"), xml2.AttrInt("fh"));
            //            }
            //        }
            //        break;
            //    }
            //    finally
            //    {
            //        if (enumerator2 is IDisposable disposable)
            //            disposable.Dispose();
            //    }
            //case Atlas.AtlasDataFormat.CrunchBinary:
            //    using (FileStream fileStream = File.OpenRead(Path.Combine(Engine.ContentDirectory, path)))
            //    {
            //        BinaryReader stream = new BinaryReader((Stream)fileStream);
            //        short num1 = stream.ReadInt16();
            //        for (int index1 = 0; index1 < (int)num1; ++index1)
            //        {
            //            string str = stream.ReadNullTerminatedString();
            //            VirtualTexture texture2 = VirtualContent.CreateTexture(Path.Combine(Path.GetDirectoryName(path), str + ".png"));
            //            atlas.Sources.Add(texture2);
            //            MTexture parent2 = new MTexture(texture2);
            //            short num2 = stream.ReadInt16();
            //            for (int index2 = 0; index2 < (int)num2; ++index2)
            //            {
            //                string atlasPath = stream.ReadNullTerminatedString();
            //                short num3 = stream.ReadInt16();
            //                short num4 = stream.ReadInt16();
            //                short num5 = stream.ReadInt16();
            //                short num6 = stream.ReadInt16();
            //                short num7 = stream.ReadInt16();
            //                short num8 = stream.ReadInt16();
            //                short num9 = stream.ReadInt16();
            //                short num10 = stream.ReadInt16();
            //                atlas.textures[atlasPath] = new MTexture(parent2, atlasPath, new Rectangle((int)num3, (int)num4, (int)num5, (int)num6), new Vector2((float)-num7, (float)-num8), (int)num9, (int)num10);
            //            }
            //        }
            //        break;
            //    }
            //case Atlas.AtlasDataFormat.CrunchXmlOrBinary:
            //    if (File.Exists(Path.Combine(Engine.ContentDirectory, path + ".bin")))
            //    {
            //        Atlas.ReadAtlasData(atlas, path + ".bin", Atlas.AtlasDataFormat.CrunchBinary);
            //        break;
            //    }
            //    Atlas.ReadAtlasData(atlas, path + ".xml", Atlas.AtlasDataFormat.CrunchXml);
            //    break;
            //case Atlas.AtlasDataFormat.CrunchBinaryNoAtlas:
            //    using (FileStream fileStream = File.OpenRead(Path.Combine(Engine.ContentDirectory, path + ".bin")))
            //    {
            //        BinaryReader stream = new BinaryReader((Stream)fileStream);
            //        short num1 = stream.ReadInt16();
            //        for (int index1 = 0; index1 < (int)num1; ++index1)
            //        {
            //            string path2_2 = stream.ReadNullTerminatedString();
            //            string path1 = Path.Combine(Path.GetDirectoryName(path), path2_2);
            //            short num2 = stream.ReadInt16();
            //            for (int index2 = 0; index2 < (int)num2; ++index2)
            //            {
            //                string index3 = stream.ReadNullTerminatedString();
            //                stream.ReadInt16();
            //                stream.ReadInt16();
            //                stream.ReadInt16();
            //                stream.ReadInt16();
            //                short num3 = stream.ReadInt16();
            //                short num4 = stream.ReadInt16();
            //                short num5 = stream.ReadInt16();
            //                short num6 = stream.ReadInt16();
            //                VirtualTexture texture2 = VirtualContent.CreateTexture(Path.Combine(path1, index3 + ".png"));
            //                atlas.Sources.Add(texture2);
            //                atlas.textures[index3] = new MTexture(texture2, new Vector2((float)-num3, (float)-num4), (int)num5, (int)num6);
            //            }
            //        }
            //        break;
            //    }
            case Atlas.AtlasDataFormat.Packer:
                using (FileStream fileStream = File.OpenRead(Path.Combine(Util.GAME_PATH_CONTENT, path + ".meta")))
                {
                    BinaryReader binaryReader = new BinaryReader((Stream)fileStream);
                    binaryReader.ReadInt32();
                    binaryReader.ReadString();
                    binaryReader.ReadInt32();
                    short num1 = binaryReader.ReadInt16();
                    for (int index1 = 0; index1 < (int)num1; ++index1)
                    {
                        string str = binaryReader.ReadString();
                        string filePath = Path.Combine(Util.GAME_PATH_CONTENT, Path.GetDirectoryName(path), str + ".data");
                        Texture2D texture2D = ReadData(str+".data");
                        //Texture2D texture2 = null;//VirtualContent.CreateTexture(Path.Combine(Path.GetDirectoryName(path), str + ".data"));
                        atlas.Sources.Add(texture2D);
                        //byte[] _bytes = texture2D.EncodeToPNG();
                        //System.IO.File.WriteAllBytes("F://test.png", _bytes);
                        MTexture parent2 = new MTexture(texture2D);
                        short num2 = binaryReader.ReadInt16();
                        for (int index2 = 0; index2 < (int)num2; ++index2)
                        {
                            string atlasPath = binaryReader.ReadString().Replace('\\', '/');
                            if (atlasPath == "tilesets/dirt")
                            {
                                Debug.Log(11);
                            }
                            short x = binaryReader.ReadInt16();
                            short y = binaryReader.ReadInt16();
                            short w = binaryReader.ReadInt16();
                            short h = binaryReader.ReadInt16();
                            short ox = binaryReader.ReadInt16();
                            short oy = binaryReader.ReadInt16();
                            short rw = binaryReader.ReadInt16();
                            short rh = binaryReader.ReadInt16();
                            atlas.textures[atlasPath] = new MTexture(parent2, atlasPath, new Rect((int)x, (int)y, (int)w, (int)h), new Vector2((float)-ox, (float)-oy), (int)rw, (int)rh);
                        }
                    }
                    if (fileStream.Position >= fileStream.Length || !(binaryReader.ReadString() == "LINKS"))
                        break;
                    short num11 = binaryReader.ReadInt16();
                    for (int index = 0; index < (int)num11; ++index)
                    {
                        string key = binaryReader.ReadString();
                        string str = binaryReader.ReadString();
                        atlas.links.Add(key, str);
                    }
                    break;
                }
            //case Atlas.AtlasDataFormat.PackerNoAtlas:
            //    using (FileStream fileStream = File.OpenRead(Path.Combine(Engine.ContentDirectory, path + ".meta")))
            //    {
            //        BinaryReader binaryReader = new BinaryReader((Stream)fileStream);
            //        binaryReader.ReadInt32();
            //        binaryReader.ReadString();
            //        binaryReader.ReadInt32();
            //        short num1 = binaryReader.ReadInt16();
            //        for (int index1 = 0; index1 < (int)num1; ++index1)
            //        {
            //            string path2_2 = binaryReader.ReadString();
            //            string path1 = Path.Combine(Path.GetDirectoryName(path), path2_2);
            //            short num2 = binaryReader.ReadInt16();
            //            for (int index2 = 0; index2 < (int)num2; ++index2)
            //            {
            //                string index3 = binaryReader.ReadString().Replace('\\', '/');
            //                binaryReader.ReadInt16();
            //                binaryReader.ReadInt16();
            //                binaryReader.ReadInt16();
            //                binaryReader.ReadInt16();
            //                short num3 = binaryReader.ReadInt16();
            //                short num4 = binaryReader.ReadInt16();
            //                short num5 = binaryReader.ReadInt16();
            //                short num6 = binaryReader.ReadInt16();
            //                VirtualTexture texture2 = VirtualContent.CreateTexture(Path.Combine(path1, index3 + ".data"));
            //                atlas.Sources.Add(texture2);
            //                atlas.textures[index3] = new MTexture(texture2, new Vector2((float)-num3, (float)-num4), (int)num5, (int)num6);
            //                atlas.textures[index3].AtlasPath = index3;
            //            }
            //        }
            //        if (fileStream.Position >= fileStream.Length || !(binaryReader.ReadString() == "LINKS"))
            //            break;
            //        short num7 = binaryReader.ReadInt16();
            //        for (int index = 0; index < (int)num7; ++index)
            //        {
            //            string key = binaryReader.ReadString();
            //            string str = binaryReader.ReadString();
            //            atlas.links.Add(key, str);
            //        }
            //        break;
            //    }
            default:
                throw new NotImplementedException();
        }
    }

    public List<MTexture> GetAtlasSubtextures(string key)
    {
        List<MTexture> mtextureList;
        if (!this.orderedTexturesCache.TryGetValue(key, out mtextureList))
        {
            mtextureList = new List<MTexture>();
            int index = 0;
            while (true)
            {
                MTexture subtextureFromAtlasAt = this.GetAtlasSubtextureFromAtlasAt(key, index);
                if (subtextureFromAtlasAt != null)
                {
                    mtextureList.Add(subtextureFromAtlasAt);
                    ++index;
                }
                else
                    break;
            }
            this.orderedTexturesCache.Add(key, mtextureList);
        }
        return mtextureList;
    }

    private MTexture GetAtlasSubtextureFromAtlasAt(string key, int index)
    {
        if (index == 0 && this.textures.ContainsKey(key))
            return this.textures[key];
        string str = index.ToString();
        for (int length = str.Length; str.Length < length + 6; str = "0" + str)
        {
            MTexture mtexture;
            if (this.textures.TryGetValue(key + str, out mtexture))
                return mtexture;
        }
        return (MTexture)null;
    }

    public static Texture2D ReadData(string path)
    {
        using (FileStream stream = File.OpenRead(Path.Combine(Util.GAME_PATH_CONTENT,"Graphics","Atlases", path)))
        {
            byte[] buffer;
            byte[] buffer2 = new byte[0x4000000];
            byte[] bytes = new byte[0x80000];

            stream.Read(bytes, 0, 0x80000);
            int startIndex = 0;
            int width = BitConverter.ToInt32(bytes, startIndex);
            int height = BitConverter.ToInt32(bytes, startIndex + 4);
            Color32[] colors = new Color32[width * height];
            bool flag4 = bytes[startIndex + 8] == 1;
            startIndex += 9;
            int elementCount = (width * height) * 4;
            int index = 0;

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            try
            {
                byte[] numPtr;
                byte[] numPtr2;
                if (((buffer = bytes) == null) || (buffer.Length == 0))
                {
                    numPtr = null;
                }
                else
                {
                    numPtr = buffer;
                }
                if (buffer2.Length == 0)
                {
                    numPtr2 = null;
                }
                else
                {
                    numPtr2 = buffer2;
                }
                while (true)
                {
                    if (index >= elementCount)
                    {
                        break;
                    }
                    int num7 = numPtr[startIndex] * 4;
                    if (!flag4)
                    {
                        numPtr2[index] = numPtr[startIndex + 3];
                        numPtr2[index + 1] = numPtr[startIndex + 2];
                        numPtr2[index + 2] = numPtr[startIndex + 1];
                        numPtr2[index + 3] = 0xff;
                        startIndex += 4;
                        colors[index / 4] = new Color32(numPtr2[index], numPtr2[index + 1], numPtr2[index + 2], numPtr2[index + 3]);
                        //texture.SetPixel(index% (width*4), index / (width), new Color(numPtr2[index], numPtr2[index + 1], numPtr2[index + 2], numPtr2[index+3]));
                    }
                    else
                    {
                        byte num8 = numPtr[startIndex + 1];
                        if (num8 > 0)
                        {
                            numPtr2[index] = numPtr[startIndex + 4];
                            numPtr2[index + 1] = numPtr[startIndex + 3];
                            numPtr2[index + 2] = numPtr[startIndex + 2];
                            numPtr2[index + 3] = num8;
                            startIndex += 5;
                        }
                        else
                        {
                            numPtr2[index] = 0;
                            numPtr2[index + 1] = 0;
                            numPtr2[index + 2] = 0;
                            numPtr2[index + 3] = 0;
                            startIndex += 2;
                        }
                        colors[index / 4] = new Color32(numPtr2[index], numPtr2[index + 1], numPtr2[index + 2], numPtr2[index + 3]);
                        //texture.SetPixel(index % (width*4), index / (width * 4), new Color(numPtr2[index+3], numPtr2[index + 2], numPtr2[index + 1], numPtr2[index]));

                    }
                    if (num7 > 4)
                    {
                        int num9 = index + 4;
                        int num10 = index + num7;
                        while (true)
                        {
                            if (num9 >= num10)
                            {
                                break;
                            }
                            numPtr2[num9] = numPtr2[index];
                            numPtr2[num9 + 1] = numPtr2[index + 1];
                            numPtr2[num9 + 2] = numPtr2[index + 2];
                            numPtr2[num9 + 3] = numPtr2[index + 3];

                            colors[num9 / 4] = new Color32(numPtr2[num9], numPtr2[num9 + 1], numPtr2[num9 + 2], numPtr2[num9 + 3]);
                            //texture.SetPixel(num9 % (width * 4), num9 / (width * 4), new Color(numPtr2[num9+3], numPtr2[num9 + 2], numPtr2[num9 + 1], numPtr2[num9]));

                            num9 += 4;
                        }
                    }
                    index += num7;
                    if (startIndex > 0x7ffe0)
                    {
                        int offset = 0x80000 - startIndex;
                        int num12 = 0;
                        while (true)
                        {
                            if (num12 >= offset)
                            {
                                stream.Read(bytes, offset, 0x80000 - offset);
                                startIndex = 0;
                                break;
                            }
                            numPtr[num12] = numPtr[startIndex + num12];
                            num12++;
                        }
                    }
                }
            }
            finally
            {
                buffer = null;
            }

            Color32[] tempColors = new Color32[colors.Length];

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    int tIndex = h * width + w;
                    int aIndex = (height - h - 1) * width + w;
                    tempColors[tIndex] = colors[aIndex];
                }
            }
            texture.SetPixels32(0, 0, width, height, tempColors);
            texture.Apply();
            //byte[] _bytes = texture.EncodeToPNG();
            //System.IO.File.WriteAllBytes("F://" + path, _bytes);
            return texture;
        }
    }

    public enum AtlasDataFormat
    {
        TexturePacker_Sparrow,
        CrunchXml,
        CrunchBinary,
        CrunchXmlOrBinary,
        CrunchBinaryNoAtlas,
        Packer,
        PackerNoAtlas,
    }
}
