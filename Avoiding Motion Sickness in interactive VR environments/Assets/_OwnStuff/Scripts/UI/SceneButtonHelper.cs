using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneButtonHelper : MonoBehaviour
{
    public SceneEntry m_LinkedEntry;
    public SceneSelector m_SceneSelector;
    public Toggle m_SceneToggle;

    public void ShowSettings()
    {
        if (!m_SceneSelector)
            return;

        m_SceneSelector.SelectSceneButton(m_LinkedEntry);
    }

    public void UpdateSceneSelected(bool value)
    {
        m_LinkedEntry.m_IsSelected = value;
    }

}
