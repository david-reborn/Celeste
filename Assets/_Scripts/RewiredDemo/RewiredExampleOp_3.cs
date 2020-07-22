using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myd.input
{
    public class RewiredExampleOp_3 : MonoBehaviour
    {
        public int playerId;
        private Player player;

        void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);
        }

        public void Update()
        {
            LogMouseValues();
        }
        void LogMouseValues()
        {
            Mouse mouse = ReInput.controllers.Mouse;
            Debug.Log("Left Mouse Button = " + mouse.GetButton(0));
            Debug.Log("Right Mouse Button (Hold) = " + mouse.GetButton(1));
            Debug.Log("Right Mouse Button (Down) = " + mouse.GetButtonDown(1));
            Debug.Log("Right Mouse Button (Up) = " + mouse.GetButtonUp(1));
        }

        void LogPlayerJoystickValues(Player player)
        {
            // Log the button and axis values for each joystick assigned to this Player
            for (int i = 0; i < player.controllers.joystickCount; i++)
            {
                Joystick joystick = player.controllers.Joysticks[i];
                Debug.Log("Joystick " + i + ":");
                LogJoystickElementValues(joystick); // log all the element values in this joystick
            }
        }

        void LogJoystickElementValues(Joystick joystick)
        {
            // Log Joystick button values
            for (int i = 0; i < joystick.buttonCount; i++)
            {
                Debug.Log("Button " + i + " = " + joystick.Buttons[i].value); // get the current value of the button
            }

            // Log Joystick axis values
            for (int i = 0; i < joystick.axisCount; i++)
            {
                Debug.Log("Axis " + i + " = " + joystick.Axes[i].value); // get the current value of the axis
            }
        }
    }
}
