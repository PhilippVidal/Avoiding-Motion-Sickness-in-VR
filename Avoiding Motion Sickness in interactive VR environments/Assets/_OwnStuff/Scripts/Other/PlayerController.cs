using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using CybSDK;
using UnityEngine.Events;

public enum RespawnType
{
    INSTANT, 
    TRIGGER,
    HIT
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CVirtDeviceController))]
[RequireComponent(typeof(Valve.VR.InteractionSystem.Player))]
public class PlayerController : MonoBehaviour
{
    [Header("Have to be set up")]
    public Camera m_Camera;
    public GameObject m_RightController;
    public GameObject m_LeftController;
    public LaserPointer m_LaserPointer;
    public Valve.VR.InteractionSystem.HandPhysics m_RightHandPhysics;
    public Valve.VR.InteractionSystem.HandPhysics m_LeftHandPhysics;
    public Transform m_HeadAttachmentTransform;
    public Valve.VR.InteractionSystem.Teleport m_TeleportScript;

    [Header("SteamVR Inputs")]
    public SteamVR_Action_Vector2 m_MovementInput;
    public SteamVR_Action_Boolean m_SnapInputRightInput;
    public SteamVR_Action_Boolean m_SnapInputLeftInput;
    public SteamVR_Action_Boolean m_JumpInput;

    [Header("Input Settings")]
    public InputDevices m_SelectedInputDevice = InputDevices.VR_CONTROLLER;
    [Space]
    public VRControllerMovementModes m_MovementMode = VRControllerMovementModes.LINEAR;
    public VRControllerRotationModes m_RotationMode = VRControllerRotationModes.SNAPPING;
    public VRControllerForwardModes m_ForwardMode = VRControllerForwardModes.HMD;

    [Header("Locomotion Settings")]
    public float m_VirtualizerVelocityMultiplier = 1.4f;  
    [Space]
    public float m_MaxMovementSpeed = 1.4f;
    public float m_MovementAcceleration = 0.7f;
    public float m_RotationSpeed = 45.0f;
    public float m_RotationSnappingIncrements = 30.0f;
    [Space]
    public bool m_JumpingAllowed = false;
    public float m_JumpHeight = 1.5f;
    public bool m_GravityEnabled = true;
    public float m_Gravity = -9.81f;

    [Header("Other Settings")]
    public float m_RespawnFadeTime = 0.7f;
    public float m_HitFadeTime = 1.0f;
    public float m_ChangeSceneFadeTime = 2.0f;
    public Color m_FadeColor = Color.black;
    public LayerMask m_GroundCheckLayerMask;

    [Header("Events")]
    public UnityEvent m_OnMenuRoomExit;
    public UnityEvent m_OnMenuRoomEnter;


    protected CharacterController m_CharacterController;
    protected CVirtDeviceController m_CVirtDeviceController;
    protected VR_InputDevice m_InputDevice;
    protected UnityEngine.SceneManagement.Scene m_OriginScene;

    protected bool m_InputEnabled = true;
    protected bool m_IsGrounded;
    protected bool m_CanJump;
    protected Vector3 m_LastPosition;
    protected Vector3 m_AbsoluteVelocity;
    protected Vector3 m_RelativeVelocity;
    protected Quaternion m_LastRotation;
    [HideInInspector] public VRCamera m_VRCamera;
    [HideInInspector] public int m_ClimbingPoints;
    [HideInInspector] public Vector3 m_InputPositionChange;
    protected float m_CurrentFallVelocity = 0.0f;
    protected GameObject m_AttachedToObject = null;
    protected CapsuleCollider m_CapsuleCollider;
    protected bool m_ForceAttached = false;
    protected RaycastHit m_GroundCheckHitInfo;
    protected bool m_HandPhysicsEnabled = true;
    protected Vector3 m_NoInputVelocityVector;

