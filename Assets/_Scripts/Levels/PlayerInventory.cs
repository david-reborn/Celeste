﻿using UnityEngine;
using System.Collections;
using System;

namespace myd.celeste {
    [Serializable]
    public struct PlayerInventory
    {
        public static readonly PlayerInventory Prologue = new PlayerInventory(0, false, true, false);
        public static readonly PlayerInventory Default = new PlayerInventory(1, true, true, false);
        public static readonly PlayerInventory OldSite = new PlayerInventory(1, false, true, false);
        public static readonly PlayerInventory CH6End = new PlayerInventory(2, true, true, false);
        public static readonly PlayerInventory TheSummit = new PlayerInventory(2, true, false, false);
        public static readonly PlayerInventory Core = new PlayerInventory(2, true, true, true);
        public static readonly PlayerInventory Farewell = new PlayerInventory(1, true, false, false);
        public int Dashes;
        public bool DreamDash;
        public bool Backpack;
        public bool NoRefills;

        public PlayerInventory(int dashes = 1, bool dreamDash = true, bool backpack = true, bool noRefills = false)
        {
            this.Dashes = dashes;
            this.DreamDash = dreamDash;
            this.Backpack = backpack;
            this.NoRefills = noRefills;
        }
    }
}
