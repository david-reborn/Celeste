using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace myd.celeste
{
    // Token: 0x0200010C RID: 268
    //public static class MInput
    //{
    //    public static MInput.KeyboardData Keyboard { get; private set; }

    //    public static MInput.MouseData Mouse { get; private set; }

    //    // Token: 0x06000846 RID: 2118 RVA: 0x00016920 File Offset: 0x00014B20
    //    internal static void Initialize()
    //    {
    //        MInput.Keyboard = new MInput.KeyboardData();
    //        MInput.Mouse = new MInput.MouseData();
    //        MInput.GamePads = new MInput.GamePadData[4];
    //        for (int i = 0; i < 4; i++)
    //        {
    //            MInput.GamePads[i] = new MInput.GamePadData(i);
    //        }
    //        MInput.VirtualInputs = new List<VirtualInput>();
    //    }

    //    // Token: 0x06000847 RID: 2119 RVA: 0x00016978 File Offset: 0x00014B78
    //    internal static void Shutdown()
    //    {
    //        foreach (MInput.GamePadData gamePadData in MInput.GamePads)
    //        {
    //            gamePadData.StopRumble();
    //        }
    //    }

    //    // Token: 0x06000848 RID: 2120 RVA: 0x000169A8 File Offset: 0x00014BA8
    //    internal static void Update()
    //    {
    //        bool flag = Engine.Instance.IsActive && MInput.Active;
    //        if (flag)
    //        {
    //            bool open = Engine.Commands.Open;
    //            if (open)
    //            {
    //                MInput.Keyboard.UpdateNull();
    //                MInput.Mouse.UpdateNull();
    //            }
    //            else
    //            {
    //                MInput.Keyboard.Update();
    //                MInput.Mouse.Update();
    //            }
    //            for (int i = 0; i < 4; i++)
    //            {
    //                MInput.GamePads[i].Update();
    //            }
    //        }
    //        else
    //        {
    //            MInput.Keyboard.UpdateNull();
    //            MInput.Mouse.UpdateNull();
    //            for (int j = 0; j < 4; j++)
    //            {
    //                MInput.GamePads[j].UpdateNull();
    //            }
    //        }
    //        MInput.UpdateVirtualInputs();
    //    }

    //    // Token: 0x06000849 RID: 2121 RVA: 0x00016A74 File Offset: 0x00014C74
    //    public static void UpdateNull()
    //    {
    //        MInput.Keyboard.UpdateNull();
    //        MInput.Mouse.UpdateNull();
    //        for (int i = 0; i < 4; i++)
    //        {
    //            MInput.GamePads[i].UpdateNull();
    //        }
    //        MInput.UpdateVirtualInputs();
    //    }

    //    // Token: 0x0600084A RID: 2122 RVA: 0x00016ABC File Offset: 0x00014CBC
    //    private static void UpdateVirtualInputs()
    //    {
    //        foreach (VirtualInput virtualInput in MInput.VirtualInputs)
    //        {
    //            virtualInput.Update();
    //        }
    //    }

    //    // Token: 0x0600084B RID: 2123 RVA: 0x00016B14 File Offset: 0x00014D14
    //    public static void RumbleFirst(float strength, float time)
    //    {
    //        MInput.GamePads[0].Rumble(strength, time);
    //    }

    //    // Token: 0x0600084C RID: 2124 RVA: 0x00016B28 File Offset: 0x00014D28
    //    public static int Axis(bool negative, bool positive, int bothValue)
    //    {
    //        int result;
    //        if (negative)
    //        {
    //            if (positive)
    //            {
    //                result = bothValue;
    //            }
    //            else
    //            {
    //                result = -1;
    //            }
    //        }
    //        else if (positive)
    //        {
    //            result = 1;
    //        }
    //        else
    //        {
    //            result = 0;
    //        }
    //        return result;
    //    }

    //    // Token: 0x0600084D RID: 2125 RVA: 0x00016B58 File Offset: 0x00014D58
    //    public static int Axis(float axisValue, float deadzone)
    //    {
    //        bool flag = Math.Abs(axisValue) >= deadzone;
    //        int result;
    //        if (flag)
    //        {
    //            result = Math.Sign(axisValue);
    //        }
    //        else
    //        {
    //            result = 0;
    //        }
    //        return result;
    //    }

    //    // Token: 0x0600084E RID: 2126 RVA: 0x00016B84 File Offset: 0x00014D84
    //    public static int Axis(bool negative, bool positive, int bothValue, float axisValue, float deadzone)
    //    {
    //        int num = MInput.Axis(axisValue, deadzone);
    //        bool flag = num == 0;
    //        if (flag)
    //        {
    //            num = MInput.Axis(negative, positive, bothValue);
    //        }
    //        return num;
    //    }

    //    // Token: 0x04000589 RID: 1417
    //    internal static List<VirtualInput> VirtualInputs;

    //    // Token: 0x0400058A RID: 1418
    //    public static bool Active = true;

    //    // Token: 0x0400058B RID: 1419
    //    public static bool Disabled = false;

    //    // Token: 0x0200039F RID: 927
    //    public class KeyboardData
    //    {
    //        // Token: 0x06001E1B RID: 7707 RVA: 0x000138CB File Offset: 0x00011ACB
    //        internal KeyboardData()
    //        {
    //        }

    //        // Token: 0x06001E1C RID: 7708 RVA: 0x000ED376 File Offset: 0x000EB576
    //        internal void Update()
    //        {
    //            this.PreviousState = this.CurrentState;
    //            this.CurrentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
    //        }

    //        // Token: 0x06001E1D RID: 7709 RVA: 0x000ED390 File Offset: 0x000EB590
    //        internal void UpdateNull()
    //        {
    //            this.PreviousState = this.CurrentState;
    //            this.CurrentState = default(KeyboardState);
    //        }

    //        // Token: 0x06001E1E RID: 7710 RVA: 0x000ED3AC File Offset: 0x000EB5AC
    //        public bool Check(Keys key)
    //        {
    //            bool disabled = MInput.Disabled;
    //            return !disabled && this.CurrentState.IsKeyDown(key);
    //        }

    //        // Token: 0x06001E1F RID: 7711 RVA: 0x000ED3D8 File Offset: 0x000EB5D8
    //        public bool Pressed(Keys key)
    //        {
    //            bool disabled = MInput.Disabled;
    //            return !disabled && this.CurrentState.IsKeyDown(key) && !this.PreviousState.IsKeyDown(key);
    //        }

    //        // Token: 0x06001E20 RID: 7712 RVA: 0x000ED418 File Offset: 0x000EB618
    //        public bool Released(Keys key)
    //        {
    //            bool disabled = MInput.Disabled;
    //            return !disabled && !this.CurrentState.IsKeyDown(key) && this.PreviousState.IsKeyDown(key);
    //        }

    //        // Token: 0x06001E21 RID: 7713 RVA: 0x000ED454 File Offset: 0x000EB654
    //        public bool Check(Keys keyA, Keys keyB)
    //        {
    //            return this.Check(keyA) || this.Check(keyB);
    //        }

    //        // Token: 0x06001E22 RID: 7714 RVA: 0x000ED47C File Offset: 0x000EB67C
    //        public bool Pressed(Keys keyA, Keys keyB)
    //        {
    //            return this.Pressed(keyA) || this.Pressed(keyB);
    //        }

    //        // Token: 0x06001E23 RID: 7715 RVA: 0x000ED4A4 File Offset: 0x000EB6A4
    //        public bool Released(Keys keyA, Keys keyB)
    //        {
    //            return this.Released(keyA) || this.Released(keyB);
    //        }

    //        // Token: 0x06001E24 RID: 7716 RVA: 0x000ED4CC File Offset: 0x000EB6CC
    //        public bool Check(Keys keyA, Keys keyB, Keys keyC)
    //        {
    //            return this.Check(keyA) || this.Check(keyB) || this.Check(keyC);
    //        }

    //        // Token: 0x06001E25 RID: 7717 RVA: 0x000ED4FC File Offset: 0x000EB6FC
    //        public bool Pressed(Keys keyA, Keys keyB, Keys keyC)
    //        {
    //            return this.Pressed(keyA) || this.Pressed(keyB) || this.Pressed(keyC);
    //        }

    //        // Token: 0x06001E26 RID: 7718 RVA: 0x000ED52C File Offset: 0x000EB72C
    //        public bool Released(Keys keyA, Keys keyB, Keys keyC)
    //        {
    //            return this.Released(keyA) || this.Released(keyB) || this.Released(keyC);
    //        }

    //        // Token: 0x06001E27 RID: 7719 RVA: 0x000ED55C File Offset: 0x000EB75C
    //        public int AxisCheck(Keys negative, Keys positive)
    //        {
    //            bool flag = this.Check(negative);
    //            int result;
    //            if (flag)
    //            {
    //                bool flag2 = this.Check(positive);
    //                if (flag2)
    //                {
    //                    result = 0;
    //                }
    //                else
    //                {
    //                    result = -1;
    //                }
    //            }
    //            else
    //            {
    //                bool flag3 = this.Check(positive);
    //                if (flag3)
    //                {
    //                    result = 1;
    //                }
    //                else
    //                {
    //                    result = 0;
    //                }
    //            }
    //            return result;
    //        }

    //        // Token: 0x06001E28 RID: 7720 RVA: 0x000ED5A0 File Offset: 0x000EB7A0
    //        public int AxisCheck(Keys negative, Keys positive, int both)
    //        {
    //            bool flag = this.Check(negative);
    //            int result;
    //            if (flag)
    //            {
    //                bool flag2 = this.Check(positive);
    //                if (flag2)
    //                {
    //                    result = both;
    //                }
    //                else
    //                {
    //                    result = -1;
    //                }
    //            }
    //            else
    //            {
    //                bool flag3 = this.Check(positive);
    //                if (flag3)
    //                {
    //                    result = 1;
    //                }
    //                else
    //                {
    //                    result = 0;
    //                }
    //            }
    //            return result;
    //        }

    //        // Token: 0x04001EBF RID: 7871
    //        public KeyboardState PreviousState;

    //        // Token: 0x04001EC0 RID: 7872
    //        public KeyboardState CurrentState;
    //    }

    //    // Token: 0x020003A0 RID: 928
    //    public class MouseData
    //    {
    //        // Token: 0x06001E29 RID: 7721 RVA: 0x000ED5E1 File Offset: 0x000EB7E1
    //        internal MouseData()
    //        {
    //            this.PreviousState = default(MouseState);
    //            this.CurrentState = default(MouseState);
    //        }

    //        // Token: 0x06001E2A RID: 7722 RVA: 0x000ED603 File Offset: 0x000EB803
    //        internal void Update()
    //        {
    //            this.PreviousState = this.CurrentState;
    //            this.CurrentState = Microsoft.Xna.Framework.Input.Mouse.GetState();
    //        }

    //        // Token: 0x06001E2B RID: 7723 RVA: 0x000ED61D File Offset: 0x000EB81D
    //        internal void UpdateNull()
    //        {
    //            this.PreviousState = this.CurrentState;
    //            this.CurrentState = default(MouseState);
    //        }

    //        // Token: 0x17000251 RID: 593
    //        // (get) Token: 0x06001E2C RID: 7724 RVA: 0x000ED638 File Offset: 0x000EB838
    //        public bool CheckLeftButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.LeftButton == ButtonState.Pressed;
    //            }
    //        }

    //        // Token: 0x17000252 RID: 594
    //        // (get) Token: 0x06001E2D RID: 7725 RVA: 0x000ED658 File Offset: 0x000EB858
    //        public bool CheckRightButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.RightButton == ButtonState.Pressed;
    //            }
    //        }

    //        // Token: 0x17000253 RID: 595
    //        // (get) Token: 0x06001E2E RID: 7726 RVA: 0x000ED678 File Offset: 0x000EB878
    //        public bool CheckMiddleButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.MiddleButton == ButtonState.Pressed;
    //            }
    //        }

    //        // Token: 0x17000254 RID: 596
    //        // (get) Token: 0x06001E2F RID: 7727 RVA: 0x000ED698 File Offset: 0x000EB898
    //        public bool PressedLeftButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.LeftButton == ButtonState.Pressed && this.PreviousState.LeftButton == ButtonState.Released;
    //            }
    //        }

    //        // Token: 0x17000255 RID: 597
    //        // (get) Token: 0x06001E30 RID: 7728 RVA: 0x000ED6CC File Offset: 0x000EB8CC
    //        public bool PressedRightButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.RightButton == ButtonState.Pressed && this.PreviousState.RightButton == ButtonState.Released;
    //            }
    //        }

    //        // Token: 0x17000256 RID: 598
    //        // (get) Token: 0x06001E31 RID: 7729 RVA: 0x000ED700 File Offset: 0x000EB900
    //        public bool PressedMiddleButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.MiddleButton == ButtonState.Pressed && this.PreviousState.MiddleButton == ButtonState.Released;
    //            }
    //        }

    //        // Token: 0x17000257 RID: 599
    //        // (get) Token: 0x06001E32 RID: 7730 RVA: 0x000ED734 File Offset: 0x000EB934
    //        public bool ReleasedLeftButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.LeftButton == ButtonState.Released && this.PreviousState.LeftButton == ButtonState.Pressed;
    //            }
    //        }

    //        // Token: 0x17000258 RID: 600
    //        // (get) Token: 0x06001E33 RID: 7731 RVA: 0x000ED764 File Offset: 0x000EB964
    //        public bool ReleasedRightButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.RightButton == ButtonState.Released && this.PreviousState.RightButton == ButtonState.Pressed;
    //            }
    //        }

    //        // Token: 0x17000259 RID: 601
    //        // (get) Token: 0x06001E34 RID: 7732 RVA: 0x000ED794 File Offset: 0x000EB994
    //        public bool ReleasedMiddleButton
    //        {
    //            get
    //            {
    //                return this.CurrentState.MiddleButton == ButtonState.Released && this.PreviousState.MiddleButton == ButtonState.Pressed;
    //            }
    //        }

    //        // Token: 0x1700025A RID: 602
    //        // (get) Token: 0x06001E35 RID: 7733 RVA: 0x000ED7C4 File Offset: 0x000EB9C4
    //        public int Wheel
    //        {
    //            get
    //            {
    //                return this.CurrentState.ScrollWheelValue;
    //            }
    //        }

    //        // Token: 0x1700025B RID: 603
    //        // (get) Token: 0x06001E36 RID: 7734 RVA: 0x000ED7E4 File Offset: 0x000EB9E4
    //        public int WheelDelta
    //        {
    //            get
    //            {
    //                return this.CurrentState.ScrollWheelValue - this.PreviousState.ScrollWheelValue;
    //            }
    //        }

    //        // Token: 0x1700025C RID: 604
    //        // (get) Token: 0x06001E37 RID: 7735 RVA: 0x000ED810 File Offset: 0x000EBA10
    //        public bool WasMoved
    //        {
    //            get
    //            {
    //                return this.CurrentState.X != this.PreviousState.X || this.CurrentState.Y != this.PreviousState.Y;
    //            }
    //        }

    //        // Token: 0x1700025D RID: 605
    //        // (get) Token: 0x06001E38 RID: 7736 RVA: 0x000ED858 File Offset: 0x000EBA58
    //        // (set) Token: 0x06001E39 RID: 7737 RVA: 0x000ED875 File Offset: 0x000EBA75
    //        public float X
    //        {
    //            get
    //            {
    //                return this.Position.X;
    //            }
    //            set
    //            {
    //                this.Position = new Vector2(value, this.Position.Y);
    //            }
    //        }

    //        // Token: 0x1700025E RID: 606
    //        // (get) Token: 0x06001E3A RID: 7738 RVA: 0x000ED890 File Offset: 0x000EBA90
    //        // (set) Token: 0x06001E3B RID: 7739 RVA: 0x000ED8AD File Offset: 0x000EBAAD
    //        public float Y
    //        {
    //            get
    //            {
    //                return this.Position.Y;
    //            }
    //            set
    //            {
    //                this.Position = new Vector2(this.Position.X, value);
    //            }
    //        }

    //        // Token: 0x1700025F RID: 607
    //        // (get) Token: 0x06001E3C RID: 7740 RVA: 0x000ED8C8 File Offset: 0x000EBAC8
    //        // (set) Token: 0x06001E3D RID: 7741 RVA: 0x000ED908 File Offset: 0x000EBB08
    //        public Vector2 Position
    //        {
    //            get
    //            {
    //                return Vector2.Transform(new Vector2((float)this.CurrentState.X, (float)this.CurrentState.Y), Matrix.Invert(Engine.ScreenMatrix));
    //            }
    //            set
    //            {
    //                Vector2 vector = Vector2.Transform(value, Engine.ScreenMatrix);
    //                Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)Math.Round((double)vector.X), (int)Math.Round((double)vector.Y));
    //            }
    //        }

    //        // Token: 0x04001EC1 RID: 7873
    //        public MouseState PreviousState;

    //        // Token: 0x04001EC2 RID: 7874
    //        public MouseState CurrentState;
    //    }
    //}
        
}
