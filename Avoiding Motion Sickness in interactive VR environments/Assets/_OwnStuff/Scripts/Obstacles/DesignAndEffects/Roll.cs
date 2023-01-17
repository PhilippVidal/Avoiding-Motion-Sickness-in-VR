using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    public float m_RotationSpeed = 20.0f;

    protected void Update()
    {        
        transform.localRotation = Quaternion.Euler(transform.TransformDirection(new Vector3(0.0f, 0.0f, m_RotationSpeed * Time.deltaTime))) * transform.localRotation;
    }

}
