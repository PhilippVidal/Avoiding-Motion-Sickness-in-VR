using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Scene Data", menuName = "SceneSettings/BaseLocomotionSettings")]
public class BaseLocomotionSettings : SceneSettings
{
    public float m_SwingingSpeedMultiplier = 1.0f;
    public float m_RollSpeed = 1.0f;
}
