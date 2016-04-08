using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class CharacterSelectManager : MonoBehaviour {
    public List<CharacterSelectUI> CharacterSelectUIList = new List<CharacterSelectUI>();

    List<InputDevice> BoundDevices = new List<InputDevice>();
    bool[] PlayerSlots = new bool[]{ false, false, false, false };
    
    void Start () {
        // Load Profiles
        ProfileManager.Load();

        InputManager.OnDeviceAttached += inputDevice => DeviceAttached(inputDevice);
        InputManager.OnDeviceDetached += inputDevice => DeviceDetached(inputDevice);
    }
	
	void Update () {
        ProcessInput();
	}

    void ProcessInput() {
        InputDevice device = InputManager.ActiveDevice;

        if (device.Action1.WasPressed || device.MenuWasPressed) {
            if (!IsDeviceBound(device)) {
                if (AvailablePlayerSlot()) {
                    BindDevice(device);

                    Player NewPlayer = new Player(GetNextPlayerIndex(), device);
                    // Set the new player to the UI stuff and game manager
                    GameManager.Instance.Players.Add(NewPlayer);
                    CharacterSelectUIList[NewPlayer.ID].AssignPlayer(NewPlayer);

                    Debug.Log("New player with an id of " + NewPlayer.ID + " created with " + device + " device.");
                } else {
                    Debug.Log("All player slots full");
                }
            } else {
                Debug.Log("Device already bound.");
            }
        } else if (device.Action3.WasPressed) {
            /*if (!GameManager.Instance.CurrentlyConnecting) {
                GameManager.Instance.PublicLobby = !GameManager.Instance.PublicLobby;
            }*/
            GameManager.Instance.isHost = !GameManager.Instance.isHost;
        }
    }

    bool IsDeviceBound(InputDevice device) {
        return BoundDevices.Contains(device);
    }
    void BindDevice(InputDevice device) {
        BoundDevices.Add(device);
    }
    public void UnbindDevice(InputDevice device) {
        BoundDevices.Remove(device);
        Player temp = GetPlayerIdWithDevice(device);
        if (temp != null) {
            PlayerSlots[temp.ID] = false;
        }
    }

    bool AvailablePlayerSlot() {
        for (int i = 0; i < PlayerSlots.Length; i++) {
            if (!PlayerSlots[i]) {
                return true;
            }
        }
        return false;
    }
    int GetNextPlayerIndex() {
        for (int i = 0; i < PlayerSlots.Length; i++) {
            if (!PlayerSlots[i]) {
                PlayerSlots[i] = true;
                return i;
            }
        }

        return -1;
    }


    void DeviceAttached(InputDevice device) {
        Debug.Log("Attached: " + device.Name);
    }
    void DeviceDetached(InputDevice device) {
        Debug.Log("Detached: " + device.Name);

        // UI Exit condition - a player is backing out
        Player temp = GetPlayerIdWithDevice(device);
        if (temp != null) {
            PlayerSlots[temp.ID] = false;
            CharacterSelectUIList[temp.ID].UnassignPlayer();
        }
    }

    Player GetPlayerIdWithDevice(InputDevice device) {
        for (int i = 0; i < GameManager.Instance.Players.Count; i++) {
            if (GameManager.Instance.Players[i].Device == device) {
                return GameManager.Instance.Players[i];
            }
        }

        return null;
    }



    // Debug
    /*public string ProfileName = "Tyler";

    [ContextMenu("Add Profile")]
    public void AddProfile() {
        // Debug Script
        Profile profile = new Profile(ProfileName);
    }

    [ContextMenu("Save Profile")]
    public void SaveProfile() {
        ProfileManager.Save();
    }*/
}
