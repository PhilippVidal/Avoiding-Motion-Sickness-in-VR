using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WalkingIntoWall : MonoBehaviour
{
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            PlayerController.Instance.FadeScreen(true, 0.0f);
        }
            
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            PlayerController.Instance.FadeScreen(false, 0.0f);
        }
    }
}
