using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class SteeringWheel : MonoBehaviour
{
    protected Interactable m_Interactable;
    protected float m_CurrentYAngle;

    public bool m_UseToggleGrabbing = false;

    [Range(0.1f, 90.0f)] public float m_MaxAngle = 70.0f;

    public Hand.AttachmentFlags m_AttachmentFlags = 0;

    protected Vector3 m_GrabStartPosition;
    protected Vector3 m_LastGrabDirection;

    protected Quaternion m_InitRotation;


    public float NormalizedValue { get { return m_CurrentYAngle / m_MaxAngle; } }


    public float debugValue;
    public float m_DebugAngle;


    [SerializeField] protected List<Hand> m_AttachedHands;

    public bool HasHandAttached => m_AttachedHands != null && m_AttachedHands.Count > 0;


    protected void Awake()
    {
        m_Interactable = GetComponent<Interactable>();
        m_InitRotation = transform.localRotation;
        m_AttachedHands = new List<Hand>();
    }

    public void DetachAllHands()
    {

        List<Hand> handsToDetach = new List<Hand>();
        for(int i = 0; i < m_AttachedHands.Count; i++)
        {
            handsToDetach.Add(m_AttachedHands[i]);
            //print("Detaching hand => " + m_AttachedHands[i].name);
            DetachHand(m_AttachedHands[i]);
        }

        for (int i = 0; i < handsToDetach.Count; i++)
        {
            //print("Detaching hand => " + handsToDetach[i].name);
            DetachHand(handsToDetach[i]);
        }
        
            
    }
    protected virtual void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        //if (m_Interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        if (startingGrabType == GrabTypes.Grip)
        {
            //print("Attaching hand => " + hand.name);
            hand.AttachObject(gameObject, startingGrabType, m_AttachmentFlags);
            hand.HoverLock(null);
            m_GrabStartPosition = transform.InverseTransformPoint(hand.transform.position);
            m_GrabStartPosition.y = 0.0f;
            m_LastGrabDirection = m_GrabStartPosition.normalized;
            m_AttachedHands.Add(hand);
        }
    }


    protected virtual void HandAttachedUpdate(Hand hand)
    {

        bool shouldLetGo = m_UseToggleGrabbing ? 
            hand.GetGrabStarting() == GrabTypes.Grip : hand.IsGrabEnding(gameObject);

        if (shouldLetGo)
        {
            DetachHand(hand);
            return;
        }


        //UpdateSteering(hand);
    }


    protected void DetachHand(Hand hand)
    {
        hand.DetachObject(gameObject);
        hand.HoverUnlock(null);
        m_AttachedHands.Remove(hand);
    }

    protected void Update()
    {
        if (m_AttachedHands.Count == 0)
            return;

        Vector3 targetDirection = Vector3.zero;

        for (int i = 0; i < m_AttachedHands.Count; i++)
        {
            targetDirection += m_AttachedHands[i].transform.forward;
        }


        if (targetDirection == Vector3.zero)
            return;


        transform.localRotation = m_InitRotation;
        Debug.DrawRay(m_AttachedHands[0].transform.position, m_AttachedHands[0].transform.forward, Color.green);
        //Transform to local space
        targetDirection = transform.InverseTransformDirection(targetDirection).normalized;
        //and project on up-vector plane to receive correct "2D" version of the vector
        targetDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up).normalized;

        Debug.DrawRay(transform.position, transform.TransformDirection(targetDirection), Color.red);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.blue);
        //Vector3 forwardVector = Quaternion.Euler(0.0f, -m_CurrentYAngle, 0.0f) * Vector3.forward;


        float signedAngle = GetSignedAngle(
            Vector3.up,
            new Vector2(targetDirection.x, targetDirection.z)
            );

        if(signedAngle < -m_MaxAngle)
            signedAngle = -m_MaxAngle;

        if (signedAngle > m_MaxAngle)
            signedAngle = m_MaxAngle;

        m_DebugAngle = signedAngle;
        m_CurrentYAngle = signedAngle;
        transform.localRotation = m_InitRotation * Quaternion.Euler(0.0f, m_CurrentYAngle, 0.0f);
    }

    protected virtual void UpdateSteering(Hand hand)
    {
        Vector3 currentDirection = transform.InverseTransformPoint(hand.transform.position);
        currentDirection.y = 0.0f;
        currentDirection.Normalize();

        Vector2 from = new Vector2(m_LastGrabDirection.x, m_LastGrabDirection.z);
        Vector2 to = new Vector2(currentDirection.x, currentDirection.z);

        float deltaAngle = GetSignedAngle(from, to);
        float newAngle = m_CurrentYAngle + deltaAngle;


        if (newAngle > m_MaxAngle)
        {
            newAngle = m_MaxAngle;
            deltaAngle = newAngle - m_CurrentYAngle;
        }
          

        if (newAngle < -m_MaxAngle)
        {
            newAngle = -m_MaxAngle;
            deltaAngle = newAngle - m_CurrentYAngle;
        }

        m_CurrentYAngle = newAngle;

        m_LastGrabDirection = Quaternion.Euler(0.0f, -deltaAngle, 0.0f) * currentDirection;
        transform.localRotation = m_InitRotation * Quaternion.Euler(0.0f, m_CurrentYAngle, 0.0f);
        debugValue = NormalizedValue;
    }


    protected float GetSignedAngle(Vector2 from, Vector2 to)
    {
        return Mathf.DeltaAngle(
            Mathf.Atan2(from.x, from.y) * Mathf.Rad2Deg,
            Mathf.Atan2(to.x, to.y) * Mathf.Rad2Deg
            );
    }
}
