using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLocomotionSettingsHandler : SceneSettingsHandler
{
    protected SceneEntry m_SceneEntry;
    protected BaseLocomotionSettings m_Settings;
    protected List<SwingingObstacle> m_SwingingObstacles;
    protected List<Roll> m_Rolls;
    public GameObject m_SwingingObstacleHolder;
    public GameObject m_RollHolder;
    //protected float m_SwingingSpeedMultiplier = 1.0f;
    //protected float m_RollSpeedMultiplier = 1.0f;

    //protected void Awake()
    //{
    //    m_SwingingObstacles = new List<SwingingObstacle>();
    //    m_Rolls = new List<Roll>();

    //    for (int i = 0; i < m_SwingingObstacleHolder.transform.childCount; i++)
    //    {
    //        SwingingObstacle swingingObstacle = m_SwingingObstacleHolder.transform.GetChild(i).GetComponent<SwingingObstacle>();
    //        if (swingingObstacle)
    //            m_SwingingObstacles.Add(swingingObstacle);           
    //    }

    //    for (int i = 0; i < m_RollHolder.transform.childCount; i++)
    //    {
    //        Roll roll = m_RollHolder.transform.GetChild(i).GetComponent<Roll>();
    //        if (roll)
    //            m_Rolls.Add(roll);
    //    }
            

    //    m_SceneEntry = GameManager.Instance.GetSceneEntryByName("BaseLocomotion");
    //    if (!m_SceneEntry)
    //        enabled = false;
    //}

    protected override void Start()
    {
        m_SwingingObstacles = new List<SwingingObstacle>();
        m_Rolls = new List<Roll>();

        for (int i = 0; i < m_SwingingObstacleHolder.transform.childCount; i++)
        {
            SwingingObstacle swingingObstacle = m_SwingingObstacleHolder.transform.GetChild(i).GetComponent<SwingingObstacle>();
            if (swingingObstacle)
                m_SwingingObstacles.Add(swingingObstacle);
        }

        for (int i = 0; i < m_RollHolder.transform.childCount; i++)
        {
            Roll roll = m_RollHolder.transform.GetChild(i).GetComponent<Roll>();
            if (roll)
                m_Rolls.Add(roll);
        }


        m_SceneEntry = GameManager.Instance.GetSceneEntryByName("BaseLocomotion");
        if (!m_SceneEntry)
            enabled = false;

        m_Settings = m_SceneEntry.m_SceneSettings as BaseLocomotionSettings;
        if (!m_Settings)
            enabled = false;

        base.Start();
    }
    protected override void ApplySettings()
    {
        foreach(SwingingObstacle swingingObstacle in m_SwingingObstacles)
        {
            swingingObstacle.m_SpeedMultiplier = m_Settings.m_SwingingSpeedMultiplier;
        }


        foreach (Roll roll in m_Rolls)
        {
            roll.m_RotationSpeed = m_Settings.m_RollSpeed;
        }
    }
}
