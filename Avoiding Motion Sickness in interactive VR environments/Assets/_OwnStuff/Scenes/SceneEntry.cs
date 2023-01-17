using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene Data", menuName = "Scene Entry")]
public class SceneEntry : ScriptableObject
{   
    public string m_LoadName;
    public bool m_IsSelected = true;
    public string m_DisplayName;
    public string m_Description;

    public GameObject m_SettingsPrefab;
    public SceneSettings m_SceneSettings;
}
