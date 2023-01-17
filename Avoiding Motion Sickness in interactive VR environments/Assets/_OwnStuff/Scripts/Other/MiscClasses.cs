using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierCurve
{
    public static PathPoint GetPoint(Transform start, Transform end, float t)
    {

        Vector3[] controlpoints = GetControlPoints(start, end);
        Vector3 p11 = Vector3.Lerp(controlpoints[0], controlpoints[1], t);
        Vector3 p12 = Vector3.Lerp(controlpoints[1], controlpoints[2], t);
        Vector3 p13 = Vector3.Lerp(controlpoints[2], controlpoints[3], t);

        Vector3 p21 = Vector3.Lerp(p11, p12, t);
        Vector3 p22 = Vector3.Lerp(p12, p13, t);

        return new PathPoint(
            Vector3.Lerp(p21, p22, t),          //Position
            p22 - p21,                          //Forward Direction
            Vector3.Lerp(start.up, end.up, t)   //Up Direction
            );
    }

    public static Vector3[] GetControlPoints(Transform p0, Transform p1)
    {
        return new Vector3[] {
            p0.position,
            p0.TransformPoint(Vector3.forward * p0.localScale.z),
            p1.TransformPoint(-1.0f * Vector3.forward * p1.localScale.z),
            p1.position
        };
    }
}

public struct PathPoint
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Direction;

    public float Speed { get { return Direction.magnitude; } }

    public PathPoint(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
        Direction = rotation * Vector3.forward;
    }

    public PathPoint(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        Position = position;
        Rotation = rotation;
        Direction = direction;
    }

    public PathPoint(Vector3 position, Vector3 forward)
    {
        Position = position;
        Rotation = Quaternion.LookRotation(forward);
        Direction = forward;
    }

    public PathPoint(Vector3 position, Vector3 forward, Vector3 up)
    {
        Position = position;
        Rotation = Quaternion.LookRotation(forward, up);
        Direction = forward;
    }


    public Vector3 LocalToWorldPosition(Vector3 local)
    {
        return (Rotation * local) + Position;
    }

    public Vector3 LocalToWorldDirection(Vector3 local)
    {
        return Rotation * local;
    }
}

public static class MiscFunctions
{
    public static int ClampedIndex(int i, int count)
    {
        if (i < 0)
            return 0;

        if (i > count - 1)
            return count - 1;


        return i;
    }

    public static int LoopedIndex(int i, int count)
    {
        return i < 0 ? count + (i % count) - 1 : i % count;
    }
}