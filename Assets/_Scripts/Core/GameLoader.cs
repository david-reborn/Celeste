using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace myd.celeste.ext
{
    public class GameLoader : MonoBehaviour
    {
        public string level ;
        public void Start()
        {
            //读取GamePlay的MTexture文件
            Debug.Log("==读取GamePlay的Texture");
            Gfx.Game = Atlas.FromAtlas(Path.Combine("Graphics", "Atlases", "Gameplay"), Atlas.AtlasDataFormat.Packer);
            Gfx.BGAutotiler = new Autotiler(Path.Combine("Graphics", "BackgroundTiles.xml"));
            Gfx.FGAutotiler = new Autotiler(Path.Combine("Graphics", "ForegroundTiles.xml"));
            Gfx.AnimatedTilesBank = new AnimatedTilesBank();
            foreach (XmlElement xml in (XmlNode)XmlUtils.LoadContentXML(Path.Combine("Graphics", "AnimatedTiles.xml"))["Data"])
            {
                if (xml != null)
                    Gfx.AnimatedTilesBank.Add(xml.Attr("name"), xml.AttrFloat("delay", 0.0f), xml.AttrVector2("posX", "posY", Vector2.zero), xml.AttrVector2("origX", "origY", Vector2.zero), Gfx.Game.GetAtlasSubtextures(xml.Attr("path")));
            }
            Debug.Log("==加载AreaData文件");
            SaveData.Start(new SaveData
            {
                Name = "test001",
                AssistMode = true,
                VariantMode = true
            }, 0);
            //加载区域
            AreaData.Load();

            Debug.Log("==创建Session");
            Session session = new Session(new AreaKey(0, AreaMode.Normal), null, null);
            bool flag = level != null && session.MapData.Get(level) != null;
            if (flag)
            {
                session.Level = level;
                session.FirstLevel = false;
            }

            Debug.Log("==加载关卡LevelLoad");
            LevelLoader loader = new LevelLoader(session, null);
        }

    }
}
