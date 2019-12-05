using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
            ErrorScene("Scene not found.");
        }
    }

    IEnumerator LoadLevelAsync(string levelName)
    {
        //Wait a bit before loading the next scene to allow the loading screen to load itself properly.
        yield return new WaitForEndOfFrame();
        //yield return new WaitForSecondsRealtime(0.5f);

        GlobalVariables.teamGoodGuys.Clear();
        GlobalVariables.teamBadBoys.Clear();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void ErrorScene(string errorCode)
    {
        GlobalVariables.errorCode = errorCode;
        SceneManager.LoadScene("Error");
    }
}
