using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] protected GameObject m_ButtonPrefab;
    [SerializeField] protected Button m_ShuffleButton;
    [SerializeField] protected Transform m_ContentTransform;
    [SerializeField] protected Transform m_SettingsTransform;
    [SerializeField] protected List<SceneEntry> m_Entries;

    protected GameObject m_SelectedSettingsGO;

    protected void Start()
    {
        if (!m_ContentTransform)
            enabled = false;

        m_Entries = GameManager.Instance.m_Scenes;
        if (m_Entries == null)
            enabled = false;

        GenerateList();
    }

    protected void GenerateList()
    {
        foreach (SceneEntry entry in m_Entries)
        {
            GenerateButton(entry);
        }
    }

    protected void GenerateButton(SceneEntry entry)
    {
        GameObject buttonInstance = Instantiate(m_ButtonPrefab, m_ContentTransform);

        Transform displayName = buttonInstance.transform.Find("Display Name");
        TextMeshProUGUI displayNameText;
        if (displayName)
        {
            if (displayName.gameObject.TryGetComponent<TextMeshProUGUI>(out displayNameText))
                displayNameText.text = entry.m_DisplayName;
        }

        Transform shortDesc = buttonInstance.transform.Find("Short Description");
        TextMeshProUGUI shortDescText;
        if (displayName)
        {
            if (shortDesc.gameObject.TryGetComponent<TextMeshProUGUI>(out shortDescText))
                shortDescText.text = entry.m_Description;
        }

        SceneButtonHelper helper;
        if (buttonInstance.TryGetComponent<SceneButtonHelper>(out helper))
        {
            helper.m_SceneSelector = this;
            helper.m_LinkedEntry = entry;
            helper.m_SceneToggle.isOn = entry.m_IsSelected;
        }
            
    }



    public void SelectSceneButton(SceneEntry entry)
    {
        if (!entry)
            return;

        if(m_SelectedSettingsGO)
            Destroy(m_SelectedSettingsGO);

        GameObject settingsPrefab = entry.m_SettingsPrefab;
        if (!settingsPrefab)
            return;

        m_SelectedSettingsGO = Instantiate(settingsPrefab, m_SettingsTransform);

        UI_SceneSettingsHelper helper; 
        if(m_SelectedSettingsGO.TryGetComponent<UI_SceneSettingsHelper>(out helper))
        {
            helper.SetSceneSettings(entry.m_SceneSettings);
        }
    }

    public void SelectAll()
    {
        if (!m_ContentTransform)
            return;

        for (int i = 0; i < m_ContentTransform.childCount; i++)
        {
            Transform child = m_ContentTransform.GetChild(i);

            SceneButtonHelper helper;
            if (child.TryGetComponent<SceneButtonHelper>(out helper))
            {             
                helper.m_SceneToggle.isOn = true;
            }
        }
    }

    public void DeselectAll()
    {
        if (!m_ContentTransform)
            return;

        for (int i = 0; i < m_ContentTransform.childCount; i++)
        {
            Transform child = m_ContentTransform.GetChild(i);

            SceneButtonHelper helper;
            if (child.TryGetComponent<SceneButtonHelper>(out helper))
            {
                helper.m_SceneToggle.isOn = false;
            }
        }
    }
}
