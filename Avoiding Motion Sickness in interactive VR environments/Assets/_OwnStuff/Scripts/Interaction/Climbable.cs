using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Climbable : MonoBehaviour
{

    protected Interactable m_Interactable;
    protected Hand.AttachmentFlags m_AttachmentFlags = 0;

    protected Vector3 m_StartPosition;
    protected Vector3 m_LastUpdatePosition;

    public Hand m_AttachedToHand;

    protected void Awake()
    {
        m_Interactable = GetComponent<Interactable>();
        SetupInteractable();
    }

    protected virtual void SetupInteractable()
    {
        m_Interactable.hideHandOnAttach = false;
        m_Interactable.hideControllerOnAttach = true;
        m_Interactable.useHandObjectAttachmentPoint = false;
        m_Interactable.handFollowTransform = true;
        m_Interactable.highlightOnHover = false;
    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        //if (m_Interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        if (startingGrabType != GrabTypes.None)
        {
            hand.AttachObject(gameObject, startingGrabType, m_AttachmentFlags);
            PlayerController.Instance.m_ClimbingPoints++;
            PlayerController.Instance.CanTeleport = false;
            m_StartPosition = hand.transform.position - PlayerController.Instance.transform.position;
            m_LastUpdatePosition = m_StartPosition;

            //Is another hand already holding onto the object? => remove it first 
            //if (m_AttachedToHand)
            //    DetachHand(m_AttachedToHand);

            m_AttachedToHand = hand;
            hand.HoverLock(null);
        }
    }

    protected virtual void HandAttachedUpdate(Hand hand)
    {
        //Detach hand if grib has stopped
        if (hand.IsGrabEnding(gameObject))
        {
            hand.DetachObject(gameObject);
            m_AttachedToHand = null;
            PlayerController.Instance.m_ClimbingPoints--;

            if(PlayerController.Instance.m_ClimbingPoints < 1)
                PlayerController.Instance.CanTeleport = true;

            hand.HoverUnlock(null);
            return;
        }

        Vector3 currentPos = hand.transform.position - PlayerController.Instance.transform.position;
        Vector3 movementVector = currentPos - m_LastUpdatePosition;
        m_LastUpdatePosition = currentPos;

        PlayerController.Instance.Move(-movementVector);
    }


}
