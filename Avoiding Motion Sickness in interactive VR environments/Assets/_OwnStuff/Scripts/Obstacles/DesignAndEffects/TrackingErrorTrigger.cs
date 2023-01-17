using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackingErrorTrigger : MonoBehaviour
{
    protected TrackingErrorType m_PreviousErrorType;
    protected float m_PreviousMinDuration;
    protected float m_PreviousMaxDuration;
    protected float m_PreviousMinDurationBetween;
    protected float m_PreviousMaxDurationBetween;

    public float m_MinDuration = 1.0f;
    public float m_MaxDuration = 3.0f;

    public float m_MinDurationBetween = 2.0f;
    public float m_MaxDurationBetween = 5.0f;

    protected bool m_WasActiveBefore = false;

    protected void OnTriggerEnter(Collider other)
    {
        if (PlayerController.Instance.gameObject == other.gameObject)
        {
            if (!TrackingErrorSimulator.Exists)
                return;

            if (TrackingErrorSimulator.Instance.IsActive)
            {                
                m_WasActiveBefore = true;
                return;
            }

            Debug.Log("[Tracking Error Simulation] Started simulation through scene trigger!");
            TrackingErrorSimulator simulator = TrackingErrorSimulator.Instance;
            m_PreviousErrorType = simulator.m_SelectedErrorType;
            m_PreviousMinDuration = simulator.m_MinDuration;
            m_PreviousMaxDuration = simulator.m_MaxDuration;
            m_PreviousMinDurationBetween = simulator.m_MinDurationBetween;
            m_PreviousMaxDurationBetween = simulator.m_MaxDurationBetween;

            simulator.m_SelectedErrorType = TrackingErrorType.TRANLATION_ROTATION;
            simulator.m_MinDuration = m_MinDuration;
            simulator.m_MaxDuration = m_MaxDuration;
            simulator.m_MinDurationBetween = m_MinDurationBetween;
            simulator.m_MaxDurationBetween = m_MaxDurationBetween;


            TrackingErrorSimulator.Instance.StartErrorSimulation();
        }
            //PlayerController.Instance.TrackingErrorSimulationEnabled = true;
    }

    protected void OnTriggerExit(Collider other)
    {
        if (PlayerController.Instance.gameObject == other.gameObject)
        {
            if (!TrackingErrorSimulator.Exists)
                return;

            if (!m_WasActiveBefore)
            {
                Debug.Log("[Tracking Error Simulation] Stopped simulation through scene trigger!");
                TrackingErrorSimulator.Instance.StopErrorSimulation();

                TrackingErrorSimulator simulator = TrackingErrorSimulator.Instance;
                simulator.m_SelectedErrorType = m_PreviousErrorType;
                simulator.m_MinDuration = m_PreviousMinDuration;
                simulator.m_MaxDuration = m_PreviousMaxDuration;
                simulator.m_MinDurationBetween = m_PreviousMinDurationBetween;
                simulator.m_MaxDurationBetween = m_PreviousMaxDurationBetween;
            }
               
        }            
    }
}
