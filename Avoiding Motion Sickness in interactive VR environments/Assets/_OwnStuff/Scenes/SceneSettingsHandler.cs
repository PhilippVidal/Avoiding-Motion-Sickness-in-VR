using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneSettingsHandler : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {
        ApplySettings();
    }

    protected abstract void ApplySettings();
}
