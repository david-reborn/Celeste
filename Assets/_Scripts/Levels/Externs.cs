using UnityEngine;
using System.Collections;


namespace myd.celeste
{

    public static class Externs
    {

        public static bool IntersectsWith(this Rect owner, Rect other)
        {
            if (owner.x <= other.x && owner.x + owner.width >= other.x && owner.y <= other.y && owner.y + other.height >= other.y)
                return true;
            return false;
        }
    }
}