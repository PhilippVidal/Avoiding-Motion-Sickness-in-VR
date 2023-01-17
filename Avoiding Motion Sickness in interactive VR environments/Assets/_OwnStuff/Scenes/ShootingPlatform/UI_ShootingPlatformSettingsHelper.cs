using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShootingPlatformSettingsHelper : UI_SceneSettingsHelper
{
    public Toggle m_FollowRotationToggle;
    public Slider m_PlatformSpeedSlider;


    protected ShootingPlatformSettings m_SceneSettings;

    public override void SetSceneSettings(SceneSettings settings)
    {
        m_SceneSettings = settings as ShootingPlatformSettings;
    }

    void Start()
    {
        m_FollowRotationToggle.isOn = m_SceneSettings.m_FollowRotation;
        m_PlatformSpeedSlider.value = m_SceneSettings.m_PlatformSpeed;
    }

    public void UpdateFollowRotation(bool value) => m_SceneSettings.m_FollowRotation = value;
    public void UpdatePlatformSpeed(float value) => m_SceneSettings.m_PlatformSpeed = value;

}
