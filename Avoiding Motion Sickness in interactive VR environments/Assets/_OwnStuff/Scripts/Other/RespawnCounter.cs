using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RespawnCounter : MonoBehaviour
{
    public int m_RespawnCount = 0;
    public TextMeshPro m_Text;

    public void ResetRespawnCount()
    {
        m_RespawnCount = 0;
        UpdateScore();
    }

    public void IncreaseRespawnCount()
    {
        m_RespawnCount++;
        UpdateScore();
    }
    
    public void DecreaseRespawnCount()
    {
        if(m_RespawnCount > 0)
        {
            m_RespawnCount--;
            UpdateScore();
        }          
    }

    protected void UpdateScore() => UpdateText(m_RespawnCount.ToString());
    protected void UpdateText(string text)
    {
        if(!m_Text)
        {
            Debug.Log("[Respawn Counter]  Trying to update text but Text-GO has not been linked!");
            return;
        }

        m_Text.text = text;
    }

}
