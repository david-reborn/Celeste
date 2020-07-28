using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            GFX.FGAutotiler = new Autotiler(Path.Combine("Graphics", "ForegroundTiles.xml"));
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
