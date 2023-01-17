using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Wearable : MonoBehaviour
{
    public Transform m_AttachedToTransform;
    public Transform m_OriginalParentTransform;
    public Hand.AttachmentFlags m_AttachmentFlags;

    public bool m_InteractableOnlyInMenuRoom = true;
    public bool m_RespawnOnDetach = true;

    protected Vector3 m_InitialPosition;
    protected Quaternion m_InitialRotation;
    protected Rigidbody m_Rigidbody;
    protected Interactable m_Interactable;
    protected Collider m_Collider;
    protected string m_InitialLayerName;

    public float m_AttachmentDistance = 0.1f;

    public bool IsAttached { get { return m_AttachedToTransform != null; } }
    public bool HasRigidbody {  get { return m_Rigidbody != null; } }

    public bool PhysicsActivated
    {
        get 
        { 
            return !m_Rigidbody.isKinematic || m_Rigidbody.useGravity; 
        }
        set 
        {
            if (value)
            {
                ActivatePhysics();
                return;
            }

            DeactivatePhysics();
        }
    }

    protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Interactable = GetComponent<Interactable>();

        m_InitialPosition = transform.position;
        m_InitialRotation = transform.rotation;
    }

    public void AttachTo(Transform attachToTransform)
    {
        if (IsAttached)
            return;

        PhysicsActivated = false;
        m_OriginalParentTransform = transform.parent;
        m_AttachedToTransform = attachToTransform;
        transform.parent = m_AttachedToTransform;
        transform.position = m_AttachedToTransform.position;
        transform.rotation = m_AttachedToTransform.rotation;
    }
    protected void ActivatePhysics()
    {
        if (!m_Rigidbody)
            return;

        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
    }
    protected void DeactivatePhysics()
    {
        if (!m_Rigidbody)
            return;

        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;
    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (startingGrabType != GrabTypes.None)
        {
            if (m_InteractableOnlyInMenuRoom && !PlayerController.Instance.IsInMenuRoom)
                return;

            if (!IsAttached)
                m_InitialLayerName = LayerMask.LayerToName(gameObject.layer);

            gameObject.layer = LayerMask.NameToLayer("RenderOverCage");

            //Detach();
            transform.parent = m_OriginalParentTransform;
            m_AttachedToTransform = null;
            hand.AttachObject(gameObject, startingGrabType, m_AttachmentFlags);
            hand.HoverLock(null);

        }
    }

    protected virtual void HandAttachedUpdate(Hand hand)
    {
        

        if (hand.IsGrabEnding(gameObject))
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(null);

            Vector3 toHead = PlayerController.Instance.HeadTransform.position - transform.position;
            float sqrDistance = toHead.sqrMagnitude;
            if (sqrDistance <= m_AttachmentDistance * m_AttachmentDistance)
            {
                AttachTo(PlayerController.Instance.HeadAttachmentTransform);
                return;
            }

            PhysicsActivated = true;
            gameObject.layer = LayerMask.NameToLayer(m_InitialLayerName);

            if (m_RespawnOnDetach)
            {
                transform.position = m_InitialPosition;
                transform.rotation = m_InitialRotation;
            }
        }
    }

    protected void Detach()
    {
        if (!IsAttached)
            return;

        transform.parent = m_OriginalParentTransform;
        PhysicsActivated = true;
    }  
}