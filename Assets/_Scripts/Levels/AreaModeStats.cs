using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace myd.celeste
{
    [Serializable]
    public class AreaModeStats
    {
        public HashSet<EntityID> Strawberries = new HashSet<EntityID>();
        public HashSet<string> Checkpoints = new HashSet<string>();
        [XmlAttribute]
        public int TotalStrawberries;
        [XmlAttribute]
        public bool Completed;
        [XmlAttribute]
        public bool SingleRunCompleted;
        [XmlAttribute]
        public bool FullClear;
        [XmlAttribute]
        public int Deaths;
        [XmlAttribute]
        public long TimePlayed;
        [XmlAttribute]
        public long BestTime;
        [XmlAttribute]
        public long BestFullClearTime;
        [XmlAttribute]
        public int BestDashes;
        [XmlAttribute]
        public int BestDeaths;
        [XmlAttribute]
        public bool HeartGem;

        public AreaModeStats Clone()
        {
            return new AreaModeStats()
            {
                TotalStrawberries = this.TotalStrawberries,
                Strawberries = new HashSet<EntityID>((IEnumerable<EntityID>)this.Strawberries),
                Completed = this.Completed,
                SingleRunCompleted = this.SingleRunCompleted,
                FullClear = this.FullClear,
                Deaths = this.Deaths,
                TimePlayed = this.TimePlayed,
                BestTime = this.BestTime,
                BestFullClearTime = this.BestFullClearTime,
                BestDashes = this.BestDashes,
                BestDeaths = this.BestDeaths,
                HeartGem = this.HeartGem,
                Checkpoints = new HashSet<string>((IEnumerable<string>)this.Checkpoints)
            };
        }
    }
}
