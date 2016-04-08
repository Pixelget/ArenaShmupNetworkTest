using UnityEngine;

public static class Base {
    public static float DeadZone = 0.1f;
    public static float Speed = 6.5f;
    public static float ProjectileSpeed = 100f;
    public static float BoostModifier = 7.5f;
    public static float BoostTime = 0.075f;
    public static float Drag = 0.7f;

    public static Vector2 RotateV2(this Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);

        return v;
    }
}
