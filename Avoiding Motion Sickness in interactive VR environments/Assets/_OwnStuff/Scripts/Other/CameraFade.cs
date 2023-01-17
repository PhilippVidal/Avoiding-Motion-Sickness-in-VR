using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{

    public Material m_FadeMaterial;

    protected Coroutine m_FadeCoroutine;
    protected Color m_CurrentColor;
    protected float m_CurrentT;
 
    static public CameraFade Instance;
    static public bool Exists { get { return Instance != null; } }
    public bool IsFading {  get { return m_FadeCoroutine != null; } }

    protected void Awake()
    {
        Instance = this;
        Fade(Color.clear, 0.0f);
    }

    public static void Fade(Color color, float time)
    {
        if (!Exists)
            return;

        Instance.FadeScreen(color, time);
    }


    protected void FadeScreen(Color color, float time)
    {
        if (!m_FadeMaterial)
            return;

        Color currentColor = m_FadeMaterial.GetColor("_Color");
        if (currentColor == color)
            return;

        if(IsFading)
        {
            StopCoroutine(m_FadeCoroutine);
            m_FadeCoroutine = null;
        }

        StartCoroutine(FadeToColorCoroutine(currentColor, color, time));
    }


    protected IEnumerator FadeToColorCoroutine(Color currentColor, Color targetColor, float time)
    {
        m_CurrentT = 0.0f;
        float passedTime = 0.0f;
        Color initialColor = currentColor;

        if (time <= 0.0f)
        {
            m_CurrentColor = targetColor;
            m_FadeMaterial.SetColor("_Color", m_CurrentColor);
            yield break;
        }

        float invTime = 1 / time;
        while (m_CurrentT < 1.0f)
        {
            m_CurrentColor = Color.Lerp(currentColor, targetColor, m_CurrentT);
            m_FadeMaterial.SetColor("_Color", m_CurrentColor);
            passedTime += Time.deltaTime;
            m_CurrentT = passedTime * invTime;

            yield return null;
        }
    }


}
