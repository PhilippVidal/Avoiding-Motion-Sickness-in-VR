using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RollercoasterSettingsHelper : UI_SceneSettingsHelper
{
    public Slider m_SpeedMultiplierSlider;
    public Slider m_RoundCountSlider;

    protected RollercoasterSettings m_SceneSettings;

    protected void Start()
    {
        m_SpeedMultiplierSlider.value = m_SceneSettings.m_SpeedMultiplier;
        m_RoundCountSlider.value = m_SceneSettings.m_RoundCount;
    }

    public override void SetSceneSettings(SceneSettings settings)
    {
        m_SceneSettings = settings as RollercoasterSettings;
    }

   
    public void UpdateSpeedMultiplier(float value) => m_SceneSettings.m_SpeedMultiplier = value;
    public void UpdateRoundCount(float value) => m_SceneSettings.m_RoundCount = (int)value;

}
