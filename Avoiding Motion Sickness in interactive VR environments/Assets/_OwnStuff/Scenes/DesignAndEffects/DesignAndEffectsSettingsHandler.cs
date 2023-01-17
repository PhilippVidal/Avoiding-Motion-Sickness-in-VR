using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignAndEffectsSettingsHandler : SceneSettingsHandler
{
    protected SceneEntry m_SceneEntry;
    protected DesignAndEffectsSettings m_Settings;
    public InstantFalling m_FastFalling;
    public Material m_VerticalTunnelMaterial;
    public Material m_HorizontalTunnelMaterial;
    public VerticalMotionTunnelAudio m_VerticalMTAudio;

    protected void Awake()
    {
        m_SceneEntry = GameManager.Instance.GetSceneEntryByName("DesignAndEffects");
        if (!m_SceneEntry)
            enabled = false;
    }

    protected override void Start()
    {
        m_Settings = m_SceneEntry.m_SceneSettings as DesignAndEffectsSettings;
        if (!m_Settings)
            enabled = false;

        base.Start();

    }
    protected override void ApplySettings()
    {
        if (m_FastFalling)
            m_FastFalling.m_GravityMultiplier = m_Settings.m_FastFallGravityMultiplier;

        if (!m_Settings)
            return;

        if(m_VerticalTunnelMaterial)
        {
            MotionTunnelSettings settings = m_Settings.m_VerticalVectionTunnelSettings;
            m_VerticalTunnelMaterial.SetFloat("_Speed", settings.m_RotationSpeed);
            //m_VerticalTunnelMaterial.SetInteger("_HorizontalSpeed", settings.m_MovesHorizontally ? 1 : 0);
            //m_VerticalTunnelMaterial.SetInteger("_VerticalSpeed", settings.m_MovesVertically ? 1 : 0);
            m_VerticalTunnelMaterial.SetFloat("_ScaleU", settings.m_ScaleU);
            m_VerticalTunnelMaterial.SetFloat("_ScaleV", settings.m_ScaleV);

            if (m_VerticalMTAudio)
            {
                m_VerticalMTAudio.m_MaterialSpeed = settings.m_RotationSpeed;
                m_VerticalMTAudio.m_AudioOn = settings.m_AudioOn;
            }           
        }

        if (m_HorizontalTunnelMaterial)
        {
            MotionTunnelSettings settings = m_Settings.m_HorizontalVectionTunnelSettings;
            m_HorizontalTunnelMaterial.SetFloat("_Speed", settings.m_RotationSpeed);
            //m_HorizontalTunnelMaterial.SetInteger("_HorizontalSpeed", settings.m_MovesHorizontally ? 1 : 0);
            //m_HorizontalTunnelMaterial.SetInteger("_VerticalSpeed", settings.m_MovesVertically ? 1 : 0);
            m_HorizontalTunnelMaterial.SetFloat("_ScaleU", settings.m_ScaleU);
            m_HorizontalTunnelMaterial.SetFloat("_ScaleV", settings.m_ScaleV);
        }
    }


    public void UpdateSettings()
    {
        ApplySettings();
    }
}
