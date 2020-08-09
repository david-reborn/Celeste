using System;
using System.Collections;
using System.Collections.Generic;

namespace myd.celeste.demo
{
    public class VirtualIntegerAxis : VirtualInput
    {
        public List<VirtualAxis.Node> Nodes;
        public bool Inverted;
        public int Value;

        public int PreviousValue { get; private set; }

        public VirtualIntegerAxis()
        {
            this.Nodes = new List<VirtualAxis.Node>();
        }

        public VirtualIntegerAxis(params VirtualAxis.Node[] nodes)
        {
            this.Nodes = new List<VirtualAxis.Node>((IEnumerable<VirtualAxis.Node>)nodes);
        }

        public override void Update()
        {
            foreach (VirtualInputNode node in this.Nodes)
                node.Update();
            this.PreviousValue = this.Value;
            this.Value = 0;
            //if (MInput.Disabled)
            //    return;
            foreach (VirtualAxis.Node node in this.Nodes)
            {
                float num = node.Value;
                if ((double)num != 0.0)
                {
                    this.Value = Math.Sign(num);
                    if (!this.Inverted)
                        break;
                    this.Value *= -1;
                    break;
                }
            }
        }

        public static implicit operator int(VirtualIntegerAxis axis)
        {
            return axis.Value;
        }


    }
}
