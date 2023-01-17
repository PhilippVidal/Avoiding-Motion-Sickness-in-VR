using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMotionTunnelAudio : MonoBehaviour
{
    public float m_Speed = 1.0f;
    public float m_MaterialSpeed = 1.0f;
    public AudioSource m_Source;
    public bool m_AudioOn;

    void Update()
    {
        transform.localRotation *= Quaternion.Euler(0, m_Speed * m_MaterialSpeed, 0);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            EnableAudio();            
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            DisableAudio();           
        }
    }

    protected void EnableAudio()
    {
        if (!m_AudioOn)
        {
            DisableAudio();
            return;
        }
            
        m_Source.loop = true;
        m_Source.Play();
        Debug.Log("[VerticalMotionTunnelAudio] Enabling Audio!");
    }

    protected void DisableAudio()
    {
        m_Source.Stop();
        Debug.Log("[VerticalMotionTunnelAudio] Disabling Audio!");
    }
}