    protected static PlayerController m_Instance;
    public static PlayerController Instance
    {
        get
        {
            if (!m_Instance)
                m_Instance = FindObjectOfType<PlayerController>();

            if (!m_Instance)
                Debug.LogWarning("[Player Controller] No player could be found in the scene!");

            return m_Instance;
        }
    }

    #region Properties

    public Valve.VR.InteractionSystem.Hand RightHand { get { return Valve.VR.InteractionSystem.Player.instance.rightHand; } }
    public Valve.VR.InteractionSystem.Hand LeftHand { get { return Valve.VR.InteractionSystem.Player.instance.leftHand; } }

    public bool InputEnabled
    {
        get
        {
            return m_InputEnabled;
        }
        set
        {
            m_InputEnabled = value;
            CanTeleport = value;
        }
    }

    public bool CanTeleport
    {
        get
        {
            return m_TeleportScript.m_CanTeleport;
        }
        set
        {
            if(InputDevice == InputDevices.VIRTUALIZER)
            {
                m_TeleportScript.m_CanTeleport = false;
                return;
            }

            m_TeleportScript.m_CanTeleport = value;
        }
    }

    protected bool m_IsInMenuRoom = true;
    public bool IsInMenuRoom
    {
        get
        {
            return m_IsInMenuRoom;
        }

        set
        {
            if (m_IsInMenuRoom == value)
                return;

            m_IsInMenuRoom = value;

            if (m_IsInMenuRoom)
            {
                m_OnMenuRoomEnter.Invoke();
                return;
            }

            m_OnMenuRoomExit.Invoke();    
        }
    }

    public InputDevices InputDevice
    {
        get
        {
            return m_SelectedInputDevice;
        }
        set
        {
            m_SelectedInputDevice = value;
            SetupInputDevice();
        }
    }

    public VRControllerMovementModes MovementMode 
    { 
        get 
        { 
            return m_MovementMode;
        } 
        set
        {
            m_MovementMode = value;
            SetupInputDevice();
        }
    }

    public VRControllerForwardModes ForwardMode
    {
        get
        {
            return m_ForwardMode;
        }
        set
        {
            m_ForwardMode = value;
            SetupInputDevice();
        }
    }

    public VRControllerRotationModes RotationMode
    {
        get
        {
            return m_RotationMode;
        }
        set
        {
            m_RotationMode = value;
            SetupInputDevice();
        }
    }
  
    public Valve.VR.InteractionSystem.Player SteamVRPlayer 
    {
        get 
        { 
            return Valve.VR.InteractionSystem.Player.instance;
        } 
    }

    public bool IsClimbing 
    { 
        get 
        { 
            return m_ClimbingPoints > 0;
        } 
    }

    public bool IsAttachedToObject { get { return m_AttachedToObject != null; } }

    //Absolute velocity => global position change / time 
    //Character standing still on moving platform => velocity != 0 
    public Vector3 AbsoluteVelocityVector { get { return m_AbsoluteVelocity; } }
    public float AbsoluteVelocity { get { return m_AbsoluteVelocity.magnitude; } }

    //Relative velocity => only character movement change / time 
    //Character standing still on moving platform => velocity = 0 
    public Vector3 RelativeVelocityVector { get { return m_CharacterController.velocity; } }
    public float RelativeVelocity { get { return m_CharacterController.velocity.magnitude; } }

    //No Input velocity => Absolute Velocity without the user input
    public Vector3 NoInputVelocityVector { get { return m_NoInputVelocityVector; } }
    public float NoInputVelocity { get { return m_NoInputVelocityVector.magnitude; } }

    public Transform HeadTransform { get { return m_Camera.transform; } }
    public CharacterController CharacterController { get { return m_CharacterController; } }

    public Transform HeadAttachmentTransform {  get { return m_HeadAttachmentTransform; } }

    public Vector3 HeadPositionNoHeight
    {
        get
        {

            Vector3 headLocalPosition = HeadTransform.localPosition;
            headLocalPosition.y = 0.0f;
            return transform.TransformPoint(headLocalPosition);
        }
    }

