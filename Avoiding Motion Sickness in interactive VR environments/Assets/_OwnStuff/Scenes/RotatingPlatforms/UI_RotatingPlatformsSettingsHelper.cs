using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RotatingPlatformsSettingsHelper : UI_SceneSettingsHelper
{
    public Toggle m_PlatformsActiveToggle;
    public Toggle m_AllRandomToggle;
    public Toggle m_IndicateDirectionToggle;

    public Slider m_MaxSpeedSlider;
    public Slider m_MinSpeedSlider;
    public Slider m_RotationDirectionSlider;

    protected RotatingPlatformsSettings m_SceneSettings;

    protected void Start()
    {
        m_PlatformsActiveToggle.isOn = m_SceneSettings.m_PlatformsActive;
        //m_AllRandomToggle.isOn = m_SceneSettings.m_AllSpeedsRandom;
        m_IndicateDirectionToggle.isOn = m_SceneSettings.m_IndicateDirection;

        m_MaxSpeedSlider.value = m_SceneSettings.m_MaxRotationSpeed;
        m_MinSpeedSlider.value = m_SceneSettings.m_MinRotationSpeed;
        m_RotationDirectionSlider.value = m_SceneSettings.m_Direction;

    }

    public override void SetSceneSettings(SceneSettings settings)
    {
        m_SceneSettings = settings as RotatingPlatformsSettings;
    }

    public void UpdatePlatformActive(bool value)
    {
        m_SceneSettings.m_PlatformsActive = value;
    }

    //public void UpdateAllSpeedsRandom(bool value)
    //{
        //m_SceneSettings.m_AllSpeedsRandom = value;
    //}

    public void UpdateIndicateDirection(bool value)
    {
        m_SceneSettings.m_IndicateDirection = value;
    }

    public void UpdateMaxRotationSpeed(float value)
    {
        m_SceneSettings.m_MaxRotationSpeed = value;
    }

    public void UpdateMinRotationSpeed(float value)
    {
        m_SceneSettings.m_MinRotationSpeed = value;
    }

    public void UpdateRotationDirection(float value)
    {
        m_SceneSettings.m_Direction = (int)value;
    }
}
