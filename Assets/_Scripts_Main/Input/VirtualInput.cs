using UnityEngine;
using System.Collections;


namespace myd.celeste.demo
{
    public abstract class VirtualInput 
    {
        public abstract void Update();

        public enum OverlapBehaviors
        {
            CancelOut,
            TakeOlder,
            TakeNewer,
        }

        public enum ThresholdModes
        {
            LargerThan,
            LessThan,
            EqualTo,
        }
    }
}
