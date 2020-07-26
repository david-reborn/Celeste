using UnityEngine;
using System.Collections;

namespace myd.celeste
{
    public class LevelLoader 
    {
        private bool started = false;
        private Session session;
        private Vector2? startPosition;
        public Level Level { get; private set; }


        public LevelLoader(Session session, Vector2? startPosition = null)
        {
            this.session = session;
            bool flag = startPosition == null;
            if (flag)
            {
                this.startPosition = session.RespawnPoint;
            }
            else
            {
                this.startPosition = startPosition;
            }
            this.Level = new Level();
            //RunThread.Start(new Action(this.LoadingThread), "LEVEL_LOADER", false);
            LoadingThread();
        }

        //加载关卡数据
        private void LoadingThread()
        {
            //MapData mapData = this.session.MapData;
            //AreaData areaData = AreaData.Get(this.session);
            //if (this.session.Area.ID == 0)
            //    SaveData.Instance.Assists.DashMode = Assists.DashModes.Normal;
        }

    }
}