﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace myd.celeste.demo
{
    /// <summary>
    /// 单个UnitSprite的数据来源，类似工厂，当需要UnitSprite时，则通过Clone创建一个备份
    /// </summary>
    public class SpriteData
    {
        public List<SpriteDataSource> Sources = new List<SpriteDataSource>();
        //public UnitSprite Sprite;
        public Atlas Atlas;

        public SpriteData(Atlas atlas)
        {
            UnitSprite spritePrefab = Resources.Load<UnitSprite>("UnitSprite");
            //Sprite = GameObject.Instantiate<UnitSprite>(spritePrefab);
            //this.Sprite.Init(atlas, "");
            this.Atlas = atlas;
        }

        public void Add(XmlElement xml, string overridePath = null)
        {
            SpriteDataSource spriteDataSource = new SpriteDataSource()
            {
                XML = xml
            };
            spriteDataSource.Path = spriteDataSource.XML.Attr("path");
            spriteDataSource.OverridePath = overridePath;
            string prefix = "Sprite '" + spriteDataSource.XML.Name + "': ";
            if (!spriteDataSource.XML.HasAttr("path") && string.IsNullOrEmpty(overridePath))
                throw new Exception(prefix + "'path' is missing!");
            spriteDataSource.prefix = prefix;
            this.Sources.Add(spriteDataSource);
        }

        private bool HasFrames(Atlas atlas, string path, int[] frames = null)
        {
            if (frames == null || frames.Length == 0)
                return atlas.GetAtlasSubtexturesAt(path, 0) != null;
            for (int index = 0; index < frames.Length; ++index)
            {
                if (atlas.GetAtlasSubtexturesAt(path, frames[index]) == null)
                    return false;
            }
            return true;
        }

        private void CheckAnimXML(XmlElement xml, string prefix, HashSet<string> ids)
        {
            if (!xml.HasAttr("id"))
                throw new Exception(prefix + "'id' is missing on " + xml.Name + "!");
            if (ids.Contains(xml.Attr("id")))
                throw new Exception(prefix + "multiple animations with id '" + xml.Attr("id") + "'!");
            ids.Add(xml.Attr("id"));
        }

        //public UnitSprite Create()
        //{
        //    return this.Sprite.CreateClone();
        //}

        //public UnitSprite CreateOn(UnitSprite sprite)
        //{
        //    return this.Sprite.CloneInto(sprite);
        //}

        //包装sprite数据
        public void WrapUnitSprite(UnitSprite sprite)
        {
            foreach (var spriteDataSource in Sources)
            {
                HashSet<string> ids = new HashSet<string>();
                foreach (XmlElement xml1 in spriteDataSource.XML.GetElementsByTagName("Anim"))
                    this.CheckAnimXML(xml1, spriteDataSource.prefix, ids);
                foreach (XmlElement xml1 in spriteDataSource.XML.GetElementsByTagName("Loop"))
                    this.CheckAnimXML(xml1, spriteDataSource.prefix, ids);
                if (spriteDataSource.XML.HasAttr("start") && !ids.Contains(spriteDataSource.XML.Attr("start")))
                    throw new Exception(spriteDataSource.prefix + "starting animation '" + spriteDataSource.XML.Attr("start") + "' is missing!");
                if (spriteDataSource.XML.HasChild("Justify") && spriteDataSource.XML.HasChild("Origin"))
                    throw new Exception(spriteDataSource.prefix + "has both Origin and Justify tags!");
                string str1 = spriteDataSource.XML.Attr("path", "");
                float defaultValue = spriteDataSource.XML.AttrFloat("delay", 0.0f);
                foreach (XmlElement xml1 in spriteDataSource.XML.GetElementsByTagName("Anim"))
                {
                    Chooser<string> into = !xml1.HasAttr("goto") ? (Chooser<string>)null : Chooser<string>.FromString<string>(xml1.Attr("goto"));
                    string id = xml1.Attr("id");
                    string str2 = xml1.Attr("path", "");
                    int[] frames = Util.ReadCSVIntWithTricks(xml1.Attr("frames", ""));
                    string path = string.IsNullOrEmpty(spriteDataSource.OverridePath) || !this.HasFrames(this.Atlas, spriteDataSource.OverridePath + str2, frames) ? str1 + str2 : spriteDataSource.OverridePath + str2;
                    sprite.Add(id, path, xml1.AttrFloat("delay", defaultValue), into, frames);
                }
                foreach (XmlElement xml1 in spriteDataSource.XML.GetElementsByTagName("Loop"))
                {
                    string id = xml1.Attr("id");
                    string str2 = xml1.Attr("path", "");
                    int[] frames = Util.ReadCSVIntWithTricks(xml1.Attr("frames", ""));
                    string path = string.IsNullOrEmpty(spriteDataSource.OverridePath) || !this.HasFrames(this.Atlas, spriteDataSource.OverridePath + str2, frames) ? str1 + str2 : spriteDataSource.OverridePath + str2;
                    sprite.AddLoop(id, path, xml1.AttrFloat("delay", defaultValue), frames);
                }
                if (spriteDataSource.XML.HasChild("Center"))
                {
                    sprite.CenterOrigin();
                    sprite.Justify = new Vector2?(new Vector2(0.5f, 0.5f));
                }
                else if (spriteDataSource.XML.HasChild("Justify"))
                {
                    sprite.JustifyOrigin(spriteDataSource.XML.ChildPosition("Justify"));
                    sprite.Justify = new Vector2?(spriteDataSource.XML.ChildPosition("Justify"));
                }
                else if (spriteDataSource.XML.HasChild("Origin"))
                    sprite.Origin = spriteDataSource.XML.ChildPosition("Origin");
                if (spriteDataSource.XML.HasChild("Position"))
                    sprite.Position = spriteDataSource.XML.ChildPosition("Position");
                if (spriteDataSource.XML.HasAttr("start"))
                    sprite.Play(spriteDataSource.XML.Attr("start"), false, false);
            }
        }
    }
}
