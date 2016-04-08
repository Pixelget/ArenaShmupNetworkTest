using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

// When Modifying these, adjust the Cycle code as well
public enum WeaponType { BurstRifle, HeavyRifle, Shotgun, LaserRifle }//, GrenadeLauncher, RocketLauncher } // Rocket Launcher like Gokudera's redirection bombs? // Laser is a single shot sniper
public enum FrameChoice { Wraith, Legion, Spirit, Saint } // TypeZero, Firehead, Reaper, Mercy, Echo, Sutherland, Gareth, Test XF-01, Sparks, Djinn
[System.Serializable]
public class Player {
    int id;
    InputDevice device;
    Profile profile;
    bool aicontrolled = false;
    FrameChoice frame;
    WeaponType weapon = WeaponType.BurstRifle;
    int WeaponIterator = 0;
    bool ready = false;

    public Player(int id, InputDevice device) {
        // Load in the weapon and abilities data
        this.id = id;
        this.device = device;
    }

    public void AssignProfile(Profile profile) {
        this.profile = new Profile(profile);

        this.weapon = this.profile.LastUsedWeapon;
        this.frame = this.profile.LastUsedFrame;
    }

    public override string ToString() {
        return profile.Name;
    }

    // Getters
    public int ID {
        get { return id; }
    }
    public InputDevice Device {
        get { return device; }
    }
    public Profile Profile {
        get { return this.profile; }
    }
    public bool AIControlled {
        get { return aicontrolled; }
    }
    public string Name {
        get { return profile.Name; }
    }
    public Weapon Weapon {
        get { return GameManager.Instance.weaponList[WeaponIterator]; }
    }
    public string WeaponName {
        get { return GameManager.Instance.weaponList[WeaponIterator].Name; }
    }
    public string Frame {
        get { return System.Enum.GetName(typeof(FrameChoice), frame); }
    }
    public string Ability {
        get { return "Ability Here"; }
    }
    public bool Ready {
        get { return ready; }
    }

    // Weapon Cycle
    public void NextWeapon() {
        WeaponIterator++;

        if (WeaponIterator >= GameManager.Instance.weaponList.Count) {
            WeaponIterator = 0;
        }
    }
    public void PrevWeapon() {
        WeaponIterator--;

        if (WeaponIterator < 0) {
            WeaponIterator = GameManager.Instance.weaponList.Count-1;
        }
    }

    // Frame Cycle
    public void NextFrame() {
        switch (frame) {
        case FrameChoice.Wraith:
            frame = FrameChoice.Legion;
            break;
        case FrameChoice.Legion:
            frame = FrameChoice.Spirit;
            break;
        case FrameChoice.Spirit:
            frame = FrameChoice.Saint;
            break;
        case FrameChoice.Saint:
            frame = FrameChoice.Wraith;
            break;
        }
    }
    public void PrevFrame() {
        switch (frame) {
        case FrameChoice.Wraith:
            frame = FrameChoice.Saint;
            break;
        case FrameChoice.Legion:
            frame = FrameChoice.Wraith;
            break;
        case FrameChoice.Spirit:
            frame = FrameChoice.Legion;
            break;
        case FrameChoice.Saint:
            frame = FrameChoice.Spirit;
            break;
        }
    }

    // Ready Selection Save
    public void IsReady() {
        ready = true;

        this.profile.LastUsedWeapon = this.weapon;
        this.profile.LastUsedFrame = this.frame;
    }
    public void NotReady() {
        ready = false;
    }
}
