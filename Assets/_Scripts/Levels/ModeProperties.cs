using UnityEngine;
using System.Collections;

namespace myd.celeste
{
    public class ModeProperties
    {
        public string PoemID;
        public string Path;
        public int TotalStrawberries;
        public int StartStrawberries;
        public EntityData[,] StrawberriesByCheckpoint;
        public CheckpointData[] Checkpoints;
        public MapData MapData;
        public PlayerInventory Inventory;
        public AudioState AudioState;
        public bool IgnoreLevelAudioLayerData;
    }
}