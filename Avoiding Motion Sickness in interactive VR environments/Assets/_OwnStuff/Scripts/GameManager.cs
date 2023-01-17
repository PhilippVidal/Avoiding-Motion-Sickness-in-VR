using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;


[System.Serializable]
public class SceneDescriptor
{
    public string Name { get; set; }
    public bool IsLoaded { get; set; }

}

public class GameManager : MonoBehaviour
{
    public Transform m_PlayerSpawnLocation;
    public RespawnLocation m_LatestRespawnLocation;
    public Transform m_MenuRoomLocation;
    public Transform m_SceneLocation;
    public Transform m_MenuRoomBoundsTransform;  
    public RespawnCounter m_RespawnCounter;
    public List<SceneEntry> m_Scenes;
    public bool m_ShuffleScenes;

    protected List<SceneEntry> m_SelectedSceneEntries;
    protected bool m_IsSceneBeingLoaded = false;
    protected AsyncOperation m_AsyncOperation;
    protected int m_CurrentlyLoadedScene = -1; 

    protected static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (!m_Instance)
                m_Instance = FindObjectOfType<GameManager>();

            if (!m_Instance)
                Debug.LogWarning("No instance of Gamemanager could be found in the scene!");

           return m_Instance;
        }
    }

   protected void Awake()
   {
       SelectAllScenes();
        LightProbes.needsRetetrahedralization += UpdateLightProbes;
   }

    public void ExitGame()
    {
        Debug.Log("[Game Manager] Quitting application!");
        Application.Quit();
    }

    public Vector3 GetRespawnLocation()
    {

        if (m_LatestRespawnLocation && !PlayerController.Instance.IsInMenuRoom)
            return m_LatestRespawnLocation.transform.position;

        if (m_PlayerSpawnLocation)
            return m_PlayerSpawnLocation.position;

        return Vector3.zero;          
    }



    #region Scene Handling

    public void ToggleSceneShuffle()
    {
        m_ShuffleScenes = !m_ShuffleScenes;
    }

    protected void ShuffleScenes()
    {
        //Only shuffle the scenes when player is menu room
        if (!PlayerController.Instance.IsInMenuRoom)
            return;

        //Nothing to shuffle
        if (m_SelectedSceneEntries == null || m_SelectedSceneEntries.Count < 2)
            return;

        List<SceneEntry> m_ShuffeledSceneEntries = new List<SceneEntry>();
        int count = m_SelectedSceneEntries.Count;
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, m_SelectedSceneEntries.Count);
            SceneEntry entry = m_SelectedSceneEntries[randomIndex];
            m_ShuffeledSceneEntries.Add(entry);
            m_SelectedSceneEntries.Remove(entry);
        }

        m_SelectedSceneEntries = m_ShuffeledSceneEntries;
    }
    public void UpdateLightProbes()
    {
        Debug.Log("[Game Manager] Updating Lightprobes due to (un)loaded scene!");
        LightProbes.Tetrahedralize();
    }

    public SceneEntry GetSceneEntryByName(string sceneName)
    {
        foreach (SceneEntry entry in m_Scenes)
        {
            if (sceneName == entry.m_LoadName)
                return entry;
        }

        return null;
    }

    public void SelectAllScenes()
    {
        foreach(SceneEntry scene in m_Scenes)
            scene.m_IsSelected = true;
    }

    public void UnselectAllScenes()
    {
        foreach (SceneEntry scene in m_Scenes)
            scene.m_IsSelected = false;
    }

    public void LoadNextScene()
    {
        if (m_CurrentlyLoadedScene == -1 || m_SelectedSceneEntries == null)
            SetupSceneList();
      
        LoadScene(m_CurrentlyLoadedScene + 1);
    }

    public void LoadPreviousScene()
    {
        if (m_CurrentlyLoadedScene == -1 || m_SelectedSceneEntries == null)
            SetupSceneList();

        LoadScene(m_CurrentlyLoadedScene - 1);
    }


    protected void SetupSceneList()
    {
        m_SelectedSceneEntries = new List<SceneEntry>();

        foreach(SceneEntry entry in m_Scenes)
        {
            if(entry.m_IsSelected)
            {
                m_SelectedSceneEntries.Add(entry);
            }
        }

        if (m_ShuffleScenes)
            ShuffleScenes();
    }

    public void LoadScene(int index)
    {
        if (m_SelectedSceneEntries == null)
            return;

        if (m_SelectedSceneEntries.Count == 0)
            return;

        if (m_IsSceneBeingLoaded)
            return;

        string sceneToUnload = "";

        if (m_CurrentlyLoadedScene > -1)
            sceneToUnload = m_SelectedSceneEntries[m_CurrentlyLoadedScene].m_LoadName;


        StartCoroutine(SwitchScenesWithFade(sceneToUnload, index));
    }    

    protected IEnumerator SwitchScenesWithFade(string sceneToUnload, int sceneToLoad)
    {
        m_IsSceneBeingLoaded = true;
        float fadeTime = PlayerController.Instance.m_ChangeSceneFadeTime;
        PlayerController.Instance.FadeScreen(true, fadeTime);

        yield return new WaitForSeconds(fadeTime);
        CameraFade.Fade(PlayerController.Instance.m_FadeColor, 0.0f);

        if (sceneToUnload != "")
        {
            m_AsyncOperation = SceneManager.UnloadSceneAsync(sceneToUnload);
            while (!m_AsyncOperation.isDone)
            {
                yield return null;
            }         
        }

        //Back to menu room, nothing to unload
        if (sceneToLoad > m_SelectedSceneEntries.Count - 1 || sceneToLoad < 0)
        {
            PlayerController.Instance.TeleportTo(m_MenuRoomLocation.position);
            PlayerController.Instance.FadeScreen(false, fadeTime);
            PlayerController.Instance.IsInMenuRoom = true;
            m_CurrentlyLoadedScene = -1;
            UnloadAllLoadedScenes();
            m_IsSceneBeingLoaded = false;
            yield break;
        }

        m_AsyncOperation = SceneManager.LoadSceneAsync(m_SelectedSceneEntries[sceneToLoad].m_LoadName, LoadSceneMode.Additive);
        while (!m_AsyncOperation.isDone)
        {
            yield return null;
        }

        m_CurrentlyLoadedScene = sceneToLoad;
        PlayerController.Instance.IsInMenuRoom = false;
        PlayerController.Instance.TeleportTo(m_SceneLocation.position);     
        PlayerController.Instance.FadeScreen(false, fadeTime);
        m_IsSceneBeingLoaded = false;
    }

    protected IEnumerator UnloadAllScenesInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        UnloadAllLoadedScenes();
    }

    protected void UnloadAllLoadedScenes()
    {

        foreach(SceneEntry entry in m_Scenes)
        {
            Scene scene = SceneManager.GetSceneByName(entry.m_LoadName);
            if (scene == null)
                continue;

            if(scene.isLoaded)
                SceneManager.UnloadSceneAsync(scene);
        }
    }

    #endregion
}


