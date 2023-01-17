using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public Slider m_AudioVolumeSlider;

    public Toggle m_LimitFrameRateToggle;
    public Slider m_FramerateValueSlider;

    public TMP_Dropdown m_SimulateTrackingErrorsDropDown;
    public Slider m_MinErrorDuration;
    public Slider m_MaxErrorDuration;
    public Slider m_MinDurationBetween;
    public Slider m_MaxDurationBetween;

    protected void Start()
    {

        UpdateUI();

    }

    public void UpdateUI()
    {

        if (m_AudioVolumeSlider)
            m_AudioVolumeSlider.value = AudioListener.volume;

        FrameLimiter limiter = FrameLimiter.Instance;
        bool frameLimiterExists = FrameLimiter.Exists;
        if (m_LimitFrameRateToggle)
            m_LimitFrameRateToggle.isOn = frameLimiterExists ? limiter.m_FramerateLimitEnabled : false;

        if (m_FramerateValueSlider)
            m_FramerateValueSlider.value = frameLimiterExists ? limiter.m_FramerateLimit : 0.0f;



        TrackingErrorSimulator trackingErrorSimulator = TrackingErrorSimulator.Instance;
        bool simulatorExists = TrackingErrorSimulator.Exists;


        if (m_SimulateTrackingErrorsDropDown)
            m_SimulateTrackingErrorsDropDown.value = simulatorExists ? (int)trackingErrorSimulator.m_SelectedErrorType : 0;

        if (m_MinErrorDuration)
            m_MinErrorDuration.value = simulatorExists ? trackingErrorSimulator.m_MinDuration : 0.0f;

        if (m_MaxErrorDuration)
            m_MaxErrorDuration.value = simulatorExists ? trackingErrorSimulator.m_MaxDuration : 0.0f;

        if (m_MinDurationBetween)
            m_MinDurationBetween.value = simulatorExists ? trackingErrorSimulator.m_MinDurationBetween : 0.0f;

        if (m_MaxDurationBetween)
            m_MaxDurationBetween.value = simulatorExists ? trackingErrorSimulator.m_MaxDurationBetween : 0.0f;
    }

    public void UpdateAudioVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void UpdateLimitFramerate(bool value)
    {
        if (FrameLimiter.Exists)
            FrameLimiter.Instance.m_FramerateLimitEnabled = value;
    }

    public void UpdateFramerateLimit(float value)
    {
        if (FrameLimiter.Exists)
            FrameLimiter.Instance.m_FramerateLimit = (int)value;
    }

    public void UpdateSimulatedErrorType(int value)
    {
        if (TrackingErrorSimulator.Exists)
            TrackingErrorSimulator.Instance.m_SelectedErrorType = (TrackingErrorType)value;
    }

    public void UpdateMinErrorDuration(float value)
    {
        if (TrackingErrorSimulator.Exists)
            TrackingErrorSimulator.Instance.m_MinDuration = value;
    }

    public void UpdateMaxErrorDuration(float value)
    {
        if (TrackingErrorSimulator.Exists)
            TrackingErrorSimulator.Instance.m_MaxDuration = value;
    }

    public void UpdateMinDurationBetween(float value)
    {
        if (TrackingErrorSimulator.Exists)
            TrackingErrorSimulator.Instance.m_MinDurationBetween = value;
    }

    public void UpdateMaxDurationBetween(float value)
    {
        if (TrackingErrorSimulator.Exists)
            TrackingErrorSimulator.Instance.m_MaxDurationBetween = value;
    }
}
