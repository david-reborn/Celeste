using UnityEngine;
using System.Collections;

public class Entity 
{
    public bool Active = true;
    public bool Visible = true;
    public bool Collidable = true;
    internal int depth = 0;
    internal double actualDepth = 0.0;
    public Vector2 Position;
    private int tag;
    private Collider2D collider;

    public int Tag
    {
        get
        {
            return this.tag;
        }
        set
        {
            if (this.tag == value)
                return;
            //if (this.Scene != null)
            //{
            //    for (int index = 0; index < BitTag.TotalTags; ++index)
            //    {
            //        int num = 1 << index;
            //        bool flag = (uint)(value & num) > 0U;
            //        if ((uint)(this.Tag & num) > 0U != flag)
            //        {
            //            if (flag)
            //                this.Scene.TagLists[index].Add(this);
            //            else
            //                this.Scene.TagLists[index].Remove(this);
            //        }
            //    }
            //}
            this.tag = value;
        }
    }

}
