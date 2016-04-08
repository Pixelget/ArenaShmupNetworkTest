using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(PlayerAnimationManager))]
public class PlayerManager : NetworkedMonoBehavior {

    Player player;
    PlayerMovementController playerMovementController;
    WeaponManager weaponManager;

    public GameObject MechArt = null;
    public GameObject EngineArt = null;
    public GameObject GunArt = null;

    private Vector2 movingDirection = Vector2.zero;
    private Vector2 facingDirection = Vector2.zero;

    public bool Firing = false;

    public void InitPlayer(Player player) {
        this.player = player;
        GetComponent<PlayerInputManager>().InitializeInput(player);
        GetComponent<WeaponManager>().InitWeapon(player.Weapon);
    }

    void Awake() {
        AddNetworkVariable(() => movingDirection, x => movingDirection = (Vector2) x);
        AddNetworkVariable(() => facingDirection, x => facingDirection = (Vector2) x);
        //AddNetworkVariable(() => player, x => player = (Player) x); // Doesnt Work
    }

    void Start () {
        playerMovementController = GetComponent<PlayerMovementController>();
        weaponManager = GetComponent<WeaponManager>();
    }
	
    void Update() {
        if (playerMovementController != null) {
            playerMovementController.Move(movingDirection, EngineArt);
            playerMovementController.Face(facingDirection, MechArt, EngineArt, GunArt);
        }

        if (weaponManager != null) {
            weaponManager.UpdateDirection(facingDirection);
        }
    }

    // Movement and Facing Code
    public void Move(Vector2 movingDirection) {
        this.movingDirection = movingDirection;
    }
    public void Face(Vector2 facingDirection) {
        this.facingDirection = facingDirection;
    }
    public void Boost() {
        if (movingDirection.normalized.magnitude > 0f)
            playerMovementController.Boost(movingDirection.normalized);
    }



    protected override void OnApplicationQuit() {
        base.OnApplicationQuit();

        // Delete local players here
        Networking.Destroy(this);
    }
}
