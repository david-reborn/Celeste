using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myd.celeste.demo
{
    public class InputManager : MonoBehaviour
    {
        public static VirtualJoystick Aim;
        public static VirtualIntegerAxis MoveX;
        public static VirtualIntegerAxis MoveY;
        public static VirtualButton Jump;
        public static VirtualButton Grab;
        public static VirtualButton Dash;
        internal static List<VirtualInput> VirtualInputs = new List<VirtualInput>();
        public static Vector2 LastAim;

        private static InputManager instance;

        public void Awake()
        {
            Debug.Log("InputManager Awake");
            instance = this;
        }

        public void Start()
        {
            Debug.Log("InputManager Start");
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

            Grab = new VirtualButton();
            Grab.Nodes.Add((VirtualButton.Node)new VirtualButton.KeyboardKey(KeyCode.J));

            Dash = new VirtualButton(0.08f);
            Dash.Nodes.Add((VirtualButton.Node)new VirtualButton.KeyboardKey(KeyCode.L));

            Aim = new VirtualJoystick(false, new VirtualJoystick.Node[1]
            {
                (VirtualJoystick.Node) new VirtualJoystick.KeyboardKeys(VirtualInput.OverlapBehaviors.TakeNewer, KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S),
            });

            VirtualInputs.Add(MoveX);
            VirtualInputs.Add(MoveY);
            VirtualInputs.Add(Jump);
            VirtualInputs.Add(Grab);
            VirtualInputs.Add(Dash);
            VirtualInputs.Add(Aim);
        }

        public void Update()
        {
            foreach (var vInput in VirtualInputs)
            {
                vInput.Update();
            }
        }

        public static Vector2 GetAimVector(Facings defaultFacing = Facings.Right)
        {
            Vector2 vector2 = Aim.Value;
            if (vector2 == Vector2.zero)
            {
                //if (SaveData.Instance != null && SaveData.Instance.Assists.DashAssist)
                //    return Input.LastAim;
                LastAim = Vector2.right * (float)defaultFacing;
            }
            //else if (SaveData.Instance != null && SaveData.Instance.Assists.ThreeSixtyDashing)
            //{
            //    LastAim = vector2.normalized;
            //}
            else
            {
                float radiansA = vector2.Angle();
                float num = (float)(0.392699092626572 - ((double)radiansA < 0.0 ? 1.0 : 0.0) * 0.0872664600610733);
                LastAim = (double)Util.AbsAngleDiff(radiansA, 0.0f) >= (double)num ? ((double)Util.AbsAngleDiff(radiansA, 3.141593f) >= (double)num ? ((double)Util.AbsAngleDiff(radiansA, -1.570796f) >= (double)num ? ((double)Util.AbsAngleDiff(radiansA, 1.570796f) >= (double)num ? new Vector2((float)Mathf.Sign(vector2.x), (float)Mathf.Sign(vector2.y)).normalized : new Vector2(0.0f, 1f)) : new Vector2(0.0f, -1f)) : new Vector2(-1f, 0.0f)) : new Vector2(1f, 0.0f);
            }
            return LastAim;
        }

    }
}
