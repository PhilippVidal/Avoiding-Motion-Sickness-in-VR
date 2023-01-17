using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderValueHelper : MonoBehaviour
{
    public TextMeshProUGUI m_TextMeshProUGUI;

    public bool m_FormatAsInteger = false;

    protected void Start()
    {
        StartCoroutine(InitSliderValues());
    }


    IEnumerator InitSliderValues()
    {
        yield return new WaitForSeconds(0.1f);
        UnityEngine.UI.Slider slider = GetComponent<UnityEngine.UI.Slider>();
        if (slider)
            UpdateTextWithValue(slider.value);
    }

    public void UpdateTextWithValue(float value)
    {
        m_TextMeshProUGUI.text = value.ToString(m_FormatAsInteger ? "0" : "F2");
    }
}
