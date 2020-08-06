using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace myd.celeste
{
    public static class Util
    {
        //public static string ROOT = "F:/Unity/Celeste/Resources/";
        //public static string GAME_PATH = "F:/Steam/steamapps/common/Celeste/";
        //public static string GAME_PATH_CONTENT = "F:/Steam/steamapps/common/Celeste/Content/";

        public static string ROOT = "F:/Celeste/Resources/";
        public static string GAME_PATH = "D:/Program Files (x86)/Steam/steamapps/common/Celeste/";
        public static string GAME_PATH_CONTENT = "D:/Program Files (x86)/Steam/steamapps/common/Celeste/Content/";

        public static Rand Random = new Rand();

        public static float ClampedMap(float val, float min, float max, float newMin = 0.0f, float newMax = 1f)
        {
            return Mathf.Clamp((float)(((double)val - (double)min) / ((double)max - (double)min)), 0.0f, 1f) * (newMax - newMin) + newMin;
        }

        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        public static string Attr(this XmlElement xml, string attributeName)
        {
            return xml.Attributes[attributeName].InnerText;
        }

        public static string Attr(this XmlElement xml, string attributeName, string v)
        {
            return xml.Attributes[attributeName]?.InnerText;
        }
        public static bool HasAttr(this XmlElement xml, string attributeName)
        {
            return xml.Attributes[attributeName] != null;
        }

        public static char AttrChar(this XmlElement xml, string attributeName)
        {
            return Convert.ToChar(xml.Attributes[attributeName]?.InnerText);
        }

        public static Stream ReadResourceStream(string path)
        {
            FileStream stream = new FileStream(ROOT + path, FileMode.Open);
            return stream;
            //return current.GetManifestResourceStream(current.GetName().Name + "." + path);
        }

        public static string ReadResource(string path)
        {
            using (FileStream stream = new FileStream(ROOT + path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static Sprite GetSubImage(Texture2D parent, ExtSprite bounds)
        {
            return Sprite.Create(parent, bounds.Bounds, new Vector2(0.5f, 0.5f));
        }

        public static Sprite GetSubImage(Sprite parent, ExtSprite bounds)
        {
            return Sprite.Create(parent.texture, bounds.Bounds, new Vector2(0.5f, 0.5f));
        }



        public static Color HexToColor(string hex)
        {
            if (hex.Length >= 6)
            {
                int r = HexToByte(hex[0]) * 16 + HexToByte(hex[1]);
                int g = HexToByte(hex[2]) * 16 + HexToByte(hex[3]);
                int b = HexToByte(hex[4]) * 16 + HexToByte(hex[5]);
                return new Color(r, g, b);
            }
            return Color.white;
        }
        public static byte HexToByte(char c)
        {
            return (byte)"0123456789ABCDEF".IndexOf(char.ToUpper(c));
        }
        

        public static Vector2 ClosestTo(this List<Vector2> list, Vector2 to)
        {
            Vector2 vector2 = list[0];
            //float num1 = Vector2.DistanceSquared(list[0], to);
            float num1 = (list[0] - to).SqrMagnitude();
            for (int index = 1; index < list.Count; ++index)
            {
                //float num2 = Vector2.DistanceSquared(list[index], to);
                float num2 = (list[index]-to).SqrMagnitude();
                if ((double)num2 < (double)num1)
                {
                    num1 = num2;
                    vector2 = list[index];
                }
            }
            return vector2;
        }

        public static void CopyTo(Texture2D dest, Texture2D src, Vector2 pos)
        {
            //if (src != null)
            //{
            //    byte[] srcData = src.GetRawTextureData();
            //    dest.LoadRawTextureData();
            //    using (Graphics g = Graphics.FromImage(dest))
            //    {
            //        g.DrawImage(src, pos);
            //    }
            //}
        }

        public static int[,] ReadCSVIntGrid(string csv, int width, int height)
        {
            int[,] numArray = new int[width, height];
            for (int index1 = 0; index1 < width; ++index1)
            {
                for (int index2 = 0; index2 < height; ++index2)
                    numArray[index1, index2] = -1;
            }
            string[] strArray1 = csv.Split('\n');
            for (int index1 = 0; index1 < height && index1 < strArray1.Length; ++index1)
            {
                string[] strArray2 = strArray1[index1].Split(new char[1]
                {
          ','
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int index2 = 0; index2 < width && index2 < strArray2.Length; ++index2)
                    numArray[index2, index1] = Convert.ToInt32(strArray2[index2]);
            }
            return numArray;
        }

        public static int[] ReadCSVInt(string csv)
        {
            if (csv == "")
                return new int[0];
            string[] strArray = csv.Split(',');
            int[] numArray = new int[strArray.Length];
            for (int index = 0; index < strArray.Length; ++index)
                numArray[index] = Convert.ToInt32(strArray[index].Trim());
            return numArray;
        }

        public static int[] ReadCSVIntWithTricks(string csv)
        {
            if (csv == "")
                return new int[0];
            string[] strArray1 = csv.Split(',');
            List<int> intList = new List<int>();
            foreach (string str in strArray1)
            {
                if (str.IndexOf('-') != -1)
                {
                    string[] strArray2 = str.Split('-');
                    int int32_1 = Convert.ToInt32(strArray2[0]);
                    int int32_2 = Convert.ToInt32(strArray2[1]);
                    for (int index = int32_1; index != int32_2; index += Math.Sign(int32_2 - int32_1))
                        intList.Add(index);
                    intList.Add(int32_2);
                }
                else if (str.IndexOf('*') != -1)
                {
                    string[] strArray2 = str.Split('*');
                    int int32_1 = Convert.ToInt32(strArray2[0]);
                    int int32_2 = Convert.ToInt32(strArray2[1]);
                    for (int index = 0; index < int32_2; ++index)
                        intList.Add(int32_1);
                }
                else
                    intList.Add(Convert.ToInt32(str));
            }
            return intList.ToArray();
        }

        public static string[] ReadCSV(string csv)
        {
            if (csv == "")
                return new string[0];
            string[] strArray = csv.Split(',');
            for (int index = 0; index < strArray.Length; ++index)
                strArray[index] = strArray[index].Trim();
            return strArray;
        }

        public static string IntGridToCSV(int[,] data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<int> intList = new List<int>();
            int num1 = 0;
            for (int index1 = 0; index1 < data.GetLength(1); ++index1)
            {
                int num2 = 0;
                for (int index2 = 0; index2 < data.GetLength(0); ++index2)
                {
                    if (data[index2, index1] == -1)
                    {
                        ++num2;
                    }
                    else
                    {
                        for (int index3 = 0; index3 < num1; ++index3)
                            stringBuilder.Append('\n');
                        for (int index3 = 0; index3 < num2; ++index3)
                            intList.Add(-1);
                        num2 = num1 = 0;
                        intList.Add(data[index2, index1]);
                    }
                }
                if (intList.Count > 0)
                {
                    stringBuilder.Append(string.Join<int>(",", (IEnumerable<int>)intList));
                    intList.Clear();
                }
                ++num1;
            }
            return stringBuilder.ToString();
        }

        public static Vector2 Floor(this Vector2 val)
        {
            return new Vector2((float)(int)Math.Floor((double)val.x), (float)(int)Math.Floor((double)val.y));
        }

    }
}