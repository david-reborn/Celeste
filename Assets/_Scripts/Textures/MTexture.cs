using UnityEngine;
using System.Collections;
using System;

public class MTexture
{
    public string AtlasPath;

    public Texture2D Texture { get; private set; }

    public Rect ClipRect { get; private set; }

    public Vector2 DrawOffset { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public Vector2 Center { get; private set; }

    public float LeftUV { get; private set; }

    public float RightUV { get; private set; }

    public float TopUV { get; private set; }

    public float BottomUV { get; private set; }

    public UnityEngine.Sprite USprite { get; private set; }

    public Vector2Int Offset { get; private set; }              //实际像素偏移量

    public MTexture(Texture2D texture)
    {
        this.Texture = texture;
        this.ClipRect = new Rect(0, 0, this.Texture.width, this.Texture.height);
        this.DrawOffset = Vector2.zero;
        this.Width = Mathf.RoundToInt(this.ClipRect.width);
        this.Height = Mathf.RoundToInt(this.ClipRect.height);
        this.SetUtil();
        //this.USprite = UnityEngine.Sprite.Create(Texture, ClipRect, Vector3.zero);
    }

    public void LoadSprite(Vector2 origin)
    {
        if (this.USprite == null)
        {
            Rect rect = new Rect(this.ClipRect.x, this.Texture.height - this.ClipRect.y - this.ClipRect.height, this.ClipRect.width, this.ClipRect.height);
            Offset = new Vector2Int(Mathf.RoundToInt(DrawOffset.x - origin.x), Mathf.RoundToInt(origin.y - DrawOffset.y - this.ClipRect.height));
            //Vector2 pivot = new Vector2((origin.x - DrawOffset.x + this.ClipRect.width / 2f) / (Width * 1.0f), 1-(origin.y - DrawOffset.y + this.ClipRect.height / 2f) / (Height * 1.0f));
            this.USprite = UnityEngine.Sprite.Create(Texture, rect, Vector2.zero, 1f);
        }
    }

    public Sprite GetSprite()
    {
        if (this.USprite == null)
        {
            Rect rect = new Rect(this.ClipRect.x, this.Texture.height - this.ClipRect.y - this.ClipRect.height, this.ClipRect.width, this.ClipRect.height);
            this.USprite = UnityEngine.Sprite.Create(Texture, rect, new Vector2(0.5f, 0.5f), 1f);
        }
        return this.USprite;
    }

    public MTexture(MTexture parent, int x, int y, int width, int height)
    {
        this.Texture = parent.Texture;
        this.ClipRect = parent.GetRelativeRect(x, y, width, height);
        this.DrawOffset = new Vector2(-Math.Min((float)x - parent.DrawOffset.x, 0f), -Math.Min((float)y - parent.DrawOffset.y, 0f));
        this.Width = width;
        this.Height = height;
        this.SetUtil();
        //this.USprite = UnityEngine.Sprite.Create(Texture, ClipRect, Vector3.zero);
    }

    public MTexture(Texture2D texture, Rect clipRect, Vector2 drawOffset, int width, int height)
    {
        this.Texture = texture;
        this.ClipRect = clipRect;
        this.DrawOffset = drawOffset;
        this.Width = width;
        this.Height = height;

        this.SetUtil();
        //this.USprite = UnityEngine.Sprite.Create(Texture, clipRect, Vector3.zero);
    }

    public MTexture(MTexture parent, string atlasPath, Rect clipRect, Vector2 drawOffset, int width, int height)
    {
        this.Texture = parent.Texture;
        this.AtlasPath = atlasPath;
        this.ClipRect = parent.GetRelativeRect(clipRect);
        this.DrawOffset = drawOffset;
        this.Width = width;
        this.Height = height;
        this.SetUtil();

        //this.USprite = UnityEngine.Sprite.Create(Texture, clipRect, Vector3.zero);
    }

    //public MTexture(MTexture parent, string atlasPath, Rect clipRect, Vector2 drawOffset, int width, int height)
    //{
    //    this.Texture = parent.Texture;
    //    this.AtlasPath = atlasPath;
    //    this.ClipRect = parent.GetRelativeRect(clipRect);
    //    this.DrawOffset = drawOffset;
    //    this.Width = width;
    //    this.Height = height;

    //    this.SetUtil();

    //    //this.USprite = UnityEngine.Sprite.Create(Texture, clipRect, );
    //}

    public Rect GetRelativeRect(Rect rect)
    {
        return this.GetRelativeRect(rect.x, rect.y, rect.width, rect.height);
    }

    public Rect GetRelativeRect(float x, float y, float width, float height)
    {
        float num1 = (this.ClipRect.x - (this.DrawOffset.x)) + x;
        float num2 = (this.ClipRect.y - (this.DrawOffset.y)) + y;
        float num3 = (int)Mathf.Clamp((float)num1, (float)this.ClipRect.xMin, (float)this.ClipRect.xMax);
        float num4 = (int)Mathf.Clamp((float)num2, (float)this.ClipRect.yMin, (float)this.ClipRect.yMax);
        return new Rect(num3, num4, Mathf.Max(0, Mathf.Min(num1 + width, this.ClipRect.xMax) - num3), Mathf.Max(0, Mathf.Min(num2 + height, this.ClipRect.yMax) - num4));
    }

    private void SetUtil()
    {
        this.Center = new Vector2((float)this.Width, (float)this.Height) * 0.5f;
        this.LeftUV = (float)this.ClipRect.xMin / (float)this.Texture.width;
        this.RightUV = (float)this.ClipRect.xMax / (float)this.Texture.width;
        this.TopUV = (float)this.ClipRect.yMin / (float)this.Texture.height;
        this.BottomUV = (float)this.ClipRect.yMax / (float)this.Texture.height;
    }
}
