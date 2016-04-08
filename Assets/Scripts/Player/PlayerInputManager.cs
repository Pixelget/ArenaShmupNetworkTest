using UnityEngine;
using System.Collections;
using InControl;

[RequireComponent(typeof(PlayerManager))]
public class PlayerInputManager : MonoBehaviour {
    
    Player player = null;
    public bool AllowInput = true;

    public PlayerManager playerManager;

    public void InitializeInput(Player player) {
        this.player = player;
    }
	
	void Update () {
        if (playerManager.IsOwner) {
            InputControl();
        }
    }

    void InputControl() {
        if ((player != null) && (AllowInput)) {
            InputDevice device = player.Device;
            //InputDevice device = InputManager.ActiveDevice;

            Vector2 currentDirection = Vector2.zero;
            Vector2 currentFacing = Vector2.zero;

            currentDirection = device.LeftStick;
            currentFacing = device.RightStick;

            // Movement Direction
            playerManager.Move(currentDirection);

            if (currentFacing.magnitude > Base.DeadZone) {
                playerManager.Face(currentFacing);
            } else {
                if (currentDirection.magnitude > Base.DeadZone) {
                    playerManager.Face(currentDirection);
                }
            }

            // Facing Direction

            // Boost
            if (device.Action1.WasPressed) {
                // Boost
                playerManager.Boost();
            }

            // Shoot
            if (device.RightTrigger.WasPressed) {
                playerManager.Firing = true;
            }
            if (device.RightTrigger.WasReleased) {
                playerManager.Firing = false;
            }

            // Ability
        }
    }
}
