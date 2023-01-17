using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.EventSystems;



public class CustomInputModule : BaseInputModule
{   
    public SteamVR_Input_Sources m_InputSource;
    public SteamVR_Action_Boolean m_InputAction;
    public Camera m_DummyCamera;

    protected Vector2 m_RayPosition;
    protected PointerEventData m_PointerEventData;


    public float PointerRayLength { get { return m_PointerEventData.pointerCurrentRaycast.distance; } }

    protected override void Awake()
    {
        base.Awake();
        m_PointerEventData = new PointerEventData(eventSystem);
    }

    protected override void Start()
    {
        base.Start();
        m_RayPosition = new Vector2(m_DummyCamera.pixelWidth * 0.5f, m_DummyCamera.pixelHeight * 0.5f);
    }

    //https://docs.unity3d.com/2017.4/Documentation/ScriptReference/EventSystems.ExecuteEvents.html
    //StandaloneInputModule.cs
    public override void Process()
    {
        m_PointerEventData.position = m_RayPosition;

        eventSystem.RaycastAll(m_PointerEventData, m_RaycastResultCache);
        m_PointerEventData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        GameObject currentOverGO = m_PointerEventData.pointerCurrentRaycast.gameObject;

        HandlePointerExitAndEnter(m_PointerEventData, currentOverGO);

        ExecuteEvents.Execute(m_PointerEventData.pointerDrag, m_PointerEventData, ExecuteEvents.dragHandler);

        if (m_InputAction.GetStateDown(m_InputSource))
        {
            LaserPointer pointer = PlayerController.Instance.m_LaserPointer;
            if(pointer && pointer.m_IsEnabled)
            {
                pointer.PlayClickDown();
            }

            currentOverGO = m_PointerEventData.pointerCurrentRaycast.gameObject;
            m_PointerEventData.pointerPressRaycast = m_PointerEventData.pointerCurrentRaycast;
            m_PointerEventData.pressPosition = m_PointerEventData.position;

            GameObject newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGO, m_PointerEventData, ExecuteEvents.pointerDownHandler);
            GameObject newClicked = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGO);
            if (!newPressed)
                newPressed = newClicked;

            m_PointerEventData.pointerPress = newPressed;
            m_PointerEventData.pointerClick = newClicked;

            m_PointerEventData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGO);
            ExecuteEvents.Execute(m_PointerEventData.pointerDrag, m_PointerEventData, ExecuteEvents.beginDragHandler);
        }

        if (m_InputAction.GetStateUp(m_InputSource))
        {
            LaserPointer pointer = PlayerController.Instance.m_LaserPointer;
            if (pointer && pointer.m_IsEnabled)
            {
                pointer.PlayClickUp();
            }

            ExecuteEvents.Execute(m_PointerEventData.pointerPress, m_PointerEventData, ExecuteEvents.pointerUpHandler);
            GameObject pointerClickHandler = ExecuteEvents.GetEventHandler<IPointerUpHandler>(currentOverGO);

            if (m_PointerEventData.pointerClick == pointerClickHandler)
                ExecuteEvents.Execute(m_PointerEventData.pointerClick, m_PointerEventData, ExecuteEvents.pointerClickHandler);

            ExecuteEvents.Execute(m_PointerEventData.pointerDrag, m_PointerEventData, ExecuteEvents.endDragHandler);
            m_PointerEventData.pointerPress = null;
            m_PointerEventData.pointerClick = null;
            m_PointerEventData.pointerDrag = null;
            eventSystem.SetSelectedGameObject(null);
        }
    }
}
