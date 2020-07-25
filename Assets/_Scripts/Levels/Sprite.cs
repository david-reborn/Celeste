using UnityEngine;
using System.Collections;


namespace myd.celeste
{
    public class ExtSprite
    {
        public Rect Bounds, Offset;
        public ExtSprite(int x, int y, int width, int height, int offsetX, int offsetY, int widthOffset, int heightOffset)
        {
            Bounds = new Rect(x, y, width, height);
            Offset = new Rect(-offsetX, -offsetY, widthOffset, heightOffset);
        }
        public override string ToString()
        {
            return Bounds.ToString() + " " + Offset.ToString();
        }
    }
}