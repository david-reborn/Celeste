using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myd.celeste.demo
{
    public class InputManager : MonoBehaviour
    {
        public static VirtualIntegerAxis MoveX;
        public static VirtualIntegerAxis MoveY;
        public static VirtualButton Jump;
        internal static List<VirtualInput> VirtualInputs = new List<VirtualInput>();

        private static InputManager instance;

        public void Awake()
        {
            Debug.Log("InputManager Awake");
            instance = this;
        }

        public void Start()
        {
            MoveX = new VirtualIntegerAxis(new VirtualAxis.Node[2]{
                (VirtualAxis.Node) new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehaviors.TakeNewer, KeyCode.LeftArrow, KeyCode.RightArrow),
                (VirtualAxis.Node) new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehaviors.TakeNewer, KeyCode.A, KeyCode.D)
            });
            MoveX.Inverted = false;
            
            MoveY = new VirtualIntegerAxis(new VirtualAxis.Node[2]
            {
                (VirtualAxis.Node) new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehaviors.TakeNewer, KeyCode.UpArrow, KeyCode.DownArrow),
                (VirtualAxis.Node) new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehaviors.TakeNewer, KeyCode.W, KeyCode.S)
            });
            Jump = new VirtualButton(0.08f);
            Jump.Nodes.Add((VirtualButton.Node)new VirtualButton.KeyboardKey(KeyCode.K));

            VirtualInputs.Add(MoveX);
            VirtualInputs.Add(MoveY);
            VirtualInputs.Add(Jump);
        }

        public void Update()
        {
            foreach (var vInput in VirtualInputs)
            {
                vInput.Update();
            }

        }
    }
}
