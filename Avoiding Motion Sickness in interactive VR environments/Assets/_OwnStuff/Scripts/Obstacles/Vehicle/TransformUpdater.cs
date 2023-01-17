using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUpdater : MonoBehaviour
{
    public Vehicle m_VehicleToFollow;


    // Update is called once per frame
    void Update()
    {
        transform.position = m_VehicleToFollow.transform.position;
        transform.rotation = m_VehicleToFollow.transform.rotation;
    }


    public void StartVehicle()
    {
        PlayerController.Instance.AttachPlayerToObject(m_VehicleToFollow.gameObject, true, true);
        PlayerController.Instance.CharacterControllerActive = false;    
        m_VehicleToFollow.m_Active = true;
        PlayerController.Instance.HandPhysicsEnabled = false;
    }
}
