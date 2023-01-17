using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "SceneSettings/SphereDodgingSettings")]
public class SphereDodgingSettings : SceneSettings
{
    [Range(1, 50)] public int m_MaxSphereAmount = 10;
    [Range(0.1f, 10.0f)] public float m_SpawnInterval = 4.0f;
    public bool m_UseRandomSpawnpoints = false;
}
