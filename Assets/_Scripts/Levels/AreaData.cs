using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml;
using System.IO;
using myd.celeste.demo;

namespace myd.celeste
{
    public class AreaData
    {
        public int CassetteCheckpointIndex = -1;
        public Color TitleBaseColor = Color.white;
        public Color TitleAccentColor = Color.gray;
        public Color TitleTextColor = Color.white;
        public float DarknessAlpha = 0.05f;
        public float BloomBase = 0.0f;
        public float BloomStrength = 1f;
        public string Jumpthru = "wood";
        public string Spike = "default";
        public string CrumbleBlock = "default";
        public string WoodPlatform = "default";
        public Color CassseteNoteColor = Color.white;
        public Color[] CobwebColor = new Color[1]
        {
      Util.HexToColor("696a6a")
        };
        public string CassetteSong = "event:/music/cassette/01_forsaken_city";
        public Session.CoreModes CoreMode = Session.CoreModes.None;
        public int MountainState = 0;
        public static List<AreaData> Areas;
        public string Name;
        public string Icon;
        public int ID;
        public bool Interlude;
        public bool CanFullClear;
        public bool IsFinal;
        public string CompleteScreenName;
        public ModeProperties[] Mode;
        public IntroTypes IntroType;
        public bool Dreaming;
        public string ColorGrade;
        public Action<Scene, bool, Action> Wipe;
        public Action<Level> OnLevelBegin;
        //public MountainCamera MountainIdle;
        //public MountainCamera MountainSelect;
        //public MountainCamera MountainZoom;
        public Vector3 MountainCursor;
        public float MountainCursorScale;

        public static void Load()
        {
            AreaData.Areas = new List<AreaData>();
            List<AreaData> areas1 = AreaData.Areas;
            AreaData areaData1 = new AreaData();
            areaData1.Name = "area_0";
            areaData1.Icon = "areas/intro";
            areaData1.Interlude = true;
            areaData1.CompleteScreenName = (string)null;
            areaData1.Mode = new ModeProperties[3]
            {
        new ModeProperties()
        {
          PoemID = (string) null,
          Path = "0-Intro",
          Checkpoints = (CheckpointData[]) null,
          Inventory = PlayerInventory.Prologue,
          AudioState = new AudioState("event:/music/lvl0/intro", "event:/env/amb/00_prologue")
        },
        null,
        null
            };
            areaData1.TitleBaseColor = Util.HexToColor("383838");
            areaData1.TitleAccentColor = Util.HexToColor("50AFAE");
            areaData1.TitleTextColor = Color.white;
            areaData1.IntroType = IntroTypes.WalkInRight;
            areaData1.Dreaming = false;
            areaData1.ColorGrade = (string)null;
            //CurtainWipe curtainWipe1;
            //areaData1.Wipe = (Action<Scene, bool, Action>)((scene, wipeIn, onComplete) => curtainWipe1 = new CurtainWipe(scene, wipeIn, onComplete));
            areaData1.DarknessAlpha = 0.05f;
            areaData1.BloomBase = 0.0f;
            areaData1.BloomStrength = 1f;
            areaData1.OnLevelBegin = (Action<Level>)null;
            areaData1.Jumpthru = "wood";
            AreaData areaData2 = areaData1;
            areas1.Add(areaData2);

            int length = Enum.GetNames(typeof(AreaMode)).Length;
            for (int id = 0; id < AreaData.Areas.Count; ++id)
            {
                AreaData.Areas[id].ID = id;
                AreaData.Areas[id].Mode[0].MapData = new MapData(new AreaKey(id, AreaMode.Normal));
                if (!AreaData.Areas[id].Interlude)
                {
                    for (int index = 1; index < length; ++index)
                    {
                        if (AreaData.Areas[id].HasMode((AreaMode)index))
                            AreaData.Areas[id].Mode[index].MapData = new MapData(new AreaKey(id, (AreaMode)index));
                    }
                }
            }
            AreaData.ReloadMountainViews();
        }

        //加载山的全景视图
        public static void ReloadMountainViews()
        {
            //foreach (XmlElement xml in (XmlNode)Util.LoadXML(Path.Combine(Engine.ContentDirectory, "Overworld", "AreaViews.xml"))["Views"])
            //{
            //    int index = xml.AttrInt("id");
            //    if (index >= 0 && index < AreaData.Areas.Count)
            //    {
            //        Vector3 pos1 = xml["Idle"].AttrVector3("position");
            //        Vector3 target1 = xml["Idle"].AttrVector3("target");
            //        AreaData.Areas[index].MountainIdle = new MountainCamera(pos1, target1);
            //        Vector3 pos2 = xml["Select"].AttrVector3("position");
            //        Vector3 target2 = xml["Select"].AttrVector3("target");
            //        AreaData.Areas[index].MountainSelect = new MountainCamera(pos2, target2);
            //        Vector3 pos3 = xml["Zoom"].AttrVector3("position");
            //        Vector3 target3 = xml["Zoom"].AttrVector3("target");
            //        AreaData.Areas[index].MountainZoom = new MountainCamera(pos3, target3);
            //        if (xml["Cursor"] != null)
            //            AreaData.Areas[index].MountainCursor = xml["Cursor"].AttrVector3("position");
            //        AreaData.Areas[index].MountainState = xml.AttrInt("state", 0);
            //    }
            //}
        }

        public static AreaData Get(int id)
        {
            return AreaData.Areas[id];
        }

        public static CheckpointData GetCheckpoint(AreaKey area, string level)
        {
            CheckpointData[] checkpoints = AreaData.Areas[area.ID].Mode[(int)area.Mode].Checkpoints;
            if (level != null && checkpoints != null)
            {
                foreach (CheckpointData checkpointData in checkpoints)
                {
                    if (checkpointData.Level.Equals(level))
                        return checkpointData;
                }
            }
            return (CheckpointData)null;
        }

        public static bool GetCheckpointDreaming(AreaKey area, string level)
        {
            CheckpointData checkpoint = AreaData.GetCheckpoint(area, level);
            return checkpoint != null ? checkpoint.Dreaming : AreaData.Areas[area.ID].Dreaming;
        }

        public static Session.CoreModes GetCheckpointCoreMode(AreaKey area, string level)
        {
            CheckpointData checkpoint = AreaData.GetCheckpoint(area, level);
            return checkpoint != null && checkpoint.CoreMode.HasValue ? checkpoint.CoreMode.Value : AreaData.Areas[area.ID].CoreMode;
        }

        public static string GetCheckpointColorGrading(AreaKey area, string level)
        {
            return AreaData.GetCheckpoint(area, level)?.ColorGrade;
        }

        public bool HasMode(AreaMode mode)
        {
            return (AreaMode)this.Mode.Length > mode && this.Mode[(int)mode] != null && this.Mode[(int)mode].Path != null;
        }
    }
}
