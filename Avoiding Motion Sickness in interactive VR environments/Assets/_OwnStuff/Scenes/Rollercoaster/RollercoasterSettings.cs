using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "SceneSettings/RollercoasterSettings")]
public class RollercoasterSettings : SceneSettings
{
    [Range(0, 5)] public float m_SpeedMultiplier = 1.0f;
    [Range(0, 5)] public int m_RoundCount = 1;
}
