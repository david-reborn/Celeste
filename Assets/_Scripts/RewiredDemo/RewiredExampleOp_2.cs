using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myd.input
{
    /// <summary>
    /// Use delegates to receive input events from a Player. There are numerous overloads 
    /// available that allow you to register to receive events on different update loops, 
    /// for specific input event types, and for specific Actions. For a list of event types 
    /// and whether they require additional arguments, see InputActionEventType.
    /// </summary>
    public class RewiredExampleOp_2 : MonoBehaviour
    {
        public int playerId;
        private Player player;

        void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);

            // Add delegates to receive input events from the Player

            // This event will be called every frame any input is updated
            player.AddInputEventDelegate(OnInputUpdate, UpdateLoopType.Update);

            // This event will be called every frame the "Fire" action is updated
            player.AddInputEventDelegate(OnFireUpdate, UpdateLoopType.Update, "Fire");

            // This event will be called when the "Fire" button is first pressed
            player.AddInputEventDelegate(OnFireButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, "Fire");

            // This event will be called when the "Fire" button is first released
            player.AddInputEventDelegate(OnFireButtonUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, "Fire");

            // This event will be called every frame the "Move Horizontal" axis is non-zero and once more when it returns to zero.
            player.AddInputEventDelegate(OnMoveHorizontal, UpdateLoopType.Update, InputActionEventType.AxisActiveOrJustInactive, "Move Horizontal");

            // This event will be called when the "Jump" button is held for at least 2 seconds and then released
            player.AddInputEventDelegate(OnJumpButtonUp, UpdateLoopType.Update, InputActionEventType.ButtonPressedForTimeJustReleased, "Jump", new object[] { 2.0f });

            // The update loop you choose for the event matters. Make sure your chosen update loop is enabled in
            // the Settings page of the Rewired editor or you won't receive any events.
        }

        void OnInputUpdate(InputActionEventData data)
        {
            switch (data.actionName)
            { // determine which action this is
                case "Move Horizontal":
                    if (data.GetAxis() != 0.0f) Debug.Log("Move Horizontal!");
                    break;
                case "Fire":
                    if (data.GetButtonDown()) Debug.Log("Fire!");
                    break;
            }
        }

        void OnFireUpdate(InputActionEventData data)
        {
            if (data.GetButtonDown()) Debug.Log("Fire!");
        }

        void OnFireButtonDown(InputActionEventData data)
        {
            Debug.Log("Fire!");
        }

        void OnFireButtonUp(InputActionEventData data)
        {
            Debug.Log("Fire Released!");
        }

        void OnJumpButtonUp(InputActionEventData data)
        {
            Debug.Log("Jump!");
        }

        void OnMoveHorizontal(InputActionEventData data)
        {
            Debug.Log("Move Horizontal: " + data.GetAxis());
        }

        void OnDestroy()
        {
            // Unsubscribe from events when object is destroyed
            player.RemoveInputEventDelegate(OnInputUpdate);
            player.RemoveInputEventDelegate(OnFireUpdate);
            player.RemoveInputEventDelegate(OnFireButtonDown);
            player.RemoveInputEventDelegate(OnFireButtonUp);
            player.RemoveInputEventDelegate(OnMoveHorizontal);
            player.RemoveInputEventDelegate(OnJumpButtonUp);
        }
    }
}
