using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Network;

[RequireComponent(typeof(PlayerManager))]
public class WeaponManager : SimpleNetworkedMonoBehavior {
    public PlayerManager playerManager;

    Weapon weapon = null;

    float cooldown;
    float firerate;
    int ammo;
    int burstShots;

    float reload;
    float accuracy = 0f;
    float accuracyResetDelayTimer = 0f;

    Vector2 direction;

    bool Reloading = false;
    bool Firing = false;
    bool Shooting = false;
    bool CurrentlyBurstFiring = false;

    List<GameObject> BulletPool = new List<GameObject>();
    
    public void InitWeapon(Weapon weapon) {
        this.weapon = weapon;
        Reset();
    }

    void Update() {
        if (weapon != null) {
            WeaponUpdate();
        } else {
            Debug.Log("Weapon being set because it is null");
            this.weapon = GameManager.Instance.weaponList[0];
            Reset();
        }
    }

    void WeaponUpdate() {
        cooldown += Time.deltaTime;
        if ((cooldown > weapon.Cooldown) && (ammo > 0) && (!Reloading)) {
            Shooting = true;
        } else if ((cooldown > weapon.Cooldown) && (ammo <= 0) && (!Reloading) && (playerManager.Firing)) {
            cooldown = 0f;
        }

        if (Reloading) {
            reload += Time.deltaTime;
            if (reload > weapon.ReloadTime) {
                // Completed the reload / Cooldown is over
                reload = 0f;
                Reloading = false;
                accuracy = 0f;
                Reset();
            }
        }

        if (Shooting && playerManager.Firing) {
            // Can shoot
            Firing = true;
            cooldown = 0f;
        }

        if (Firing || CurrentlyBurstFiring) {
            switch (weapon.ShotType) {
                case WeaponShotType.Single:
                    SingleShotMode();
                    break;
                case WeaponShotType.Burst:
                    BurstFireMode();
                    break;
                case WeaponShotType.Spray:
                    SprayShotMode();
                    break;
                case WeaponShotType.Laser:
                default:
                    Debug.Log("Laser dont work yet.");
                    break;
            }
        }

        if (Firing) {
            accuracy += Time.deltaTime;
            if (accuracy > weapon.AccuracyReductionTime) {
                accuracy = weapon.AccuracyReductionTime;
            }
        } else {
            accuracyResetDelayTimer += Time.deltaTime;
            if (accuracyResetDelayTimer > weapon.AccuracyResetDelay) {
                accuracy -= Time.deltaTime * weapon.AccuracyReductionRate;
            }
            if (accuracy <= 0f) {
                accuracy = 0f;
                accuracyResetDelayTimer = 0f;
            }
        }
    }

    void SingleShotMode() {
        // Fire a shot
        Shoot();

        Shooting = false;
        Firing = false;

        ammo--;
        firerate = 0f;

        if (ammo <= 0) {
            Reloading = true;
        }
    }

    void BurstFireMode() {
        // Empty the clip
        firerate += Time.deltaTime;
        if (firerate > weapon.FireRate) {
            CurrentlyBurstFiring = true;
            // Fire a shot
            Shoot();

            burstShots--;
            firerate = 0f;
        }

        // if no ammo remains
        if (burstShots <= 0) {
            // Reset Weapon
            // Set the canShoot and Fire to false
            Shooting = false;
            Firing = false;
            // Set the ammo to totalAmmo
            burstShots = weapon.BurstFireShots;
            CurrentlyBurstFiring = false;
            ammo--;

            if (ammo <= 0) {
                Reloading = true;
            }
        }
    }

    void SprayShotMode() {
        // Fire a shot spray

        // Accuracy Calculations Here
        Vector2 Spread_Start = (Base.RotateV2(direction, -weapon.Spread * weapon.SprayAngle) * (1f - weapon.Accuracy)).normalized;
        Vector2 Spread_End = (Base.RotateV2(direction, weapon.Spread * weapon.SprayAngle) * (1f - weapon.Accuracy)).normalized;
        float Angle = Vector2.Angle(Spread_Start, Spread_End) / weapon.BurstFireShots;

        // Fire Shot in incremental spray using the spread
        for (int i = 1; i <= weapon.BurstFireShots; i++) {
            Shoot(Base.RotateV2(Spread_Start, Angle * i));
        }

        Shooting = false;
        Firing = false;

        ammo--;
        firerate = 0f;

        if (ammo <= 0) {
            Reloading = true;
        }
    }

    // RPC this?
    void Shoot() {
        // network instantiate a shot?
        ShotEffects();

        // Accuracy Calculations Here
        Vector2 ProjectileDirection = Vector2.zero;
        Vector2 Spread = Vector2.zero;
        Spread = new Vector2(Random.Range(-weapon.Spread, weapon.Spread), Random.Range(-weapon.Spread, weapon.Spread));
        Spread = (Spread * (accuracy / weapon.AccuracyReductionTime)) * (1f - weapon.Accuracy);
        ProjectileDirection = ((direction * 2f) + Spread).normalized;

        //CreateBullet(ProjectileDirection);
        RPC("CreateBullet", NetworkReceivers.All, ProjectileDirection);

        // Create the projectile object
        Debug.DrawRay(transform.position, ProjectileDirection * 10f, Color.red, 5f);
    }

    void Shoot(Vector2 aimDirection) {
        // network instantiate a shot?
        ShotEffects();

        // Create the projectile object using the determined aim direction
        Debug.DrawRay(transform.position, aimDirection * 10f, Color.red, 5f);
        RPC("CreateBullet", NetworkReceivers.All, aimDirection);
        //CreateBullet(aimDirection);
    }

    [BRPC]
    void CreateBullet(Vector2 dir) {
        GameObject temp = GetNextBullet();
        if (temp != null) {
            temp.transform.position = transform.position;
            temp.SetActive(true);
            temp.GetComponent<ProjectileMove>().InitProjectile(dir, weapon.Range);
        } else {
            //Networking.Instantiate("Projectile_Bullet", transform.position, Quaternion.identity, NetworkReceivers.All, (go) => BulletCreated(go, dir));
            temp = (GameObject) Instantiate(weapon.bulletPrefab, transform.position, Quaternion.identity);
            BulletCreated(temp, dir);
        }
    }

    void BulletCreated(GameObject go, Vector2 aimDir) {
        BulletPool.Add(go.gameObject);
        go.gameObject.GetComponent<ProjectileMove>().InitProjectile(aimDir, weapon.Range);
    }

    GameObject GetNextBullet() {
        for (int i = 0; i < BulletPool.Count; i++) {
            if (!BulletPool[i].activeSelf) {
                return BulletPool[i];
            }
        }
        return null;
    }

    // Or this might need to be an RPC? with a network instantiate on the bullets
    void ShotEffects() {
        // TODO Play Audio
        //SoundManager.PlayClip(weapon.ShotSound)
        MuzzleEffects();
    }

    void MuzzleEffects() {
        // TODO MuzzleFlash
    }

    public void Reset() {
        ammo = weapon.Ammo;
        firerate = weapon.FireRate;
        burstShots = weapon.BurstFireShots;
    }

    public void UpdateDirection(Vector2 direction) {
        this.direction = direction.normalized;
    }
}
