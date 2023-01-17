using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneElevator : MonoBehaviour
{
    public void GoToNextScene()
    {
        if(GameManager.Instance)
            GameManager.Instance.LoadNextScene();
    }

    public void GoToPreviousScene()
    {
        if (GameManager.Instance)
            GameManager.Instance.LoadPreviousScene();
    }

    public void GoToMainMenu()
    {
        if (GameManager.Instance)
            GameManager.Instance.LoadScene(-1);
    }
}
