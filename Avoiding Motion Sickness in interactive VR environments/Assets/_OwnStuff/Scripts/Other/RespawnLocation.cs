using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    public void SetAsCurrent()
    {
        GameManager.Instance.m_LatestRespawnLocation = this;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            SetAsCurrent();
            Debug.Log("[Respawn Location] Respawnlocation has been updated!");
        }     
    }
}
