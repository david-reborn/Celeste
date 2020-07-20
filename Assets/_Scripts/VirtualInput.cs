using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace myd.celeste
{
    public abstract class VirtualInput : MonoBehaviour
    {
        // Token: 0x06000867 RID: 2151 RVA: 0x000170FC File Offset: 0x000152FC
        public VirtualInput()
        {
            MInput.VirtualInputs.Add(this);
        }

        // Token: 0x06000868 RID: 2152 RVA: 0x00017112 File Offset: 0x00015312
        public void Deregister()
        {
            MInput.VirtualInputs.Remove(this);
        }

        // Token: 0x06000869 RID: 2153
        public abstract void Update();

        // Token: 0x020003C2 RID: 962
        public enum OverlapBehaviors
        {
            // Token: 0x04001F00 RID: 7936
            CancelOut,
            // Token: 0x04001F01 RID: 7937
            TakeOlder,
            // Token: 0x04001F02 RID: 7938
            TakeNewer
        }

        // Token: 0x020003C3 RID: 963
        public enum ThresholdModes
        {
            // Token: 0x04001F04 RID: 7940
            LargerThan,
            // Token: 0x04001F05 RID: 7941
            LessThan,
            // Token: 0x04001F06 RID: 7942
            EqualTo
        }

    }
}