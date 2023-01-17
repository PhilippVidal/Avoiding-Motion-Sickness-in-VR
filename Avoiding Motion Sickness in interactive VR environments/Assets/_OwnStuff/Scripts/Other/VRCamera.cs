using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class VRCamera : MonoBehaviour
{
    protected Transform m_TrackingOverrideTransform;

    protected TrackedPoseDriver m_TrackedPoseDriver;

    public bool TrackingOverrideEnabled { get { return m_TrackingOverrideTransform != null; } }

    protected void Awake()
    {
        m_TrackedPoseDriver = GetComponent<TrackedPoseDriver>();
        TrackerOverride = null;
       
    }
    public Transform TrackerOverride
    {
        get { return m_TrackingOverrideTransform; }
        set
        {
            if (!value)
            {
                DisableOverriding();
                return;
            }

            EnableOverriding(value);
        }
    }

    public void LateUpdate()
    {
        if (!TrackingOverrideEnabled)
            return;


        transform.localPosition = TrackerOverride.localPosition;
        transform.localRotation = TrackerOverride.localRotation;
    }

    protected void EnableOverriding(Transform transform)
    {
        Debug.Log("[VR Camera] Enabling tracking override!");
        m_TrackingOverrideTransform = transform;
        m_TrackedPoseDriver.enabled = false;

    }

    protected void DisableOverriding()
    {
        Debug.Log("[VR Camera] Disabeling tracking override!");
        m_TrackingOverrideTransform = null;
        m_TrackedPoseDriver.enabled = true;
    }

}
