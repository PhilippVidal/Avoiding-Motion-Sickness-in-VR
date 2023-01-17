using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SceneListHandler : MonoBehaviour
{
//    [SerializeField] protected GameObject m_ButtonPrefab;
//    [SerializeField] protected Transform m_ContentTransform;


//    protected void Start()
//    {
//        GenerateList();
//    }

//    protected void GenerateList()
//    {
//        if (!m_ContentTransform)
//            return;

//        List<SceneEntry> entries = GameManager.Instance.m_Scenes;
//        if (entries == null)
//            return;

//        foreach (SceneEntry entry in entries)
//        {
//            GameObject buttonInstance = Instantiate(m_ButtonPrefab, m_ContentTransform);

//            Transform displayName = buttonInstance.transform.Find("Display Name");
//            TextMeshProUGUI displayNameText;
//            if (displayName)
//            {
//                if (displayName.gameObject.TryGetComponent<TextMeshProUGUI>(out displayNameText))
//                    displayNameText.text = entry.m_DisplayName;
//            }

//            Transform shortDesc = buttonInstance.transform.Find("Short Description");
//            TextMeshProUGUI shortDescText;
//            if (displayName)
//            {
//                if (shortDesc.gameObject.TryGetComponent<TextMeshProUGUI>(out shortDescText))
//                    shortDescText.text = entry.m_Description;
//            }
//        }
//    }

}
