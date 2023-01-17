using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public TextMeshPro m_Text;
    
    public void UpdateText(string text) => m_Text.text = text;
}
