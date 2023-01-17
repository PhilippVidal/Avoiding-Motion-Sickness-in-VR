using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{

    [Range(-1.0f, 1.0f)] public float ForwardAxis = 0.0f;
    [Range(-1.0f, 1.0f)] public float RightAxis = 0.0f;

    public float m_MaxAcceleration = 10.0f;
    //public float m_MaxBreakForce = 300f;
    public float m_MaxTurnAngle = 20f;

    public float m_MaxSpeed = 3.0f;
    protected float m_CurrentSpeed;

    public float m_CurrentAcceleration = 0.0f;
    public float m_CurrentBreakForce = 0.0f;
    public float m_CurrentTurnAngle = 0.0f;

    protected Rigidbody m_Rigidbody;

    public bool m_Active = false;

    public float m_VerticalInputDeadZone = 0.25f;
    public float m_BreakStrength = 0.8f;


    public Valve.VR.InteractionSystem.LinearMapping m_VerticalLinearMapping;
    public SteeringWheel m_SteeringWheel;

    public Transform m_GetInLocation;
    public Transform m_GetOutLocation;

    public float m_Value;

    public AudioSource m_EngineAudioSource;
    public AudioSource m_WindAudioSource;
    public AudioSource m_CrashAudioSource;
    public Valve.VR.InteractionSystem.LinearDrive m_LinearDrive;
    protected bool m_LinearDriveCanUpdate = true;
    protected DrivingModes m_DrivingMode = DrivingModes.IDLE;


    public SteamVR_Action_Single m_AcceleratorInput;
    public SteamVR_Input_Sources m_AcceleratorSource = SteamVR_Input_Sources.RightHand;
    public SteamVR_Action_Single m_BrakeInput;
    public SteamVR_Input_Sources m_BrakeSource = SteamVR_Input_Sources.LeftHand;

    protected float m_AcceleratorInputValue = 0.0f;
    protected float m_BrakeInputValue = 0.0f;

    protected Vector3 m_ResetPosition;
    protected Quaternion m_ResetRotation;



    protected Vector3 m_PositionPriorGetIn;
    protected Quaternion m_RotationPriorGetIn;

    public enum DrivingModes
    {       
        IDLE,
        BACKWARDS,
        FORWARDS
    }

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        m_LinearDrive.repositionGameObject = false;

        m_ResetPosition = transform.position;
        m_ResetRotation = transform.rotation;
    }


    protected void Update()
    {
        EvaluateLinearDrive();
        EvaluateInput();       
    }

    public void Reset()
    {
        m_Rigidbody.position = m_ResetPosition;
        m_Rigidbody.rotation = m_ResetRotation;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_CurrentSpeed = 0.0f;
        transform.localRotation = m_ResetRotation;
    }

    protected void FixedUpdate()
    {      
        if (!m_Active)       
            return;

        float verticalInput = GetInputVertical(); 
        float horizontalInput = GetInputHorizontal();

        m_Value = verticalInput;

        float sign = GetDirectionSign();

        m_CurrentAcceleration = verticalInput * m_MaxAcceleration * Time.fixedDeltaTime;
        
        m_CurrentSpeed += sign * m_CurrentAcceleration;
        m_CurrentSpeed -= sign * m_BreakStrength * m_BrakeInputValue * Time.fixedDeltaTime;

        //Slow-down effect/drag
        m_CurrentSpeed -= sign * 5.0f * Time.fixedDeltaTime;

        if ((sign > 0.0f && m_CurrentSpeed < 0.0f) || (sign < 0.0f && m_CurrentSpeed > 0.0f))
            m_CurrentSpeed = 0.0f;


        //if(verticalInput == 0.0f)
        //{
        //    //float brakeValue = m_CurrentSpeed < 0.0f ? 1.0f : -1.0f;
        //    float brakeValue = m_BreakStrength * Time.fixedDeltaTime;
        //    float currentSpeedAbs = Mathf.Abs(m_CurrentSpeed);

        //    if(currentSpeedAbs < brakeValue)
        //        brakeValue = currentSpeedAbs;

        //    m_CurrentSpeed += m_CurrentSpeed < 0.0f ? brakeValue : -brakeValue;
        //}


        if (m_CurrentSpeed > m_MaxSpeed)
            m_CurrentSpeed = m_MaxSpeed;

        if (m_CurrentSpeed < -m_MaxSpeed)
            m_CurrentSpeed = -m_MaxSpeed;

        m_CurrentTurnAngle = horizontalInput * m_MaxTurnAngle;//  * Mathf.Clamp01(1.0f - Mathf.Abs(m_CurrentSpeed / m_MaxSpeed));

        if (m_CurrentSpeed != 0.0f)
        {
            Quaternion rotation = Quaternion.Euler(0.0f, sign * m_CurrentTurnAngle * Time.fixedDeltaTime, 0.0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rotation);
        }



        //m_Rigidbody.rotation *= rotation;
        m_Rigidbody.MovePosition((m_CurrentSpeed * Time.fixedDeltaTime * transform.forward) + transform.position);

        if (m_EngineAudioSource)
        {
            m_EngineAudioSource.pitch = 0.3f + 1.0f * Mathf.Abs(m_CurrentSpeed / m_MaxSpeed);
        }

        if (m_WindAudioSource)
        {
            m_WindAudioSource.volume = Mathf.Clamp01(m_CurrentSpeed / m_MaxSpeed);
        }
        //UpdateWheelValues();      
    }

    protected void LateUpdate()
    {
        if (transform.localRotation.eulerAngles.x != 0.0f || transform.localRotation.eulerAngles.z != 0.0f)
            transform.localRotation = Quaternion.Euler(0.0f, transform.localRotation.eulerAngles.y, 0.0f);

    }

    protected float GetDirectionSign()
    {
        if (m_DrivingMode == DrivingModes.FORWARDS)
            return 1.0f;

        if (m_DrivingMode == DrivingModes.BACKWARDS)
            return -1.0f;


        return 0.0f;
    }

    protected virtual void EvaluateInput()
    {
        if (!m_SteeringWheel.HasHandAttached)
        {
            m_AcceleratorInputValue = 0.0f;
            m_BrakeInputValue = 1.0f;

            return;
        }
            

        m_AcceleratorInputValue = m_AcceleratorInput.GetAxis(m_AcceleratorSource);
        m_BrakeInputValue = m_BrakeInput.GetAxis(m_BrakeSource);
    }




    //protected void UpdateWheelValues()
    //{

    //    m_WheelColliderLF.steerAngle = m_CurrentTurnAngle;
    //    m_WheelColliderRF.steerAngle = m_CurrentTurnAngle;



    //    m_WheelColliderLF.motorTorque = m_CurrentAcceleration;
    //    m_WheelColliderRF.motorTorque = m_CurrentAcceleration;


    //    m_WheelColliderLF.brakeTorque = m_CurrentBreakForce;
    //    m_WheelColliderRF.brakeTorque = m_CurrentBreakForce;
    //    m_WheelColliderLB.brakeTorque = m_CurrentBreakForce;
    //    m_WheelColliderRB.brakeTorque = m_CurrentBreakForce;

    //}


    protected float GetInputVertical()
    {

        return m_AcceleratorInputValue;

        //float value = m_VerticalLinearMapping.value * 2.0f - 1.0f;

        //if (value < m_VerticalInputDeadZone && value > -m_VerticalInputDeadZone)
        //    return 0.0f;

        //float difference = 1.0f - m_VerticalInputDeadZone;
        //return value < 0.0f ? (value + m_VerticalInputDeadZone) / difference : (value - m_VerticalInputDeadZone) / difference;
    }

    protected float GetInputHorizontal()
    {
        //float value = m_HorizontalLinearMapping.value;
        //return value * 2.0f - 1.0f;

        return m_SteeringWheel.NormalizedValue;
    }

    public void EvaluateLinearDrive()
    {
        m_LinearDrive.repositionGameObject = false;
        float linearDriveValue = m_LinearDrive.linearMapping.value;
        DrivingModes mode = EvalDrivingMode(linearDriveValue);

        //print("Value => " + linearDriveValue);


        if (mode == m_DrivingMode || mode == DrivingModes.IDLE)
            return;

        //print("Value before change => " + linearDriveValue);

        //print("Updated mode from " + m_DrivingMode.ToString() + " to " + mode.ToString());
        m_LinearDrive.repositionGameObject = true;
        m_DrivingMode = mode;

        //print("Mode is now => " + m_DrivingMode.ToString());
    }

    protected DrivingModes EvalDrivingMode(float value)
    {
        if (value == 0.0f)
            return DrivingModes.FORWARDS;

        if (value == 1.0f)
            return DrivingModes.BACKWARDS;

        return DrivingModes.IDLE;
    }

    public void GetIn()
    {
        StartAudio();
        AttachPlayer();
        m_Active = true;
    }

    public void GetOut()
    {
        EndAudio();
        DetachPlayer();
        m_Active = false;
        Reset();     
    }

    protected void AttachPlayer()
    {
        PlayerController player = PlayerController.Instance;

        m_PositionPriorGetIn = player.transform.position;
        m_RotationPriorGetIn = player.transform.rotation;

        player.OrientatePlayer(transform.rotation);
        player.CharacterControllerActive = false;

        //Vector3 position = m_GetInLocation.position - transform.TransformVector(Vector3.up * player.PlayerHeight * 0.33f);

        Vector3 position = m_GetInLocation.position - transform.TransformVector(Vector3.up * player.PlayerHeight);
        player.TeleportTo(position);
        player.AttachPlayerToObject(gameObject, true, true);
        player.HandPhysicsEnabled = false;
    }


    protected void DetachPlayer()
    {
        m_SteeringWheel.DetachAllHands();
        PlayerController player = PlayerController.Instance;
        player.HandPhysicsEnabled = true;

        //StartCoroutine(EnableHandPhysicsAfterSeconds(3));
        player.DetachPlayerFromObject();    
        player.CharacterControllerActive = true;
        player.OrientatePlayer(m_RotationPriorGetIn);



        if (m_GetOutLocation)
        {
            player.TeleportTo(m_GetOutLocation.position);
            return;
        }


        player.TeleportTo(m_PositionPriorGetIn);

    }


    IEnumerator EnableHandPhysicsAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PlayerController.Instance.HandPhysicsEnabled = true;
    }

    protected void StartAudio()
    {
        if (m_EngineAudioSource)
            m_EngineAudioSource.Play();

        if (m_WindAudioSource)
            m_WindAudioSource.Play();
    }

    protected void EndAudio()
    {
        if (m_EngineAudioSource)
            m_EngineAudioSource.Stop();

        if (m_WindAudioSource)
            m_WindAudioSource.Stop();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        
        if (m_CrashAudioSource && !m_CrashAudioSource.isPlaying)
        {
            float impulseWeight = collision.impulse.magnitude / 20.0f;
            //Debug.Log(impulseWeight);
            m_CrashAudioSource.volume = 0.60f * Mathf.Clamp01(impulseWeight);
            m_CrashAudioSource.Play();
        }
    }

}
