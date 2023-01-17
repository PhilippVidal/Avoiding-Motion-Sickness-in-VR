using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageCamera : MonoBehaviour
{
    protected Camera m_Camera;

    protected void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    protected void Update()
    {
        m_Camera.fieldOfView = PlayerController.Instance.m_Camera.fieldOfView;
    }
}
