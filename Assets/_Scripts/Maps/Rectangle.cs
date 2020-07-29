using UnityEngine;
using System.Collections;

public class Rectangle 
{

    public static Rectangle Empty = new Rectangle(0,0,0,0);
    Rect rect;

    public Rectangle(float x, float y, int w, int h)
    {
        rect = new Rect(x,y,w,h);
    }

    public int Left { get => Mathf.RoundToInt(rect.xMin); set => rect.xMin = value; }
    public int Right { get => Mathf.RoundToInt(rect.xMax); set => rect.xMax = value; }
    public int Top { get => Mathf.RoundToInt(rect.yMin); set => rect.yMin = value; }
    public int Bottom { get => Mathf.RoundToInt(rect.yMax); set => rect.yMax = value; }

    public int X { get => Mathf.RoundToInt(rect.x); set => rect.x = value; }
    public int Y { get => Mathf.RoundToInt(rect.y); set => rect.y = value; }

    public int Width { get => Mathf.RoundToInt(rect.width); set => rect.width = value; }
    public int Height { get => Mathf.RoundToInt(rect.height); set => rect.height = value; }

    public bool Contains(int x, int y)
    {
        return this.X <= x && x < this.X + this.Width && this.Y <= y && y < this.Y + this.Height;
    }

    public bool Contains(Vector2 value)
    {
        return this.X <= value.x && value.x < this.X + this.Width && this.Y <= value.y && value.y < this.Y + this.Height;
    }

    public bool Contains(Rectangle value)
    {
        return this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height;
    }

    public void Contains(ref Vector2 value, out bool result)
    {
        result = this.X <= value.x && value.x < this.X + this.Width && this.Y <= value.y && value.y < this.Y + this.Height;
    }

    public void Contains(ref Rectangle value, out bool result)
    {
        result = this.X <= value.X && value.X + value.Width <= this.X + this.Width && this.Y <= value.Y && value.Y + value.Height <= this.Y + this.Height;
    }
}
