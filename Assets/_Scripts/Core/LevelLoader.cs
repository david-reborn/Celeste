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
            LoadingThread();

            Debug.Log("LoadingThread Finished");
        }

        //加载关卡数据
        private void LoadingThread()
        {
            MapData mapData = this.session.MapData;
            
            AreaData areaData = AreaData.Get(this.session.Area.ID);
            //if (this.session.Area.ID == 0)
            //    SaveData.Instance.Assists.DashMode = Assists.DashModes.Normal;
            //this.Level.Add((Monocle.Renderer)(this.Level.Background = new BackdropRenderer()));
            //this.Level.Add((Entity)new DustEdges());
            //this.Level.Add((Entity)new WaterSurface());
            //this.Level.Add((Entity)new MirrorSurfaces());
            //this.Level.Add((Entity)new GlassBlockBg());
            //this.Level.Add((Entity)new LightningRenderer());
            //this.Level.Add((Entity)new SeekerBarrierRenderer());
            this.Level.Background = new BackdropRenderer();
            this.Level.BackgroundColor = mapData.BackgroundColor;
            this.Level.Background.Backdrops = mapData.CreateBackdrops(mapData.Background);
            foreach (Backdrop backdrop in this.Level.Background.Backdrops)
                backdrop.Renderer = this.Level.Background;
        }

    }
}