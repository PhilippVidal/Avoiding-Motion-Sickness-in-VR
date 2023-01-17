using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent onButtonDown;
    public ConfigurableJoint m_ConfigurableJoint;
    public Transform m_ButtonTransform;
    public AudioSource m_AudioSource;

    protected bool m_IsBeingPressed;
    protected Vector3 m_InitialPosition;
    protected float m_PressThreshold = 0.1f;



    protected void Awake()
    {
        m_InitialPosition = m_ButtonTransform.localPosition;
    }

    protected void Update()
    {
        float value = Vector3.Distance(m_InitialPosition, m_ButtonTransform.localPosition) / m_ConfigurableJoint.linearLimit.limit;

        if (0.03f > Mathf.Abs(value))
            value = 0.0f;

        value = Mathf.Clamp(value, -1.0f, 1.0f);

        if (!m_IsBeingPressed && value + m_PressThreshold >= 1.0f)
        {
            m_IsBeingPressed = true;
            onButtonDown.Invoke();
            
            if(m_AudioSource)
                m_AudioSource.Play();
        }

        if (m_IsBeingPressed && value - m_PressThreshold <= 0.0f)
        {
            m_IsBeingPressed = false;
        }             
    }
}
