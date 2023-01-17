using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DesignAndEffectsSettingsHelper : UI_SceneSettingsHelper
{
    public Slider m_FallingGravityMultiplierSlider;

    public Toggle m_VMTVectionAudioEffectToggle;
    public Slider m_VMTRotationSpeedSlider;
    public Toggle m_VMTMovesHorizontallyToggle;
    public Toggle m_VMTMovesVerticalToggle;
    public Slider m_VMTScaleUSlider;
    public Slider m_VMTScaleVSlider;


    public Slider m_HMTRotationSpeedSlider;
    public Toggle m_HMTMovesHorizontallyToggle;
    public Toggle m_HMTMovesVerticalToggle;
    public Slider m_HMTScaleUSlider;
    public Slider m_HMTScaleVSlider;

    protected DesignAndEffectsSettings m_SceneSettings;

    public override void SetSceneSettings(SceneSettings settings)
    {
        m_SceneSettings = settings as DesignAndEffectsSettings;
    }

    void Start()
    {
        m_FallingGravityMultiplierSlider.value = m_SceneSettings.m_FastFallGravityMultiplier;

        m_VMTVectionAudioEffectToggle.isOn = m_SceneSettings.m_VerticalVectionTunnelSettings.m_AudioOn;
        m_VMTRotationSpeedSlider.value = m_SceneSettings.m_VerticalVectionTunnelSettings.m_RotationSpeed;
        m_VMTMovesHorizontallyToggle.isOn = m_SceneSettings.m_VerticalVectionTunnelSettings.m_MovesHorizontally;
        m_VMTMovesVerticalToggle.isOn = m_SceneSettings.m_VerticalVectionTunnelSettings.m_MovesVertically;
        m_VMTScaleUSlider.value = m_SceneSettings.m_VerticalVectionTunnelSettings.m_ScaleU;
        m_VMTScaleVSlider.value = m_SceneSettings.m_VerticalVectionTunnelSettings.m_ScaleV;


        m_HMTRotationSpeedSlider.value = m_SceneSettings.m_HorizontalVectionTunnelSettings.m_RotationSpeed;
        m_HMTMovesHorizontallyToggle.isOn = m_SceneSettings.m_HorizontalVectionTunnelSettings.m_MovesHorizontally;
        m_HMTMovesVerticalToggle.isOn = m_SceneSettings.m_HorizontalVectionTunnelSettings.m_MovesVertically;
        m_HMTScaleUSlider.value = m_SceneSettings.m_HorizontalVectionTunnelSettings.m_ScaleU;
        m_HMTScaleVSlider.value = m_SceneSettings.m_HorizontalVectionTunnelSettings.m_ScaleV;
    }

    public void UpdateFallingGravityMultiplier(float value) => m_SceneSettings.m_FastFallGravityMultiplier = value;


    public void UpdateVMTAudioOn(bool value) => m_SceneSettings.m_VerticalVectionTunnelSettings.m_AudioOn = value;
    public void UpdateVMTRotationSpeed(float value) => m_SceneSettings.m_VerticalVectionTunnelSettings.m_RotationSpeed = value;
    public void UpdateVMTMovesHorizontally(bool value) => m_SceneSettings.m_VerticalVectionTunnelSettings.m_MovesHorizontally = value;
    public void UpdateVMTMovesVertically(bool value) => m_SceneSettings.m_VerticalVectionTunnelSettings.m_MovesVertically = value;
    public void UpdateVMTScaleU(float value) => m_SceneSettings.m_VerticalVectionTunnelSettings.m_ScaleU = value;
    public void UpdateVMTScaleV(float value) => m_SceneSettings.m_VerticalVectionTunnelSettings.m_ScaleV = value;

    public void UpdateHMTRotationSpeed(float value) => m_SceneSettings.m_HorizontalVectionTunnelSettings.m_RotationSpeed = value;
    public void UpdateHMTMovesHorizontally(bool value) => m_SceneSettings.m_HorizontalVectionTunnelSettings.m_MovesHorizontally = value;
    public void UpdateHMTMovesVertically(bool value) => m_SceneSettings.m_HorizontalVectionTunnelSettings.m_MovesVertically = value;
    public void UpdateHMTScaleU(float value) => m_SceneSettings.m_HorizontalVectionTunnelSettings.m_ScaleU = value;
    public void UpdateHMTScaleV(float value) => m_SceneSettings.m_HorizontalVectionTunnelSettings.m_ScaleV = value;

}
