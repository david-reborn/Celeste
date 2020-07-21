using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace myd.celeste
{
    // Token: 0x0200010E RID: 270
    //public class VirtualButton : VirtualInput
    //{
    //    // Token: 0x170000C3 RID: 195
    //    // (get) Token: 0x06000858 RID: 2136 RVA: 0x00016D08 File Offset: 0x00014F08
    //    // (set) Token: 0x06000859 RID: 2137 RVA: 0x00016D10 File Offset: 0x00014F10
    //    public bool Repeating { get; private set; }

    //    // Token: 0x0600085A RID: 2138 RVA: 0x00016D19 File Offset: 0x00014F19
    //    public VirtualButton(float bufferTime)
    //    {
    //        this.Nodes = new List<VirtualButton.Node>();
    //        this.BufferTime = bufferTime;
    //    }

    //    // Token: 0x0600085B RID: 2139 RVA: 0x00016D35 File Offset: 0x00014F35
    //    public VirtualButton() : this(0f)
    //    {
    //    }

    //    // Token: 0x0600085C RID: 2140 RVA: 0x00016D44 File Offset: 0x00014F44
    //    public VirtualButton(float bufferTime, params VirtualButton.Node[] nodes)
    //    {
    //        this.Nodes = new List<VirtualButton.Node>(nodes);
    //        this.BufferTime = bufferTime;
    //    }

    //    // Token: 0x0600085D RID: 2141 RVA: 0x00016D61 File Offset: 0x00014F61
    //    public VirtualButton(params VirtualButton.Node[] nodes) : this(0f, nodes)
    //    {
    //    }

    //    // Token: 0x0600085E RID: 2142 RVA: 0x00016D71 File Offset: 0x00014F71
    //    public void SetRepeat(float repeatTime)
    //    {
    //        this.SetRepeat(repeatTime, repeatTime);
    //    }

    //    // Token: 0x0600085F RID: 2143 RVA: 0x00016D80 File Offset: 0x00014F80
    //    public void SetRepeat(float firstRepeatTime, float multiRepeatTime)
    //    {
    //        this.firstRepeatTime = firstRepeatTime;
    //        this.multiRepeatTime = multiRepeatTime;
    //        this.canRepeat = (this.firstRepeatTime > 0f);
    //        bool flag = !this.canRepeat;
    //        if (flag)
    //        {
    //            this.Repeating = false;
    //        }
    //    }

    //    // Token: 0x06000860 RID: 2144 RVA: 0x00016DC4 File Offset: 0x00014FC4
    //    public override void Update()
    //    {
    //        this.consumed = false;
    //        this.bufferCounter -= Time.deltaTime;
    //        bool flag = false;
    //        foreach (VirtualButton.Node node in this.Nodes)
    //        {
    //            node.Update();
    //            bool pressed = node.Pressed;
    //            if (pressed)
    //            {
    //                this.bufferCounter = this.BufferTime;
    //                flag = true;
    //            }
    //            else
    //            {
    //                bool check = node.Check;
    //                if (check)
    //                {
    //                    flag = true;
    //                }
    //            }
    //        }
    //        bool flag2 = !flag;
    //        if (flag2)
    //        {
    //            this.Repeating = false;
    //            this.repeatCounter = 0f;
    //            this.bufferCounter = 0f;
    //        }
    //        else
    //        {
    //            bool flag3 = this.canRepeat;
    //            if (flag3)
    //            {
    //                this.Repeating = false;
    //                bool flag4 = this.repeatCounter == 0f;
    //                if (flag4)
    //                {
    //                    this.repeatCounter = this.firstRepeatTime;
    //                }
    //                else
    //                {
    //                    this.repeatCounter -= Engine.DeltaTime;
    //                    bool flag5 = this.repeatCounter <= 0f;
    //                    if (flag5)
    //                    {
    //                        this.Repeating = true;
    //                        this.repeatCounter = this.multiRepeatTime;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    // Token: 0x170000C4 RID: 196
    //    // (get) Token: 0x06000861 RID: 2145 RVA: 0x00016F04 File Offset: 0x00015104
    //    public bool Check
    //    {
    //        get
    //        {
    //            bool disabled = MInput.Disabled;
    //            bool result;
    //            if (disabled)
    //            {
    //                result = false;
    //            }
    //            else
    //            {
    //                foreach (VirtualButton.Node node in this.Nodes)
    //                {
    //                    bool check = node.Check;
    //                    if (check)
    //                    {
    //                        return true;
    //                    }
    //                }
    //                result = false;
    //            }
    //            return result;
    //        }
    //    }

    //    // Token: 0x170000C5 RID: 197
    //    // (get) Token: 0x06000862 RID: 2146 RVA: 0x00016F78 File Offset: 0x00015178
    //    public bool Pressed
    //    {
    //        get
    //        {
    //            bool flag = this.DebugOverridePressed != null && MInput.Keyboard.Check(this.DebugOverridePressed.Value);
    //            bool result;
    //            if (flag)
    //            {
    //                result = true;
    //            }
    //            else
    //            {
    //                bool disabled = MInput.Disabled;
    //                if (disabled)
    //                {
    //                    result = false;
    //                }
    //                else
    //                {
    //                    bool flag2 = this.consumed;
    //                    if (flag2)
    //                    {
    //                        result = false;
    //                    }
    //                    else
    //                    {
    //                        bool flag3 = this.bufferCounter > 0f || this.Repeating;
    //                        if (flag3)
    //                        {
    //                            result = true;
    //                        }
    //                        else
    //                        {
    //                            foreach (VirtualButton.Node node in this.Nodes)
    //                            {
    //                                bool pressed = node.Pressed;
    //                                if (pressed)
    //                                {
    //                                    return true;
    //                                }
    //                            }
    //                            result = false;
    //                        }
    //                    }
    //                }
    //            }
    //            return result;
    //        }
    //    }

    //    // Token: 0x170000C6 RID: 198
    //    // (get) Token: 0x06000863 RID: 2147 RVA: 0x0001704C File Offset: 0x0001524C
    //    public bool Released
    //    {
    //        get
    //        {
    //            bool disabled = MInput.Disabled;
    //            bool result;
    //            if (disabled)
    //            {
    //                result = false;
    //            }
    //            else
    //            {
    //                foreach (VirtualButton.Node node in this.Nodes)
    //                {
    //                    bool released = node.Released;
    //                    if (released)
    //                    {
    //                        return true;
    //                    }
    //                }
    //                result = false;
    //            }
    //            return result;
    //        }
    //    }

    //    // Token: 0x06000864 RID: 2148 RVA: 0x000170C0 File Offset: 0x000152C0
    //    public void ConsumeBuffer()
    //    {
    //        this.bufferCounter = 0f;
    //    }

    //    // Token: 0x06000865 RID: 2149 RVA: 0x000170CE File Offset: 0x000152CE
    //    public void ConsumePress()
    //    {
    //        this.bufferCounter = 0f;
    //        this.consumed = true;
    //    }

    //    // Token: 0x06000866 RID: 2150 RVA: 0x000170E4 File Offset: 0x000152E4
    //    public static implicit operator bool(VirtualButton button)
    //    {
    //        return button.Check;
    //    }

    //    // Token: 0x0400058F RID: 1423
    //    public List<VirtualButton.Node> Nodes;

    //    // Token: 0x04000590 RID: 1424
    //    public float BufferTime;

    //    // Token: 0x04000592 RID: 1426
    //    public Keys? DebugOverridePressed;

    //    // Token: 0x04000593 RID: 1427
    //    private float firstRepeatTime;

    //    // Token: 0x04000594 RID: 1428
    //    private float multiRepeatTime;

    //    // Token: 0x04000595 RID: 1429
    //    private float bufferCounter;

    //    // Token: 0x04000596 RID: 1430
    //    private float repeatCounter;

    //    // Token: 0x04000597 RID: 1431
    //    private bool canRepeat;

    //    // Token: 0x04000598 RID: 1432
    //    private bool consumed;

    //    // Token: 0x020003AA RID: 938
    //    public abstract class Node : VirtualInputNode
    //    {
    //        // Token: 0x17000277 RID: 631
    //        // (get) Token: 0x06001E92 RID: 7826
    //        public abstract bool Check { get; }

    //        // Token: 0x17000278 RID: 632
    //        // (get) Token: 0x06001E93 RID: 7827
    //        public abstract bool Pressed { get; }

    //        // Token: 0x17000279 RID: 633
    //        // (get) Token: 0x06001E94 RID: 7828
    //        public abstract bool Released { get; }
    //    }

    //    // Token: 0x020003AB RID: 939
    //    public class KeyboardKey : VirtualButton.Node
    //    {
    //        // Token: 0x06001E96 RID: 7830 RVA: 0x000EED70 File Offset: 0x000ECF70
    //        public KeyboardKey(Keys key)
    //        {
    //            this.Key = key;
    //        }

    //        // Token: 0x1700027A RID: 634
    //        // (get) Token: 0x06001E97 RID: 7831 RVA: 0x000EED84 File Offset: 0x000ECF84
    //        public override bool Check
    //        {
    //            get
    //            {
    //                return MInput.Keyboard.Check(this.Key);
    //            }
    //        }

    //        // Token: 0x1700027B RID: 635
    //        // (get) Token: 0x06001E98 RID: 7832 RVA: 0x000EEDA8 File Offset: 0x000ECFA8
    //        public override bool Pressed
    //        {
    //            get
    //            {
    //                return MInput.Keyboard.Pressed(this.Key);
    //            }
    //        }

    //        // Token: 0x1700027C RID: 636
    //        // (get) Token: 0x06001E99 RID: 7833 RVA: 0x000EEDCC File Offset: 0x000ECFCC
    //        public override bool Released
    //        {
    //            get
    //            {
    //                return MInput.Keyboard.Released(this.Key);
    //            }
    //        }

    //        // Token: 0x04001ED8 RID: 7896
    //        public Keys Key;
    //    }

       
    //    // Token: 0x020003BB RID: 955
    //    public class MouseLeftButton : VirtualButton.Node
    //    {
    //        // Token: 0x170002AA RID: 682
    //        // (get) Token: 0x06001ED6 RID: 7894 RVA: 0x000EF694 File Offset: 0x000ED894
    //        public override bool Check
    //        {
    //            get
    //            {
    //                return MInput.Mouse.CheckLeftButton;
    //            }
    //        }

    //        // Token: 0x170002AB RID: 683
    //        // (get) Token: 0x06001ED7 RID: 7895 RVA: 0x000EF6B0 File Offset: 0x000ED8B0
    //        public override bool Pressed
    //        {
    //            get
    //            {
    //                return MInput.Mouse.PressedLeftButton;
    //            }
    //        }

    //        // Token: 0x170002AC RID: 684
    //        // (get) Token: 0x06001ED8 RID: 7896 RVA: 0x000EF6CC File Offset: 0x000ED8CC
    //        public override bool Released
    //        {
    //            get
    //            {
    //                return MInput.Mouse.ReleasedLeftButton;
    //            }
    //        }
    //    }

    //    // Token: 0x020003BC RID: 956
    //    public class MouseRightButton : VirtualButton.Node
    //    {
    //        // Token: 0x170002AD RID: 685
    //        // (get) Token: 0x06001EDA RID: 7898 RVA: 0x000EF6F4 File Offset: 0x000ED8F4
    //        public override bool Check
    //        {
    //            get
    //            {
    //                return MInput.Mouse.CheckRightButton;
    //            }
    //        }

    //        // Token: 0x170002AE RID: 686
    //        // (get) Token: 0x06001EDB RID: 7899 RVA: 0x000EF710 File Offset: 0x000ED910
    //        public override bool Pressed
    //        {
    //            get
    //            {
    //                return MInput.Mouse.PressedRightButton;
    //            }
    //        }

    //        // Token: 0x170002AF RID: 687
    //        // (get) Token: 0x06001EDC RID: 7900 RVA: 0x000EF72C File Offset: 0x000ED92C
    //        public override bool Released
    //        {
    //            get
    //            {
    //                return MInput.Mouse.ReleasedRightButton;
    //            }
    //        }
    //    }

    //    // Token: 0x020003BD RID: 957
    //    public class MouseMiddleButton : VirtualButton.Node
    //    {
    //        // Token: 0x170002B0 RID: 688
    //        // (get) Token: 0x06001EDE RID: 7902 RVA: 0x000EF748 File Offset: 0x000ED948
    //        public override bool Check
    //        {
    //            get
    //            {
    //                return MInput.Mouse.CheckMiddleButton;
    //            }
    //        }

    //        // Token: 0x170002B1 RID: 689
    //        // (get) Token: 0x06001EDF RID: 7903 RVA: 0x000EF764 File Offset: 0x000ED964
    //        public override bool Pressed
    //        {
    //            get
    //            {
    //                return MInput.Mouse.PressedMiddleButton;
    //            }
    //        }

    //        // Token: 0x170002B2 RID: 690
    //        // (get) Token: 0x06001EE0 RID: 7904 RVA: 0x000EF780 File Offset: 0x000ED980
    //        public override bool Released
    //        {
    //            get
    //            {
    //                return MInput.Mouse.ReleasedMiddleButton;
    //            }
    //        }
    //    }

    //    //// Token: 0x020003BE RID: 958
    //    //public class VirtualAxisTrigger : VirtualButton.Node
    //    //{
    //    //    // Token: 0x06001EE2 RID: 7906 RVA: 0x000EF79C File Offset: 0x000ED99C
    //    //    public VirtualAxisTrigger(VirtualAxis axis, VirtualInput.ThresholdModes mode, float threshold)
    //    //    {
    //    //        this.axis = axis;
    //    //        this.Mode = mode;
    //    //        this.Threshold = threshold;
    //    //    }

    //    //    // Token: 0x170002B3 RID: 691
    //    //    // (get) Token: 0x06001EE3 RID: 7907 RVA: 0x000EF7BC File Offset: 0x000ED9BC
    //    //    public override bool Check
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.axis.Value >= this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.axis.Value <= this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.axis.Value == this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x170002B4 RID: 692
    //    //    // (get) Token: 0x06001EE4 RID: 7908 RVA: 0x000EF830 File Offset: 0x000EDA30
    //    //    public override bool Pressed
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.axis.Value >= this.Threshold && this.axis.PreviousValue < this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.axis.Value <= this.Threshold && this.axis.PreviousValue > this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.axis.Value == this.Threshold && this.axis.PreviousValue != this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x170002B5 RID: 693
    //    //    // (get) Token: 0x06001EE5 RID: 7909 RVA: 0x000EF8E0 File Offset: 0x000EDAE0
    //    //    public override bool Released
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.axis.Value < this.Threshold && this.axis.PreviousValue >= this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.axis.Value > this.Threshold && this.axis.PreviousValue <= this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.axis.Value != this.Threshold && this.axis.PreviousValue == this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x04001EF3 RID: 7923
    //    //    public VirtualInput.ThresholdModes Mode;

    //    //    // Token: 0x04001EF4 RID: 7924
    //    //    public float Threshold;

    //    //    // Token: 0x04001EF5 RID: 7925
    //    //    private VirtualAxis axis;

    //    //    // Token: 0x02000799 RID: 1945
    //    //    public enum Modes
    //    //    {
    //    //        // Token: 0x040031B2 RID: 12722
    //    //        LargerThan,
    //    //        // Token: 0x040031B3 RID: 12723
    //    //        LessThan,
    //    //        // Token: 0x040031B4 RID: 12724
    //    //        Equals
    //    //    }
    //    //}

    //    //// Token: 0x020003BF RID: 959
    //    //public class VirtualIntegerAxisTrigger : VirtualButton.Node
    //    //{
    //    //    // Token: 0x06001EE6 RID: 7910 RVA: 0x000EF993 File Offset: 0x000EDB93
    //    //    public VirtualIntegerAxisTrigger(VirtualIntegerAxis axis, VirtualInput.ThresholdModes mode, int threshold)
    //    //    {
    //    //        this.axis = axis;
    //    //        this.Mode = mode;
    //    //        this.Threshold = threshold;
    //    //    }

    //    //    // Token: 0x170002B6 RID: 694
    //    //    // (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000EF9B4 File Offset: 0x000EDBB4
    //    //    public override bool Check
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.axis.Value >= this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.axis.Value <= this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.axis.Value == this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x170002B7 RID: 695
    //    //    // (get) Token: 0x06001EE8 RID: 7912 RVA: 0x000EFA28 File Offset: 0x000EDC28
    //    //    public override bool Pressed
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.axis.Value >= this.Threshold && this.axis.PreviousValue < this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.axis.Value <= this.Threshold && this.axis.PreviousValue > this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.axis.Value == this.Threshold && this.axis.PreviousValue != this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x170002B8 RID: 696
    //    //    // (get) Token: 0x06001EE9 RID: 7913 RVA: 0x000EFAD8 File Offset: 0x000EDCD8
    //    //    public override bool Released
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.axis.Value < this.Threshold && this.axis.PreviousValue >= this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.axis.Value > this.Threshold && this.axis.PreviousValue <= this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.axis.Value != this.Threshold && this.axis.PreviousValue == this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x04001EF6 RID: 7926
    //    //    public VirtualInput.ThresholdModes Mode;

    //    //    // Token: 0x04001EF7 RID: 7927
    //    //    public int Threshold;

    //    //    // Token: 0x04001EF8 RID: 7928
    //    //    private VirtualIntegerAxis axis;

    //    //    // Token: 0x0200079A RID: 1946
    //    //    public enum Modes
    //    //    {
    //    //        // Token: 0x040031B6 RID: 12726
    //    //        LargerThan,
    //    //        // Token: 0x040031B7 RID: 12727
    //    //        LessThan,
    //    //        // Token: 0x040031B8 RID: 12728
    //    //        Equals
    //    //    }
    //    //}

    //    //// Token: 0x020003C0 RID: 960
    //    //public class VirtualJoystickXTrigger : VirtualButton.Node
    //    //{
    //    //    // Token: 0x06001EEA RID: 7914 RVA: 0x000EFB8B File Offset: 0x000EDD8B
    //    //    public VirtualJoystickXTrigger(VirtualJoystick joystick, VirtualInput.ThresholdModes mode, float threshold)
    //    //    {
    //    //        this.joystick = joystick;
    //    //        this.Mode = mode;
    //    //        this.Threshold = threshold;
    //    //    }

    //    //    // Token: 0x170002B9 RID: 697
    //    //    // (get) Token: 0x06001EEB RID: 7915 RVA: 0x000EFBAC File Offset: 0x000EDDAC
    //    //    public override bool Check
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.joystick.Value.X >= this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.joystick.Value.X <= this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.joystick.Value.X == this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x170002BA RID: 698
    //    //    // (get) Token: 0x06001EEC RID: 7916 RVA: 0x000EFC2C File Offset: 0x000EDE2C
    //    //    public override bool Pressed
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.joystick.Value.X >= this.Threshold && this.joystick.PreviousValue.X < this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.joystick.Value.X <= this.Threshold && this.joystick.PreviousValue.X > this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.joystick.Value.X == this.Threshold && this.joystick.PreviousValue.X != this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x170002BB RID: 699
    //    //    // (get) Token: 0x06001EED RID: 7917 RVA: 0x000EFCFC File Offset: 0x000EDEFC
    //    //    public override bool Released
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.joystick.Value.X < this.Threshold && this.joystick.PreviousValue.X >= this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.joystick.Value.X > this.Threshold && this.joystick.PreviousValue.X <= this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.joystick.Value.X != this.Threshold && this.joystick.PreviousValue.X == this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x04001EF9 RID: 7929
    //    //    public VirtualInput.ThresholdModes Mode;

    //    //    // Token: 0x04001EFA RID: 7930
    //    //    public float Threshold;

    //    //    // Token: 0x04001EFB RID: 7931
    //    //    private VirtualJoystick joystick;

    //    //    // Token: 0x0200079B RID: 1947
    //    //    public enum Modes
    //    //    {
    //    //        // Token: 0x040031BA RID: 12730
    //    //        LargerThan,
    //    //        // Token: 0x040031BB RID: 12731
    //    //        LessThan,
    //    //        // Token: 0x040031BC RID: 12732
    //    //        Equals
    //    //    }
    //    //}

    //    //// Token: 0x020003C1 RID: 961
    //    //public class VirtualJoystickYTrigger : VirtualButton.Node
    //    //{
    //    //    // Token: 0x06001EEE RID: 7918 RVA: 0x000EFDCD File Offset: 0x000EDFCD
    //    //    public VirtualJoystickYTrigger(VirtualJoystick joystick, VirtualInput.ThresholdModes mode, float threshold)
    //    //    {
    //    //        this.joystick = joystick;
    //    //        this.Mode = mode;
    //    //        this.Threshold = threshold;
    //    //    }

    //    //    // Token: 0x170002BC RID: 700
    //    //    // (get) Token: 0x06001EEF RID: 7919 RVA: 0x000EFDEC File Offset: 0x000EDFEC
    //    //    public override bool Check
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.joystick.Value.X >= this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.joystick.Value.X <= this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.joystick.Value.X == this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x170002BD RID: 701
    //    //    // (get) Token: 0x06001EF0 RID: 7920 RVA: 0x000EFE6C File Offset: 0x000EE06C
    //    //    public override bool Pressed
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.joystick.Value.X >= this.Threshold && this.joystick.PreviousValue.X < this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.joystick.Value.X <= this.Threshold && this.joystick.PreviousValue.X > this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.joystick.Value.X == this.Threshold && this.joystick.PreviousValue.X != this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x170002BE RID: 702
    //    //    // (get) Token: 0x06001EF1 RID: 7921 RVA: 0x000EFF3C File Offset: 0x000EE13C
    //    //    public override bool Released
    //    //    {
    //    //        get
    //    //        {
    //    //            bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
    //    //            bool result;
    //    //            if (flag)
    //    //            {
    //    //                result = (this.joystick.Value.X < this.Threshold && this.joystick.PreviousValue.X >= this.Threshold);
    //    //            }
    //    //            else
    //    //            {
    //    //                bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
    //    //                if (flag2)
    //    //                {
    //    //                    result = (this.joystick.Value.X > this.Threshold && this.joystick.PreviousValue.X <= this.Threshold);
    //    //                }
    //    //                else
    //    //                {
    //    //                    result = (this.joystick.Value.X != this.Threshold && this.joystick.PreviousValue.X == this.Threshold);
    //    //                }
    //    //            }
    //    //            return result;
    //    //        }
    //    //    }

    //    //    // Token: 0x04001EFC RID: 7932
    //    //    public VirtualInput.ThresholdModes Mode;

    //    //    // Token: 0x04001EFD RID: 7933
    //    //    public float Threshold;

    //    //    // Token: 0x04001EFE RID: 7934
    //    //    private VirtualJoystick joystick;
    //    //}
    //}
}