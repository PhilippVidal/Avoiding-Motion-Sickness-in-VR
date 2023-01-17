using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class MovingEntity : MonoBehaviour
{
    [SerializeField] protected bool m_IsActive;
    [SerializeField] protected CharacterController m_CharacterController;

    protected virtual void Update()
    {

        if (!m_IsActive)
            return;

        UpdateMovement();
    }

    //Do some movement or rotation
    protected abstract void UpdateMovement();

    public virtual void SetCharacterController(CharacterController controller)
    {
        m_CharacterController = controller;
    }

    public void Activate()
    {
        m_IsActive = true;
    }

    public void Deactivate()
    {
        m_IsActive = false;
    }
}
