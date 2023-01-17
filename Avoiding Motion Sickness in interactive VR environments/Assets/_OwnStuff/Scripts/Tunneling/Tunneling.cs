using UnityEngine;

public enum TunnelingModes
{
    DISABLED,
    BLACKOUT,
    CAGE
}
public class Tunneling : MonoBehaviour
{
    
    [Header("Have to be set up")]
    public Camera m_CageCamera;
    public GameObject m_Cage;
    public Material m_BlackoutMaterial;
    public Material m_CageMaterial;
    public Material m_CageGridMaterial;

    [Header("Settings")]
    public TunnelingModes m_TunnelingMode = TunnelingModes.DISABLED;
    public TunnelingMotionModes m_SelectedMotionMode = TunnelingMotionModes.TIME;
    public bool m_IgnorePlayerInput = false;
    public float m_SmoothingOffset = 0.1f;
    public float m_MaxRadius = 1.0f;
    public float m_MinRadius = 0.4f;
    public float m_FadeInTime = 2.0f;
    public float m_FadeOutTime = 1.0f;
    public float m_MaxSpeedReference = 3.0f;
    public float m_SpeedSmoothing = 0.3f;
   
   

    public static Tunneling Instance;
    public static bool Exists { get { return Instance != null; } }

    public bool IsTunnelingEnabled { get { return m_TunnelingMode != TunnelingModes.DISABLED; } }

    public Material Material { get { return m_TunnelingMaterial; } }


    [HideInInspector] public float m_CurrentRadius = 1.0f;
    [HideInInspector] public float m_MotionValue;
    [HideInInspector] public RenderTexture m_CageRenderTexture;
    protected Material m_TunnelingMaterial;
    protected TunnelingMotionMode m_MotionMode;

    public TunnelingModes TunnelingMode
    {
        get { return m_TunnelingMode; }
        set
        {
            m_TunnelingMode = value;
            UpdateTunnelingMode();
        }
    }

    public TunnelingMotionModes MotionMode
    {
        get { return m_SelectedMotionMode; }
        set
        {
            m_SelectedMotionMode = value;
            UpdateMotionMode();
        }
    }

    protected void UpdateTunnelingMode()
    {
        switch(m_TunnelingMode)
        {
            case TunnelingModes.DISABLED:
                DisableCage();
                break;
            case TunnelingModes.BLACKOUT:
                DisableCage();
                m_TunnelingMaterial = m_BlackoutMaterial;
                break;
            case TunnelingModes.CAGE:
                EnableCage();
                m_TunnelingMaterial = m_CageMaterial;
                break;
        }
    }
    protected void UpdateMotionMode()
    {
        switch (m_SelectedMotionMode)
        {
            case TunnelingMotionModes.INSTANT:
                m_MotionMode = new TunnelingMotionMode_Instant();
                break;
            case TunnelingMotionModes.SPEED:
                m_MotionMode = new TunnelingMotionMode_Speed();
                break;
            case TunnelingMotionModes.TIME:
                m_MotionMode = new TunnelingMotionMode_Time();
                break;
            case TunnelingMotionModes.ALWAYS:
                m_MotionMode = new TunnelingMotionMode_Always();
                break;
            default:
                Debug.LogWarning("[Tunneling] No entry for selected motion mode!");
                break;
        }
    }

    protected void EnableCage()
    {
        if (m_CageCamera)
            m_CageCamera.enabled = true;
        if (m_Cage)
            m_Cage.SetActive(true);
        InitCageRenderTexture();
    }
    protected void DisableCage()
    {
        if(m_CageCamera)
            m_CageCamera.enabled = false;
        if (m_CageRenderTexture)
            Destroy(m_CageRenderTexture);
        if (m_Cage)
            m_Cage.SetActive(false);
    }

    


    protected void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    protected void Start()
    {
        UpdateMotionMode();
    }

    protected void Update()
    {
        if (m_TunnelingMode == TunnelingModes.DISABLED)
            return;

        m_MotionMode.Update();
        m_MotionValue = m_MotionMode.GetMotionValue();
        m_CurrentRadius = m_MaxRadius - m_MotionValue * (m_MaxRadius - m_MinRadius);
        UpdateTunneling();

        if (m_TunnelingMode != TunnelingModes.CAGE)
            return;

        if (!m_CageRenderTexture)
            InitCageRenderTexture();
    }

    public void UpdateTunneling()
    {
        m_CurrentRadius = m_MaxRadius - m_MotionValue * (m_MaxRadius - m_MinRadius);
    }

    void InitCageRenderTexture()
    {
        if (UnityEngine.XR.XRSettings.eyeTextureWidth == 0)
            return;

        m_CageRenderTexture = new RenderTexture(UnityEngine.XR.XRSettings.eyeTextureDesc);
        m_CageRenderTexture.Create();
    }

  
}
