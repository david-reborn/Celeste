using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace myd.celeste
{
    // Token: 0x0200010C RID: 268
    public static class MInput
    {
        public static MInput.KeyboardData Keyboard { get; private set; }

        public static MInput.MouseData Mouse { get; private set; }

        public static MInput.GamePadData[] GamePads { get; private set; }

        // Token: 0x06000846 RID: 2118 RVA: 0x00016920 File Offset: 0x00014B20
        internal static void Initialize()
        {
            MInput.Keyboard = new MInput.KeyboardData();
            MInput.Mouse = new MInput.MouseData();
            MInput.GamePads = new MInput.GamePadData[4];
            for (int i = 0; i < 4; i++)
            {
                MInput.GamePads[i] = new MInput.GamePadData(i);
            }
            MInput.VirtualInputs = new List<VirtualInput>();
        }

        // Token: 0x06000847 RID: 2119 RVA: 0x00016978 File Offset: 0x00014B78
        internal static void Shutdown()
        {
            foreach (MInput.GamePadData gamePadData in MInput.GamePads)
            {
                gamePadData.StopRumble();
            }
        }

        // Token: 0x06000848 RID: 2120 RVA: 0x000169A8 File Offset: 0x00014BA8
        internal static void Update()
        {
            bool flag = Engine.Instance.IsActive && MInput.Active;
            if (flag)
            {
                bool open = Engine.Commands.Open;
                if (open)
                {
                    MInput.Keyboard.UpdateNull();
                    MInput.Mouse.UpdateNull();
                }
                else
                {
                    MInput.Keyboard.Update();
                    MInput.Mouse.Update();
                }
                for (int i = 0; i < 4; i++)
                {
                    MInput.GamePads[i].Update();
                }
            }
            else
            {
                MInput.Keyboard.UpdateNull();
                MInput.Mouse.UpdateNull();
                for (int j = 0; j < 4; j++)
                {
                    MInput.GamePads[j].UpdateNull();
                }
            }
            MInput.UpdateVirtualInputs();
        }

        // Token: 0x06000849 RID: 2121 RVA: 0x00016A74 File Offset: 0x00014C74
        public static void UpdateNull()
        {
            MInput.Keyboard.UpdateNull();
            MInput.Mouse.UpdateNull();
            for (int i = 0; i < 4; i++)
            {
                MInput.GamePads[i].UpdateNull();
            }
            MInput.UpdateVirtualInputs();
        }

        // Token: 0x0600084A RID: 2122 RVA: 0x00016ABC File Offset: 0x00014CBC
        private static void UpdateVirtualInputs()
        {
            foreach (VirtualInput virtualInput in MInput.VirtualInputs)
            {
                virtualInput.Update();
            }
        }

        // Token: 0x0600084B RID: 2123 RVA: 0x00016B14 File Offset: 0x00014D14
        public static void RumbleFirst(float strength, float time)
        {
            MInput.GamePads[0].Rumble(strength, time);
        }

        // Token: 0x0600084C RID: 2124 RVA: 0x00016B28 File Offset: 0x00014D28
        public static int Axis(bool negative, bool positive, int bothValue)
        {
            int result;
            if (negative)
            {
                if (positive)
                {
                    result = bothValue;
                }
                else
                {
                    result = -1;
                }
            }
            else if (positive)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        // Token: 0x0600084D RID: 2125 RVA: 0x00016B58 File Offset: 0x00014D58
        public static int Axis(float axisValue, float deadzone)
        {
            bool flag = Math.Abs(axisValue) >= deadzone;
            int result;
            if (flag)
            {
                result = Math.Sign(axisValue);
            }
            else
            {
                result = 0;
            }
            return result;
        }

        // Token: 0x0600084E RID: 2126 RVA: 0x00016B84 File Offset: 0x00014D84
        public static int Axis(bool negative, bool positive, int bothValue, float axisValue, float deadzone)
        {
            int num = MInput.Axis(axisValue, deadzone);
            bool flag = num == 0;
            if (flag)
            {
                num = MInput.Axis(negative, positive, bothValue);
            }
            return num;
        }

        // Token: 0x04000589 RID: 1417
        internal static List<VirtualInput> VirtualInputs;

        // Token: 0x0400058A RID: 1418
        public static bool Active = true;

        // Token: 0x0400058B RID: 1419
        public static bool Disabled = false;

        // Token: 0x0200039F RID: 927
        public class KeyboardData
        {
            // Token: 0x06001E1B RID: 7707 RVA: 0x000138CB File Offset: 0x00011ACB
            internal KeyboardData()
            {
            }

            // Token: 0x06001E1C RID: 7708 RVA: 0x000ED376 File Offset: 0x000EB576
            internal void Update()
            {
                this.PreviousState = this.CurrentState;
                this.CurrentState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            }

            // Token: 0x06001E1D RID: 7709 RVA: 0x000ED390 File Offset: 0x000EB590
            internal void UpdateNull()
            {
                this.PreviousState = this.CurrentState;
                this.CurrentState = default(KeyboardState);
            }

            // Token: 0x06001E1E RID: 7710 RVA: 0x000ED3AC File Offset: 0x000EB5AC
            public bool Check(Keys key)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.IsKeyDown(key);
            }

            // Token: 0x06001E1F RID: 7711 RVA: 0x000ED3D8 File Offset: 0x000EB5D8
            public bool Pressed(Keys key)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.IsKeyDown(key) && !this.PreviousState.IsKeyDown(key);
            }

            // Token: 0x06001E20 RID: 7712 RVA: 0x000ED418 File Offset: 0x000EB618
            public bool Released(Keys key)
            {
                bool disabled = MInput.Disabled;
                return !disabled && !this.CurrentState.IsKeyDown(key) && this.PreviousState.IsKeyDown(key);
            }

            // Token: 0x06001E21 RID: 7713 RVA: 0x000ED454 File Offset: 0x000EB654
            public bool Check(Keys keyA, Keys keyB)
            {
                return this.Check(keyA) || this.Check(keyB);
            }

            // Token: 0x06001E22 RID: 7714 RVA: 0x000ED47C File Offset: 0x000EB67C
            public bool Pressed(Keys keyA, Keys keyB)
            {
                return this.Pressed(keyA) || this.Pressed(keyB);
            }

            // Token: 0x06001E23 RID: 7715 RVA: 0x000ED4A4 File Offset: 0x000EB6A4
            public bool Released(Keys keyA, Keys keyB)
            {
                return this.Released(keyA) || this.Released(keyB);
            }

            // Token: 0x06001E24 RID: 7716 RVA: 0x000ED4CC File Offset: 0x000EB6CC
            public bool Check(Keys keyA, Keys keyB, Keys keyC)
            {
                return this.Check(keyA) || this.Check(keyB) || this.Check(keyC);
            }

            // Token: 0x06001E25 RID: 7717 RVA: 0x000ED4FC File Offset: 0x000EB6FC
            public bool Pressed(Keys keyA, Keys keyB, Keys keyC)
            {
                return this.Pressed(keyA) || this.Pressed(keyB) || this.Pressed(keyC);
            }

            // Token: 0x06001E26 RID: 7718 RVA: 0x000ED52C File Offset: 0x000EB72C
            public bool Released(Keys keyA, Keys keyB, Keys keyC)
            {
                return this.Released(keyA) || this.Released(keyB) || this.Released(keyC);
            }

            // Token: 0x06001E27 RID: 7719 RVA: 0x000ED55C File Offset: 0x000EB75C
            public int AxisCheck(Keys negative, Keys positive)
            {
                bool flag = this.Check(negative);
                int result;
                if (flag)
                {
                    bool flag2 = this.Check(positive);
                    if (flag2)
                    {
                        result = 0;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                else
                {
                    bool flag3 = this.Check(positive);
                    if (flag3)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                return result;
            }

            // Token: 0x06001E28 RID: 7720 RVA: 0x000ED5A0 File Offset: 0x000EB7A0
            public int AxisCheck(Keys negative, Keys positive, int both)
            {
                bool flag = this.Check(negative);
                int result;
                if (flag)
                {
                    bool flag2 = this.Check(positive);
                    if (flag2)
                    {
                        result = both;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                else
                {
                    bool flag3 = this.Check(positive);
                    if (flag3)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                return result;
            }

            // Token: 0x04001EBF RID: 7871
            public KeyboardState PreviousState;

            // Token: 0x04001EC0 RID: 7872
            public KeyboardState CurrentState;
        }

        // Token: 0x020003A0 RID: 928
        public class MouseData
        {
            // Token: 0x06001E29 RID: 7721 RVA: 0x000ED5E1 File Offset: 0x000EB7E1
            internal MouseData()
            {
                this.PreviousState = default(MouseState);
                this.CurrentState = default(MouseState);
            }

            // Token: 0x06001E2A RID: 7722 RVA: 0x000ED603 File Offset: 0x000EB803
            internal void Update()
            {
                this.PreviousState = this.CurrentState;
                this.CurrentState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            }

            // Token: 0x06001E2B RID: 7723 RVA: 0x000ED61D File Offset: 0x000EB81D
            internal void UpdateNull()
            {
                this.PreviousState = this.CurrentState;
                this.CurrentState = default(MouseState);
            }

            // Token: 0x17000251 RID: 593
            // (get) Token: 0x06001E2C RID: 7724 RVA: 0x000ED638 File Offset: 0x000EB838
            public bool CheckLeftButton
            {
                get
                {
                    return this.CurrentState.LeftButton == ButtonState.Pressed;
                }
            }

            // Token: 0x17000252 RID: 594
            // (get) Token: 0x06001E2D RID: 7725 RVA: 0x000ED658 File Offset: 0x000EB858
            public bool CheckRightButton
            {
                get
                {
                    return this.CurrentState.RightButton == ButtonState.Pressed;
                }
            }

            // Token: 0x17000253 RID: 595
            // (get) Token: 0x06001E2E RID: 7726 RVA: 0x000ED678 File Offset: 0x000EB878
            public bool CheckMiddleButton
            {
                get
                {
                    return this.CurrentState.MiddleButton == ButtonState.Pressed;
                }
            }

            // Token: 0x17000254 RID: 596
            // (get) Token: 0x06001E2F RID: 7727 RVA: 0x000ED698 File Offset: 0x000EB898
            public bool PressedLeftButton
            {
                get
                {
                    return this.CurrentState.LeftButton == ButtonState.Pressed && this.PreviousState.LeftButton == ButtonState.Released;
                }
            }

            // Token: 0x17000255 RID: 597
            // (get) Token: 0x06001E30 RID: 7728 RVA: 0x000ED6CC File Offset: 0x000EB8CC
            public bool PressedRightButton
            {
                get
                {
                    return this.CurrentState.RightButton == ButtonState.Pressed && this.PreviousState.RightButton == ButtonState.Released;
                }
            }

            // Token: 0x17000256 RID: 598
            // (get) Token: 0x06001E31 RID: 7729 RVA: 0x000ED700 File Offset: 0x000EB900
            public bool PressedMiddleButton
            {
                get
                {
                    return this.CurrentState.MiddleButton == ButtonState.Pressed && this.PreviousState.MiddleButton == ButtonState.Released;
                }
            }

            // Token: 0x17000257 RID: 599
            // (get) Token: 0x06001E32 RID: 7730 RVA: 0x000ED734 File Offset: 0x000EB934
            public bool ReleasedLeftButton
            {
                get
                {
                    return this.CurrentState.LeftButton == ButtonState.Released && this.PreviousState.LeftButton == ButtonState.Pressed;
                }
            }

            // Token: 0x17000258 RID: 600
            // (get) Token: 0x06001E33 RID: 7731 RVA: 0x000ED764 File Offset: 0x000EB964
            public bool ReleasedRightButton
            {
                get
                {
                    return this.CurrentState.RightButton == ButtonState.Released && this.PreviousState.RightButton == ButtonState.Pressed;
                }
            }

            // Token: 0x17000259 RID: 601
            // (get) Token: 0x06001E34 RID: 7732 RVA: 0x000ED794 File Offset: 0x000EB994
            public bool ReleasedMiddleButton
            {
                get
                {
                    return this.CurrentState.MiddleButton == ButtonState.Released && this.PreviousState.MiddleButton == ButtonState.Pressed;
                }
            }

            // Token: 0x1700025A RID: 602
            // (get) Token: 0x06001E35 RID: 7733 RVA: 0x000ED7C4 File Offset: 0x000EB9C4
            public int Wheel
            {
                get
                {
                    return this.CurrentState.ScrollWheelValue;
                }
            }

            // Token: 0x1700025B RID: 603
            // (get) Token: 0x06001E36 RID: 7734 RVA: 0x000ED7E4 File Offset: 0x000EB9E4
            public int WheelDelta
            {
                get
                {
                    return this.CurrentState.ScrollWheelValue - this.PreviousState.ScrollWheelValue;
                }
            }

            // Token: 0x1700025C RID: 604
            // (get) Token: 0x06001E37 RID: 7735 RVA: 0x000ED810 File Offset: 0x000EBA10
            public bool WasMoved
            {
                get
                {
                    return this.CurrentState.X != this.PreviousState.X || this.CurrentState.Y != this.PreviousState.Y;
                }
            }

            // Token: 0x1700025D RID: 605
            // (get) Token: 0x06001E38 RID: 7736 RVA: 0x000ED858 File Offset: 0x000EBA58
            // (set) Token: 0x06001E39 RID: 7737 RVA: 0x000ED875 File Offset: 0x000EBA75
            public float X
            {
                get
                {
                    return this.Position.X;
                }
                set
                {
                    this.Position = new Vector2(value, this.Position.Y);
                }
            }

            // Token: 0x1700025E RID: 606
            // (get) Token: 0x06001E3A RID: 7738 RVA: 0x000ED890 File Offset: 0x000EBA90
            // (set) Token: 0x06001E3B RID: 7739 RVA: 0x000ED8AD File Offset: 0x000EBAAD
            public float Y
            {
                get
                {
                    return this.Position.Y;
                }
                set
                {
                    this.Position = new Vector2(this.Position.X, value);
                }
            }

            // Token: 0x1700025F RID: 607
            // (get) Token: 0x06001E3C RID: 7740 RVA: 0x000ED8C8 File Offset: 0x000EBAC8
            // (set) Token: 0x06001E3D RID: 7741 RVA: 0x000ED908 File Offset: 0x000EBB08
            public Vector2 Position
            {
                get
                {
                    return Vector2.Transform(new Vector2((float)this.CurrentState.X, (float)this.CurrentState.Y), Matrix.Invert(Engine.ScreenMatrix));
                }
                set
                {
                    Vector2 vector = Vector2.Transform(value, Engine.ScreenMatrix);
                    Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)Math.Round((double)vector.X), (int)Math.Round((double)vector.Y));
                }
            }

            // Token: 0x04001EC1 RID: 7873
            public MouseState PreviousState;

            // Token: 0x04001EC2 RID: 7874
            public MouseState CurrentState;
        }

        // Token: 0x020003A1 RID: 929
        public class GamePadData
        {
            // Token: 0x06001E3E RID: 7742 RVA: 0x000ED942 File Offset: 0x000EBB42
            internal GamePadData(int playerIndex)
            {
                this.PlayerIndex = (PlayerIndex)Calc.Clamp(playerIndex, 0, 3);
            }

            // Token: 0x06001E3F RID: 7743 RVA: 0x000ED95C File Offset: 0x000EBB5C
            public void Update()
            {
                this.PreviousState = this.CurrentState;
                this.CurrentState = GamePad.GetState(this.PlayerIndex);
                this.Attached = this.CurrentState.IsConnected;
                bool flag = this.rumbleTime > 0f;
                if (flag)
                {
                    this.rumbleTime -= Engine.DeltaTime;
                    bool flag2 = this.rumbleTime <= 0f;
                    if (flag2)
                    {
                        GamePad.SetVibration(this.PlayerIndex, 0f, 0f);
                    }
                }
            }

            // Token: 0x06001E40 RID: 7744 RVA: 0x000ED9E8 File Offset: 0x000EBBE8
            public void UpdateNull()
            {
                this.PreviousState = this.CurrentState;
                this.CurrentState = default(GamePadState);
                this.Attached = GamePad.GetState(this.PlayerIndex).IsConnected;
                bool flag = this.rumbleTime > 0f;
                if (flag)
                {
                    this.rumbleTime -= Engine.DeltaTime;
                }
                GamePad.SetVibration(this.PlayerIndex, 0f, 0f);
            }

            // Token: 0x06001E41 RID: 7745 RVA: 0x000EDA60 File Offset: 0x000EBC60
            public void Rumble(float strength, float time)
            {
                bool flag = this.rumbleTime <= 0f || strength > this.rumbleStrength || (strength == this.rumbleStrength && time > this.rumbleTime);
                if (flag)
                {
                    GamePad.SetVibration(this.PlayerIndex, strength, strength);
                    this.rumbleStrength = strength;
                    this.rumbleTime = time;
                }
            }

            // Token: 0x06001E42 RID: 7746 RVA: 0x000EDABE File Offset: 0x000EBCBE
            public void StopRumble()
            {
                GamePad.SetVibration(this.PlayerIndex, 0f, 0f);
                this.rumbleTime = 0f;
            }

            // Token: 0x06001E43 RID: 7747 RVA: 0x000EDAE4 File Offset: 0x000EBCE4
            public bool Check(Buttons button)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.IsButtonDown(button);
            }

            // Token: 0x06001E44 RID: 7748 RVA: 0x000EDB10 File Offset: 0x000EBD10
            public bool Pressed(Buttons button)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.IsButtonDown(button) && this.PreviousState.IsButtonUp(button);
            }

            // Token: 0x06001E45 RID: 7749 RVA: 0x000EDB4C File Offset: 0x000EBD4C
            public bool Released(Buttons button)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.IsButtonUp(button) && this.PreviousState.IsButtonDown(button);
            }

            // Token: 0x06001E46 RID: 7750 RVA: 0x000EDB88 File Offset: 0x000EBD88
            public bool Check(Buttons buttonA, Buttons buttonB)
            {
                return this.Check(buttonA) || this.Check(buttonB);
            }

            // Token: 0x06001E47 RID: 7751 RVA: 0x000EDBB0 File Offset: 0x000EBDB0
            public bool Pressed(Buttons buttonA, Buttons buttonB)
            {
                return this.Pressed(buttonA) || this.Pressed(buttonB);
            }

            // Token: 0x06001E48 RID: 7752 RVA: 0x000EDBD8 File Offset: 0x000EBDD8
            public bool Released(Buttons buttonA, Buttons buttonB)
            {
                return this.Released(buttonA) || this.Released(buttonB);
            }

            // Token: 0x06001E49 RID: 7753 RVA: 0x000EDC00 File Offset: 0x000EBE00
            public bool Check(Buttons buttonA, Buttons buttonB, Buttons buttonC)
            {
                return this.Check(buttonA) || this.Check(buttonB) || this.Check(buttonC);
            }

            // Token: 0x06001E4A RID: 7754 RVA: 0x000EDC30 File Offset: 0x000EBE30
            public bool Pressed(Buttons buttonA, Buttons buttonB, Buttons buttonC)
            {
                return this.Pressed(buttonA) || this.Pressed(buttonB) || this.Check(buttonC);
            }

            // Token: 0x06001E4B RID: 7755 RVA: 0x000EDC60 File Offset: 0x000EBE60
            public bool Released(Buttons buttonA, Buttons buttonB, Buttons buttonC)
            {
                return this.Released(buttonA) || this.Released(buttonB) || this.Check(buttonC);
            }

            // Token: 0x06001E4C RID: 7756 RVA: 0x000EDC90 File Offset: 0x000EBE90
            public Vector2 GetLeftStick()
            {
                Vector2 left = this.CurrentState.ThumbSticks.Left;
                left.Y = -left.Y;
                return left;
            }

            // Token: 0x06001E4D RID: 7757 RVA: 0x000EDCC8 File Offset: 0x000EBEC8
            public Vector2 GetLeftStick(float deadzone)
            {
                Vector2 vector = this.CurrentState.ThumbSticks.Left;
                bool flag = vector.LengthSquared() < deadzone * deadzone;
                if (flag)
                {
                    vector = Vector2.Zero;
                }
                else
                {
                    vector.Y = -vector.Y;
                }
                return vector;
            }

            // Token: 0x06001E4E RID: 7758 RVA: 0x000EDD18 File Offset: 0x000EBF18
            public Vector2 GetRightStick()
            {
                Vector2 right = this.CurrentState.ThumbSticks.Right;
                right.Y = -right.Y;
                return right;
            }

            // Token: 0x06001E4F RID: 7759 RVA: 0x000EDD50 File Offset: 0x000EBF50
            public Vector2 GetRightStick(float deadzone)
            {
                Vector2 vector = this.CurrentState.ThumbSticks.Right;
                bool flag = vector.LengthSquared() < deadzone * deadzone;
                if (flag)
                {
                    vector = Vector2.Zero;
                }
                else
                {
                    vector.Y = -vector.Y;
                }
                return vector;
            }

            // Token: 0x06001E50 RID: 7760 RVA: 0x000EDDA0 File Offset: 0x000EBFA0
            public bool LeftStickLeftCheck(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.X <= -deadzone;
            }

            // Token: 0x06001E51 RID: 7761 RVA: 0x000EDDD4 File Offset: 0x000EBFD4
            public bool LeftStickLeftPressed(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.X <= -deadzone && this.PreviousState.ThumbSticks.Left.X > -deadzone;
            }

            // Token: 0x06001E52 RID: 7762 RVA: 0x000EDE24 File Offset: 0x000EC024
            public bool LeftStickLeftReleased(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.X > -deadzone && this.PreviousState.ThumbSticks.Left.X <= -deadzone;
            }

            // Token: 0x06001E53 RID: 7763 RVA: 0x000EDE74 File Offset: 0x000EC074
            public bool LeftStickRightCheck(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.X >= deadzone;
            }

            // Token: 0x06001E54 RID: 7764 RVA: 0x000EDEA4 File Offset: 0x000EC0A4
            public bool LeftStickRightPressed(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.X >= deadzone && this.PreviousState.ThumbSticks.Left.X < deadzone;
            }

            // Token: 0x06001E55 RID: 7765 RVA: 0x000EDEF0 File Offset: 0x000EC0F0
            public bool LeftStickRightReleased(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.X < deadzone && this.PreviousState.ThumbSticks.Left.X >= deadzone;
            }

            // Token: 0x06001E56 RID: 7766 RVA: 0x000EDF40 File Offset: 0x000EC140
            public bool LeftStickDownCheck(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.Y <= -deadzone;
            }

            // Token: 0x06001E57 RID: 7767 RVA: 0x000EDF74 File Offset: 0x000EC174
            public bool LeftStickDownPressed(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.Y <= -deadzone && this.PreviousState.ThumbSticks.Left.Y > -deadzone;
            }

            // Token: 0x06001E58 RID: 7768 RVA: 0x000EDFC4 File Offset: 0x000EC1C4
            public bool LeftStickDownReleased(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.Y > -deadzone && this.PreviousState.ThumbSticks.Left.Y <= -deadzone;
            }

            // Token: 0x06001E59 RID: 7769 RVA: 0x000EE014 File Offset: 0x000EC214
            public bool LeftStickUpCheck(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.Y >= deadzone;
            }

            // Token: 0x06001E5A RID: 7770 RVA: 0x000EE044 File Offset: 0x000EC244
            public bool LeftStickUpPressed(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.Y >= deadzone && this.PreviousState.ThumbSticks.Left.Y < deadzone;
            }

            // Token: 0x06001E5B RID: 7771 RVA: 0x000EE090 File Offset: 0x000EC290
            public bool LeftStickUpReleased(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Left.Y < deadzone && this.PreviousState.ThumbSticks.Left.Y >= deadzone;
            }

            // Token: 0x06001E5C RID: 7772 RVA: 0x000EE0E0 File Offset: 0x000EC2E0
            public float LeftStickHorizontal(float deadzone)
            {
                float x = this.CurrentState.ThumbSticks.Left.X;
                bool flag = Math.Abs(x) < deadzone;
                float result;
                if (flag)
                {
                    result = 0f;
                }
                else
                {
                    result = x;
                }
                return result;
            }

            // Token: 0x06001E5D RID: 7773 RVA: 0x000EE124 File Offset: 0x000EC324
            public float LeftStickVertical(float deadzone)
            {
                float y = this.CurrentState.ThumbSticks.Left.Y;
                bool flag = Math.Abs(y) < deadzone;
                float result;
                if (flag)
                {
                    result = 0f;
                }
                else
                {
                    result = -y;
                }
                return result;
            }

            // Token: 0x06001E5E RID: 7774 RVA: 0x000EE168 File Offset: 0x000EC368
            public bool RightStickLeftCheck(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.X <= -deadzone;
            }

            // Token: 0x06001E5F RID: 7775 RVA: 0x000EE19C File Offset: 0x000EC39C
            public bool RightStickLeftPressed(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.X <= -deadzone && this.PreviousState.ThumbSticks.Right.X > -deadzone;
            }

            // Token: 0x06001E60 RID: 7776 RVA: 0x000EE1EC File Offset: 0x000EC3EC
            public bool RightStickLeftReleased(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.X > -deadzone && this.PreviousState.ThumbSticks.Right.X <= -deadzone;
            }

            // Token: 0x06001E61 RID: 7777 RVA: 0x000EE23C File Offset: 0x000EC43C
            public bool RightStickRightCheck(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.X >= deadzone;
            }

            // Token: 0x06001E62 RID: 7778 RVA: 0x000EE26C File Offset: 0x000EC46C
            public bool RightStickRightPressed(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.X >= deadzone && this.PreviousState.ThumbSticks.Right.X < deadzone;
            }

            // Token: 0x06001E63 RID: 7779 RVA: 0x000EE2B8 File Offset: 0x000EC4B8
            public bool RightStickRightReleased(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.X < deadzone && this.PreviousState.ThumbSticks.Right.X >= deadzone;
            }

            // Token: 0x06001E64 RID: 7780 RVA: 0x000EE308 File Offset: 0x000EC508
            public bool RightStickUpCheck(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.Y <= -deadzone;
            }

            // Token: 0x06001E65 RID: 7781 RVA: 0x000EE33C File Offset: 0x000EC53C
            public bool RightStickUpPressed(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.Y <= -deadzone && this.PreviousState.ThumbSticks.Right.Y > -deadzone;
            }

            // Token: 0x06001E66 RID: 7782 RVA: 0x000EE38C File Offset: 0x000EC58C
            public bool RightStickUpReleased(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.Y > -deadzone && this.PreviousState.ThumbSticks.Right.Y <= -deadzone;
            }

            // Token: 0x06001E67 RID: 7783 RVA: 0x000EE3DC File Offset: 0x000EC5DC
            public bool RightStickDownCheck(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.Y >= deadzone;
            }

            // Token: 0x06001E68 RID: 7784 RVA: 0x000EE40C File Offset: 0x000EC60C
            public bool RightStickDownPressed(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.Y >= deadzone && this.PreviousState.ThumbSticks.Right.Y < deadzone;
            }

            // Token: 0x06001E69 RID: 7785 RVA: 0x000EE458 File Offset: 0x000EC658
            public bool RightStickDownReleased(float deadzone)
            {
                return this.CurrentState.ThumbSticks.Right.Y < deadzone && this.PreviousState.ThumbSticks.Right.Y >= deadzone;
            }

            // Token: 0x06001E6A RID: 7786 RVA: 0x000EE4A8 File Offset: 0x000EC6A8
            public float RightStickHorizontal(float deadzone)
            {
                float x = this.CurrentState.ThumbSticks.Right.X;
                bool flag = Math.Abs(x) < deadzone;
                float result;
                if (flag)
                {
                    result = 0f;
                }
                else
                {
                    result = x;
                }
                return result;
            }

            // Token: 0x06001E6B RID: 7787 RVA: 0x000EE4EC File Offset: 0x000EC6EC
            public float RightStickVertical(float deadzone)
            {
                float y = this.CurrentState.ThumbSticks.Right.Y;
                bool flag = Math.Abs(y) < deadzone;
                float result;
                if (flag)
                {
                    result = 0f;
                }
                else
                {
                    result = -y;
                }
                return result;
            }

            // Token: 0x17000260 RID: 608
            // (get) Token: 0x06001E6C RID: 7788 RVA: 0x000EE530 File Offset: 0x000EC730
            public int DPadHorizontal
            {
                get
                {
                    return (this.CurrentState.DPad.Right == ButtonState.Pressed) ? 1 : ((this.CurrentState.DPad.Left == ButtonState.Pressed) ? -1 : 0);
                }
            }

            // Token: 0x17000261 RID: 609
            // (get) Token: 0x06001E6D RID: 7789 RVA: 0x000EE578 File Offset: 0x000EC778
            public int DPadVertical
            {
                get
                {
                    return (this.CurrentState.DPad.Down == ButtonState.Pressed) ? 1 : ((this.CurrentState.DPad.Up == ButtonState.Pressed) ? -1 : 0);
                }
            }

            // Token: 0x17000262 RID: 610
            // (get) Token: 0x06001E6E RID: 7790 RVA: 0x000EE5C0 File Offset: 0x000EC7C0
            public Vector2 DPad
            {
                get
                {
                    return new Vector2((float)this.DPadHorizontal, (float)this.DPadVertical);
                }
            }

            // Token: 0x17000263 RID: 611
            // (get) Token: 0x06001E6F RID: 7791 RVA: 0x000EE5E8 File Offset: 0x000EC7E8
            public bool DPadLeftCheck
            {
                get
                {
                    return this.CurrentState.DPad.Left == ButtonState.Pressed;
                }
            }

            // Token: 0x17000264 RID: 612
            // (get) Token: 0x06001E70 RID: 7792 RVA: 0x000EE610 File Offset: 0x000EC810
            public bool DPadLeftPressed
            {
                get
                {
                    return this.CurrentState.DPad.Left == ButtonState.Pressed && this.PreviousState.DPad.Left == ButtonState.Released;
                }
            }

            // Token: 0x17000265 RID: 613
            // (get) Token: 0x06001E71 RID: 7793 RVA: 0x000EE654 File Offset: 0x000EC854
            public bool DPadLeftReleased
            {
                get
                {
                    return this.CurrentState.DPad.Left == ButtonState.Released && this.PreviousState.DPad.Left == ButtonState.Pressed;
                }
            }

            // Token: 0x17000266 RID: 614
            // (get) Token: 0x06001E72 RID: 7794 RVA: 0x000EE694 File Offset: 0x000EC894
            public bool DPadRightCheck
            {
                get
                {
                    return this.CurrentState.DPad.Right == ButtonState.Pressed;
                }
            }

            // Token: 0x17000267 RID: 615
            // (get) Token: 0x06001E73 RID: 7795 RVA: 0x000EE6BC File Offset: 0x000EC8BC
            public bool DPadRightPressed
            {
                get
                {
                    return this.CurrentState.DPad.Right == ButtonState.Pressed && this.PreviousState.DPad.Right == ButtonState.Released;
                }
            }

            // Token: 0x17000268 RID: 616
            // (get) Token: 0x06001E74 RID: 7796 RVA: 0x000EE700 File Offset: 0x000EC900
            public bool DPadRightReleased
            {
                get
                {
                    return this.CurrentState.DPad.Right == ButtonState.Released && this.PreviousState.DPad.Right == ButtonState.Pressed;
                }
            }

            // Token: 0x17000269 RID: 617
            // (get) Token: 0x06001E75 RID: 7797 RVA: 0x000EE740 File Offset: 0x000EC940
            public bool DPadUpCheck
            {
                get
                {
                    return this.CurrentState.DPad.Up == ButtonState.Pressed;
                }
            }

            // Token: 0x1700026A RID: 618
            // (get) Token: 0x06001E76 RID: 7798 RVA: 0x000EE768 File Offset: 0x000EC968
            public bool DPadUpPressed
            {
                get
                {
                    return this.CurrentState.DPad.Up == ButtonState.Pressed && this.PreviousState.DPad.Up == ButtonState.Released;
                }
            }

            // Token: 0x1700026B RID: 619
            // (get) Token: 0x06001E77 RID: 7799 RVA: 0x000EE7AC File Offset: 0x000EC9AC
            public bool DPadUpReleased
            {
                get
                {
                    return this.CurrentState.DPad.Up == ButtonState.Released && this.PreviousState.DPad.Up == ButtonState.Pressed;
                }
            }

            // Token: 0x1700026C RID: 620
            // (get) Token: 0x06001E78 RID: 7800 RVA: 0x000EE7EC File Offset: 0x000EC9EC
            public bool DPadDownCheck
            {
                get
                {
                    return this.CurrentState.DPad.Down == ButtonState.Pressed;
                }
            }

            // Token: 0x1700026D RID: 621
            // (get) Token: 0x06001E79 RID: 7801 RVA: 0x000EE814 File Offset: 0x000ECA14
            public bool DPadDownPressed
            {
                get
                {
                    return this.CurrentState.DPad.Down == ButtonState.Pressed && this.PreviousState.DPad.Down == ButtonState.Released;
                }
            }

            // Token: 0x1700026E RID: 622
            // (get) Token: 0x06001E7A RID: 7802 RVA: 0x000EE858 File Offset: 0x000ECA58
            public bool DPadDownReleased
            {
                get
                {
                    return this.CurrentState.DPad.Down == ButtonState.Released && this.PreviousState.DPad.Down == ButtonState.Pressed;
                }
            }

            // Token: 0x06001E7B RID: 7803 RVA: 0x000EE898 File Offset: 0x000ECA98
            public bool LeftTriggerCheck(float threshold)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.Triggers.Left >= threshold;
            }

            // Token: 0x06001E7C RID: 7804 RVA: 0x000EE8D0 File Offset: 0x000ECAD0
            public bool LeftTriggerPressed(float threshold)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.Triggers.Left >= threshold && this.PreviousState.Triggers.Left < threshold;
            }

            // Token: 0x06001E7D RID: 7805 RVA: 0x000EE920 File Offset: 0x000ECB20
            public bool LeftTriggerReleased(float threshold)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.Triggers.Left < threshold && this.PreviousState.Triggers.Left >= threshold;
            }

            // Token: 0x06001E7E RID: 7806 RVA: 0x000EE974 File Offset: 0x000ECB74
            public bool RightTriggerCheck(float threshold)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.Triggers.Right >= threshold;
            }

            // Token: 0x06001E7F RID: 7807 RVA: 0x000EE9AC File Offset: 0x000ECBAC
            public bool RightTriggerPressed(float threshold)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.Triggers.Right >= threshold && this.PreviousState.Triggers.Right < threshold;
            }

            // Token: 0x06001E80 RID: 7808 RVA: 0x000EE9FC File Offset: 0x000ECBFC
            public bool RightTriggerReleased(float threshold)
            {
                bool disabled = MInput.Disabled;
                return !disabled && this.CurrentState.Triggers.Right < threshold && this.PreviousState.Triggers.Right >= threshold;
            }

            // Token: 0x04001EC3 RID: 7875
            public readonly PlayerIndex PlayerIndex;

            // Token: 0x04001EC4 RID: 7876
            public GamePadState PreviousState;

            // Token: 0x04001EC5 RID: 7877
            public GamePadState CurrentState;

            // Token: 0x04001EC6 RID: 7878
            public bool Attached;

            // Token: 0x04001EC7 RID: 7879
            private float rumbleStrength;

            // Token: 0x04001EC8 RID: 7880
            private float rumbleTime;
        }
    }
}
