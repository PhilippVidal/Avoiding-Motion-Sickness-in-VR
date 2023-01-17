using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantFalling : MonoBehaviour
{
    [SerializeField] protected bool m_PlayerInArea = false;

    protected Coroutine m_InstantFallingCoroutine = null;

    protected float m_GravityOnEnter;
    protected float m_GravityToApply;
    public float m_GravityMultiplier = 10.0f;



    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            m_PlayerInArea = true;
            m_GravityOnEnter = PlayerController.Instance.m_Gravity;
            m_GravityToApply = m_GravityOnEnter * m_GravityMultiplier;
            m_InstantFallingCoroutine = StartCoroutine(InstantFallingCoroutine());
        }          
    }


    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            m_PlayerInArea = false;
            PlayerController.Instance.m_Gravity = m_GravityOnEnter;
            StopCoroutine(m_InstantFallingCoroutine);
        }         
    }


    protected IEnumerator InstantFallingCoroutine()
    {
        //Vector3 hitLocation;
        while(m_PlayerInArea)
        {
            PlayerController.Instance.m_Gravity = m_GravityOnEnter;
            if (!IsGrounded())
            {
                PlayerController.Instance.m_Gravity = m_GravityToApply;
            }
                
            yield return null;
        }
    }

    protected bool IsGrounded()
    {
        PlayerController player = PlayerController.Instance;
        //hitPosition = Vector3.zero;

        //If ground gets hid beneath the player within 1 meter, don't do anything
        if (Physics.Raycast(player.HeadPositionNoHeight, Vector3.down, 1.0f))
            return true;

        ////If there is no ground beneath the player, try to find the closest ground location beneath the player
        //RaycastHit hitInfo;
        //if (!Physics.Raycast(player.HeadPositionNoHeight, Vector3.down, out hitInfo, 100.0f))
        //    return true;


        //hitPosition = hitInfo.point;
        return false;
    }
}
