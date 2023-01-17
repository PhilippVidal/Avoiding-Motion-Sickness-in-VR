using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    protected ShootingGame m_ShootingGame;
    protected bool m_IsActive = true;
    
    protected void Start()
    {
        m_ShootingGame = transform.parent.parent.GetComponent<ShootingGame>();
        m_ShootingGame.AddTarget(this);
    }

    public void ApplyDamage()
    {
        if (!m_IsActive)
            return;

        m_ShootingGame.IncrementScore();
        Deactivate();
    }


    public void Activate()
    {
        m_IsActive = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = m_ShootingGame.m_TargetMaterial;
    }

    public void Deactivate()
    {
        m_IsActive = false;
        transform.GetChild(0).GetComponent<MeshRenderer>().material = m_ShootingGame.m_HitMaterial;
    }
}
