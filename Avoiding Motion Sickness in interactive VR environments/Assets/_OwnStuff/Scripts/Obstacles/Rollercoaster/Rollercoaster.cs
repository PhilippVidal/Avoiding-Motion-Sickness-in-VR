using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct RollercoasterPoint
{
    public float Speed;
    public float TimeToReachSpeed;
}

public class Rollercoaster : MonoBehaviour
{
    //public bool m_Loop = false;
    public bool m_MatchDirection = false;

    public float m_SpeedMultiplier = 0.10f;
    public Spline m_SplineToFollow;

    public Transform m_PlayerAttachPoint;
    public Transform m_PlayerDetachPoint;

    protected float m_T = 0.0f;
    //protected CharacterController m_CharacterController;
    protected Coroutine m_Coroutine;

    public UnityEvent m_OnTraversingStarted;
    public UnityEvent m_OnRoundCompleted;
    public UnityEvent m_OnAllRoundsCompleted;

    public int m_Rounds = 1;
    protected int m_CurrentRoundCount = 0;

    public RollercoasterPoint[] m_RollercoasterPoints;
    protected int m_LastRollercoasterPoint = 0;
    protected float m_TimeSinceLastRollercoasterPoint = 0.0f;
    public AudioSource m_TrackAudioSource;
    public AudioSource m_WindAudioSource;
    [SerializeField] protected float m_CurrentSpeed = 0.0f;

    protected float VelocityWeight { get { return 1 / (m_SplineToFollow.GetPoint(m_T).Speed * m_SplineToFollow.SegmentCount); } }
    protected virtual IEnumerator FollowTrack()
    {
        StartAudio();

        while (m_T < 1.0f)
        {
            UpdateTransform();
            yield return null;
        }

        OnRoundCompleted();
    }

    protected void StartAudio()
    {
        if(m_TrackAudioSource)
            m_TrackAudioSource.Play();

        if (m_WindAudioSource)
            m_WindAudioSource.Play();
    }

    protected void EndAudio()
    {
        if (m_TrackAudioSource)
            m_TrackAudioSource.Stop();

        if (m_WindAudioSource)
            m_WindAudioSource.Stop();
    }

    protected virtual void UpdateTransform()
    {

        m_CurrentSpeed = GetSpeed();
        m_T += Time.deltaTime * VelocityWeight * m_CurrentSpeed;

        if (m_TrackAudioSource)
        {
            m_TrackAudioSource.pitch = 0.3f + 1.0f * (m_CurrentSpeed / (5.0f * m_SpeedMultiplier));
        }

        if(m_WindAudioSource)
        {
            m_WindAudioSource.volume = m_CurrentSpeed / (5.0f * m_SpeedMultiplier);
        }


        if (m_T > 1.0f)
            m_T = 1.0f;

        Vector3 transformPosition = transform.position;
        PathPoint point = m_SplineToFollow.GetPoint(m_T);
        transform.position = point.Position;

        if (!m_MatchDirection)
            return;

        transform.rotation = point.Rotation;
    }

    public void StartTraversing()
    { 

        m_CurrentRoundCount = 0;
        m_T = 0.0f;
        m_LastRollercoasterPoint = 0;
        m_TimeSinceLastRollercoasterPoint = 0.0f;
        OnTraversingStarted();       
        AttachPlayer();
        m_Coroutine = StartCoroutine(FollowTrack());
    }

    protected void AttachPlayer()
    {
        PlayerController player = PlayerController.Instance;
        player.OrientatePlayer(m_PlayerAttachPoint.rotation);
        player.CharacterControllerActive = false;

        Vector3 position = m_PlayerAttachPoint.position - transform.TransformVector(Vector3.up * player.PlayerHeight); ;// - transform.TransformVector(Vector3.up * player.PlayerHeight * 0.33f);
        player.TeleportTo(position);
        player.AttachPlayerToObject(gameObject, true, true);
    }

    protected virtual void OnTraversingStarted()
    {
        m_OnTraversingStarted.Invoke();
    }

    protected virtual void OnRoundCompleted()
    {
        m_CurrentRoundCount++;
        m_OnAllRoundsCompleted.Invoke();
        if (m_CurrentRoundCount < m_Rounds)
        {
            m_T = 0.0f;
            m_Coroutine = StartCoroutine(FollowTrack());
            return;
        }

        OnAllRoundsCompleted();
    }

    protected virtual void OnAllRoundsCompleted()
    {
        m_OnAllRoundsCompleted.Invoke();
        PlayerController.Instance.CharacterControllerActive = true;
        PlayerController.Instance.DetachPlayerFromObject();
        PlayerController.Instance.TeleportTo(m_PlayerDetachPoint.position);
        m_Coroutine = null;
        EndAudio();
    }


    public float GetSpeed()
    {      
        int currentIndex = m_SplineToFollow.GetStartIndex(m_T);
        currentIndex = MiscFunctions.ClampedIndex(currentIndex, m_RollercoasterPoints.Length);

        if (currentIndex < 0)
            return 1.0f;

        int nextIndex = currentIndex < m_RollercoasterPoints.Length - 1 ? currentIndex + 1 : currentIndex;


        m_TimeSinceLastRollercoasterPoint = 
            currentIndex == m_LastRollercoasterPoint ? m_TimeSinceLastRollercoasterPoint + Time.deltaTime : 0.0f;


        RollercoasterPoint currentPoint = m_RollercoasterPoints[currentIndex];
        RollercoasterPoint nextPoint = m_RollercoasterPoints[nextIndex];

        float t = nextPoint.TimeToReachSpeed < 0.01f ? 1.0f : m_TimeSinceLastRollercoasterPoint / nextPoint.TimeToReachSpeed;
        float speed = Mathf.Lerp(
            currentPoint.Speed, 
            nextPoint.Speed,
            t
            );

        m_LastRollercoasterPoint = currentIndex;
        return speed * m_SpeedMultiplier;
    }
}

