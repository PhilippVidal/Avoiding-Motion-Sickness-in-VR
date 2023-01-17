using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    public Transform m_TeleportLocation;

    public void TeleportPlayertoLocation()
    {
        //PlayerController.Instance.TeleportTo(m_TeleportLocation.position);
        PlayerController.Instance.TeleportToWithFade(m_TeleportLocation.position, PlayerController.Instance.m_ChangeSceneFadeTime);
    }
}
