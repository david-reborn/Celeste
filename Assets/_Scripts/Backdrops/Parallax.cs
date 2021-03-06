﻿using UnityEngine;
using System.Collections;
using myd.celeste;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class Parallax : Backdrop
{
    private List<Sprite> sprites = new List<Sprite>();
    public Parallax(MTexture texture)
    {
        this.Name = texture.AtlasPath;
        this.Texture = texture;
    }

    public override void OnRefresh()
    {
        Vector2 vector2_1 = new Vector2(Camera.main.transform.position.x+10, Camera.main.transform.position.y).Floor();
        Vector2 vector2_2 = (this.Position - vector2_1 * this.Scroll).Floor();

        if (this.LoopX)
        {
            while ((double)vector2_2.x < 0.0)
                vector2_2.x += (float)this.Texture.Width;
            while ((double)vector2_2.x > 0.0)
                vector2_2.x -= (float)this.Texture.Width;
        }
        if (this.LoopY)
        {
            while ((double)vector2_2.y < 0.0)
                vector2_2.y += (float)this.Texture.Height;
            while ((double)vector2_2.y > 0.0)
                vector2_2.y -= (float)this.Texture.Height;
        }
        for (float x = vector2_2.x; (double)x < 320.0; x += (float)this.Texture.Width)
        {
            for (float y = vector2_2.y; (double)y < 180.0; y += (float)this.Texture.Height)
            {
                GameObject gb = MyGameLoader.instance.ShowSprite(this.Texture, Name);
                gb.transform.position = new Vector3(x / 100f, -y/100f, 0);
                //this.Texture.Draw(new Vector2(x, y), Vector2.Zero, color, 1f, 0.0f, flip);
                if (!this.LoopY)
                    break;
            }
            if (!this.LoopX)
                break;
        }
    }
    //// Token: 0x06001B7E RID: 7038 RVA: 0x000C94D0 File Offset: 0x000C76D0
    //public override void Update(Scene scene)
    //{
    //    base.Update(scene);
    //    this.Position += this.Speed * Engine.DeltaTime;
    //    this.Position += this.WindMultiplier * (scene as Level).Wind * Engine.DeltaTime;
    //    bool doFadeIn = this.DoFadeIn;
    //    if (doFadeIn)
    //    {
    //        this.fadeIn = Calc.Approach(this.fadeIn, (float)(this.Visible ? 1 : 0), Engine.DeltaTime);
    //    }
    //    else
    //    {
    //        this.fadeIn = (float)(this.Visible ? 1 : 0);
    //    }
    //}

    //// Token: 0x06001B7F RID: 7039 RVA: 0x000C957C File Offset: 0x000C777C
    //public override void Render(Scene scene)
    //{
    //    Level level = scene as Level;
    //    Vector2 vector = (level.Camera.Position + this.CameraOffset).Floor();
    //    Vector2 vector2 = (this.Position - vector * this.Scroll).Floor();
    //    float num = this.fadeIn * this.Alpha * this.FadeAlphaMultiplier;
    //    bool flag = this.FadeX != null;
    //    if (flag)
    //    {
    //        num *= this.FadeX.Value(vector.X + 160f);
    //    }
    //    bool flag2 = this.FadeY != null;
    //    if (flag2)
    //    {
    //        num *= this.FadeY.Value(vector.Y + 90f);
    //    }
    //    Color color = this.Color;
    //    bool flag3 = num < 1f;
    //    if (flag3)
    //    {
    //        color *= num;
    //    }
    //    bool flag4 = color.A > 1;
    //    if (flag4)
    //    {
    //        bool loopX = this.LoopX;
    //        if (loopX)
    //        {
    //            while (vector2.X < 0f)
    //            {
    //                vector2.X += (float)this.Texture.Width;
    //            }
    //            while (vector2.X > 0f)
    //            {
    //                vector2.X -= (float)this.Texture.Width;
    //            }
    //        }
    //        bool loopY = this.LoopY;
    //        if (loopY)
    //        {
    //            while (vector2.Y < 0f)
    //            {
    //                vector2.Y += (float)this.Texture.Height;
    //            }
    //            while (vector2.Y > 0f)
    //            {
    //                vector2.Y -= (float)this.Texture.Height;
    //            }
    //        }
    //        SpriteEffects flip = SpriteEffects.None;
    //        bool flag5 = this.FlipX && this.FlipY;
    //        if (flag5)
    //        {
    //            flip = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
    //        }
    //        else
    //        {
    //            bool flipX = this.FlipX;
    //            if (flipX)
    //            {
    //                flip = SpriteEffects.FlipHorizontally;
    //            }
    //            else
    //            {
    //                bool flipY = this.FlipY;
    //                if (flipY)
    //                {
    //                    flip = SpriteEffects.FlipVertically;
    //                }
    //            }
    //        }
    //        for (float num2 = vector2.X; num2 < 320f; num2 += (float)this.Texture.Width)
    //        {
    //            for (float num3 = vector2.Y; num3 < 180f; num3 += (float)this.Texture.Height)
    //            {
    //                this.Texture.Draw(new Vector2(num2, num3), Vector2.Zero, color, 1f, 0f, flip);
    //                bool flag6 = !this.LoopY;
    //                if (flag6)
    //                {
    //                    break;
    //                }
    //            }
    //            bool flag7 = !this.LoopX;
    //            if (flag7)
    //            {
    //                break;
    //            }
    //        }
    //    }
    //}

    // Token: 0x04001843 RID: 6211
    public Vector2 CameraOffset = Vector2.zero;

    // Token: 0x04001844 RID: 6212
    public BlendState BlendState = BlendState.defaultValue;

    // Token: 0x04001845 RID: 6213
    public MTexture Texture;

    // Token: 0x04001846 RID: 6214
    public bool DoFadeIn;

    // Token: 0x04001847 RID: 6215
    public float Alpha = 1f;

    // Token: 0x04001848 RID: 6216
    private float fadeIn = 1f;
}
