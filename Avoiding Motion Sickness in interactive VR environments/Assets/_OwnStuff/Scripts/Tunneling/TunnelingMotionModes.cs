using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TunnelingMotionModes
{
    INSTANT,
    SPEED,
    TIME,
    ALWAYS

}
public abstract class TunnelingMotionMode
{
    protected float m_MotionValue;

    abstract public void Update();
    virtual public float GetMotionValue()
    {
        return m_MotionValue;
    }
}

public class TunnelingMotionMode_Instant : TunnelingMotionMode
{
    public override void Update()
    {
        PlayerController player = PlayerController.Instance;
        float velocity = Tunneling.Instance.m_IgnorePlayerInput ? player.NoInputVelocity : player.AbsoluteVelocity;
        m_MotionValue = velocity > 0.05f ? 1.0f : 0.0f;
        //m_MotionValue = PlayerController.Instance.m_InputVector.magnitude > 0.05f ? 1.0f : 0.0f;
        //Debug.Log(m_Player.AbsoluteVelocity);
        //Vector2 horizontalVelocity = new Vector2(m_playerRigidbody.velocity.x, m_playerRigidbody.velocity.z);
        //m_MotionValue = horizontalVelocity != Vector2.zero ? 1.0f : 0.0f;
    }
}

public class TunnelingMotionMode_Speed : TunnelingMotionMode
{
    protected float m_MaxChangePerFrame = 0.05f;
    public override void Update()
    {
        PlayerController player = PlayerController.Instance;
        float velocity = Tunneling.Instance.m_IgnorePlayerInput ? player.NoInputVelocity : player.AbsoluteVelocity;
        //float velocity = PlayerController.Instance.AbsoluteVelocity;
        ////float velocity = PlayerController.Instance.m_InputVector.magnitude;
        //if (velocity < 0.05f && velocity > -0.05f)
        //{
        //    float fadeOutTime = Tunneling.Instance.m_FadeOutTime;
        //    fadeOutTime = fadeOutTime > 0.0f ? fadeOutTime : 0.001f;
        //    m_MotionValue = Mathf.Clamp01(m_MotionValue - Time.deltaTime / fadeOutTime);
        //    return;
        //}

        //float newValue = Mathf.Clamp01(velocity / Tunneling.Instance.m_MaxSpeedReference);

        //float difference = m_MotionValue - newValue;
        //if (difference > m_MaxChangePerFrame)
        //{
        //    newValue = m_MotionValue - m_MaxChangePerFrame;
        //}
        //else if(difference < -m_MaxChangePerFrame)
        //{
        //    newValue = m_MotionValue + m_MaxChangePerFrame;
        //}

        if (velocity < 0.05f && velocity > -0.05f)
        {
            float fadeOutTime = Tunneling.Instance.m_FadeOutTime;
            fadeOutTime = fadeOutTime > 0.0f ? fadeOutTime : 0.001f;
            m_MotionValue = Mathf.Clamp01(m_MotionValue - Time.deltaTime / fadeOutTime);
            return;
        }

        float currentVelocity = 0;
        m_MotionValue = Mathf.SmoothDamp(
            m_MotionValue, 
            Mathf.Clamp01(velocity / Tunneling.Instance.m_MaxSpeedReference), 
            ref currentVelocity, 
            Tunneling.Instance.m_SpeedSmoothing
            );

        //m_MotionValue = newValue;
        ////m_MotionValue = Mathf.Clamp01(m_playerRigidbody.velocity.magnitude / GameManager.VRSettings.VRPlayerMovementSpeed);
    }
}

public class TunnelingMotionMode_Time : TunnelingMotionMode
{
    override public void Update()
    {
        Tunneling tunneling = Tunneling.Instance;
        float value = Time.deltaTime;
        float fadeInTime = tunneling.m_FadeInTime;
        float fadeOutTime = tunneling.m_FadeOutTime;

        fadeInTime = fadeInTime > 0 ? tunneling.m_FadeInTime : 0.001f;
        fadeOutTime = fadeOutTime > 0 ? tunneling.m_FadeOutTime : 0.001f;

        //If player is moving => increase value (weighted with fade-in-time)
        //else                => decrease value (weighted with fade-out-time)
        PlayerController player = PlayerController.Instance;
        float velocity = tunneling.m_IgnorePlayerInput ? player.NoInputVelocity : player.AbsoluteVelocity;
        value *= velocity > 0.01f ? (1 / fadeInTime) : ((1 / fadeOutTime) * -1.0f);
        //value *= PlayerController.Instance.m_InputVector.magnitude != 0.0f ? fadeInTimeReciprocal : (fadeOutTimeReciprocal * -1.0f);
        m_MotionValue = Mathf.Clamp01(m_MotionValue + value);
    }
}

public class TunnelingMotionMode_Always : TunnelingMotionMode
{
    override public void Update()
    {
        m_MotionValue = 1.0f;
    }
}
