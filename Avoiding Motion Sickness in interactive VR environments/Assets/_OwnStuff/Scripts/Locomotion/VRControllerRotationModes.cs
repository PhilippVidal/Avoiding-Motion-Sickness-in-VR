using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VRControllerRotationModes
{
    CONTINUOUS,
    SNAPPING
}

public abstract class VRControllerRotationMode
{
    abstract public Quaternion GetRotationChange();
}

public class VRControllerRotationMode_Snapping : VRControllerRotationMode
{
    public override Quaternion GetRotationChange()
    {
        float inputRight = PlayerController.Instance.m_SnapInputRightInput.stateDown ? 1.0f : 0.0f;
        float inputLeft = PlayerController.Instance.m_SnapInputLeftInput.stateDown ? 1.0f : 0.0f;
        float angle = PlayerController.Instance.m_RotationSnappingIncrements * (inputRight - inputLeft);

        return Quaternion.Euler(0.0f, angle, 0.0f);
    }
}

public class VRControllerRotationMode_Continuous : VRControllerRotationMode
{
    public override Quaternion GetRotationChange()
    {
        float inputRight = PlayerController.Instance.m_SnapInputRightInput.state ? 1.0f : 0.0f;
        float inputLeft = PlayerController.Instance.m_SnapInputLeftInput.state ? 1.0f : 0.0f;
        float angle = PlayerController.Instance.m_RotationSpeed * (inputRight - inputLeft) * Time.deltaTime;
        return Quaternion.Euler(0.0f, angle, 0.0f);
    }
}


