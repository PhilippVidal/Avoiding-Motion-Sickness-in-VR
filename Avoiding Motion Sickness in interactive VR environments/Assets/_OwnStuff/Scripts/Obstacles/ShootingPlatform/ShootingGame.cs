using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShootingGame : MonoBehaviour
{

    public ItemPackageSpawner m_PackageSpawner;

    protected bool m_PlayerHasBow = false;
    protected List<Target> m_Targets = new List<Target>();
    public int m_Score;

    public Material m_TargetMaterial;
    public Material m_HitMaterial;
    public Scoreboard m_Scoreboard;

    public void IncrementScore()
    {
        m_Score++;
        UpdateScoreboard();
    }

    protected void Start()
    {
        UpdateScoreboard();
    }

    protected void UpdateScoreboard()
    {
        if (m_Scoreboard)
            m_Scoreboard.UpdateText(string.Format("{0} / {1}", m_Score, m_Targets.Count));
    }

    public void StartGame()
    {
        m_Score = 0;
        EnableAllTargets();
        UpdateScoreboard();
    }

    public void AddTarget(Target target)
    {
        m_Targets.Add(target);
    }

    public void EnableAllTargets()
    {
        foreach(Target target in m_Targets)
            target.Activate();
    }

    public void DisableAllTargets()
    {
        foreach (Target target in m_Targets)
            target.Deactivate();
    }

    public void ActivatePlayerHasBow()
    {
        m_PlayerHasBow = true;
    }

    public void DeactivatePlayerHasBow()
    {
        m_PlayerHasBow = false;
    }

    public void AttachBow()
    {
        if (m_PlayerHasBow)
            return;

        if(Player.instance.leftHand)
        {
            m_PackageSpawner.SpawnAndAttachObject(Player.instance.leftHand, GrabTypes.Scripted);
            return;
        }


        if (Player.instance.rightHand)
        {
            m_PackageSpawner.SpawnAndAttachObject(Player.instance.rightHand, GrabTypes.Scripted);
            return;
        }
    }

    public void DetachBow()
    {
        if (Player.instance.leftHand)
        {
            m_PackageSpawner.TakeBackItem(Player.instance.leftHand);
            return;
        }


        if (Player.instance.leftHand)
        {
            m_PackageSpawner.TakeBackItem(Player.instance.rightHand);
            return;
        }
    }
}
