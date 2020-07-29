using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using myd.celeste;
using System.IO;
using System.Globalization;

public static class XmlUtils 
{
    public static XmlDocument LoadContentXML(string filename)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(Path.Combine(Util.GAME_PATH_CONTENT, filename));
        //using (FileStream inStream = TitleContainer.OpenStream(Path.Combine(Engine.Instance.Content.RootDirectory, filename)))
        //    xmlDocument.Load(inStream);
        return xmlDocument;
    }

    public static float AttrFloat(this XmlElement xml, string attributeName)
    {
        return Convert.ToSingle(xml.Attributes[attributeName].InnerText, (IFormatProvider)CultureInfo.InvariantCulture);
    }

    public static float AttrFloat(this XmlElement xml, string attributeName, float defaultValue)
    {
        return !xml.HasAttr(attributeName) ? defaultValue : Convert.ToSingle(xml.Attributes[attributeName].InnerText, (IFormatProvider)CultureInfo.InvariantCulture);
    }

    public static Vector3 AttrVector3(this XmlElement xml, string attributeName)
    {
        string[] strArray = xml.Attr(attributeName).Split(',');
        return new Vector3(float.Parse(strArray[0].Trim(), (IFormatProvider)CultureInfo.InvariantCulture), float.Parse(strArray[1].Trim(), (IFormatProvider)CultureInfo.InvariantCulture), float.Parse(strArray[2].Trim(), (IFormatProvider)CultureInfo.InvariantCulture));
    }

    public static Vector2 AttrVector2(
      this XmlElement xml,
      string xAttributeName,
      string yAttributeName)
    {
        return new Vector2(xml.AttrFloat(xAttributeName), xml.AttrFloat(yAttributeName));
    }

    public static Vector2 AttrVector2(
      this XmlElement xml,
      string xAttributeName,
      string yAttributeName,
      Vector2 defaultValue)
    {
        return new Vector2(xml.AttrFloat(xAttributeName, defaultValue.x), xml.AttrFloat(yAttributeName, defaultValue.y));
    }

    public static bool AttrBool(this XmlElement xml, string attributeName)
    {
        return Convert.ToBoolean(xml.Attributes[attributeName].InnerText);
    }

    public static bool AttrBool(this XmlElement xml, string attributeName, bool defaultValue)
    {
        return !xml.HasAttr(attributeName) ? defaultValue : xml.AttrBool(attributeName);
    }

    public static char AttrChar(this XmlElement xml, string attributeName)
    {
        return Convert.ToChar(xml.Attributes[attributeName].InnerText);
    }

    public static char AttrChar(this XmlElement xml, string attributeName, char defaultValue)
    {
        return !xml.HasAttr(attributeName) ? defaultValue : xml.AttrChar(attributeName);
    }

    public static T AttrEnum<T>(this XmlElement xml, string attributeName) where T : struct
    {
        if (Enum.IsDefined(typeof(T), (object)xml.Attributes[attributeName].InnerText))
            return (T)Enum.Parse(typeof(T), xml.Attributes[attributeName].InnerText);
        throw new Exception("The attribute value cannot be converted to the enum type.");
    }

    public static T AttrEnum<T>(this XmlElement xml, string attributeName, T defaultValue) where T : struct
    {
        return !xml.HasAttr(attributeName) ? defaultValue : xml.AttrEnum<T>(attributeName);
    }

    public static Color AttrHexColor(this XmlElement xml, string attributeName)
    {
        return Util.HexToColor(xml.Attr(attributeName));
    }

    public static Color AttrHexColor(
      this XmlElement xml,
      string attributeName,
      Color defaultValue)
    {
        return !xml.HasAttr(attributeName) ? defaultValue : xml.AttrHexColor(attributeName);
    }

    public static Color AttrHexColor(
      this XmlElement xml,
      string attributeName,
      string defaultValue)
    {
        return !xml.HasAttr(attributeName) ? Util.HexToColor(defaultValue) : xml.AttrHexColor(attributeName);
    }
}