    public bool CharacterControllerActive 
    { 
        get { return m_CharacterController.enabled; }
        set { m_CharacterController.enabled = value; }
    }

    public float PlayerHeight { get { return HeadTransform.localPosition.y; } }

    public bool HandPhysicsEnabled
    {
        get { return m_HandPhysicsEnabled; }
        set
        {
            if (value)
            {
                Valve.VR.InteractionSystem.RenderModel model;

                if (m_RightHandPhysics)
                {
                    m_RightHandPhysics.enabled = true;
                    m_RightHandPhysics.handCollider.gameObject.SetActive(true);

                    model = m_RightHandPhysics.hand.mainRenderModel;
                    if (model)
                        model.MatchHandToTransform(model.transform);

                    model = m_RightHandPhysics.hand.hoverhighlightRenderModel;
                    if (model)
                        model.MatchHandToTransform(model.transform);
                }

                if (m_LeftHandPhysics)
                {
                    m_LeftHandPhysics.enabled = true;
                    m_LeftHandPhysics.handCollider.gameObject.SetActive(true);

                    model = m_LeftHandPhysics.hand.mainRenderModel;
                    if (model)
                        model.MatchHandToTransform(model.transform);

                    model = m_LeftHandPhysics.hand.hoverhighlightRenderModel;
                    if (model)
                        model.MatchHandToTransform(model.transform);
                }

                Debug.Log("[Player Controller] Hand physics have been enabled!");
                m_HandPhysicsEnabled = true;
                return;
            }


            if (m_RightHandPhysics)
            {
                m_RightHandPhysics.enabled = false;
                m_RightHandPhysics.handCollider.gameObject.SetActive(false);
            }

            if (m_LeftHandPhysics)
            {
                m_LeftHandPhysics.enabled = false;
                m_LeftHandPhysics.handCollider.gameObject.SetActive(false);
            }


            Debug.Log("[Player Controller] Hand physics have been disabled!");
            m_HandPhysicsEnabled = false;
        }
    }

    #endregion

    #region Unity Functions 

    protected void Awake()
    {
        if (!TryGetComponent<CVirtDeviceController>(out m_CVirtDeviceController))
        {
            CLogger.LogError(string.Format("Player Controller requires a CVirtDeviceController attached to gameobject '{0}'.", gameObject.name));
            enabled = false;
            return;
        }

        m_CharacterController = GetComponent<CharacterController>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_VRCamera = m_Camera.GetComponent<VRCamera>();
    }

    protected void Start()
    {
        //TryToFindStartPosition();
        m_LastPosition = transform.position;
        m_LastRotation = transform.rotation;
        SetupInputDevice();
        m_OriginScene = gameObject.scene;
    }

    protected void Update()
    {
        DoGroundCheck();
        UpdateRotation();
        UpdateTranslation();
        UpdateBodyPosition();
        UpdateHands();
    }

    protected void LateUpdate()
    {
        //if(IsAttachedToObject)
        //{
        //    m_AbsoluteVelocity = (m_AttachedToObject.transform.position - m_LastPosition) / Time.deltaTime;
        //    m_LastPosition = m_AttachedToObject.transform.position;
        //    return;
        //}

        m_AbsoluteVelocity = (transform.position - m_LastPosition) / Time.deltaTime;
        m_NoInputVelocityVector = ((transform.position - m_LastPosition) - m_InputPositionChange) / Time.deltaTime;
        m_LastPosition = transform.position;
        m_LastRotation = transform.rotation;
    }

