using UnityEngine;

namespace Common.UnityExtensions;

public static class VectorExtensions
{
    public static float Distance(this Vector3 a, Vector3 b) => Vector3.Distance(a, b);

    public static Vector3 Direction(this Vector3 a, Vector3 b, bool normalize = true)
    {
        Vector3 direction = b - a;
        if (normalize)
            direction.Normalize();
        return direction;
    }
    
    public static float Angle(this Vector3 a, Vector3 b) => Vector3.Angle(a, b);
    
    public static bool InRange(this Vector3 a, Vector3 b, float range)
    {
        return a.Distance(b) <= range;
    }
    
    public static bool InViewAngle(this Vector3 a, Vector3 b, float angle)
    {
        angle = Mathf.Clamp(angle, 0, 180);
        return Vector3.Angle(a, b) <= angle;
    }
    
    public static float SignedAngle(this Vector3 from, Vector3 to, Vector3 normal)
    {
        return Vector3.SignedAngle(from, to, normal);
    }
}