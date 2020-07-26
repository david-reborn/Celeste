using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.IO;

namespace myd.celeste
{
    public static class BinaryPacker
    {
        private static string[] stringLookup;

        public static BinaryPacker.Element FromBinary(string filename)
        {
            BinaryPacker.Element element;
            using (FileStream fileStream = File.OpenRead(filename))
            {
                BinaryReader reader = new BinaryReader((Stream)fileStream);
                reader.ReadString();
                string str = reader.ReadString();
                short num = reader.ReadInt16();
                BinaryPacker.stringLookup = new string[(int)num];
                for (int index = 0; index < (int)num; ++index)
                    BinaryPacker.stringLookup[index] = reader.ReadString();
                element = BinaryPacker.ReadElement(reader);
                element.Package = str;
            }
            return element;
        }

        private static BinaryPacker.Element ReadElement(BinaryReader reader)
        {
            BinaryPacker.Element element = new BinaryPacker.Element();
            element.Name = BinaryPacker.stringLookup[(int)reader.ReadInt16()];
            byte num1 = reader.ReadByte();
            if (num1 > (byte)0)
                element.Attributes = new Dictionary<string, object>();
            for (int index = 0; index < (int)num1; ++index)
            {
                string key = BinaryPacker.stringLookup[(int)reader.ReadInt16()];
                byte num2 = reader.ReadByte();
                object obj = (object)null;
                switch (num2)
                {
                    case 0:
                        obj = (object)reader.ReadBoolean();
                        break;
                    case 1:
                        obj = (object)Convert.ToInt32(reader.ReadByte());
                        break;
                    case 2:
                        obj = (object)Convert.ToInt32(reader.ReadInt16());
                        break;
                    case 3:
                        obj = (object)reader.ReadInt32();
                        break;
                    case 4:
                        obj = (object)reader.ReadSingle();
                        break;
                    case 5:
                        obj = (object)BinaryPacker.stringLookup[(int)reader.ReadInt16()];
                        break;
                    case 6:
                        obj = (object)reader.ReadString();
                        break;
                    case 7:
                        short num3 = reader.ReadInt16();
                        obj = (object)RunLengthEncoding.Decode(reader.ReadBytes((int)num3));
                        break;
                }
                element.Attributes.Add(key, obj);
            }
            short num4 = reader.ReadInt16();
            if (num4 > (short)0)
                element.Children = new List<BinaryPacker.Element>();
            for (int index = 0; index < (int)num4; ++index)
                element.Children.Add(BinaryPacker.ReadElement(reader));
            return element;
        }

        public class Element
        {
            public string Package;
            public string Name;
            public Dictionary<string, object> Attributes;
            public List<BinaryPacker.Element> Children;

            public bool HasAttr(string name)
            {
                return this.Attributes != null && this.Attributes.ContainsKey(name);
            }

            public string Attr(string name, string defaultValue = "")
            {
                object obj;
                if (this.Attributes == null || !this.Attributes.TryGetValue(name, out obj))
                    obj = (object)defaultValue;
                return obj.ToString();
            }

            public bool AttrBool(string name, bool defaultValue = false)
            {
                object obj;
                if (this.Attributes == null || !this.Attributes.TryGetValue(name, out obj))
                    obj = (object)defaultValue;
                return obj is bool flag ? flag : bool.Parse(obj.ToString());
            }

            public float AttrFloat(string name, float defaultValue = 0.0f)
            {
                object obj;
                if (this.Attributes == null || !this.Attributes.TryGetValue(name, out obj))
                    obj = (object)defaultValue;
                return obj is float num ? num : float.Parse(obj.ToString(), (IFormatProvider)CultureInfo.InvariantCulture);
            }
        }
    }
}
