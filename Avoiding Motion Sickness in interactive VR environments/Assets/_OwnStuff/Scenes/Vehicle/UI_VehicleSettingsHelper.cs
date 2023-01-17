using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_VehicleSettingsHelper : UI_SceneSettingsHelper
{
    public Slider m_RoundCountSlider;

    public Slider m_MaxAccelerationSlider;
    public Slider m_MaxTurnAngleSlider;
    public Slider m_MaxSpeedSlider;
    public Slider m_BreakStrengthSlider;

    public Slider m_SpeedMultiplierSlider;


    protected VehicleSettings m_SceneSettings;

    protected void Start()
    {
        m_RoundCountSlider.value = m_SceneSettings.m_RoundCount;
        m_MaxAccelerationSlider.value = m_SceneSettings.m_MaxAcceleration;
        m_MaxTurnAngleSlider.value = m_SceneSettings.m_MaxTurnAngle;
        m_MaxSpeedSlider.value = m_SceneSettings.m_MaxSpeed;
        m_BreakStrengthSlider.value = m_SceneSettings.m_BreakStrength;
        m_SpeedMultiplierSlider.value = m_SceneSettings.m_SpeedMultiplier;
    }

    public override void SetSceneSettings(SceneSettings settings)
    {
        m_SceneSettings = settings as VehicleSettings;
    }



    public void UpdateRoundCount(float value)
    {
        m_SceneSettings.m_RoundCount = (int)value;
    }
    public void UpdateMaxAcceleration(float value)
    {
        m_SceneSettings.m_MaxAcceleration = value;
    }

    public void UpdateMaxTurnAngle(float value)
    {
        m_SceneSettings.m_MaxTurnAngle = value;
    }

    public void UpdateMaxSpeed(float value)
    {
        m_SceneSettings.m_MaxSpeed = value;
    }

    public void UpdateBreakStrength(float value)
    {
        m_SceneSettings.m_BreakStrength = value;
    }

    public void UpdateSpeedMultiplier(float value)
    {
        m_SceneSettings.m_SpeedMultiplier = value;
    }

}
