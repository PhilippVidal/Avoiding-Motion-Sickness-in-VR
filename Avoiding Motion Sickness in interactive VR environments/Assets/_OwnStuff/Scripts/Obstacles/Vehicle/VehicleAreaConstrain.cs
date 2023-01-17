using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAreaConstrain : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Vehicle"))
        {
            other.GetComponent<Vehicle>().Reset();
        }
    }
}
