using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GeneralSettings : MonoBehaviour
{
    public TMP_Dropdown m_InputDeviceDropDown;
    public Slider m_VirtualizerVelocityMultiplierSlider;

    public TMP_Dropdown m_MovementModeDropDown;
    public TMP_Dropdown m_ForwardModeDropDown;
    public Slider m_MaxMovementSpeedSlider;
    public Slider m_MovementAccelerationSlider;

    public TMP_Dropdown m_RotationModeDropDown;
    public Slider m_RotationSpeedSlider;
    public Slider m_SnapIncrementsSlider;



    //Tunneling
    public TMP_Dropdown m_TunnelingModeDropDown;
    public TMP_Dropdown m_TunnelingMotionModeDropDown;

    public Toggle m_TunnelingIgnorePlayerInputToggle;
    public Slider m_TunnelingMaxRadiusSlider;
    public Slider m_TunnelingMinRadiusSlider;
    public Slider m_TunnelingFadeInTimeSlider;
    public Slider m_TunnelingFadeOutTimeSlider;
    public Slider m_TunnelingSmoothingOffsetSlider;
    public Slider m_TunnelingMaxSpeedReferenceSlider;

    public Slider m_CageLineWidth;
    public Slider m_CageGridSize;

    public Slider m_CageLineColorR;
    public Slider m_CageLineColorG;
    public Slider m_CageLineColorB;
    public Slider m_CageBackgroundColorR;
    public Slider m_CageBackgroundColorG;
    public Slider m_CageBackgroundColorB;


    protected void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (m_InputDeviceDropDown)
            m_InputDeviceDropDown.value = (int)PlayerController.Instance.m_SelectedInputDevice;

        if (m_VirtualizerVelocityMultiplierSlider)
            m_VirtualizerVelocityMultiplierSlider.value = PlayerController.Instance.m_VirtualizerVelocityMultiplier;

        if (m_MovementModeDropDown)
            m_MovementModeDropDown.value = (int)PlayerController.Instance.m_MovementMode;

        if (m_ForwardModeDropDown)
            m_ForwardModeDropDown.value = (int)PlayerController.Instance.m_ForwardMode;

        if (m_MaxMovementSpeedSlider)
            m_MaxMovementSpeedSlider.value = PlayerController.Instance.m_MaxMovementSpeed;

        if (m_MovementAccelerationSlider)
            m_MovementAccelerationSlider.value = PlayerController.Instance.m_MovementAcceleration;

        if (m_RotationModeDropDown)
            m_RotationModeDropDown.value = (int)PlayerController.Instance.RotationMode;

        if (m_RotationSpeedSlider)
            m_RotationSpeedSlider.value = PlayerController.Instance.m_RotationSpeed;

        if (m_SnapIncrementsSlider)
            m_SnapIncrementsSlider.value = PlayerController.Instance.m_RotationSnappingIncrements;

        Tunneling tunneling = Tunneling.Instance;
        if (!tunneling)
            return;

        if (m_TunnelingModeDropDown)
            m_TunnelingModeDropDown.value = (int)tunneling.TunnelingMode;

        if (m_TunnelingMotionModeDropDown)
            m_TunnelingMotionModeDropDown.value = (int)tunneling.MotionMode;

        if (m_TunnelingIgnorePlayerInputToggle)
            m_TunnelingIgnorePlayerInputToggle.isOn = tunneling.m_IgnorePlayerInput;

        if (m_TunnelingMaxRadiusSlider)
            m_TunnelingMaxRadiusSlider.value = tunneling.m_MaxRadius;

        if (m_TunnelingMinRadiusSlider)
            m_TunnelingMinRadiusSlider.value = tunneling.m_MinRadius;

        if (m_TunnelingFadeInTimeSlider)
            m_TunnelingFadeInTimeSlider.value = tunneling.m_FadeInTime;

        if (m_TunnelingFadeOutTimeSlider)
            m_TunnelingFadeOutTimeSlider.value = tunneling.m_FadeOutTime;

        if (m_TunnelingSmoothingOffsetSlider)
            m_TunnelingSmoothingOffsetSlider.value = tunneling.m_SmoothingOffset;

        if(m_TunnelingMaxSpeedReferenceSlider)
            m_TunnelingMaxSpeedReferenceSlider.value = tunneling.m_MaxSpeedReference;

        Material cageGridMaterial = Tunneling.Instance.m_CageGridMaterial;
        if (!cageGridMaterial)
            return;

        if (m_CageLineWidth)
            m_CageLineWidth.value = cageGridMaterial.GetFloat("_LineWidth");

        if (m_CageGridSize)
            m_CageGridSize.value = cageGridMaterial.GetFloat("_GridSize");

        Color lineColor = GetCurrentCageLineColor();
        Color backgroundColor = GetCurrentCageBackgroundColor();

        if (m_CageLineColorR)
            m_CageLineColorR.value = lineColor.r;

        if (m_CageLineColorG)
            m_CageLineColorG.value = lineColor.g;

        if (m_CageLineColorB)
            m_CageLineColorB.value = lineColor.b;

        if (m_CageBackgroundColorR)
            m_CageBackgroundColorR.value = backgroundColor.r;

        if (m_CageBackgroundColorG)
            m_CageBackgroundColorG.value = backgroundColor.g;

        if (m_CageBackgroundColorB)
            m_CageBackgroundColorB.value = backgroundColor.b;
    }

    public void UpdateInputDevice(int mode) => PlayerController.Instance.InputDevice = (InputDevices)mode;

    public void UpdateVirtualizerVelocityMultiplier(float value) => PlayerController.Instance.m_VirtualizerVelocityMultiplier = value;

    public void UpdateMovementMode(int mode) => PlayerController.Instance.MovementMode = (VRControllerMovementModes)mode;

    public void UpdateForwardMode(int mode) => PlayerController.Instance.ForwardMode = (VRControllerForwardModes)mode;

    public void UpdateMaxMovementSpeed(float value) => PlayerController.Instance.m_MaxMovementSpeed = value;

    public void UpdateMovementAcceleration(float value) => PlayerController.Instance.m_MovementAcceleration = value;

    public void UpdateRotationMode(int mode) => PlayerController.Instance.RotationMode = (VRControllerRotationModes)mode;

    public void UpdateRotationSpeed(float value) => PlayerController.Instance.m_RotationSpeed = value;

    public void UpdateSnapIncrements(float value) => PlayerController.Instance.m_RotationSnappingIncrements = value;


    public void UpdateTunnelingMode(int mode)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.TunnelingMode = (TunnelingModes)mode;
    }

    public void UpdateTunnelingMotionMode(int mode)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.MotionMode = (TunnelingMotionModes)mode;
    }

    public void UpdateTunnelingIgnorePlayerInput(bool value)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.m_IgnorePlayerInput = value;
    }

    public void UpdateTunnelingMaxRadius(float value)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.m_MaxRadius = value;
    }

    public void UpdateTunnelingMinRadius(float value)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.m_MinRadius = value;
    }

    public void UpdateTunnelingFadeInTime(float value)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.m_FadeInTime = value;
    }

    public void UpdateTunnelingFadeoutTime(float value)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.m_FadeOutTime = value;
    }

    public void UpdateTunnelingSmoothingOffset(float value)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.m_SmoothingOffset = value;
    }

    public void UpdateTunnelingMaxSpeedRefence(float value)
    {
        if (Tunneling.Instance)
            Tunneling.Instance.m_MaxSpeedReference = value;
    }


    public void UpdateCageLineWidth(float value)
    {
        if (Tunneling.Instance && Tunneling.Instance.m_CageGridMaterial)
            Tunneling.Instance.m_CageGridMaterial.SetFloat("_LineWidth", value);
    }

    public void UpdateCageGridSize(float value)
    {
        if (Tunneling.Instance && Tunneling.Instance.m_CageGridMaterial)
            Tunneling.Instance.m_CageGridMaterial.SetFloat("_GridSize", value);
    }

    protected void UpdateCageLineColor(Color color)
    {
        if (Tunneling.Instance && Tunneling.Instance.m_CageGridMaterial)
            Tunneling.Instance.m_CageGridMaterial.SetColor("_LineColor", color);
    }

    protected void UpdateCageBackgroundColor(Color color)
    {
        if (Tunneling.Instance && Tunneling.Instance.m_CageGridMaterial)
            Tunneling.Instance.m_CageGridMaterial.SetColor("_BackgroundColor", color);
    }

    public void UpdateCageLineColorR(float value)
    {
        UpdateCageLineColor(GetNewLineColor(value, -1.0f, -1.0f));
    }

    public void UpdateCageLineColorG(float value)
    {
        UpdateCageLineColor(GetNewLineColor(-1.0f, value, -1.0f));
    }

    public void UpdateCageLineColorB(float value)
    {
        UpdateCageLineColor(GetNewLineColor(-1.0f, -1.0f, value));
    }

    public void UpdateCageBackgroundColorR(float value)
    {
        UpdateCageBackgroundColor(GetNewBackgroundColor(value, -1.0f, -1.0f));
    }

    public void UpdateCageBackgroundColorG(float value)
    {
        UpdateCageBackgroundColor(GetNewBackgroundColor(-1.0f, value, -1.0f));
    }

    public void UpdateCageBackgroundColorB(float value)
    {
        UpdateCageBackgroundColor(GetNewBackgroundColor(-1.0f, -1.0f, value));
    }

    protected Color GetNewBackgroundColor(float r, float g, float b)
    {
        Color color = GetCurrentCageBackgroundColor();

        if (r >= 0)
            color.r = r;

        if (g >= 0)
            color.g = g;

        if (b >= 0)
            color.b = b;

        return color;

    }

    protected Color GetNewLineColor(float r, float g, float b)
    {
        Color color = GetCurrentCageLineColor();

        if (r >= 0)
            color.r = r;

        if (g >= 0)
            color.g = g;

        if (b >= 0)
            color.b = b;

        return color;

    }

    protected Color GetCurrentCageLineColor()
    {
        if (!Tunneling.Instance || !Tunneling.Instance.m_CageGridMaterial)
            return Color.black;

        return Tunneling.Instance.m_CageGridMaterial.GetColor("_LineColor");
    }

    protected Color GetCurrentCageBackgroundColor()
    {
        if (!Tunneling.Instance || !Tunneling.Instance.m_CageGridMaterial)
            return Color.black;

        return Tunneling.Instance.m_CageGridMaterial.GetColor("_BackgroundColor");
    }
}
