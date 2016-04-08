using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;
using System;

public enum CharacterSelectState { PressStart, ProfileSelect, ProfileCreate, CharacterSelect }
public class CharacterSelectUI : MonoBehaviour {

    [Header("UI Panels")]
    public GameObject PressStartPanel;
    public GameObject CharacterSelectPanel;
    public GameObject ProfileCreationPanel;
    public GameObject ProfileSelectPanel;

    [Header("Character Select Values")]
    public Text ProfileNameText;
    public Text AIControlledText;
    public Text FrameNameText;
    public Text WeaponNameText;
    public Text AbilityNameText;
    public Text ReadyText;

    Player player;
    CharacterSelectState state = CharacterSelectState.PressStart;
    
	void Update () {
        switch(state) {
        case CharacterSelectState.ProfileSelect:
            // TODO Replace with actual profile code
            player.AssignProfile(new Profile("Player " + (player.ID+1)));
            StateSwitch(CharacterSelectState.CharacterSelect);
            break;
        case CharacterSelectState.CharacterSelect:
            if (player != null)
                ProcessCharacterSelectInput();
            break;
        }
        UpdateGUI();
	}

    void ProcessCharacterSelectInput() {
        if (player.Device.Action1.WasPressed) {
            player.IsReady();
        }
        if (player.Device.Action2.WasPressed) {
            if (player.Ready) {
                player.NotReady();
            } else {
                Debug.Log("Player backing out.");
                UnassignPlayer();
                //StateSwitch(CharacterSelectState.ProfileSelect); // TODO Switch to Profile Select
            }
        }

        if (player.Device.RightBumper.WasPressed) {
            player.NextWeapon();
        }
        if (player.Device.LeftBumper.WasPressed) {
            player.PrevWeapon();
        }

        if (player.Device.Direction.Right.WasPressed) {
            player.NextFrame();
        }
        if (player.Device.Direction.Left.WasPressed) {
            player.PrevFrame();
        }
    }

    void UpdateGUI() {
        if (player != null) {
            ProfileNameText.text = player.Profile.Name;
            FrameNameText.text = player.Frame;
            WeaponNameText.text = player.WeaponName;
            AbilityNameText.text = player.Ability;

            if (player.AIControlled) {
                AIControlledText.text = "AI Controlled";
            } else {
                AIControlledText.text = "Player Controlled";
            }

            if (player.Ready) {
                ReadyText.text = "Ready";
            } else {
                ReadyText.text = "Not Ready";
            }
        }
    }

    public void AssignPlayer(Player player) {
        this.player = player;
        StateSwitch(CharacterSelectState.ProfileSelect);
    }

    void StateSwitch(CharacterSelectState NextState) {
        // Hide current state panels
        switch(state) {
        case CharacterSelectState.PressStart:
            PressStartPanel.SetActive(false);
            break;
        case CharacterSelectState.ProfileSelect:
            ProfileSelectPanel.SetActive(false);
            break;
        case CharacterSelectState.ProfileCreate:
            ProfileCreationPanel.SetActive(false);
            break;
        case CharacterSelectState.CharacterSelect:
            CharacterSelectPanel.SetActive(false);
            break;
        }

        // show next state panels
        switch (NextState) {
        case CharacterSelectState.PressStart:
            PressStartPanel.SetActive(true);
            break;
        case CharacterSelectState.ProfileSelect:
            ProfileSelectPanel.SetActive(true);
            break;
        case CharacterSelectState.ProfileCreate:
            ProfileCreationPanel.SetActive(true);
            break;
        case CharacterSelectState.CharacterSelect:
            CharacterSelectPanel.SetActive(true);
            break;
        }

        this.state = NextState;
    }

    public void UnassignPlayer() {
        GameObject.Find("CharacterSelectManager").GetComponent<CharacterSelectManager>().UnbindDevice(player.Device);
        // Remove the player from the GameManager
        GameManager.Instance.Players.Remove(player);
        
        StateSwitch(CharacterSelectState.PressStart);
    }
}
