using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSphere : MonoBehaviour
{
    public SphereSpawner m_Spawner;
    public float m_CurrentLifetime;
    public float m_MaxLifetime = 40.0f;
    public float m_MaxSpeedReference = 10.0f;
    public float m_MaxRollingAudioPitch = 2.0f;
    public AudioSource m_AudioSourceRolling;
    public AudioSource m_AudioSourceCollision;
    protected Rigidbody m_Rigidbody;
    [SerializeField] protected float m_CurrentSpeed;
    protected float m_LifetimeAtLastCollision;

    protected void Awake()
    {        
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        m_Spawner.DespawnSphere(this);
    }

    protected void Update()
    {
        m_CurrentLifetime += Time.deltaTime;

        if (m_CurrentLifetime > m_MaxLifetime)
            Respawn();
    }

    protected void FixedUpdate()
    {
        if (!m_Rigidbody)
            return;

        m_CurrentSpeed = m_Rigidbody.velocity.magnitude;

        if (m_AudioSourceRolling)
        {
            //Map current speed to range 0 - 1
            float vel = Mathf.Clamp(m_CurrentSpeed, 0.0f, m_MaxSpeedReference) / m_MaxSpeedReference;

            if (Mathf.Abs(m_Rigidbody.velocity.y) > 2.5f)
                m_AudioSourceRolling.Stop();

            m_AudioSourceRolling.volume = vel * 0.2f;
            m_AudioSourceRolling.pitch = vel * m_MaxRollingAudioPitch;
        }
       
    }

    protected void OnCollisionEnter(Collision collision)
    {
       // Debug.Log(collision.impulse.magnitude);
        if(!m_AudioSourceRolling.isPlaying)
            m_AudioSourceRolling.Play();

        //if (collision.gameObject.CompareTag("Floor") && Mathf.Abs(m_Rigidbody.velocity.y) < 2.5f)
         //   return;

        //float timeSinceLastCollision = m_CurrentLifetime - m_LifetimeAtLastCollision;
        //if (timeSinceLastCollision < 0.4f)
        //    return;

        if(m_AudioSourceCollision && !m_AudioSourceCollision.isPlaying)
        {
            float impulseWeight = collision.impulse.magnitude / 3000.0f;
            m_AudioSourceCollision.volume = 0.15f * Mathf.Clamp01(impulseWeight);
            m_AudioSourceCollision.Play();
        }
    }
}
