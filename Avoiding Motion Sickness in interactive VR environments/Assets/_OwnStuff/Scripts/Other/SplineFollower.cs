using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SplineFollower : MonoBehaviour
{
    public bool m_MatchDirection = false;
    public bool m_NormalizeSpeed = true;
    public float m_SpeedMultiplier = 1.0f;
    public int m_RoundsToComplete = 1;
    public Transform m_GetInLocation;
    public Transform m_GetOutLocation;
    public Spline m_Path;
    public SplineReferencePoint[] m_ReferencePoints;
    public AudioSource m_AudioSource;
    public AudioSource m_EngineSource;
    public AudioSource m_WindSource;
    public float m_CurrentSpeed;
    //public float m_VerticalOffset = 0.33f;


    protected float m_T = 0.0f;
    protected Coroutine m_TraversingCoroutine;
    protected int m_CurrentRoundCount = 0;

    protected int m_LastReferencePoint = 0;
    protected float m_TimeSinceLastReferencePoint = 0.0f;

    //Events
    public UnityEvent m_OnTraversingStarted;
    public UnityEvent m_OnRoundCompleted;
    public UnityEvent m_OnAllRoundsCompleted;


    [System.Serializable]
    public struct SplineReferencePoint
    {
        public float Speed;
        public float TimeToReachSpeed;
    }


    protected float VelocityWeight
    {
        get
        {
            return 1 / (m_Path.GetPoint(m_T).Speed * m_Path.SegmentCount);
        }
    }


    protected virtual IEnumerator FollowPathCoroutine()
    {
        StartAudio();
        while (m_T < 1.0f)
        {
            UpdateTransform();
            yield return null;
        }

        OnRoundCompleted();
    }

    protected virtual void UpdateTransform()
    {
        m_CurrentSpeed = GetSpeed();
        m_T += m_CurrentSpeed * VelocityWeight * Time.deltaTime;

        if (m_EngineSource)
        {
            m_EngineSource.pitch = 0.3f + 1.0f * (m_CurrentSpeed / (4.0f * m_SpeedMultiplier));
        }

        if (m_WindSource)
        {
            m_WindSource.volume = m_CurrentSpeed / (4.0f * m_SpeedMultiplier);
        }

        if (m_T > 1.0f)
            m_T = 1.0f;

        PathPoint point = m_Path.GetPoint(m_T);
        transform.position = point.Position;

        if (m_MatchDirection)
        {
            transform.rotation = point.Rotation;
        }
    }

    public virtual void StartTraversing()
    {
        if (m_AudioSource)
            m_AudioSource.Play();

        m_CurrentRoundCount = 0;
        m_T = 0.0f;
        m_LastReferencePoint = 0;
        m_TimeSinceLastReferencePoint = 0.0f;
        OnTraversingStarted();
        AttachPlayer();
        m_TraversingCoroutine = StartCoroutine(FollowPathCoroutine());
    }

    protected virtual void AttachPlayer()
    {
        PlayerController player = PlayerController.Instance;
        player.OrientatePlayer(transform.rotation);
        player.CharacterControllerActive = false;

        Vector3 getInLocation = m_GetInLocation ? m_GetInLocation.position : transform.position + transform.TransformVector(Vector3.up * player.PlayerHeight);

        //Vector3 position = getInLocation - transform.TransformVector(Vector3.up * player.PlayerHeight * m_VerticalOffset);
        Vector3 position = getInLocation - transform.TransformVector(Vector3.up * player.PlayerHeight);
        player.TeleportTo(position);
        player.AttachPlayerToObject(gameObject, true, true);
    }

    protected virtual void DetachPlayer()
    {

    }


    protected virtual void OnTraversingStarted()
    {
        m_OnTraversingStarted.Invoke();
    }

    protected virtual void OnRoundCompleted()
    {
        m_CurrentRoundCount++;
        m_OnAllRoundsCompleted.Invoke();
        if (m_CurrentRoundCount < m_RoundsToComplete)
        {
            m_T = 0.0f;
            m_TraversingCoroutine = StartCoroutine(FollowPathCoroutine());
            return;
        }

        OnAllRoundsCompleted();
    }

    protected virtual void OnAllRoundsCompleted()
    {
        m_OnAllRoundsCompleted.Invoke();
        PlayerController.Instance.CharacterControllerActive = true;
        PlayerController.Instance.DetachPlayerFromObject();
        m_TraversingCoroutine = null;

        if(m_GetOutLocation)
            PlayerController.Instance.TeleportTo(m_GetOutLocation.position);

        if (m_AudioSource)
            m_AudioSource.Stop();

        EndAudio();
    }

    public float GetSpeed()
    {
        int currentIndex = m_Path.GetStartIndex(m_T);
        currentIndex = MiscFunctions.ClampedIndex(currentIndex, m_ReferencePoints.Length);

        if (currentIndex < 0)
            return 1.0f;

        int nextIndex = currentIndex < m_ReferencePoints.Length - 1 ? currentIndex + 1 : currentIndex;


        m_TimeSinceLastReferencePoint =
            currentIndex == m_LastReferencePoint ? m_TimeSinceLastReferencePoint + Time.deltaTime : 0.0f;


        SplineReferencePoint currentPoint = m_ReferencePoints[currentIndex];
        SplineReferencePoint nextPoint = m_ReferencePoints[nextIndex];

        float t = nextPoint.TimeToReachSpeed < 0.01f ?
            1.0f : m_TimeSinceLastReferencePoint / nextPoint.TimeToReachSpeed;

        float speed = Mathf.Lerp(currentPoint.Speed, nextPoint.Speed, t);

        m_LastReferencePoint = currentIndex;
        return speed * m_SpeedMultiplier;
    }

    protected void OnDrawGizmos()
    {
        if (!m_Path)
            return;

        int referencePointCount = m_ReferencePoints.Length;
        int controlPointCount = m_Path.ControlPointCount;
        float size = 0.5f;
        float maxTimeValue = 5.0f;
        for (int i = 0; i < controlPointCount; i++)
        {
            int targetIndex = i;

            if(i > referencePointCount - 1)
                targetIndex = referencePointCount - 1;
            
            SplineReferencePoint point = m_ReferencePoints[targetIndex];
            float drawSize = size * point.Speed;
            float timeValue = point.TimeToReachSpeed;
            timeValue = timeValue > maxTimeValue ? 1.0f  : timeValue / maxTimeValue;
            Color color = new Color(1.0f - timeValue, timeValue, 0.0f);
            Gizmos.color = color;
            Gizmos.DrawCube(m_Path.transform.GetChild(i).position, new Vector3(drawSize, drawSize, drawSize));
        }
    }

    protected void StartAudio()
    {
        if (m_EngineSource)
            m_EngineSource.Play();

        if (m_WindSource)
            m_WindSource.Play();
    }

    protected void EndAudio()
    {
        if (m_EngineSource)
            m_EngineSource.Stop();

        if (m_WindSource)
            m_WindSource.Stop();
    }
}
