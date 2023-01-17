using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "SceneSettings/RotatingPlatformsSettings")]
public class RotatingPlatformsSettings : SceneSettings
{
    public bool m_PlatformsActive = true;
    //public bool m_AllSpeedsRandom = true;
    public bool m_IndicateDirection = false;
    [Range(0, 90)] public float m_MaxRotationSpeed = 50f;
    [Range(0, 90)] public float m_MinRotationSpeed = 5f;
    [Range(-1, 1)] public int m_Direction = 0;    
}


public class SceneSettings : ScriptableObject
{

}
