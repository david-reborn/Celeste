using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace myd.celeste.demo
{
    public class VirtualAxis : VirtualInput
    {
        public List<VirtualAxis.Node> Nodes;

        public float Value { get; private set; }

        public float PreviousValue { get; private set; }

        public VirtualAxis()
        {
            this.Nodes = new List<VirtualAxis.Node>();
        }

        public VirtualAxis(params VirtualAxis.Node[] nodes)
        {
            this.Nodes = new List<VirtualAxis.Node>((IEnumerable<VirtualAxis.Node>)nodes);
        }

        public override void Update()
        {
            foreach (VirtualInputNode node in this.Nodes)
                node.Update();
            this.PreviousValue = this.Value;
            this.Value = 0.0f;
            //if (Input.en.Disabled)
            //    return;
            foreach (VirtualAxis.Node node in this.Nodes)
            {
                float num = node.Value;
                if ((double)num != 0.0)
                {
                    this.Value = num;
                    break;
                }
            }
        }

        public static implicit operator float(VirtualAxis axis)
        {
            return axis.Value;
        }

        public abstract class Node : VirtualInputNode
        {
            public abstract float Value { get; }
        }

        public class KeyboardKeys : VirtualAxis.Node
        {
            public VirtualInput.OverlapBehaviors OverlapBehavior;
            public KeyCode Positive;
            public KeyCode Negative;
            private float value;
            private bool turned;

            public KeyboardKeys(
              VirtualInput.OverlapBehaviors overlapBehavior,
              KeyCode negative,
              KeyCode positive)
            {
                this.OverlapBehavior = overlapBehavior;
                this.Negative = negative;
                this.Positive = positive;
            }

            public override void Update()
            {
                if (Input.GetKey(this.Positive))
                {
                    if (Input.GetKey(this.Negative))
                    {
                        switch (this.OverlapBehavior)
                        {
                            case VirtualInput.OverlapBehaviors.TakeOlder:
                                break;
                            case VirtualInput.OverlapBehaviors.TakeNewer:
                                if (this.turned)
                                    break;
                                this.value *= -1f;
                                this.turned = true;
                                break;
                            default:
                                this.value = 0.0f;
                                break;
                        }
                    }
                    else
                    {
                        this.turned = false;
                        this.value = 1f;
                    }
                }
                else if (Input.GetKey(this.Negative))
                {
                    this.turned = false;
                    this.value = -1f;
                }
                else
                {
                    this.turned = false;
                    this.value = 0.0f;
                }
            }

            public override float Value
            {
                get
                {
                    return this.value;
                }
            }
        }
    }
}