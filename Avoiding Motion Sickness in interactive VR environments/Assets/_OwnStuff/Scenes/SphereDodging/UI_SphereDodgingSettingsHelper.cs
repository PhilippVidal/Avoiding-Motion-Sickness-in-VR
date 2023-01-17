using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SphereDodgingSettingsHelper : UI_SceneSettingsHelper
{
    public Toggle m_UseRandomSpawnpointsToggle;
    public Slider m_MaxSphereAmountSlider;
    public Slider m_SpawnIntervalSlider;

    protected SphereDodgingSettings m_SceneSettings;

    protected void Start()
    {
        m_UseRandomSpawnpointsToggle.isOn = m_SceneSettings.m_UseRandomSpawnpoints;
        m_MaxSphereAmountSlider.value = m_SceneSettings.m_MaxSphereAmount;
        m_SpawnIntervalSlider.value = m_SceneSettings.m_SpawnInterval;
    }

    public override void SetSceneSettings(SceneSettings settings)
    {
        m_SceneSettings = settings as SphereDodgingSettings;
    }

    public void UpdateUseRandomSpawnpoints(bool value) => m_SceneSettings.m_UseRandomSpawnpoints = value;
    public void UpdateMaxSphereAmount(float value) => m_SceneSettings.m_MaxSphereAmount = (int)value;
    public void UpdateSpawnInterval(float value) => m_SceneSettings.m_SpawnInterval = value;
}
