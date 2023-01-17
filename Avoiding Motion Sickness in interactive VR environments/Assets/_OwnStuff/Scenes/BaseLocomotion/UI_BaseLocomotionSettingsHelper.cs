using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BaseLocomotionSettingsHelper : UI_SceneSettingsHelper
{

    public Slider m_SwingingSpeedMultiplierSlider;
    public Slider m_RollSpeedMultiplierSlider;

    protected BaseLocomotionSettings m_SceneSettings;

    public override void SetSceneSettings(SceneSettings settings)
    {
        m_SceneSettings = settings as BaseLocomotionSettings;
    }

    void Start()
    {
        m_SwingingSpeedMultiplierSlider.value = m_SceneSettings.m_SwingingSpeedMultiplier;
        m_RollSpeedMultiplierSlider.value = m_SceneSettings.m_RollSpeed;
    }

    public void UpdateSwingingSpeedMultiplier(float value) => m_SceneSettings.m_SwingingSpeedMultiplier = value;
    public void UpdateRollSpeed(float value) => m_SceneSettings.m_RollSpeed = value;
}
