using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace myd.celeste.demo
{
    public class VirtualButton : VirtualInput
    {
        public List<VirtualButton.Node> Nodes;
        public float BufferTime;
        private float firstRepeatTime;
        private float multiRepeatTime;
        private float bufferCounter;
        private float repeatCounter;
        private bool canRepeat;
        private bool consumed;

        public bool Repeating { get; private set; }

        public VirtualButton(float bufferTime)
        {
            this.Nodes = new List<VirtualButton.Node>();
            this.BufferTime = bufferTime;
        }

        public VirtualButton()
          : this(0.0f)
        {
        }

        public VirtualButton(float bufferTime, params VirtualButton.Node[] nodes)
        {
            this.Nodes = new List<VirtualButton.Node>((IEnumerable<VirtualButton.Node>)nodes);
            this.BufferTime = bufferTime;
        }

        public VirtualButton(params VirtualButton.Node[] nodes)
          : this(0.0f, nodes)
        {
        }

        public void SetRepeat(float repeatTime)
        {
            this.SetRepeat(repeatTime, repeatTime);
        }

        public void SetRepeat(float firstRepeatTime, float multiRepeatTime)
        {
            this.firstRepeatTime = firstRepeatTime;
            this.multiRepeatTime = multiRepeatTime;
            this.canRepeat = (double)this.firstRepeatTime > 0.0;
            if (this.canRepeat)
                return;
            this.Repeating = false;
        }

        public override void Update()
        {
            this.consumed = false;
            this.bufferCounter -= Time.deltaTime;
            bool flag = false;
            foreach (VirtualButton.Node node in this.Nodes)
            {
                node.Update();
                if (node.Pressed)
                {
                    this.bufferCounter = this.BufferTime;
                    flag = true;
                }
                else if (node.Check)
                    flag = true;
            }
            if (!flag)
            {
                this.Repeating = false;
                this.repeatCounter = 0.0f;
                this.bufferCounter = 0.0f;
            }
            else
            {
                if (!this.canRepeat)
                    return;
                this.Repeating = false;
                if ((double)this.repeatCounter == 0.0)
                {
                    this.repeatCounter = this.firstRepeatTime;
                }
                else
                {
                    this.repeatCounter -= Time.deltaTime;
                    if ((double)this.repeatCounter <= 0.0)
                    {
                        this.Repeating = true;
                        this.repeatCounter = this.multiRepeatTime;
                    }
                }
            }
        }

        public bool Check
        {
            get
            {
                //if (MInput.Disabled)
                //    return false;
                foreach (VirtualButton.Node node in this.Nodes)
                {
                    if (node.Check)
                        return true;
                }
                return false;
            }
        }

        public bool Pressed
        {
            get
            {
                //if (this.DebugOverridePressed.HasValue && Input.GetButton(this.DebugOverridePressed.Value))
                //    return true;
                //if (MInput.Disabled || this.consumed)
                if (this.consumed)
                    return false;
                if ((double)this.bufferCounter > 0.0 || this.Repeating)
                    return true;
                foreach (VirtualButton.Node node in this.Nodes)
                {
                    if (node.Pressed)
                        return true;
                }
                return false;
            }
        }

        public bool Released
        {
            get
            {
                //if (MInput.Disabled)
                //    return false;
                foreach (VirtualButton.Node node in this.Nodes)
                {
                    if (node.Released)
                        return true;
                }
                return false;
            }
        }

        public void ConsumeBuffer()
        {
            this.bufferCounter = 0.0f;
        }

        public void ConsumePress()
        {
            this.bufferCounter = 0.0f;
            this.consumed = true;
        }

        public static implicit operator bool(VirtualButton button)
        {
            return button.Check;
        }

        public abstract class Node : VirtualInputNode
        {
            public abstract bool Check { get; }

            public abstract bool Pressed { get; }

            public abstract bool Released { get; }
        }

        public class KeyboardKey : VirtualButton.Node
        {
            public KeyCode Key;

            public KeyboardKey(KeyCode key)
            {
                this.Key = key;
            }

            public override bool Check
            {
                get
                {
                    return Input.GetKey(this.Key);
                }
            }

            public override bool Pressed
            {
                get
                {
                    return Input.GetKeyDown(this.Key);
                }
            }

            public override bool Released
            {
                get
                {
                    return Input.GetKeyUp(this.Key);
                }
            }
        }

        
    }
}