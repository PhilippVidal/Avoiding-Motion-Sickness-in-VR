using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spline : MonoBehaviour
{
    [Range(0.0f, 1.0f)] public float m_DebugT = 0.0f;
    public int ControlPointCount { get { return transform.childCount; } }

    public int SegmentCount { get { return transform.childCount - 1; } }

    public PathPoint GetPoint(float t)
    {
        t = Mathf.Clamp01(t);
        int i = GetStartIndex(t);

        return BezierCurve.GetPoint(
            transform.GetChild(i),
            transform.GetChild(i + 1),
            GetAdjustedT(t)
            );
    }

    public int GetStartIndex(float t)
    {       
        if (t < 1.0f)
            return Mathf.FloorToInt(t * SegmentCount);
        
        return ControlPointCount - 2;
    }

    protected float GetAdjustedT(float t)
    {
        if (t < 1.0f)
        {
            t *= SegmentCount;
            return t - (int)t;
        }

        return 1.0f;
    }

#if UNITY_EDITOR
    protected void OnDrawGizmos()
    {

        //Draw all controlpoints
        Gizmos.color = Color.green;
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.1f);
        }


        if (SegmentCount < 1)
            return;

        //Draw all bezier curve segments
        for (int i = 0; i < SegmentCount; i++)
        {
            Vector3[] controlpoints = BezierCurve.GetControlPoints(transform.GetChild(i), transform.GetChild(i + 1));
            Handles.DrawBezier(
                controlpoints[0],
                controlpoints[3],
                controlpoints[1],
                controlpoints[2],
                Color.white,
                null,
                2.0f
                );
        }

        //Draw point at t = m_DebugT
        PathPoint point = GetPoint(m_DebugT);
        Handles.DoPositionHandle(point.Position, point.Rotation);


        //Draw forward direction
        Handles.color = Color.magenta;
        Handles.ArrowHandleCap(
            0,
            point.Position,
            point.Rotation,
            point.Speed,
            EventType.Repaint
            );

        //Draw up direction
        Handles.color = Color.cyan;
        Handles.ArrowHandleCap(
            0,
            point.Position,
            point.Rotation * Quaternion.Euler(-90.0f, 0.0f, 0.0f),
            0.3f,
            EventType.Repaint
            );
    }
#endif

}
