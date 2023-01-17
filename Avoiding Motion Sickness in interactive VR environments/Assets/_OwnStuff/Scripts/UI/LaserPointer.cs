using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{

    public float m_LaserLength = 5.0f;
    public CustomInputModule m_CustomInputModule;
    protected LineRenderer m_LineRenderer;
    public bool m_IsEnabled = false;

    public AudioSource m_ClickSource;
    public AudioClip m_ClickDownClip;
    public AudioClip m_ClickUpClip;

    protected void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    public void PlayClickDown()
    {
        if (m_ClickSource && m_ClickDownClip)
        {
            m_ClickSource.clip = m_ClickDownClip;
            m_ClickSource.Play();
        }
            
    }

    public void PlayClickUp()
    {
        if (m_ClickSource && m_ClickUpClip)
        {
            m_ClickSource.clip = m_ClickUpClip;
            m_ClickSource.Play();
        }
    }

    protected void Update()
    {
        if (!m_IsEnabled)
            return;

        float pointerRayDistance = m_CustomInputModule.PointerRayLength;
        float length = pointerRayDistance != 0 ? pointerRayDistance : m_LaserLength;

        RaycastHit hitInfo;
        Vector3 laserEndPosition = Physics.Raycast(transform.position, transform.forward, out hitInfo, m_LaserLength) ? hitInfo.point : transform.position + (transform.forward * length);

        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, laserEndPosition);
    } 


    public void Activate()
    {
        m_IsEnabled = true;
        m_LineRenderer.enabled = true;
    }

    public void Deactivate()
    {
        m_LineRenderer.enabled = false;
        m_IsEnabled = false;  
    }
}
