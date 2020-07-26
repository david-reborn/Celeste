using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myd.celeste.ext
{
    public class GameLoader : MonoBehaviour
    {
        public string level = null;
        public void Start()
        {
            Session session = new Session(new AreaKey(0, AreaMode.Normal), null, null);
            bool flag = level != null && session.MapData.Get(level) != null;
            if (flag)
            {
                session.Level = level;
                session.FirstLevel = false;
            }
            new LevelLoader(session, null);
        }

        public void LoadLevel()
        {

        }
    }
}
