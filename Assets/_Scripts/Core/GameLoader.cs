using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myd.celeste.ext
{
    public class GameLoader : MonoBehaviour
    {
        public string level ;
        public void Start()
        {
            SaveData.Start(new SaveData
            {
                Name = "test001",
                AssistMode = true,
                VariantMode = true
            }, 0);
            //加载区域
            AreaData.Load();

            //创建Session
            Session session = new Session(new AreaKey(0, AreaMode.Normal), null, null);
            bool flag = level != null && session.MapData.Get(level) != null;
            if (flag)
            {
                session.Level = level;
                session.FirstLevel = false;
            }
            LevelLoader loader = new LevelLoader(session, null);
        }

    }
}
