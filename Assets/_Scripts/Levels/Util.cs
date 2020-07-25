using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;
using System;
using System.Xml;

namespace myd.celeste
{
    public static class Util
    {
        public static string ROOT = "F:/Unity/Celeste/Resources/";
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
            Assembly current = Assembly.GetExecutingAssembly();
            using (Stream file = current.GetManifestResourceStream(current.GetName().Name + "." + path))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static Sprite GetSubImage(Texture2D parent, ExtSprite bounds)
        {
            return Sprite.Create(parent, bounds.Bounds, new Vector2(0.5f, 0.5f));
        }
    }
}