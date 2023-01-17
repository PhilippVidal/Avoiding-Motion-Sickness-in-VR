using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSettingsHandler : SceneSettingsHandler
{
    protected SceneEntry m_SceneEntry;
    protected VehicleSettings m_Settings;

    public VehicleObstacle m_VehicleObstacle;
    public Vehicle m_Vehicle;
    public SplineFollower m_SplineFollower;

    protected void Awake()
    {
        m_SceneEntry = GameManager.Instance.GetSceneEntryByName("Vehicle");
        if (!m_SceneEntry)
            enabled = false;
    }

    protected override void Start()
    {
        m_Settings = m_SceneEntry.m_SceneSettings as VehicleSettings;
        if (!m_Settings)
            enabled = false;

        base.Start();
    }

    protected override void ApplySettings()
    {
        m_Vehicle.m_MaxAcceleration = m_Settings.m_MaxAcceleration;
        m_Vehicle.m_MaxTurnAngle = m_Settings.m_MaxTurnAngle;
        m_Vehicle.m_MaxSpeed = m_Settings.m_MaxSpeed;
        m_Vehicle.m_BreakStrength = m_Settings.m_BreakStrength;
        m_VehicleObstacle.m_LapCount = m_Settings.m_RoundCount;
        m_SplineFollower.m_SpeedMultiplier = m_Settings.m_SpeedMultiplier;
        m_SplineFollower.m_RoundsToComplete = m_Settings.m_RoundCount;
    }
}
