using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using System;
using System.Xml;
using System.Collections.Generic;

namespace myd.celeste
{
    public static class Util
    {
        public static string ROOT = "F:/Unity/Celeste/Resources/";
        public static string GAME_PATH = "F:/steam/steamapps/common/Celeste/";
        public static string GAME_PATH_CONTENT = "F:/steam/steamapps/common/Celeste/Content/";

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

        public static T Choose<T>(this Rand random, T a, T b)
        {
            return GiveMe<T>(random.Next(2), a, b);
        }
        public static T Choose<T>(this Rand random, T a, T b, T c)
        {
            return GiveMe<T>(random.Next(3), a, b, c);
        }
        public static T Choose<T>(this Rand random, T a, T b, T c, T d)
        {
            return GiveMe<T>(random.Next(4), a, b, c, d);
        }
        public static T Choose<T>(this Rand random, T a, T b, T c, T d, T e)
        {
            return GiveMe<T>(random.Next(5), a, b, c, d, e);
        }
        public static T Choose<T>(this Rand random, T a, T b, T c, T d, T e, T f)
        {
            return GiveMe<T>(random.Next(6), a, b, c, d, e, f);
        }
        public static T Choose<T>(this Rand random, params T[] choices)
        {
            return choices[random.Next(choices.Length)];
        }
        public static T Choose<T>(this Rand random, List<T> choices)
        {
            return choices[random.Next(choices.Count)];
        }

        public static string Attr(this XmlElement xml, string attributeName)
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
        public static T GiveMe<T>(int index, T a, T b)
        {
            if (index == 0)
            {
                return a;
            }
            if (index != 1)
            {
                throw new Exception("Index was out of range!");
            }
            return b;
        }
        public static T GiveMe<T>(int index, T a, T b, T c)
        {
            switch (index)
            {
                case 0: return a;
                case 1: return b;
                case 2: return c;
                default: throw new Exception("Index was out of range!");
            }
        }
        public static T GiveMe<T>(int index, T a, T b, T c, T d)
        {
            switch (index)
            {
                case 0: return a;
                case 1: return b;
                case 2: return c;
                case 3: return d;
                default: throw new Exception("Index was out of range!");
            }
        }
        public static T GiveMe<T>(int index, T a, T b, T c, T d, T e)
        {
            switch (index)
            {
                case 0: return a;
                case 1: return b;
                case 2: return c;
                case 3: return d;
                case 4: return e;
                default: throw new Exception("Index was out of range!");
            }
        }
        public static T GiveMe<T>(int index, T a, T b, T c, T d, T e, T f)
        {
            switch (index)
            {
                case 0: return a;
                case 1: return b;
                case 2: return c;
                case 3: return d;
                case 4: return e;
                case 5: return f;
                default: throw new Exception("Index was out of range!");
            }
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


    }
}