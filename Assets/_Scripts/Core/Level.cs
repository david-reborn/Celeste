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
        public BackdropRenderer Foreground;

        public BackgroundTiles BgTiles;
        public Vector2 StartPosition;

        public SolidTiles tilles;

        public TileGrid FgTilesLightMask;

        private Session.CoreModes coreMode;

        public Session.CoreModes CoreMode
        {
            get
            {
                return this.coreMode;
            }
            set
            {
                //if (this.coreMode == value)
                //    return;
                //this.coreMode = value;
                //this.Session.SetFlag("cold", this.coreMode == Session.CoreModes.Cold);
                //Audio.SetParameter(Audio.CurrentAmbienceEventInstance, "room_state", this.coreMode == Session.CoreModes.Hot ? 0.0f : 1f);
                //if (Audio.CurrentMusic == "event:/music/lvl9/main")
                //{
                //    this.Session.Audio.Music.Layer(1, this.coreMode == Session.CoreModes.Hot);
                //    this.Session.Audio.Music.Layer(2, this.coreMode == Session.CoreModes.Cold);
                //    this.Session.Audio.Apply(false);
                //}
                //foreach (CoreModeListener component in this.Tracker.GetComponents<CoreModeListener>())
                //{
                //    if (component.OnChange != null)
                //        component.OnChange(value);
                //}
            }
        }
    }
}