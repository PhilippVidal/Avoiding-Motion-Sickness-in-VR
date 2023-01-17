using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleObstacle : MonoBehaviour
{

    public Vehicle m_Vehicle;
    public SplineFollower m_SplineFollower;
    public Checkpoint[] m_Checkpoints;
    public int m_CurrentLap = 1;
    public int m_LapCount = 1;

    protected bool m_IsPassive = true;

    protected void Start()
    {
        ResetCheckpoints();
    }


    protected void Reset()
    {
        ResetCheckpoints();
        m_CurrentLap = 1;
    }

    public void ResetCheckpoints()
    {
        foreach (Checkpoint cp in m_Checkpoints)
            cp.m_Triggered = false;
    }

    public void StartObstacleActive()
    {
        Reset();
        m_Vehicle.GetIn();
        m_IsPassive = false;
    }

    public void StartObstaclePassive()
    {       
        Reset();
        m_SplineFollower.StartTraversing();
        m_IsPassive = true;
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
            OnFinishPassed();

    }

    protected void OnFinishPassed()
    {
        bool allCheckpointsPassed = true;

        foreach (Checkpoint cp in m_Checkpoints)
        {
            if (!cp.m_Triggered)
                allCheckpointsPassed = false;
        }


        if (allCheckpointsPassed)
            OnLapCompleted();
    }

    protected void OnLapCompleted()
    {
        ResetCheckpoints();

        if (m_CurrentLap < m_LapCount)
        {
            m_CurrentLap++;
            return;
        }
            
        if(!m_IsPassive)
            m_Vehicle.GetOut();
    }
}
