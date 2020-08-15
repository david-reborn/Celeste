using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace myd.celeste.demo
{
    public class VirtualJoystick : VirtualInput
    {

        public List<VirtualJoystick.Node> Nodes;
        public bool Normalized;
        public float? SnapSlices;
        public bool InvertedX;
        public bool InvertedY;

        public Vector2 Value { get; private set; }

        public Vector2 PreviousValue { get; private set; }

        public VirtualJoystick(bool normalized)
        {
            this.Nodes = new List<VirtualJoystick.Node>();
            this.Normalized = normalized;
        }

        public VirtualJoystick(bool normalized, params VirtualJoystick.Node[] nodes)
        {
            this.Nodes = new List<VirtualJoystick.Node>((IEnumerable<VirtualJoystick.Node>)nodes);
            this.Normalized = normalized;
        }

        public override void Update()
        {
            foreach (VirtualInputNode node in this.Nodes)
                node.Update();
            this.PreviousValue = this.Value;
            this.Value = Vector2.zero;
            //if (MInput.Disabled)
            //    return;
            foreach (VirtualJoystick.Node node in this.Nodes)
            {
                Vector2 vec = node.Value;
                if (vec != Vector2.zero)
                {
                    if (this.Normalized)
                    {
                        if (this.SnapSlices.HasValue)
                            vec = vec.SnappedNormal(this.SnapSlices.Value);
                        else
                            vec.Normalize();
                    }
                    else if (this.SnapSlices.HasValue)
                        vec = vec.Snapped(this.SnapSlices.Value);
                    if (this.InvertedX)
                        vec.x *= -1f;
                    if (this.InvertedY)
                        vec.y *= -1f;
                    this.Value = vec;
                    break;
                }
            }
        }

        public static implicit operator Vector2(VirtualJoystick joystick)
        {
            return joystick.Value;
        }

        public abstract class Node : VirtualInputNode
        {
            public abstract Vector2 Value { get; }
        }

        

        public class KeyboardKeys : VirtualJoystick.Node
        {
            public VirtualInput.OverlapBehaviors OverlapBehavior;
            public KeyCode Left;
            public KeyCode Right;
            public KeyCode Up;
            public KeyCode Down;
            private bool turnedX;
            private bool turnedY;
            private Vector2 value;

            public KeyboardKeys(
              VirtualInput.OverlapBehaviors overlapBehavior,
              KeyCode left,
              KeyCode right,
              KeyCode up,
              KeyCode down)
            {
                this.OverlapBehavior = overlapBehavior;
                this.Left = left;
                this.Right = right;
                this.Up = up;
                this.Down = down;
            }

            public override void Update()
            {
                if (Input.GetKey(this.Left))
                {
                    if (Input.GetKey(this.Right))
                    {
                        switch (this.OverlapBehavior)
                        {
                            case VirtualInput.OverlapBehaviors.TakeOlder:
                                break;
                            case VirtualInput.OverlapBehaviors.TakeNewer:
                                if (!this.turnedX)
                                {
                                    this.value.x *= -1f;
                                    this.turnedX = true;
                                    break;
                                }
                                break;
                            default:
                                this.value.x = 0.0f;
                                break;
                        }
                    }
                    else
                    {
                        this.turnedX = false;
                        this.value.x = -1f;
                    }
                }
                else if (Input.GetKey(this.Right))
                {
                    this.turnedX = false;
                    this.value.x = 1f;
                }
                else
                {
                    this.turnedX = false;
                    this.value.x = 0.0f;
                }
                if (Input.GetKey(this.Up))
                {
                    if (Input.GetKey(this.Down))
                    {
                        switch (this.OverlapBehavior)
                        {
                            case VirtualInput.OverlapBehaviors.TakeOlder:
                                break;
                            case VirtualInput.OverlapBehaviors.TakeNewer:
                                if (this.turnedY)
                                    break;
                                this.value.y *= -1f;
                                this.turnedY = true;
                                break;
                            default:
                                this.value.y = 0.0f;
                                break;
                        }
                    }
                    else
                    {
                        this.turnedY = false;
                        this.value.y = -1f;
                    }
                }
                else if (Input.GetKey(this.Down))
                {
                    this.turnedY = false;
                    this.value.y = 1f;
                }
                else
                {
                    this.turnedY = false;
                    this.value.y = 0.0f;
                }
            }

            public override Vector2 Value
            {
                get
                {
                    return this.value;
                }
            }
        }
    }
}
