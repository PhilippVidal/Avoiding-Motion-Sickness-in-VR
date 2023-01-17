using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackingErrorType
{
    NONE, 
    TRANSLATION,
    ROTATION,
    TRANLATION_ROTATION,
    RANDOM
}

public class TrackingErrorSimulator : MonoBehaviour
{
    public float m_MinDuration = 1.0f;
    public float m_MaxDuration = 3.0f;

    public float m_MinDurationBetween = 5.0f;
    public float m_MaxDurationBetween = 15.0f;

    public Transform m_ErrorReferenceTransform; 

    protected Vector3 m_LastLocalCameraPosition;
    protected Vector3 m_CurrentLocalCameraPosition;

    protected Quaternion m_LastLocalCameraRotation;
    protected Quaternion m_CurrentLocalCameraRotation;

    //Needs to be a sibling of the VRCamera
    public UnityEngine.SpatialTracking.TrackedPoseDriver m_OverridePoseDriver;


    public TrackingErrorType m_SelectedErrorType = TrackingErrorType.NONE;

    protected Coroutine m_ErrorSimulationCoroutine = null;
    
    public bool IsSimulating { get; private set; }

    public bool IsEnabled { get { return m_SelectedErrorType != TrackingErrorType.NONE; } }
    public bool IsActive
    {
        get { return m_ErrorSimulationCoroutine != null; }
    }
    
    
    protected int ErrorTypeCount { get { return System.Enum.GetValues(typeof(TrackingErrorType)).Length; } }

    public static TrackingErrorSimulator Instance;
    public static bool Exists { get { return Instance != null; } }
   

    protected void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    protected void Update()
    {
        Camera cam = PlayerController.Instance.m_Camera;
        if (!cam)
            return;

        m_LastLocalCameraPosition = m_CurrentLocalCameraPosition;
        m_CurrentLocalCameraPosition = cam.transform.localPosition;

        m_LastLocalCameraRotation = m_CurrentLocalCameraRotation;
        m_CurrentLocalCameraRotation = cam.transform.localRotation;
    }

    public void StartErrorSimulation()
    {
        Debug.Log("[Tracking Error Simulation] m_SelectedErrorType => " + m_SelectedErrorType);
        if (m_SelectedErrorType == TrackingErrorType.NONE)
            return;
        Debug.Log("[Tracking Error Simulation] IsActive => " + IsActive);
        if (IsActive)
            StopErrorSimulation();

        m_ErrorSimulationCoroutine = StartCoroutine(TrackingErrorCoroutine());
    }

    public void StopErrorSimulation()
    {
        if (m_ErrorSimulationCoroutine == null)
            return;

        StopCoroutine(m_ErrorSimulationCoroutine);
        m_ErrorSimulationCoroutine = null;
        PlayerController.Instance.m_VRCamera.TrackerOverride = null;
        m_OverridePoseDriver.enabled = false;
        IsSimulating = false;
        Debug.Log("[Tracking Error Simulation] Error Simulation has been deactivated!");
        //Reset();
    }

    protected IEnumerator TrackingErrorCoroutine()
    {
        Debug.Log("[Tracking Error Simulation] Error Simulation has been activated!");
        VRCamera vrCamera = PlayerController.Instance.m_VRCamera;
        if(!vrCamera)
        {
            Debug.LogWarning("[Tracking Error Simulation] No VRCamera set on player, stopping error simulation!");
            m_ErrorSimulationCoroutine = null;
            yield break;
        }

        while (true)
        {
            float timeTillNextError = Random.Range(m_MinDurationBetween, m_MaxDurationBetween);
            float passedTime = 0.0f;
            while (passedTime < timeTillNextError)
            {
                passedTime += Time.deltaTime;
                yield return null;
            }

            //if a random error should be simulated,
            //select a random one except for enum entries 0 (=> NONE) and ErrorTypeCount - 1 (=> RANDOM)
            TrackingErrorType errorType = m_SelectedErrorType == TrackingErrorType.RANDOM ?
                (TrackingErrorType)Random.Range(1, ErrorTypeCount - 1) : m_SelectedErrorType;

            Vector3 translationDelta = m_CurrentLocalCameraPosition - m_LastLocalCameraPosition;
            Quaternion rotationDelta = m_CurrentLocalCameraRotation * Quaternion.Inverse(m_LastLocalCameraRotation);

            float simulationDuration = Random.Range(m_MinDuration, m_MaxDuration);
            passedTime = 0.0f;
            IsSimulating = true;


            m_ErrorReferenceTransform.localPosition = vrCamera.transform.localPosition;
            m_ErrorReferenceTransform.localRotation = vrCamera.transform.localRotation;
            vrCamera.TrackerOverride = m_ErrorReferenceTransform;
            m_OverridePoseDriver.enabled = true;

            //delay by one second to prevent position and rotation jumps 
            yield return null;

            while (passedTime < simulationDuration)
            {
                float deltaTime = Time.deltaTime;
                bool updateTranslation = (errorType == TrackingErrorType.TRANSLATION) || (errorType == TrackingErrorType.TRANLATION_ROTATION);//= (errorType & TrackingErrorType.TRANSLATION) != 0;
                bool updateRotation = (errorType == TrackingErrorType.ROTATION) || (errorType == TrackingErrorType.TRANLATION_ROTATION);//= (errorType & TrackingErrorType.ROTATION) != 0;

                if (updateTranslation)
                {
                    m_ErrorReferenceTransform.localPosition += translationDelta;
                }
                else
                {
                    m_ErrorReferenceTransform.localPosition = m_OverridePoseDriver.transform.localPosition;
                }
                                 
                if (updateRotation)
                {
                    m_ErrorReferenceTransform.localRotation *= rotationDelta;
                }
                else
                {
                    m_ErrorReferenceTransform.localRotation = m_OverridePoseDriver.transform.localRotation;
                }
                    

                passedTime += deltaTime;
                yield return null;
            }
            vrCamera.TrackerOverride = null;
            m_OverridePoseDriver.enabled = false;
            IsSimulating = false;           
        }      
    }

    //protected void Reset()
    //{
    //    transform.localPosition = Vector3.zero;
    //    transform.localRotation = Quaternion.identity;
    //    m_CurrentErrorDuration = 0.0f;
    //    m_IsErrorBeingSimulated = false;
    //}

}
