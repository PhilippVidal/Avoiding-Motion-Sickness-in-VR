using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "SceneSettings/ShootingPlatformSettings")]
public class ShootingPlatformSettings : SceneSettings
{
    public bool m_FollowRotation = false;
    [Range(0, 10)] public float m_PlatformSpeed = 1.0f;
}
