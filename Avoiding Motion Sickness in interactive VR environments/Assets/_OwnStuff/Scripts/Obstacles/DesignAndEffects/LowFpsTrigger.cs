using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class LowFpsTrigger : MonoBehaviour
{
    //[Range(5, 300)] public int m_TargetFramerate = 30;

    protected float m_PreviousTargetFramerate;
    protected bool m_PreviousFramerateLimitEnabled;
    public int m_FramerateToSimulate = 20;


    protected bool m_WasActiveBefore = false;

    protected void OnTriggerEnter(Collider other)
    {
        
        if (PlayerController.Instance.gameObject == other.gameObject)
        {

            if (!FrameLimiter.Exists)
                return;

            if (FrameLimiter.Instance.IsLimitingFrames)
            {
                m_WasActiveBefore = true;
                return;
            }

            FrameLimiter limiter = FrameLimiter.Instance;
            Debug.Log("[Frame Limiter] Started limiting framerate through scene trigger!");
            m_PreviousFramerateLimitEnabled = limiter.m_FramerateLimitEnabled;
            m_PreviousTargetFramerate = (float)limiter.m_FramerateLimit;
            limiter.m_FramerateLimitEnabled = true;
            limiter.m_FramerateLimit = m_FramerateToSimulate;
            limiter.StartLimitingFramerate();
        }
            
    }

    protected void OnTriggerExit(Collider other)
    {
        if (PlayerController.Instance.gameObject == other.gameObject)
        {
            if (!FrameLimiter.Exists)
                return;

            if (!m_WasActiveBefore)
            {
                Debug.Log("[Frame Limiter] Stopped limiting framerate through scene trigger!");
                FrameLimiter limiter = FrameLimiter.Instance;
                limiter.StopLimitingFramerate();
                limiter.m_FramerateLimitEnabled = m_PreviousFramerateLimitEnabled;
                limiter.m_FramerateLimit = (int)m_PreviousTargetFramerate;
            }
        }

    }
}