    protected void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Something entered trigger==> " + other.gameObject.name);
        Vector3 direction = transform.position - other.ClosestPoint(transform.position);
        Debug.DrawRay(transform.position, direction, Color.red);
        Hit(other);
    }

    #endregion

    #region Locomotion, Position and Rotation

    protected void DoGroundCheck()
    {

        Vector3 cameraPosition = m_Camera.transform.localPosition;
        cameraPosition.y = 0.0f;

        Vector3 startPos = m_Camera.transform.position;
        startPos.y -= m_Camera.transform.localPosition.y;

        Debug.DrawLine(startPos, startPos + Vector3.down * 0.1f, Color.red);
        if (!Physics.Raycast(startPos, -transform.up, out m_GroundCheckHitInfo, 0.1f, m_GroundCheckLayerMask))
        {
            if (transform.parent && !m_ForceAttached)
                DetachPlayerFromObject();

            return;
        }

        Debug.DrawLine(startPos, m_GroundCheckHitInfo.point, Color.green);
        GameObject hitGO = m_GroundCheckHitInfo.collider.gameObject;

        //Already attached, don't do anything
        if (hitGO == m_AttachedToObject)
            return;

        //object to attach to => attach
        if (hitGO.CompareTag("MovingEntity"))
        {
            MovingEntity entity;
            if (hitGO.TryGetComponent<MovingEntity>(out entity))
            {
                entity.SetCharacterController(m_CharacterController);
                AttachPlayerToObject(hitGO);
            }
        }
    }

    protected void UpdateTranslation()
    {
        m_InputPositionChange = Vector3.zero;
        //Don't update when forcefully attached to some object => object updates position
        if (IsAttachedToObject && m_ForceAttached)
            return;

        m_IsGrounded = m_CharacterController.isGrounded;
        if (m_IsGrounded)
            m_CanJump = true;

        //Input movement
        Vector3 horizontalTranslationVector = GetInputDeviceTranslation();
        

        if (m_GroundCheckHitInfo.normal != Vector3.zero)
        {
            Vector3 projectedVector = Vector3.ProjectOnPlane(horizontalTranslationVector, m_GroundCheckHitInfo.normal);
            horizontalTranslationVector = projectedVector.normalized * horizontalTranslationVector.magnitude;
        }
            


        Debug.DrawRay(transform.position, horizontalTranslationVector, Color.yellow);

        Vector3 oldPos = transform.position;
        m_CharacterController.Move(horizontalTranslationVector * Time.deltaTime);
        Vector3 newPos = transform.position;
        m_InputPositionChange = newPos - oldPos;

        //When climbing, don't update vertical velocity
        if (IsClimbing || IsAttachedToObject && m_ForceAttached)
        {
            m_CurrentFallVelocity = 0.0f;
            return;
        }


        m_CurrentFallVelocity = GetGravity();


        //Jumping
        if (m_JumpInput.state && m_CanJump && m_JumpingAllowed)
        {
            m_CurrentFallVelocity += Mathf.Sqrt(m_JumpHeight * -3.0f * m_Gravity);
            m_CanJump = false;
        }

        Vector3 gravityMovementVector = Vector3.up * m_CurrentFallVelocity * Time.deltaTime;
        m_CharacterController.Move(gravityMovementVector);
    }

    protected float GetGravity()
    {
        if (m_GravityEnabled && !m_IsGrounded)
            return m_CurrentFallVelocity + m_Gravity * Time.deltaTime;

        //Fix for character controller not setting CharacterController.isGrounded reliably
        return -0.1f;
    }

    protected Vector3 GetInputDeviceTranslation()
    {
        //Don't care about controller input while climbing or when Input is disabled
        if (IsClimbing || !InputEnabled)
            return Vector3.zero;

        //Locomotion input from input device -> no vertical direction
        Vector3 inputTranslation = m_InputDevice.GetMovementVector();
        inputTranslation.y = 0.0f;

        return inputTranslation;
    }

    protected void UpdateRotation()
    {
        if (m_SelectedInputDevice == InputDevices.VIRTUALIZER)
            return;

        if (m_ForceAttached || IsClimbing)
            return;

        //Quaternion updatedRotation = m_InputDevice.GetUpdatedRotation();
        //if (updatedRotation == Quaternion.identity && )
        //     return;

        transform.rotation = m_InputDevice.GetUpdatedRotation();
        //transform.localRotation = m_InputDevice.GetUpdatedRotation();
    }

    protected void UpdateBodyPosition()
    {
        Vector3 cameraLocalPosition = m_Camera.transform.localPosition;
        float playerHeight = cameraLocalPosition.y;
        float centerHeight = playerHeight * 0.5f;
        m_CharacterController.height = playerHeight;

        cameraLocalPosition.y = centerHeight;
        m_CharacterController.center = cameraLocalPosition;
    }

    protected void UpdateHands()
    {
        //If hand physics are enabled there is
        //no need to manually update their position
        if (HandPhysicsEnabled)
            return;


        Valve.VR.InteractionSystem.RenderModel mainRenderModel;
        Valve.VR.InteractionSystem.Hand hand = RightHand;
        if (hand)
        {
            mainRenderModel = RightHand.mainRenderModel;
            if (mainRenderModel)
            {
                mainRenderModel.SetHandRotation(RightHand.transform.rotation);
                mainRenderModel.SetHandPosition(RightHand.transform.position);
            }
        }

        hand = LeftHand;
        if (hand)
        {
            mainRenderModel = LeftHand.mainRenderModel;
            if (mainRenderModel)
            {
                mainRenderModel.SetHandRotation(LeftHand.transform.rotation);
                mainRenderModel.SetHandPosition(LeftHand.transform.position);
            }
        }
    }

    public void OrientatePlayer(Quaternion rotation)
    {
        Quaternion headLocalRotation = HeadTransform.localRotation;
        Quaternion headWorldRotationY = new Quaternion(
            0.0f,
            headLocalRotation.y,
            0.0f,
            headLocalRotation.w * Mathf.Pow((headLocalRotation.w * headLocalRotation.w) + (headLocalRotation.y * headLocalRotation.y), -0.5f)
            );

        transform.rotation = rotation * Quaternion.Inverse(headWorldRotationY);
    }

    public void Move(Vector3 direction)
    {
        m_CharacterController.Move(direction);
    }

    public void TeleportTo(Vector3 position)
    {
        //Disable character controller to allow manual position changes 
        CharacterControllerActive = false;

        //Take room-scale player offset into account 
        Vector3 headOffset = HeadTransform.position - transform.position;
        headOffset.y = 0.0f;
        transform.position = position - headOffset;

        CharacterControllerActive = true;

        ResetVelocity();

        ////Disable character controller to allow manual position changes 
        //Vector3 playerOffset = m_VRCamera.transform.localPosition;
        //m_CharacterController.enabled = false;
        //Vector3 teleportPosition = position;
        ////Take room-scale player offset into account 
        ////Vector3 playerOffset = m_CharacterController.center;
        //playerOffset.y = 0.0f;
        ////playerOffset = new Vector3(100f, 100f, 100f);
        //Debug.Log("PlayerOffset => " + playerOffset.ToString());
        //transform.position = teleportPosition - playerOffset;
        //m_CharacterController.enabled = true;
    }

    public void TeleportToWithFade(Vector3 position, float fateTime)
    {
        StartFadeOut(fateTime);
        StartCoroutine(TeleportAfterFade(position, fateTime));
    }

    public void TeleportToLastCheckpoint(RespawnType type = RespawnType.TRIGGER)
    {

        Vector3 respawnPosition = GameManager.Instance.GetRespawnLocation();
        switch (type)
        {
            case RespawnType.INSTANT:
                TeleportTo(respawnPosition);
                break;
            case RespawnType.TRIGGER:
                TeleportToWithFade(respawnPosition, m_RespawnFadeTime);
                break;
            case RespawnType.HIT:
                TeleportToWithFade(respawnPosition, m_HitFadeTime);
                break;
            default:
                TeleportTo(respawnPosition);
                break;

        }
    }

    protected IEnumerator TeleportAfterFade(Vector3 position, float fateTime)
    {
        yield return new WaitForSeconds(fateTime);
        TeleportTo(position);
        StartFadeIn(fateTime);
        yield return null;
    }

    #endregion

    #region Scene Interaction 
    public void RespawnPlayer()
    {
        Debug.Log("[Player Controller] Respawning player!");

        if(GameManager.Instance.m_RespawnCounter)
        {
            GameManager.Instance.m_RespawnCounter.IncreaseRespawnCount();
        }

        TeleportToLastCheckpoint();
    }

    public void AttachPlayerToObject(GameObject obj, bool disableInput = false, bool forceAttach = false)
    {
        if (disableInput || forceAttach)
            InputEnabled = false;

        if (forceAttach)
            m_ForceAttached = true;


        transform.parent = obj.transform;
        m_AttachedToObject = obj;
        CanTeleport = false;
        //m_LastPosition = m_AttachedToObject.transform.position;
    }

    public void DetachPlayerFromObject()
    {
        if (m_AttachedToObject)
        {
            MovingEntity entity;
            if (m_AttachedToObject.CompareTag("MovingEntity") && m_AttachedToObject.TryGetComponent<MovingEntity>(out entity))
            {
                entity.SetCharacterController(null);
            }
        }

        m_ForceAttached = false;
        InputEnabled = true;
        m_AttachedToObject = null;
        transform.parent = null;
        //m_LastPosition = transform.position;
        ResetScene();
        CanTeleport = true;
    }

    public void Hit(Collider other)
    {
        if (other.gameObject.CompareTag("MovingObstacle"))
        {
            RespawnPlayer();
        }
    }
    #endregion

    #region Utility 

    public void ResetScene()
    {
        if (gameObject.scene == m_OriginScene)
            return;

        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, m_OriginScene);
    }

    public void SetupInputDevice()
    {
        m_InputDevice = null;
        CanTeleport = true;
        switch (m_SelectedInputDevice)
        {
            case InputDevices.VR_CONTROLLER:
                m_InputDevice = new VR_InputDevice_VRController();
                break;
            case InputDevices.VIRTUALIZER:
                m_InputDevice = new VR_InputDevice_Virtualizer(m_CVirtDeviceController);
                CanTeleport = false;
                break;
            default:
                m_InputDevice = new VR_InputDevice_VRController();
                break;
        }
    }

    public void FadeScreen(bool fadeOut = true, float seconds = 1.0f)
    {
        if (fadeOut)
        {
            StartFadeOut(seconds);
            return;
        }

        StartFadeIn(seconds);
    }

    protected void StartFadeOut(float fateOutTime)
    {
        CameraFade.Fade(m_FadeColor, fateOutTime);
    }

    protected void StartFadeIn(float fateInTime)
    {
        CameraFade.Fade(Color.clear, fateInTime);
    }

    protected void ResetVelocity()
    {
        m_AbsoluteVelocity = Vector3.zero;
        m_LastPosition = transform.position;
    }


    #endregion




    //public void SetLastCheckpoint(Transform transform)
    //{
    //    m_LastCheckpoint = transform;
    //}

    //public bool TryToFindStartPosition()
    //{
    //    GameObject startPositionObject = GameObject.Find("SceneStartPosition");

    //    if (!startPositionObject)
    //        return false;

    //    m_StartPosition = startPositionObject.transform;
    //    return true;
    //}

    //protected void DisableCharacterControllerForSeconds(float seconds)
    //{
    //    StartCoroutine(DisableCharacterControllerCoroutine(seconds));
    //}

    //protected IEnumerator DisableCharacterControllerCoroutine(float time)
    //{
    //    m_CharacterController.enabled = false;
    //    yield return new WaitForSeconds(time);
    //    m_CharacterController.enabled = true;
    //}
 
}
