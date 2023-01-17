using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class FrameLimiter : MonoBehaviour
{

    [Range(10, 60)] public int m_FramerateLimit = 60;
    public bool m_FramerateLimitEnabled = false;

    protected Coroutine m_FrameLimitingCoroutine;

    public bool IsLimitingFrames { get { return m_FramerateLimitEnabled && m_FrameLimitingCoroutine != null; } }


    public static FrameLimiter Instance;
    public static bool Exists { get { return Instance != null; } }


    protected void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void StartLimitingFramerate()
    {
        if (!m_FramerateLimitEnabled)
            return;

        if (m_FramerateLimit < 10)
            return;

        if (PlayerController.Instance.IsInMenuRoom)
            return;


        StopLimitingFramerate();
        EnableUpdateOnly();
        m_FrameLimitingCoroutine = StartCoroutine(FrameLimitingCoroutine());
        Debug.Log("[Frame Limiter] Framerate limiting has been started!");
    }

    protected void EnableUpdateOnly()
    {
        UnityEngine.SpatialTracking.TrackedPoseDriver driver =
            PlayerController.Instance.m_VRCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
        if (!driver)
            return;

        driver.updateType = UnityEngine.SpatialTracking.TrackedPoseDriver.UpdateType.Update;
    }

    protected void DisableUpdateOnly()
    {
        UnityEngine.SpatialTracking.TrackedPoseDriver driver =
            PlayerController.Instance.m_VRCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
        if (!driver)
            return;

        driver.updateType = UnityEngine.SpatialTracking.TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;
    }

    public void StopLimitingFramerate()
    {
        if (m_FrameLimitingCoroutine == null)
            return;
        
        StopCoroutine(m_FrameLimitingCoroutine);
        m_FrameLimitingCoroutine = null;
        DisableUpdateOnly();
        Debug.Log("[Frame Limiter] Framerate limiting has been stopped!");
    }


    protected IEnumerator FrameLimitingCoroutine()
    {
        while(m_FramerateLimitEnabled)
        {
            yield return new WaitForEndOfFrame();


            float deltaTime = Time.deltaTime;
            float currentFramerate = 1 / deltaTime;

            float timeToSleep =
            currentFramerate < (float)m_FramerateLimit ? 0 : 1.0f / (float)m_FramerateLimit;
            //Debug.Log("Time To Sleep => " + timeToSleep);

            //timeToSleep -= deltaTime; 
            if (timeToSleep > 0)        
                Thread.Sleep((int)(timeToSleep * 1000.0f));          
        }

        DisableUpdateOnly();
    }
}
