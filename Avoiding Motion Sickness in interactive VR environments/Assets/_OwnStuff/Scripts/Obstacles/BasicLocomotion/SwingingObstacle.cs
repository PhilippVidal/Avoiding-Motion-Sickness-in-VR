using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingObstacle : MonoBehaviour
{
    public float m_MaxAngle = 30.0f;
    public float m_Speed = 1.0f;
    public float m_CurrentValue = 0.0f;
    public float m_SpeedMultiplier = 1.0f;
    public AudioSource m_SourceSphere;
    public AudioSource m_SourceJoint;


    protected Quaternion m_Start;
    protected Quaternion m_End;
    
    protected float m_PreviousT;

    protected void Start()
    {
        m_Start = GetSwingingRotation(m_MaxAngle);
        m_End = GetSwingingRotation(-m_MaxAngle);
        //m_Source = GetComponent<AudioSource>();
        m_PreviousT = 0.0f;
    }
    protected void Update()
    {
        m_CurrentValue += Time.deltaTime;
        float t = Mathf.Sin(m_CurrentValue * m_Speed * m_SpeedMultiplier);
        t = (t + 1.0f) * 0.5f;
        transform.rotation = Quaternion.Lerp(m_Start, m_End, t);

        if (t > 0.9f && t < m_PreviousT)
            PlayAudio();

        if (t < 0.1f && t > m_PreviousT)
            PlayAudio();

        m_PreviousT = t;
    }


    protected void PlayAudio()
    {
        float pitch = m_Speed * m_SpeedMultiplier;

        if (m_SourceSphere)
        {
            m_SourceSphere.pitch = pitch;
            m_SourceSphere.Play();
        }

        if(m_SourceJoint)
        {
            m_SourceJoint.pitch = pitch;
            m_SourceJoint.Play();
        }

        
    }

    Quaternion GetSwingingRotation(float angle)
    {
        Quaternion pR = transform.rotation;
        float angleZ = pR.eulerAngles.z + angle;

        if (angleZ > 180)
            angleZ -= 360;
        else if (angleZ < -180)
            angleZ += 360;

        pR.eulerAngles = new Vector3(pR.eulerAngles.x, pR.eulerAngles.y, angleZ);
        return pR;
    }
}
