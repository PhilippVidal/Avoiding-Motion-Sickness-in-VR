using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "SceneSettings/VehicleSettings")]
public class VehicleSettings : SceneSettings
{
    public int m_RoundCount = 1;

    [Header("Active")]
    public float m_MaxAcceleration = 10;
    public float m_MaxTurnAngle = 45;
    public float m_MaxSpeed = 20;
    public float m_BreakStrength = 10;

    [Header("Passive")]
    public float m_SpeedMultiplier = 2;

}
