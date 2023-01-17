using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlatformSettingsHandler : SceneSettingsHandler
{
    protected SceneEntry m_SceneEntry;
    protected ShootingPlatformSettings m_Settings;


    public SplineFollower m_SplineFollower;

    protected void Awake()
    {
        //TODO: Setup

        m_SceneEntry = GameManager.Instance.GetSceneEntryByName("ShootingPlatform");
        if (!m_SceneEntry)
            enabled = false;
    }

    protected override void Start()
    {
        m_Settings = m_SceneEntry.m_SceneSettings as ShootingPlatformSettings;
        if (!m_Settings)
            enabled = false;

        base.Start();
    }
    protected override void ApplySettings()
    {
        if(m_SplineFollower)
        {
            m_SplineFollower.m_SpeedMultiplier = m_Settings.m_PlatformSpeed;
            m_SplineFollower.m_MatchDirection = m_Settings.m_FollowRotation;
        }
        
    }
}
