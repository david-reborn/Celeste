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
        public static SaveData Instance;
        public List<AreaStats> Areas = new List<AreaStats>();

        public int MaxArea
        {
            get
            {
                return AreaData.Areas.Count - 1;
            }
        }
    }
}
