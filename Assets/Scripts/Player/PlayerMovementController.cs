using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerManager))]
public class PlayerMovementController : MonoBehaviour {

    PlayerManager playerManager;

    Vector2 Velocity = Vector2.zero;
    int flipDirection = 1;
    float boostTimer = 0f;

    public bool Boosting = false;
    Vector2 boostingDirection = Vector2.zero;

    Vector4 CurrentBounds;
    Vector4 Bounds;
    Vector4 MapBounds;

    void Update() {
        CalculateBounds();
    }

    public void Boost(Vector2 boostingDirection) {
        this.boostingDirection = boostingDirection;
        Boosting = true;
        GetComponent<PlayerAnimationManager>().Boosting(Boosting);
    }

    // Break These Out
    public void Move(Vector2 movingDirection, GameObject EngineArt) {
        if (Boosting) {
            // Boosting
            boostTimer += Time.deltaTime;
            if (boostTimer > Base.BoostTime) {
                boostTimer = 0f;
                Boosting = false;
                GetComponent<PlayerAnimationManager>().Boosting(Boosting);
            }
            Velocity += boostingDirection * Base.Speed * Base.BoostModifier * Time.deltaTime;
        } else {
            // Moving
            //transform.position += new Vector3(movingDirection.x, movingDirection.y, 0f) * Base.Speed * Time.deltaTime;
            Velocity += movingDirection * Base.Speed * Time.deltaTime;

            if (movingDirection.magnitude > Base.DeadZone) {
                GetComponent<PlayerAnimationManager>().Moving(true);
            } else {
                GetComponent<PlayerAnimationManager>().Moving(false);
            }
        }
        
        transform.position += BoundsMovement(Velocity);

        Velocity *= Base.Drag;
    }
    public void Face(Vector2 facingDirection, GameObject MechArt, GameObject EngineArt, GameObject GunArt) {
        // Mech Facing
        if (facingDirection.x < 0f) {
            // Because default is left facing, might need to change the default facing, alternatively if it was right this needs to be negative
            flipDirection = 1;
        } else {
            flipDirection = -1;
        }

        Vector3 tempScale = MechArt.transform.localScale;
        tempScale.x = flipDirection * Mathf.Abs(tempScale.x);
        MechArt.transform.localScale = tempScale;

        // Engine Position
        Vector3 tempPosition = EngineArt.transform.localPosition;
        tempPosition.x = flipDirection * Mathf.Abs(tempPosition.x);
        EngineArt.transform.localPosition = tempPosition;
        tempScale = EngineArt.transform.localScale;
        tempScale.x = flipDirection * Mathf.Abs(tempScale.x);
        EngineArt.transform.localScale = tempScale;

        // Gun Facing
        GunArt.transform.right = facingDirection;
    }

    Vector3 BoundsMovement(Vector2 velocity) {
        Vector3 Position = transform.position + new Vector3(velocity.x, velocity.y, 0f);
        if (Position.x < Bounds.z) {
            if (velocity.x < 0f)
                velocity.x = 0f;
        } else if (Position.x > Bounds.w) {
            if (velocity.x > 0f)
                velocity.x = 0f;
        }

        if (Position.y < Bounds.x) {
            if (velocity.y < 0f)
                velocity.y = 0f;
        } else if (Position.y > Bounds.y) {
            if (velocity.y > 0f)
                velocity.y = 0f;
        }

        return velocity;
    }
    
    void CalculateBounds() {
        // Bounds
        float MapSize = 40f;
        float ScreenSize = 19f;

        CurrentBounds = new Vector4(Camera.main.transform.position.y - Camera.main.orthographicSize,   // Bottom
                                    Camera.main.transform.position.y + Camera.main.orthographicSize,   // Top
                                    Camera.main.transform.position.x - (Camera.main.orthographicSize * Screen.width / Screen.height), // Left
                                    Camera.main.transform.position.x + (Camera.main.orthographicSize * Screen.width / Screen.height)); // Right
        Bounds = new Vector4(Camera.main.transform.position.y - ScreenSize,   // Bottom
                             Camera.main.transform.position.y + ScreenSize,   // Top
                             Camera.main.transform.position.x - (ScreenSize * Screen.width / Screen.height), // Left
                             Camera.main.transform.position.x + (ScreenSize * Screen.width / Screen.height)); // Right

        MapBounds = new Vector4(-MapSize,   // Bottom
                             MapSize,   // Top
                             -(MapSize * Screen.width / Screen.height), // Left
                             (MapSize * Screen.width / Screen.height)); // Right

        // Current Bounds
        Debug.DrawLine(new Vector3(CurrentBounds.z, CurrentBounds.y, 0f), new Vector3(CurrentBounds.w, CurrentBounds.y, 0f), Color.white);
        Debug.DrawLine(new Vector3(CurrentBounds.w, CurrentBounds.y, 0f), new Vector3(CurrentBounds.w, CurrentBounds.x, 0f), Color.white);
        Debug.DrawLine(new Vector3(CurrentBounds.w, CurrentBounds.x, 0f), new Vector3(CurrentBounds.z, CurrentBounds.x, 0f), Color.white);
        Debug.DrawLine(new Vector3(CurrentBounds.z, CurrentBounds.x, 0f), new Vector3(CurrentBounds.z, CurrentBounds.y, 0f), Color.white);

        // Screen Bounds
        Debug.DrawLine(new Vector3(Bounds.z, Bounds.y, 0f), new Vector3(Bounds.w, Bounds.y, 0f), Color.red);
        Debug.DrawLine(new Vector3(Bounds.w, Bounds.y, 0f), new Vector3(Bounds.w, Bounds.x, 0f), Color.red);
        Debug.DrawLine(new Vector3(Bounds.w, Bounds.x, 0f), new Vector3(Bounds.z, Bounds.x, 0f), Color.red);
        Debug.DrawLine(new Vector3(Bounds.z, Bounds.x, 0f), new Vector3(Bounds.z, Bounds.y, 0f), Color.red);

        // Map Bounds
        Debug.DrawLine(new Vector3(MapBounds.z, MapBounds.y, 0f), new Vector3(MapBounds.w, MapBounds.y, 0f), Color.green);
        Debug.DrawLine(new Vector3(MapBounds.w, MapBounds.y, 0f), new Vector3(MapBounds.w, MapBounds.x, 0f), Color.green);
        Debug.DrawLine(new Vector3(MapBounds.w, MapBounds.x, 0f), new Vector3(MapBounds.z, MapBounds.x, 0f), Color.green);
        Debug.DrawLine(new Vector3(MapBounds.z, MapBounds.x, 0f), new Vector3(MapBounds.z, MapBounds.y, 0f), Color.green);
    }
}
