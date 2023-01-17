using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSceneToggler : MonoBehaviour
{

    public void SetInfo(SceneEntry entry)
    {
        if (!entry)
            return;

        Transform labelChild = transform.Find("Label");
        if (!labelChild)
            return;

        UnityEngine.UI.Text text;
        if (labelChild.TryGetComponent<UnityEngine.UI.Text>(out text))
            text.text = entry.m_DisplayName;

    }


    //protected void Awake()
    //{
    //    Transform labelChild = transform.Find("Label");
    //    if (!labelChild)
    //        return;

    //    UnityEngine.UI.Text text;
    //    if (labelChild.TryGetComponent<UnityEngine.UI.Text>(out text))
    //        text.text = gameObject.name;
    //}

    //public void ToggleScene(bool value)
    //{
    //    if (value)
    //    {
    //        GameManager.Instance.SelectScene(name);
    //        return;
    //    }

    //    GameManager.Instance.UnselectScene(name);
    //}
}
