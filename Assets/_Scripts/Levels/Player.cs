using UnityEngine;
using System.Collections;

namespace myd.celeste
{
    public class Player
    {
        public Player.IntroTypes IntroType;

        public enum IntroTypes
        {
            Transition,
            Respawn,
            WalkInRight,
            WalkInLeft,
            Jump,
            WakeUp,
            Fall,
            TempleMirrorVoid,
            None,
            ThinkForABit,
        }
    }

    

}