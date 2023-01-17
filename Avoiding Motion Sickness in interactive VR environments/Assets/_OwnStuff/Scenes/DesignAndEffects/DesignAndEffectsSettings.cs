using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "SceneSettings/DesignAndEffectsSettings")]
public class DesignAndEffectsSettings : SceneSettings
{
    [Range(1.0f, 1000.0f)] public float m_FastFallGravityMultiplier = 10.0f;
    public MotionTunnelSettings m_VerticalVectionTunnelSettings;
    public MotionTunnelSettings m_HorizontalVectionTunnelSettings;
}



[System.Serializable]
public class MotionTunnelSettings
{
    public bool m_AudioOn = false;
    [Range(0.0f, 1.0f)] public float m_RotationSpeed = 0.1f;
    public bool m_MovesHorizontally = false;
    public bool m_MovesVertically = true;
    [Range(-5.0f, 5.0f)] public float m_ScaleU = 0.1f;
    [Range(-5.0f, 5.0f)] public float m_ScaleV = 0.1f;   
}
