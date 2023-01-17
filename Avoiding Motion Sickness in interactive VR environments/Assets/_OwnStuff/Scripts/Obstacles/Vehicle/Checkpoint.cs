using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool m_Triggered = false;

    protected void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Vehicle"))
            m_Triggered = true;
    }
}
