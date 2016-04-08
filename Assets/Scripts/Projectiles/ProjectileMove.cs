using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public enum ProjectileType { Bullet, Missile }
public class ProjectileMove : SimpleNetworkedMonoBehavior {
    ProjectileType type = ProjectileType.Bullet;
    GameObject target;
	Vector2 direction;
	Vector3 velocity;
    float range;
    Vector2 startPosition;

    void Awake() {
        //AddNetworkVariable(() => direction, x => direction = (Vector2) x);
    }

    public void InitProjectile(Vector2 direction, float range) {
        this.direction = direction;
        this.type = ProjectileType.Bullet;
        this.range = range;

        startPosition = transform.position;

        FaceDirection();
    }
    public void InitProjectile(GameObject target, float range) {
        this.target = target;
        this.type = ProjectileType.Missile;
        this.range = range;

        startPosition = transform.position;

        FaceDirection();
    }

    void Update () {
        // Range Check
        if ((transform.position - new Vector3(startPosition.x, startPosition.y, 0f)).magnitude > range) {
            // Destroy
            RPC("DestroyProjectile");
        }
        
        // Movement
        switch (type) {
            case ProjectileType.Bullet:
                StraightMove();
                break;
            case ProjectileType.Missile:
                TrackMovement();
                break;
        }
        FaceDirection();
	}

    [BRPC]
    void DestroyProjectile() {
        //Networking.Destroy(this);
        //Destroy(this);
    }

    void StraightMove() {
        velocity = direction.normalized * Base.ProjectileSpeed * Time.deltaTime;

        transform.position += velocity;
        velocity *= Base.Drag;
    }

    void TrackMovement() {

    }

    public void FaceDirection() {
		float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}


    protected override void OnApplicationQuit() {
        base.OnApplicationQuit();

        // Delete local players here
        Networking.Destroy(this);
    }
}
