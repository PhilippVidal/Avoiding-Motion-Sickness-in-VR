using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VRControllerMovementModes
{
    LINEAR,
    ACCELERATION
}

public abstract class VRControllerMovementMode
{
    abstract public float GetMovementDistance();
}

public class VRControllerMovementMode_Linear : VRControllerMovementMode
{
    public override float GetMovementDistance()
    {
        return PlayerController.Instance.m_MaxMovementSpeed;
    }
}

public class VRControllerMovementMode_Acceleration : VRControllerMovementMode
{
    float m_Velocity = 0.0f;

    public override float GetMovementDistance()
    {
        //Stop moving immediately when there is no more input
        if (PlayerController.Instance.m_MovementInput.axis == Vector2.zero)
            m_Velocity = 0.0f;

        m_Velocity += PlayerController.Instance.m_MovementAcceleration * Time.deltaTime;
        return Mathf.Clamp(m_Velocity, 0.0f, PlayerController.Instance.m_MaxMovementSpeed);
    }
}
