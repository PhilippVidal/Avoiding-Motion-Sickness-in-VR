using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollercoasterSettingsHandler : SceneSettingsHandler
{
    protected SceneEntry m_SceneEntry;
    protected RollercoasterSettings m_Settings;

    public Rollercoaster m_Rollercoaster;

    protected void Awake()
    {
        if (!m_Rollercoaster)
            enabled = false;
        

        m_SceneEntry = GameManager.Instance.GetSceneEntryByName("Rollercoaster");
        if (!m_SceneEntry)
            enabled = false;
    }

    protected override void Start()
    {
        m_Settings = m_SceneEntry.m_SceneSettings as RollercoasterSettings;
        if (!m_Settings)
            enabled = false;

        base.Start();
    }
    protected override void ApplySettings()
    {
        m_Rollercoaster.m_SpeedMultiplier = m_Settings.m_SpeedMultiplier;
        m_Rollercoaster.m_Rounds = m_Settings.m_RoundCount;
    }
}
