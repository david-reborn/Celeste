using UnityEngine;
using System.Collections;

namespace myd.celeste.demo
{
    public struct CollisionData
    {
        // Token: 0x040018F4 RID: 6388
        public Vector2 Direction;

        // Token: 0x040018F5 RID: 6389
        public Vector2 Moved;

        // Token: 0x040018F6 RID: 6390
        public Vector2 TargetPosition;

        // Token: 0x040018F7 RID: 6391
        //public Platform Hit;

        // Token: 0x040018F8 RID: 6392
        //public Solid Pusher;

        // Token: 0x040018F9 RID: 6393
        public static readonly CollisionData Empty = default(CollisionData);
    }
}
