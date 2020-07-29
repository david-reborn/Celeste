using UnityEngine;
using System.Collections;

namespace myd.celeste
{
    /// <summary>
    /// 单个关卡场景
    /// </summary>
    public class Level
    {
        public Color BackgroundColor = Color.black;
        public BackdropRenderer Background;

        public BackgroundTiles BgTiles;
    }
}