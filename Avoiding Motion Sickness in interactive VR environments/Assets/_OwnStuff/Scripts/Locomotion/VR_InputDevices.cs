using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using CybSDK;

public enum InputDevices
{
    VR_CONTROLLER,
    VIRTUALIZER
}

public enum VRControllerForwardModes
{
    HMD,
    CONTROLLER_RIGHT,
    CONTROLLER_LEFT
}

public abstract class VR_InputDevice
{
    protected Transform m_Forward;

    abstract public Vector3 GetMovementVector();
    abstract public Quaternion GetUpdatedRotation();
}


public class VR_InputDevice_VRController : VR_InputDevice
{
    protected VRControllerRotationMode m_RotationMode;
    protected VRControllerMovementMode m_MovementMode;

    public VR_InputDevice_VRController()
    {
        SetForwardMode();
        SetMovementMode();
        SetRotationMode();
    }

    public override Vector3 GetMovementVector()
    {
        Vector2 translationInput = PlayerController.Instance.m_MovementInput.axis;

        Vector3 forward = m_Forward.forward;
        forward.y = 0.0f;
        Vector3 right = m_Forward.right;
        right.y = 0.0f;

        Vector3 direction = forward.normalized * translationInput.y + right.normalized * translationInput.x;
        float distance = m_MovementMode.GetMovementDistance();

        return direction.normalized * distance;
    }

    public override Quaternion GetUpdatedRotation()
    {
        return PlayerController.Instance.transform.rotation * m_RotationMode.GetRotationChange();
        //return PlayerController.Instance.transform.localRotation * m_RotationMode.GetRotationChange();
    }

    protected void SetForwardMode()
    {
        switch (PlayerController.Instance.m_ForwardMode)
        {
            case VRControllerForwardModes.HMD:
                m_Forward = PlayerController.Instance.m_Camera.transform;
                break;
            case VRControllerForwardModes.CONTROLLER_RIGHT:
                m_Forward = PlayerController.Instance.m_RightController.transform;
                break;
            case VRControllerForwardModes.CONTROLLER_LEFT:
                m_Forward = PlayerController.Instance.m_LeftController.transform;
                break;
        }
    }

    protected void SetMovementMode()
    {
        switch (PlayerController.Instance.m_MovementMode)
        {
            case VRControllerMovementModes.LINEAR:
                m_MovementMode = new VRControllerMovementMode_Linear();
                break;
            case VRControllerMovementModes.ACCELERATION:
                m_MovementMode = new VRControllerMovementMode_Acceleration();
                break;
        }
    }

    protected void SetRotationMode()
    {
        switch (PlayerController.Instance.m_RotationMode)
        {
            case VRControllerRotationModes.SNAPPING:
                m_RotationMode = new VRControllerRotationMode_Snapping();
                break;
            case VRControllerRotationModes.CONTINUOUS:
                m_RotationMode = new VRControllerRotationMode_Continuous();
                break;
        }
    }
}

public class VR_InputDevice_Virtualizer : VR_InputDevice
{
    protected CVirtDeviceController m_DeviceController;
    protected IVirtDevice m_Virtualizer;
    protected Transform m_ForwardDirection;
    protected Quaternion m_GlobalOrientation;

    public VR_InputDevice_Virtualizer(CVirtDeviceController deviceController)
    {
        if (m_ForwardDirection == null)
            m_ForwardDirection = PlayerController.Instance.transform.Find("Forward");

        if (!m_ForwardDirection)
            Debug.Log("[Input Device | Virtualizer] No Forward Direction gameobject found!");

        m_DeviceController = deviceController;
        if(!m_DeviceController)
            Debug.Log("[Input Device | Virtualizer] There is no Virt Device Controller attached!");

        Debug.LogWarning(string.Format("[Input Device | Virtualizer] Virt Device is {0}", m_DeviceController.IsDecoupled() ? "decoupled!" : "NOT decoupled!"));
    }

    public override Vector3 GetMovementVector()
    {
        //Get Virtualizer Device to make sure it is still active and accessible 
        m_Virtualizer = m_DeviceController.GetDevice();
        if (m_Virtualizer == null)
        {
            Debug.LogWarning("[Input Device | Virtualizer] Virtualizer not found!");
            return Vector3.zero;
        }
            

        if (!m_Virtualizer.IsOpen())
        {
            Debug.LogWarning("[Input Device | Virtualizer] Virtualizer can not be accessed!");
            return Vector3.zero;
        }
            


        Vector3 movementVector = m_Virtualizer.GetMovementVector() * PlayerController.Instance.m_VirtualizerVelocityMultiplier;
        Quaternion localRotation = m_Virtualizer.GetPlayerOrientationQuaternion();

        //if not decoupled, rotate player & headset
        if (!m_DeviceController.IsDecoupled())
        {
            PlayerController.Instance.transform.rotation = localRotation;
            return localRotation * movementVector;
        }

        if (m_ForwardDirection != null)
        {
            m_ForwardDirection.localRotation = localRotation;
            return m_ForwardDirection.rotation * movementVector;
        }

        return (localRotation * PlayerController.Instance.transform.rotation) * movementVector;
    }

    public override Quaternion GetUpdatedRotation()
    {
        //return PlayerController.Instance.transform.rotation;
        return Quaternion.identity;
    }
}
