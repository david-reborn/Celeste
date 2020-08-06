using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace myd.celeste.demo
{
    /// <summary>
    /// 所有Sprite的数据存储集合，即SpriteData的集合，便于搜索获取
    /// </summary>
    public class SpriteBank
    {
        public Atlas Atlas;
        public XmlDocument XML;
        public Dictionary<string, SpriteData> SpriteData;

        public SpriteBank(Atlas atlas, XmlDocument xml)
        {
            this.Atlas = atlas;
            this.XML = xml;
            this.SpriteData = new Dictionary<string, SpriteData>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
            Dictionary<string, XmlElement> dictionary = new Dictionary<string, XmlElement>();
            foreach (object childNode in this.XML["Sprites"].ChildNodes)
            {
                if (childNode is XmlElement)
                {
                    XmlElement xml1 = childNode as XmlElement;
                    dictionary.Add(xml1.Name, xml1);
                    if (this.SpriteData.ContainsKey(xml1.Name))
                        throw new Exception("Duplicate sprite name in SpriteData: '" + xml1.Name + "'!");
                    SpriteData spriteData = this.SpriteData[xml1.Name] = new SpriteData(this.Atlas);
                    //if (xml1.HasAttr("copy"))
                    //    spriteData.Add(dictionary[xml1.Attr("copy")], xml1.Attr("path"));
                    //spriteData.Add(xml1, (string)null);
                }
            }
        }

        public SpriteBank(Atlas atlas, string xmlPath):this(atlas, XmlUtils.LoadContentXML(xmlPath))
        {
        }

        public bool Has(string id)
        {
            return this.SpriteData.ContainsKey(id);
        }

        public UnitSprite Create(string id)
        {
            if (this.SpriteData.ContainsKey(id))
                return this.SpriteData[id].Create();
            throw new Exception("Missing animation name in SpriteData: '" + id + "'!");
        }

        public UnitSprite CreateOn(UnitSprite sprite, string id)
        {
            if (this.SpriteData.ContainsKey(id))
                return this.SpriteData[id].CreateOn(sprite);
            throw new Exception("Missing animation name in SpriteData: '" + id + "'!");
        }
    }
}
