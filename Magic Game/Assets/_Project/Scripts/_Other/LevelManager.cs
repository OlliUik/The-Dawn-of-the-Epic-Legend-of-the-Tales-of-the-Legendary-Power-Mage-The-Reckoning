using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private float loadSafetyTime = 15.0f;
    private float loadSafetyTimeTemp;

    void Awake()
    {
        //Remove all entities from global entity lists during level load
        GlobalVariables.teamBadBoys.Clear();
        GlobalVariables.teamGoodGuys.Clear();

        if (GlobalVariables.scenesInBuild.Count == 0)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                GlobalVariables.scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }
        }
    }

    void Start()
    {
        //Reset the amount of crystals collected when a new level is loaded
        GlobalVariables.crystalsCollected = 0;

        loadSafetyTimeTemp = loadSafetyTime;

        if (Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
        }

        if (GlobalVariables.scenesInBuild.Contains(GlobalVariables.loadLevel))
        {
            StartCoroutine(LoadLevelAsync(GlobalVariables.loadLevel));
        }
        else
        {
            ErrorScene("DEAD-BEEF");
        }
    }

    IEnumerator LoadLevelAsync(string levelName)
    {
        //Wait a bit before loading the next scene to allow the loading screen to load itself properly.
        yield return new WaitForSecondsRealtime(0.5f);

        GlobalVariables.teamBadBoys.Clear();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        while (!asyncLoad.isDone)
        {
            loadSafetyTimeTemp -= Time.deltaTime;
            if (loadSafetyTimeTemp < 0.0f)
            {
                ErrorScene("D007-D007");
            }
            yield return null;
        }
    }

    void ErrorScene(string errorCode)
    {
        GlobalVariables.errorCode = errorCode;
        SceneManager.LoadScene("Error");
    }
}
