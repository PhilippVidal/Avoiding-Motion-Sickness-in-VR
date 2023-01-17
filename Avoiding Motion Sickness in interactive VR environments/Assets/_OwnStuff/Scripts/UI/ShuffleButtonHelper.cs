using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShuffleButtonHelper : MonoBehaviour
{
    public TextMeshProUGUI m_Text;

    protected void Start()
    {
        UpdateText();
    }


    public void UpdateText()
    {
        if (!m_Text)
            return;

        if (GameManager.Instance.m_ShuffleScenes)
        {
            m_Text.text = "Shuffled Scenes";
            return;
        }

        m_Text.text = "Sorted Scenes";
    }
}
