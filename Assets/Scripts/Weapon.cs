using UnityEngine;
using System.Collections;

public enum WeaponShotType { Single, Burst, Spray, Laser }
[System.Serializable]
public class Weapon {
    public string Name = "";

    [Header("Fire Settings")]
    public WeaponShotType ShotType = WeaponShotType.Single;
    public int Ammo;
    public int BurstFireShots; // How many shots per burst
    public float BurstDelay; // Time between each bullet in a burst
    public float FireRate; // Time between burst shots
    public float ReloadTime; // Time is takes to reload / Cooldown between shots
    public float Cooldown;

    [Header ("Accuracy Settings")]
    [Range(0f, 1f)]
    public float Accuracy;
    public float AccuracyReductionTime;
    public float AccuracyResetDelay;
    [Range(0f, 1f)]
    public float AccuracyReductionRate;
    [Range(0f, 1f)]
    public float Spread;
    public int SprayAngle;

    [Header("Damage Settings")]
    public float Range;
    public int Damage;
    public GameObject bulletPrefab;

    [Header ("Effect Settings")]
    public AudioClip ShotSound;
}
