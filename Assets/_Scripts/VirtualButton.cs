using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace myd.celeste
{
    // Token: 0x0200010E RID: 270
    public class VirtualButton : VirtualInput
    {
        // Token: 0x170000C3 RID: 195
        // (get) Token: 0x06000858 RID: 2136 RVA: 0x00016D08 File Offset: 0x00014F08
        // (set) Token: 0x06000859 RID: 2137 RVA: 0x00016D10 File Offset: 0x00014F10
        public bool Repeating { get; private set; }

        // Token: 0x0600085A RID: 2138 RVA: 0x00016D19 File Offset: 0x00014F19
        public VirtualButton(float bufferTime)
        {
            this.Nodes = new List<VirtualButton.Node>();
            this.BufferTime = bufferTime;
        }

        // Token: 0x0600085B RID: 2139 RVA: 0x00016D35 File Offset: 0x00014F35
        public VirtualButton() : this(0f)
        {
        }

        // Token: 0x0600085C RID: 2140 RVA: 0x00016D44 File Offset: 0x00014F44
        public VirtualButton(float bufferTime, params VirtualButton.Node[] nodes)
        {
            this.Nodes = new List<VirtualButton.Node>(nodes);
            this.BufferTime = bufferTime;
        }

        // Token: 0x0600085D RID: 2141 RVA: 0x00016D61 File Offset: 0x00014F61
        public VirtualButton(params VirtualButton.Node[] nodes) : this(0f, nodes)
        {
        }

        // Token: 0x0600085E RID: 2142 RVA: 0x00016D71 File Offset: 0x00014F71
        public void SetRepeat(float repeatTime)
        {
            this.SetRepeat(repeatTime, repeatTime);
        }

        // Token: 0x0600085F RID: 2143 RVA: 0x00016D80 File Offset: 0x00014F80
        public void SetRepeat(float firstRepeatTime, float multiRepeatTime)
        {
            this.firstRepeatTime = firstRepeatTime;
            this.multiRepeatTime = multiRepeatTime;
            this.canRepeat = (this.firstRepeatTime > 0f);
            bool flag = !this.canRepeat;
            if (flag)
            {
                this.Repeating = false;
            }
        }

        // Token: 0x06000860 RID: 2144 RVA: 0x00016DC4 File Offset: 0x00014FC4
        public override void Update()
        {
            this.consumed = false;
            this.bufferCounter -= Time.deltaTime;
            bool flag = false;
            foreach (VirtualButton.Node node in this.Nodes)
            {
                node.Update();
                bool pressed = node.Pressed;
                if (pressed)
                {
                    this.bufferCounter = this.BufferTime;
                    flag = true;
                }
                else
                {
                    bool check = node.Check;
                    if (check)
                    {
                        flag = true;
                    }
                }
            }
            bool flag2 = !flag;
            if (flag2)
            {
                this.Repeating = false;
                this.repeatCounter = 0f;
                this.bufferCounter = 0f;
            }
            else
            {
                bool flag3 = this.canRepeat;
                if (flag3)
                {
                    this.Repeating = false;
                    bool flag4 = this.repeatCounter == 0f;
                    if (flag4)
                    {
                        this.repeatCounter = this.firstRepeatTime;
                    }
                    else
                    {
                        this.repeatCounter -= Engine.DeltaTime;
                        bool flag5 = this.repeatCounter <= 0f;
                        if (flag5)
                        {
                            this.Repeating = true;
                            this.repeatCounter = this.multiRepeatTime;
                        }
                    }
                }
            }
        }

        // Token: 0x170000C4 RID: 196
        // (get) Token: 0x06000861 RID: 2145 RVA: 0x00016F04 File Offset: 0x00015104
        public bool Check
        {
            get
            {
                bool disabled = MInput.Disabled;
                bool result;
                if (disabled)
                {
                    result = false;
                }
                else
                {
                    foreach (VirtualButton.Node node in this.Nodes)
                    {
                        bool check = node.Check;
                        if (check)
                        {
                            return true;
                        }
                    }
                    result = false;
                }
                return result;
            }
        }

        // Token: 0x170000C5 RID: 197
        // (get) Token: 0x06000862 RID: 2146 RVA: 0x00016F78 File Offset: 0x00015178
        public bool Pressed
        {
            get
            {
                bool flag = this.DebugOverridePressed != null && MInput.Keyboard.Check(this.DebugOverridePressed.Value);
                bool result;
                if (flag)
                {
                    result = true;
                }
                else
                {
                    bool disabled = MInput.Disabled;
                    if (disabled)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag2 = this.consumed;
                        if (flag2)
                        {
                            result = false;
                        }
                        else
                        {
                            bool flag3 = this.bufferCounter > 0f || this.Repeating;
                            if (flag3)
                            {
                                result = true;
                            }
                            else
                            {
                                foreach (VirtualButton.Node node in this.Nodes)
                                {
                                    bool pressed = node.Pressed;
                                    if (pressed)
                                    {
                                        return true;
                                    }
                                }
                                result = false;
                            }
                        }
                    }
                }
                return result;
            }
        }

        // Token: 0x170000C6 RID: 198
        // (get) Token: 0x06000863 RID: 2147 RVA: 0x0001704C File Offset: 0x0001524C
        public bool Released
        {
            get
            {
                bool disabled = MInput.Disabled;
                bool result;
                if (disabled)
                {
                    result = false;
                }
                else
                {
                    foreach (VirtualButton.Node node in this.Nodes)
                    {
                        bool released = node.Released;
                        if (released)
                        {
                            return true;
                        }
                    }
                    result = false;
                }
                return result;
            }
        }

        // Token: 0x06000864 RID: 2148 RVA: 0x000170C0 File Offset: 0x000152C0
        public void ConsumeBuffer()
        {
            this.bufferCounter = 0f;
        }

        // Token: 0x06000865 RID: 2149 RVA: 0x000170CE File Offset: 0x000152CE
        public void ConsumePress()
        {
            this.bufferCounter = 0f;
            this.consumed = true;
        }

        // Token: 0x06000866 RID: 2150 RVA: 0x000170E4 File Offset: 0x000152E4
        public static implicit operator bool(VirtualButton button)
        {
            return button.Check;
        }

        // Token: 0x0400058F RID: 1423
        public List<VirtualButton.Node> Nodes;

        // Token: 0x04000590 RID: 1424
        public float BufferTime;

        // Token: 0x04000592 RID: 1426
        public Keys? DebugOverridePressed;

        // Token: 0x04000593 RID: 1427
        private float firstRepeatTime;

        // Token: 0x04000594 RID: 1428
        private float multiRepeatTime;

        // Token: 0x04000595 RID: 1429
        private float bufferCounter;

        // Token: 0x04000596 RID: 1430
        private float repeatCounter;

        // Token: 0x04000597 RID: 1431
        private bool canRepeat;

        // Token: 0x04000598 RID: 1432
        private bool consumed;

        // Token: 0x020003AA RID: 938
        public abstract class Node : VirtualInputNode
        {
            // Token: 0x17000277 RID: 631
            // (get) Token: 0x06001E92 RID: 7826
            public abstract bool Check { get; }

            // Token: 0x17000278 RID: 632
            // (get) Token: 0x06001E93 RID: 7827
            public abstract bool Pressed { get; }

            // Token: 0x17000279 RID: 633
            // (get) Token: 0x06001E94 RID: 7828
            public abstract bool Released { get; }
        }

        // Token: 0x020003AB RID: 939
        public class KeyboardKey : VirtualButton.Node
        {
            // Token: 0x06001E96 RID: 7830 RVA: 0x000EED70 File Offset: 0x000ECF70
            public KeyboardKey(Keys key)
            {
                this.Key = key;
            }

            // Token: 0x1700027A RID: 634
            // (get) Token: 0x06001E97 RID: 7831 RVA: 0x000EED84 File Offset: 0x000ECF84
            public override bool Check
            {
                get
                {
                    return MInput.Keyboard.Check(this.Key);
                }
            }

            // Token: 0x1700027B RID: 635
            // (get) Token: 0x06001E98 RID: 7832 RVA: 0x000EEDA8 File Offset: 0x000ECFA8
            public override bool Pressed
            {
                get
                {
                    return MInput.Keyboard.Pressed(this.Key);
                }
            }

            // Token: 0x1700027C RID: 636
            // (get) Token: 0x06001E99 RID: 7833 RVA: 0x000EEDCC File Offset: 0x000ECFCC
            public override bool Released
            {
                get
                {
                    return MInput.Keyboard.Released(this.Key);
                }
            }

            // Token: 0x04001ED8 RID: 7896
            public Keys Key;
        }

        // Token: 0x020003AC RID: 940
        public class PadButton : VirtualButton.Node
        {
            // Token: 0x06001E9A RID: 7834 RVA: 0x000EEDEE File Offset: 0x000ECFEE
            public PadButton(int gamepadIndex, Buttons button)
            {
                this.GamepadIndex = gamepadIndex;
                this.Button = button;
            }

            // Token: 0x1700027D RID: 637
            // (get) Token: 0x06001E9B RID: 7835 RVA: 0x000EEE08 File Offset: 0x000ED008
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].Check(this.Button);
                }
            }

            // Token: 0x1700027E RID: 638
            // (get) Token: 0x06001E9C RID: 7836 RVA: 0x000EEE34 File Offset: 0x000ED034
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].Pressed(this.Button);
                }
            }

            // Token: 0x1700027F RID: 639
            // (get) Token: 0x06001E9D RID: 7837 RVA: 0x000EEE60 File Offset: 0x000ED060
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].Released(this.Button);
                }
            }

            // Token: 0x04001ED9 RID: 7897
            public int GamepadIndex;

            // Token: 0x04001EDA RID: 7898
            public Buttons Button;
        }

        // Token: 0x020003AD RID: 941
        public class PadLeftStickRight : VirtualButton.Node
        {
            // Token: 0x06001E9E RID: 7838 RVA: 0x000EEE89 File Offset: 0x000ED089
            public PadLeftStickRight(int gamepadindex, float deadzone)
            {
                this.GamepadIndex = gamepadindex;
                this.Deadzone = deadzone;
            }

            // Token: 0x17000280 RID: 640
            // (get) Token: 0x06001E9F RID: 7839 RVA: 0x000EEEA4 File Offset: 0x000ED0A4
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickRightCheck(this.Deadzone);
                }
            }

            // Token: 0x17000281 RID: 641
            // (get) Token: 0x06001EA0 RID: 7840 RVA: 0x000EEED0 File Offset: 0x000ED0D0
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickRightPressed(this.Deadzone);
                }
            }

            // Token: 0x17000282 RID: 642
            // (get) Token: 0x06001EA1 RID: 7841 RVA: 0x000EEEFC File Offset: 0x000ED0FC
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickRightReleased(this.Deadzone);
                }
            }

            // Token: 0x04001EDB RID: 7899
            public int GamepadIndex;

            // Token: 0x04001EDC RID: 7900
            public float Deadzone;
        }

        // Token: 0x020003AE RID: 942
        public class PadLeftStickLeft : VirtualButton.Node
        {
            // Token: 0x06001EA2 RID: 7842 RVA: 0x000EEF25 File Offset: 0x000ED125
            public PadLeftStickLeft(int gamepadindex, float deadzone)
            {
                this.GamepadIndex = gamepadindex;
                this.Deadzone = deadzone;
            }

            // Token: 0x17000283 RID: 643
            // (get) Token: 0x06001EA3 RID: 7843 RVA: 0x000EEF40 File Offset: 0x000ED140
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickLeftCheck(this.Deadzone);
                }
            }

            // Token: 0x17000284 RID: 644
            // (get) Token: 0x06001EA4 RID: 7844 RVA: 0x000EEF6C File Offset: 0x000ED16C
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickLeftPressed(this.Deadzone);
                }
            }

            // Token: 0x17000285 RID: 645
            // (get) Token: 0x06001EA5 RID: 7845 RVA: 0x000EEF98 File Offset: 0x000ED198
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickLeftReleased(this.Deadzone);
                }
            }

            // Token: 0x04001EDD RID: 7901
            public int GamepadIndex;

            // Token: 0x04001EDE RID: 7902
            public float Deadzone;
        }

        // Token: 0x020003AF RID: 943
        public class PadLeftStickUp : VirtualButton.Node
        {
            // Token: 0x06001EA6 RID: 7846 RVA: 0x000EEFC1 File Offset: 0x000ED1C1
            public PadLeftStickUp(int gamepadindex, float deadzone)
            {
                this.GamepadIndex = gamepadindex;
                this.Deadzone = deadzone;
            }

            // Token: 0x17000286 RID: 646
            // (get) Token: 0x06001EA7 RID: 7847 RVA: 0x000EEFDC File Offset: 0x000ED1DC
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickUpCheck(this.Deadzone);
                }
            }

            // Token: 0x17000287 RID: 647
            // (get) Token: 0x06001EA8 RID: 7848 RVA: 0x000EF008 File Offset: 0x000ED208
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickUpPressed(this.Deadzone);
                }
            }

            // Token: 0x17000288 RID: 648
            // (get) Token: 0x06001EA9 RID: 7849 RVA: 0x000EF034 File Offset: 0x000ED234
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickUpReleased(this.Deadzone);
                }
            }

            // Token: 0x04001EDF RID: 7903
            public int GamepadIndex;

            // Token: 0x04001EE0 RID: 7904
            public float Deadzone;
        }

        // Token: 0x020003B0 RID: 944
        public class PadLeftStickDown : VirtualButton.Node
        {
            // Token: 0x06001EAA RID: 7850 RVA: 0x000EF05D File Offset: 0x000ED25D
            public PadLeftStickDown(int gamepadindex, float deadzone)
            {
                this.GamepadIndex = gamepadindex;
                this.Deadzone = deadzone;
            }

            // Token: 0x17000289 RID: 649
            // (get) Token: 0x06001EAB RID: 7851 RVA: 0x000EF078 File Offset: 0x000ED278
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickDownCheck(this.Deadzone);
                }
            }

            // Token: 0x1700028A RID: 650
            // (get) Token: 0x06001EAC RID: 7852 RVA: 0x000EF0A4 File Offset: 0x000ED2A4
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickDownPressed(this.Deadzone);
                }
            }

            // Token: 0x1700028B RID: 651
            // (get) Token: 0x06001EAD RID: 7853 RVA: 0x000EF0D0 File Offset: 0x000ED2D0
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftStickDownReleased(this.Deadzone);
                }
            }

            // Token: 0x04001EE1 RID: 7905
            public int GamepadIndex;

            // Token: 0x04001EE2 RID: 7906
            public float Deadzone;
        }

        // Token: 0x020003B1 RID: 945
        public class PadRightStickRight : VirtualButton.Node
        {
            // Token: 0x06001EAE RID: 7854 RVA: 0x000EF0F9 File Offset: 0x000ED2F9
            public PadRightStickRight(int gamepadindex, float deadzone)
            {
                this.GamepadIndex = gamepadindex;
                this.Deadzone = deadzone;
            }

            // Token: 0x1700028C RID: 652
            // (get) Token: 0x06001EAF RID: 7855 RVA: 0x000EF114 File Offset: 0x000ED314
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickRightCheck(this.Deadzone);
                }
            }

            // Token: 0x1700028D RID: 653
            // (get) Token: 0x06001EB0 RID: 7856 RVA: 0x000EF140 File Offset: 0x000ED340
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickRightPressed(this.Deadzone);
                }
            }

            // Token: 0x1700028E RID: 654
            // (get) Token: 0x06001EB1 RID: 7857 RVA: 0x000EF16C File Offset: 0x000ED36C
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickRightReleased(this.Deadzone);
                }
            }

            // Token: 0x04001EE3 RID: 7907
            public int GamepadIndex;

            // Token: 0x04001EE4 RID: 7908
            public float Deadzone;
        }

        // Token: 0x020003B2 RID: 946
        public class PadRightStickLeft : VirtualButton.Node
        {
            // Token: 0x06001EB2 RID: 7858 RVA: 0x000EF195 File Offset: 0x000ED395
            public PadRightStickLeft(int gamepadindex, float deadzone)
            {
                this.GamepadIndex = gamepadindex;
                this.Deadzone = deadzone;
            }

            // Token: 0x1700028F RID: 655
            // (get) Token: 0x06001EB3 RID: 7859 RVA: 0x000EF1B0 File Offset: 0x000ED3B0
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickLeftCheck(this.Deadzone);
                }
            }

            // Token: 0x17000290 RID: 656
            // (get) Token: 0x06001EB4 RID: 7860 RVA: 0x000EF1DC File Offset: 0x000ED3DC
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickLeftPressed(this.Deadzone);
                }
            }

            // Token: 0x17000291 RID: 657
            // (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000EF208 File Offset: 0x000ED408
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickLeftReleased(this.Deadzone);
                }
            }

            // Token: 0x04001EE5 RID: 7909
            public int GamepadIndex;

            // Token: 0x04001EE6 RID: 7910
            public float Deadzone;
        }

        // Token: 0x020003B3 RID: 947
        public class PadRightStickUp : VirtualButton.Node
        {
            // Token: 0x06001EB6 RID: 7862 RVA: 0x000EF231 File Offset: 0x000ED431
            public PadRightStickUp(int gamepadindex, float deadzone)
            {
                this.GamepadIndex = gamepadindex;
                this.Deadzone = deadzone;
            }

            // Token: 0x17000292 RID: 658
            // (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000EF24C File Offset: 0x000ED44C
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickUpCheck(this.Deadzone);
                }
            }

            // Token: 0x17000293 RID: 659
            // (get) Token: 0x06001EB8 RID: 7864 RVA: 0x000EF278 File Offset: 0x000ED478
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickUpPressed(this.Deadzone);
                }
            }

            // Token: 0x17000294 RID: 660
            // (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000EF2A4 File Offset: 0x000ED4A4
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickUpReleased(this.Deadzone);
                }
            }

            // Token: 0x04001EE7 RID: 7911
            public int GamepadIndex;

            // Token: 0x04001EE8 RID: 7912
            public float Deadzone;
        }

        // Token: 0x020003B4 RID: 948
        public class PadRightStickDown : VirtualButton.Node
        {
            // Token: 0x06001EBA RID: 7866 RVA: 0x000EF2CD File Offset: 0x000ED4CD
            public PadRightStickDown(int gamepadindex, float deadzone)
            {
                this.GamepadIndex = gamepadindex;
                this.Deadzone = deadzone;
            }

            // Token: 0x17000295 RID: 661
            // (get) Token: 0x06001EBB RID: 7867 RVA: 0x000EF2E8 File Offset: 0x000ED4E8
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickDownCheck(this.Deadzone);
                }
            }

            // Token: 0x17000296 RID: 662
            // (get) Token: 0x06001EBC RID: 7868 RVA: 0x000EF314 File Offset: 0x000ED514
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickDownPressed(this.Deadzone);
                }
            }

            // Token: 0x17000297 RID: 663
            // (get) Token: 0x06001EBD RID: 7869 RVA: 0x000EF340 File Offset: 0x000ED540
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightStickDownReleased(this.Deadzone);
                }
            }

            // Token: 0x04001EE9 RID: 7913
            public int GamepadIndex;

            // Token: 0x04001EEA RID: 7914
            public float Deadzone;
        }

        // Token: 0x020003B5 RID: 949
        public class PadLeftTrigger : VirtualButton.Node
        {
            // Token: 0x06001EBE RID: 7870 RVA: 0x000EF369 File Offset: 0x000ED569
            public PadLeftTrigger(int gamepadIndex, float threshold)
            {
                this.GamepadIndex = gamepadIndex;
                this.Threshold = threshold;
            }

            // Token: 0x17000298 RID: 664
            // (get) Token: 0x06001EBF RID: 7871 RVA: 0x000EF384 File Offset: 0x000ED584
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftTriggerCheck(this.Threshold);
                }
            }

            // Token: 0x17000299 RID: 665
            // (get) Token: 0x06001EC0 RID: 7872 RVA: 0x000EF3B0 File Offset: 0x000ED5B0
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftTriggerPressed(this.Threshold);
                }
            }

            // Token: 0x1700029A RID: 666
            // (get) Token: 0x06001EC1 RID: 7873 RVA: 0x000EF3DC File Offset: 0x000ED5DC
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].LeftTriggerReleased(this.Threshold);
                }
            }

            // Token: 0x04001EEB RID: 7915
            public int GamepadIndex;

            // Token: 0x04001EEC RID: 7916
            public float Threshold;
        }

        // Token: 0x020003B6 RID: 950
        public class PadRightTrigger : VirtualButton.Node
        {
            // Token: 0x06001EC2 RID: 7874 RVA: 0x000EF405 File Offset: 0x000ED605
            public PadRightTrigger(int gamepadIndex, float threshold)
            {
                this.GamepadIndex = gamepadIndex;
                this.Threshold = threshold;
            }

            // Token: 0x1700029B RID: 667
            // (get) Token: 0x06001EC3 RID: 7875 RVA: 0x000EF420 File Offset: 0x000ED620
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightTriggerCheck(this.Threshold);
                }
            }

            // Token: 0x1700029C RID: 668
            // (get) Token: 0x06001EC4 RID: 7876 RVA: 0x000EF44C File Offset: 0x000ED64C
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightTriggerPressed(this.Threshold);
                }
            }

            // Token: 0x1700029D RID: 669
            // (get) Token: 0x06001EC5 RID: 7877 RVA: 0x000EF478 File Offset: 0x000ED678
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].RightTriggerReleased(this.Threshold);
                }
            }

            // Token: 0x04001EED RID: 7917
            public int GamepadIndex;

            // Token: 0x04001EEE RID: 7918
            public float Threshold;
        }

        // Token: 0x020003B7 RID: 951
        public class PadDPadRight : VirtualButton.Node
        {
            // Token: 0x06001EC6 RID: 7878 RVA: 0x000EF4A1 File Offset: 0x000ED6A1
            public PadDPadRight(int gamepadIndex)
            {
                this.GamepadIndex = gamepadIndex;
            }

            // Token: 0x1700029E RID: 670
            // (get) Token: 0x06001EC7 RID: 7879 RVA: 0x000EF4B4 File Offset: 0x000ED6B4
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadRightCheck;
                }
            }

            // Token: 0x1700029F RID: 671
            // (get) Token: 0x06001EC8 RID: 7880 RVA: 0x000EF4D8 File Offset: 0x000ED6D8
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadRightPressed;
                }
            }

            // Token: 0x170002A0 RID: 672
            // (get) Token: 0x06001EC9 RID: 7881 RVA: 0x000EF4FC File Offset: 0x000ED6FC
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadRightReleased;
                }
            }

            // Token: 0x04001EEF RID: 7919
            public int GamepadIndex;
        }

        // Token: 0x020003B8 RID: 952
        public class PadDPadLeft : VirtualButton.Node
        {
            // Token: 0x06001ECA RID: 7882 RVA: 0x000EF51F File Offset: 0x000ED71F
            public PadDPadLeft(int gamepadIndex)
            {
                this.GamepadIndex = gamepadIndex;
            }

            // Token: 0x170002A1 RID: 673
            // (get) Token: 0x06001ECB RID: 7883 RVA: 0x000EF530 File Offset: 0x000ED730
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadLeftCheck;
                }
            }

            // Token: 0x170002A2 RID: 674
            // (get) Token: 0x06001ECC RID: 7884 RVA: 0x000EF554 File Offset: 0x000ED754
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadLeftPressed;
                }
            }

            // Token: 0x170002A3 RID: 675
            // (get) Token: 0x06001ECD RID: 7885 RVA: 0x000EF578 File Offset: 0x000ED778
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadLeftReleased;
                }
            }

            // Token: 0x04001EF0 RID: 7920
            public int GamepadIndex;
        }

        // Token: 0x020003B9 RID: 953
        public class PadDPadUp : VirtualButton.Node
        {
            // Token: 0x06001ECE RID: 7886 RVA: 0x000EF59B File Offset: 0x000ED79B
            public PadDPadUp(int gamepadIndex)
            {
                this.GamepadIndex = gamepadIndex;
            }

            // Token: 0x170002A4 RID: 676
            // (get) Token: 0x06001ECF RID: 7887 RVA: 0x000EF5AC File Offset: 0x000ED7AC
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadUpCheck;
                }
            }

            // Token: 0x170002A5 RID: 677
            // (get) Token: 0x06001ED0 RID: 7888 RVA: 0x000EF5D0 File Offset: 0x000ED7D0
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadUpPressed;
                }
            }

            // Token: 0x170002A6 RID: 678
            // (get) Token: 0x06001ED1 RID: 7889 RVA: 0x000EF5F4 File Offset: 0x000ED7F4
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadUpReleased;
                }
            }

            // Token: 0x04001EF1 RID: 7921
            public int GamepadIndex;
        }

        // Token: 0x020003BA RID: 954
        public class PadDPadDown : VirtualButton.Node
        {
            // Token: 0x06001ED2 RID: 7890 RVA: 0x000EF617 File Offset: 0x000ED817
            public PadDPadDown(int gamepadIndex)
            {
                this.GamepadIndex = gamepadIndex;
            }

            // Token: 0x170002A7 RID: 679
            // (get) Token: 0x06001ED3 RID: 7891 RVA: 0x000EF628 File Offset: 0x000ED828
            public override bool Check
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadDownCheck;
                }
            }

            // Token: 0x170002A8 RID: 680
            // (get) Token: 0x06001ED4 RID: 7892 RVA: 0x000EF64C File Offset: 0x000ED84C
            public override bool Pressed
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadDownPressed;
                }
            }

            // Token: 0x170002A9 RID: 681
            // (get) Token: 0x06001ED5 RID: 7893 RVA: 0x000EF670 File Offset: 0x000ED870
            public override bool Released
            {
                get
                {
                    return MInput.GamePads[this.GamepadIndex].DPadDownReleased;
                }
            }

            // Token: 0x04001EF2 RID: 7922
            public int GamepadIndex;
        }

        // Token: 0x020003BB RID: 955
        public class MouseLeftButton : VirtualButton.Node
        {
            // Token: 0x170002AA RID: 682
            // (get) Token: 0x06001ED6 RID: 7894 RVA: 0x000EF694 File Offset: 0x000ED894
            public override bool Check
            {
                get
                {
                    return MInput.Mouse.CheckLeftButton;
                }
            }

            // Token: 0x170002AB RID: 683
            // (get) Token: 0x06001ED7 RID: 7895 RVA: 0x000EF6B0 File Offset: 0x000ED8B0
            public override bool Pressed
            {
                get
                {
                    return MInput.Mouse.PressedLeftButton;
                }
            }

            // Token: 0x170002AC RID: 684
            // (get) Token: 0x06001ED8 RID: 7896 RVA: 0x000EF6CC File Offset: 0x000ED8CC
            public override bool Released
            {
                get
                {
                    return MInput.Mouse.ReleasedLeftButton;
                }
            }
        }

        // Token: 0x020003BC RID: 956
        public class MouseRightButton : VirtualButton.Node
        {
            // Token: 0x170002AD RID: 685
            // (get) Token: 0x06001EDA RID: 7898 RVA: 0x000EF6F4 File Offset: 0x000ED8F4
            public override bool Check
            {
                get
                {
                    return MInput.Mouse.CheckRightButton;
                }
            }

            // Token: 0x170002AE RID: 686
            // (get) Token: 0x06001EDB RID: 7899 RVA: 0x000EF710 File Offset: 0x000ED910
            public override bool Pressed
            {
                get
                {
                    return MInput.Mouse.PressedRightButton;
                }
            }

            // Token: 0x170002AF RID: 687
            // (get) Token: 0x06001EDC RID: 7900 RVA: 0x000EF72C File Offset: 0x000ED92C
            public override bool Released
            {
                get
                {
                    return MInput.Mouse.ReleasedRightButton;
                }
            }
        }

        // Token: 0x020003BD RID: 957
        public class MouseMiddleButton : VirtualButton.Node
        {
            // Token: 0x170002B0 RID: 688
            // (get) Token: 0x06001EDE RID: 7902 RVA: 0x000EF748 File Offset: 0x000ED948
            public override bool Check
            {
                get
                {
                    return MInput.Mouse.CheckMiddleButton;
                }
            }

            // Token: 0x170002B1 RID: 689
            // (get) Token: 0x06001EDF RID: 7903 RVA: 0x000EF764 File Offset: 0x000ED964
            public override bool Pressed
            {
                get
                {
                    return MInput.Mouse.PressedMiddleButton;
                }
            }

            // Token: 0x170002B2 RID: 690
            // (get) Token: 0x06001EE0 RID: 7904 RVA: 0x000EF780 File Offset: 0x000ED980
            public override bool Released
            {
                get
                {
                    return MInput.Mouse.ReleasedMiddleButton;
                }
            }
        }

        // Token: 0x020003BE RID: 958
        public class VirtualAxisTrigger : VirtualButton.Node
        {
            // Token: 0x06001EE2 RID: 7906 RVA: 0x000EF79C File Offset: 0x000ED99C
            public VirtualAxisTrigger(VirtualAxis axis, VirtualInput.ThresholdModes mode, float threshold)
            {
                this.axis = axis;
                this.Mode = mode;
                this.Threshold = threshold;
            }

            // Token: 0x170002B3 RID: 691
            // (get) Token: 0x06001EE3 RID: 7907 RVA: 0x000EF7BC File Offset: 0x000ED9BC
            public override bool Check
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.axis.Value >= this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.axis.Value <= this.Threshold);
                        }
                        else
                        {
                            result = (this.axis.Value == this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x170002B4 RID: 692
            // (get) Token: 0x06001EE4 RID: 7908 RVA: 0x000EF830 File Offset: 0x000EDA30
            public override bool Pressed
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.axis.Value >= this.Threshold && this.axis.PreviousValue < this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.axis.Value <= this.Threshold && this.axis.PreviousValue > this.Threshold);
                        }
                        else
                        {
                            result = (this.axis.Value == this.Threshold && this.axis.PreviousValue != this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x170002B5 RID: 693
            // (get) Token: 0x06001EE5 RID: 7909 RVA: 0x000EF8E0 File Offset: 0x000EDAE0
            public override bool Released
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.axis.Value < this.Threshold && this.axis.PreviousValue >= this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.axis.Value > this.Threshold && this.axis.PreviousValue <= this.Threshold);
                        }
                        else
                        {
                            result = (this.axis.Value != this.Threshold && this.axis.PreviousValue == this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x04001EF3 RID: 7923
            public VirtualInput.ThresholdModes Mode;

            // Token: 0x04001EF4 RID: 7924
            public float Threshold;

            // Token: 0x04001EF5 RID: 7925
            private VirtualAxis axis;

            // Token: 0x02000799 RID: 1945
            public enum Modes
            {
                // Token: 0x040031B2 RID: 12722
                LargerThan,
                // Token: 0x040031B3 RID: 12723
                LessThan,
                // Token: 0x040031B4 RID: 12724
                Equals
            }
        }

        // Token: 0x020003BF RID: 959
        public class VirtualIntegerAxisTrigger : VirtualButton.Node
        {
            // Token: 0x06001EE6 RID: 7910 RVA: 0x000EF993 File Offset: 0x000EDB93
            public VirtualIntegerAxisTrigger(VirtualIntegerAxis axis, VirtualInput.ThresholdModes mode, int threshold)
            {
                this.axis = axis;
                this.Mode = mode;
                this.Threshold = threshold;
            }

            // Token: 0x170002B6 RID: 694
            // (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000EF9B4 File Offset: 0x000EDBB4
            public override bool Check
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.axis.Value >= this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.axis.Value <= this.Threshold);
                        }
                        else
                        {
                            result = (this.axis.Value == this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x170002B7 RID: 695
            // (get) Token: 0x06001EE8 RID: 7912 RVA: 0x000EFA28 File Offset: 0x000EDC28
            public override bool Pressed
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.axis.Value >= this.Threshold && this.axis.PreviousValue < this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.axis.Value <= this.Threshold && this.axis.PreviousValue > this.Threshold);
                        }
                        else
                        {
                            result = (this.axis.Value == this.Threshold && this.axis.PreviousValue != this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x170002B8 RID: 696
            // (get) Token: 0x06001EE9 RID: 7913 RVA: 0x000EFAD8 File Offset: 0x000EDCD8
            public override bool Released
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.axis.Value < this.Threshold && this.axis.PreviousValue >= this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.axis.Value > this.Threshold && this.axis.PreviousValue <= this.Threshold);
                        }
                        else
                        {
                            result = (this.axis.Value != this.Threshold && this.axis.PreviousValue == this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x04001EF6 RID: 7926
            public VirtualInput.ThresholdModes Mode;

            // Token: 0x04001EF7 RID: 7927
            public int Threshold;

            // Token: 0x04001EF8 RID: 7928
            private VirtualIntegerAxis axis;

            // Token: 0x0200079A RID: 1946
            public enum Modes
            {
                // Token: 0x040031B6 RID: 12726
                LargerThan,
                // Token: 0x040031B7 RID: 12727
                LessThan,
                // Token: 0x040031B8 RID: 12728
                Equals
            }
        }

        // Token: 0x020003C0 RID: 960
        public class VirtualJoystickXTrigger : VirtualButton.Node
        {
            // Token: 0x06001EEA RID: 7914 RVA: 0x000EFB8B File Offset: 0x000EDD8B
            public VirtualJoystickXTrigger(VirtualJoystick joystick, VirtualInput.ThresholdModes mode, float threshold)
            {
                this.joystick = joystick;
                this.Mode = mode;
                this.Threshold = threshold;
            }

            // Token: 0x170002B9 RID: 697
            // (get) Token: 0x06001EEB RID: 7915 RVA: 0x000EFBAC File Offset: 0x000EDDAC
            public override bool Check
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.joystick.Value.X >= this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.joystick.Value.X <= this.Threshold);
                        }
                        else
                        {
                            result = (this.joystick.Value.X == this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x170002BA RID: 698
            // (get) Token: 0x06001EEC RID: 7916 RVA: 0x000EFC2C File Offset: 0x000EDE2C
            public override bool Pressed
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.joystick.Value.X >= this.Threshold && this.joystick.PreviousValue.X < this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.joystick.Value.X <= this.Threshold && this.joystick.PreviousValue.X > this.Threshold);
                        }
                        else
                        {
                            result = (this.joystick.Value.X == this.Threshold && this.joystick.PreviousValue.X != this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x170002BB RID: 699
            // (get) Token: 0x06001EED RID: 7917 RVA: 0x000EFCFC File Offset: 0x000EDEFC
            public override bool Released
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.joystick.Value.X < this.Threshold && this.joystick.PreviousValue.X >= this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.joystick.Value.X > this.Threshold && this.joystick.PreviousValue.X <= this.Threshold);
                        }
                        else
                        {
                            result = (this.joystick.Value.X != this.Threshold && this.joystick.PreviousValue.X == this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x04001EF9 RID: 7929
            public VirtualInput.ThresholdModes Mode;

            // Token: 0x04001EFA RID: 7930
            public float Threshold;

            // Token: 0x04001EFB RID: 7931
            private VirtualJoystick joystick;

            // Token: 0x0200079B RID: 1947
            public enum Modes
            {
                // Token: 0x040031BA RID: 12730
                LargerThan,
                // Token: 0x040031BB RID: 12731
                LessThan,
                // Token: 0x040031BC RID: 12732
                Equals
            }
        }

        // Token: 0x020003C1 RID: 961
        public class VirtualJoystickYTrigger : VirtualButton.Node
        {
            // Token: 0x06001EEE RID: 7918 RVA: 0x000EFDCD File Offset: 0x000EDFCD
            public VirtualJoystickYTrigger(VirtualJoystick joystick, VirtualInput.ThresholdModes mode, float threshold)
            {
                this.joystick = joystick;
                this.Mode = mode;
                this.Threshold = threshold;
            }

            // Token: 0x170002BC RID: 700
            // (get) Token: 0x06001EEF RID: 7919 RVA: 0x000EFDEC File Offset: 0x000EDFEC
            public override bool Check
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.joystick.Value.X >= this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.joystick.Value.X <= this.Threshold);
                        }
                        else
                        {
                            result = (this.joystick.Value.X == this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x170002BD RID: 701
            // (get) Token: 0x06001EF0 RID: 7920 RVA: 0x000EFE6C File Offset: 0x000EE06C
            public override bool Pressed
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.joystick.Value.X >= this.Threshold && this.joystick.PreviousValue.X < this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.joystick.Value.X <= this.Threshold && this.joystick.PreviousValue.X > this.Threshold);
                        }
                        else
                        {
                            result = (this.joystick.Value.X == this.Threshold && this.joystick.PreviousValue.X != this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x170002BE RID: 702
            // (get) Token: 0x06001EF1 RID: 7921 RVA: 0x000EFF3C File Offset: 0x000EE13C
            public override bool Released
            {
                get
                {
                    bool flag = this.Mode == VirtualInput.ThresholdModes.LargerThan;
                    bool result;
                    if (flag)
                    {
                        result = (this.joystick.Value.X < this.Threshold && this.joystick.PreviousValue.X >= this.Threshold);
                    }
                    else
                    {
                        bool flag2 = this.Mode == VirtualInput.ThresholdModes.LessThan;
                        if (flag2)
                        {
                            result = (this.joystick.Value.X > this.Threshold && this.joystick.PreviousValue.X <= this.Threshold);
                        }
                        else
                        {
                            result = (this.joystick.Value.X != this.Threshold && this.joystick.PreviousValue.X == this.Threshold);
                        }
                    }
                    return result;
                }
            }

            // Token: 0x04001EFC RID: 7932
            public VirtualInput.ThresholdModes Mode;

            // Token: 0x04001EFD RID: 7933
            public float Threshold;

            // Token: 0x04001EFE RID: 7934
            private VirtualJoystick joystick;
        }
    }
}