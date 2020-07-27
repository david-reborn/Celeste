using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace myd.celeste
{
    [Serializable]
    public class SaveData
    {
        public string Name = "Madeline";
        //public Assists Assists = Assists.Default;
        public HashSet<string> Flags = new HashSet<string>();
        public List<string> Poem = new List<string>();
        public List<AreaStats> Areas = new List<AreaStats>();
        public const int MaxStrawberries = 175;
        public const int MaxGoldenStrawberries = 25;
        public const int MaxStrawberriesDLC = 202;
        public const int MaxHeartGems = 24;
        public const int MaxCassettes = 8;
        public const int MaxCompletions = 8;
        public static SaveData Instance;
        public string Version;
        public long Time;
        public DateTime LastSave;
        public bool CheatMode;
        public bool AssistMode;
        public bool VariantMode;
        public string TheoSisterName;
        public int UnlockedAreas;
        public int TotalDeaths;
        public int TotalStrawberries;
        public int TotalGoldenStrawberries;
        public int TotalJumps;
        public int TotalWallJumps;
        public int TotalDashes;
        public bool[] SummitGems;
        public bool RevealedChapter9;
        public AreaKey LastArea;
        public Session CurrentSession;
        [XmlIgnore]
        [NonSerialized]
        public int FileSlot;
        [XmlIgnore]
        [NonSerialized]
        public bool DoNotSave;
        [XmlIgnore]
        [NonSerialized]
        public bool DebugMode;

        public int MaxArea
        {
            get
            {
                return AreaData.Areas.Count - 1;
            }
        }

        public static void Start(SaveData data, int slot)
        {
            SaveData.Instance = data;
            SaveData.Instance.FileSlot = slot;
            SaveData.Instance.AfterInitialize();
        }
        public void AfterInitialize()
        {
            //while (this.Areas.Count < AreaData.Areas.Count)
            //    this.Areas.Add(new AreaStats(this.Areas.Count));
            //while (this.Areas.Count > AreaData.Areas.Count)
            //    this.Areas.RemoveAt(this.Areas.Count - 1);
            //int num = -1;
            //for (int index = 0; index < this.Areas.Count; ++index)
            //{
            //    if (this.Areas[index].Modes[0].Completed || this.Areas[index].Modes.Length > 1 && this.Areas[index].Modes[1].Completed)
            //        num = index;
            //}
            //if (this.UnlockedAreas < num + 1 && this.MaxArea >= num + 1)
            //    this.UnlockedAreas = num + 1;
            //if (this.DebugMode)
            //{
            //    this.CurrentSession = (Session)null;
            //    this.RevealedChapter9 = true;
            //    this.UnlockedAreas = this.MaxArea;
            //}
            //if (this.CheatMode)
            //    this.UnlockedAreas = this.MaxArea;
            //if (string.IsNullOrEmpty(this.TheoSisterName))
            //{
            //    this.TheoSisterName = Dialog.Clean("THEO_SISTER_NAME", (Language)null);
            //    if (this.Name.IndexOf(this.TheoSisterName, StringComparison.InvariantCultureIgnoreCase) >= 0)
            //        this.TheoSisterName = Dialog.Clean("THEO_SISTER_ALT_NAME", (Language)null);
            //}
            //this.AssistModeChecks();
            //foreach (AreaStats area in this.Areas)
            //    area.CleanCheckpoints();
            //if (this.Version == null || !(new System.Version(this.Version) < new System.Version(1, 2, 1, 1)))
            //    return;
            //for (int index1 = 0; index1 < this.Areas.Count; ++index1)
            //{
            //    if (this.Areas[index1] != null)
            //    {
            //        for (int index2 = 0; index2 < this.Areas[index1].Modes.Length; ++index2)
            //        {
            //            if (this.Areas[index1].Modes[index2] != null)
            //            {
            //                if (this.Areas[index1].Modes[index2].BestTime > 0L)
            //                    this.Areas[index1].Modes[index2].SingleRunCompleted = true;
            //                this.Areas[index1].Modes[index2].BestTime = 0L;
            //                this.Areas[index1].Modes[index2].BestFullClearTime = 0L;
            //            }
            //        }
            //    }
            //}
        }

    }
}
