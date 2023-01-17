using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformSettingsHandler : SceneSettingsHandler
{
    protected SceneEntry m_SceneEntry;
    protected RotatingPlatformsSettings m_Settings; 
    protected List<RotatingPlatform> m_RotatingPlatforms;
    protected float m_MinSpeed = 5.0f;
    protected float m_MaxSpeed = 60.0f;

    public Texture m_DirectionIndicationTexture;
    public Texture m_StandardTexture;
    public Material m_PlatformMaterial; 

    protected void Awake()
    {
        m_RotatingPlatforms = new List<RotatingPlatform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            RotatingPlatform platform = transform.GetChild(i).GetComponent<RotatingPlatform>();
            if(platform)
                m_RotatingPlatforms.Add(platform);
        }


        m_SceneEntry = GameManager.Instance.GetSceneEntryByName("RotatingPlatforms");
        if(!m_SceneEntry)
            enabled = false;

        //SetRandomSpeeds();
        //ActivateAll();
    }

    protected override void Start()
    {
        m_Settings = m_SceneEntry.m_SceneSettings as RotatingPlatformsSettings;
        if(!m_Settings)
            enabled = false;

        base.Start();   
    }

    protected override void ApplySettings()
    {
        ToggleIndicationTextureOnAllPlatforms(m_Settings.m_IndicateDirection);

        float sign = m_Settings.m_Direction;

        m_MinSpeed = m_Settings.m_MinRotationSpeed;
        m_MaxSpeed = m_Settings.m_MaxRotationSpeed;

        if (m_MinSpeed > m_MaxSpeed)
            m_MaxSpeed = m_MinSpeed;

       
        foreach (RotatingPlatform platform in m_RotatingPlatforms)
        {
            float signToUse = sign;
            if (sign == 0)
                signToUse = Random.value > 0.5f ? 1.0f : -1.0f;

            platform.m_RotationSpeed = signToUse * (m_MinSpeed + Random.value * (m_MaxSpeed - m_MinSpeed));
        }        
        
        if(m_Settings.m_PlatformsActive)
        {
            ActivateAll();
            return;
        }

        DeactivateAll();
    }

    protected void ToggleIndicationTextureOnAllPlatforms(bool value)
    {
        if (value)
        {
            m_PlatformMaterial.SetTexture("_BaseMap", m_DirectionIndicationTexture);
            return;
        }

        m_PlatformMaterial.SetTexture("_BaseMap", m_StandardTexture);
    }

    protected void SetRandomSpeeds()
    {
        foreach (RotatingPlatform platform in m_RotatingPlatforms)
        {
            float sign = Random.value > 0.5f ? 1.0f : -1.0f;

            platform.m_RotationSpeed =  sign * ( m_MinSpeed + Random.value * (m_MaxSpeed - m_MinSpeed));
        }
    }

    public void ActivateAll()
    {
        foreach (RotatingPlatform platform in m_RotatingPlatforms)
            platform.Activate();
    }

    public void DeactivateAll()
    {
        foreach (RotatingPlatform platform in m_RotatingPlatforms)
            platform.Deactivate();
    }
}
