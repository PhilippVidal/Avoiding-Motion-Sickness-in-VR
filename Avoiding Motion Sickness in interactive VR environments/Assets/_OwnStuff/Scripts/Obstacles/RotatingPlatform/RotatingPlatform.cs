using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RotatingPlatform : MovingEntity
{
    //Degrees/Second
    public float m_RotationSpeed;
    public Vector3 m_RotationAxis = Vector3.up;
    public AudioSource m_AudioSource;

    protected override void UpdateMovement()
    {
        transform.rotation = Quaternion.Euler(m_RotationAxis * m_RotationSpeed * Time.deltaTime) * transform.rotation;

        if (m_AudioSource)
            m_AudioSource.pitch = Mathf.Abs(2.727272f * (m_RotationSpeed / 90.0f));

    }
}
