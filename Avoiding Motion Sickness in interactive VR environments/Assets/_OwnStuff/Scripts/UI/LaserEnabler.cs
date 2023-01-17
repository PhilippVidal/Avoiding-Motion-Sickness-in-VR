using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnabler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerController.Instance.gameObject)
        {
            //Debug.Log("Enabling Laserpointer!");
            LaserPointer pointer = PlayerController.Instance.m_LaserPointer;
            if(pointer)
                pointer.Activate();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            //Debug.Log("Disabling Laserpointer!");
            LaserPointer pointer = PlayerController.Instance.m_LaserPointer;
            if (pointer)
                pointer.Deactivate();
        }

    }
}
