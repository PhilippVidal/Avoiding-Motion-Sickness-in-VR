using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDodgingSettingsHandler : SceneSettingsHandler
{
    protected SceneEntry m_SceneEntry;
    protected SphereDodgingSettings m_Settings;

    public SphereSpawner m_SphereSpawner;


    protected void Awake()
    {
        m_SceneEntry = GameManager.Instance.GetSceneEntryByName("SphereDodging");
        if (!m_SceneEntry)
            enabled = false;
    }

    protected override void Start()
    {
        m_Settings = m_SceneEntry.m_SceneSettings as SphereDodgingSettings;
        if (!m_Settings)
            enabled = false;

        base.Start();
    }

    protected override void ApplySettings()
    {
        if (!m_SphereSpawner)
            return;

        m_SphereSpawner.m_UseRandomSpawnpoint = m_Settings.m_UseRandomSpawnpoints;
        m_SphereSpawner.m_SpawnInterval = m_Settings.m_SpawnInterval;
        m_SphereSpawner.m_MaxSphereAmount = m_Settings.m_MaxSphereAmount;
        m_SphereSpawner.ActivateSpawner();
    }
}
