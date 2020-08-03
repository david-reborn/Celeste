using UnityEngine;
using System.Collections;
using myd.celeste;

public class Snow : Backdrop
{
    public static readonly Color[] ForegroundColors = new Color[]
        {
            Color.white,
            Color.blue
        };
    // Token: 0x0400183C RID: 6204
    public static readonly Color[] BackgroundColors = new Color[]
    {
            new Color(0.2f, 0.2f, 0.2f, 1f),
            new Color(0.1f, 0.2f, 0.5f, 1f)
    };

    // Token: 0x0400183D RID: 6205
    public float Alpha = 1f;

    // Token: 0x0400183E RID: 6206
    private float visibleFade = 1f;

    // Token: 0x0400183F RID: 6207
    private float linearFade = 1f;

    // Token: 0x04001840 RID: 6208
    private Color[] colors;

    // Token: 0x04001841 RID: 6209
    private Color[] blendedColors;
    // Token: 0x06001B78 RID: 7032 RVA: 0x000C90B4 File Offset: 0x000C72B4
    public Snow(bool foreground)
    {
        this.colors = (foreground ? Snow.ForegroundColors : Snow.BackgroundColors);
        this.blendedColors = new Color[this.colors.Length];
        int num = foreground ? 120 : 40;
        int num2 = foreground ? 300 : 100;
        //for (int i = 0; i < this.particles.Length; i++)
        //{
        //    this.particles[i].Init(this.colors.Length, (float)num, (float)num2);
        //}
    }
}
