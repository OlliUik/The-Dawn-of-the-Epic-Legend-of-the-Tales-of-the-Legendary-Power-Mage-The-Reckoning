//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CallSceneChange))]
[RequireComponent(typeof(Dropdown))]
public class ListAllScenes : MonoBehaviour
{
    public List<string> excludedScenes = null;

    private Dropdown dropMenu = null;

    private void Awake()
    {
        dropMenu = GetComponent<Dropdown>();
    }

    private void Start()
    {
        /*
        if (GlobalVariables.scenesInBuild == null || GlobalVariables.scenesInBuild.Count == 0)
        {
            Debug.LogWarning(this + " tried to get available scenes, but the list hasn't been initialized!");
            return;
        }
        */

        /* vvv This part is here only for testing purposes, remove later! vvv */
        if (GlobalVariables.scenesInBuild.Count == 0)
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                GlobalVariables.scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }
        }
        /* ^^^ This part is here only for testing purposes, remove later! ^^^ */

        dropMenu.ClearOptions();
        List<string> includedScenes = new List<string>(GlobalVariables.scenesInBuild);

        if (excludedScenes != null)
        {
            foreach (string s in excludedScenes)
            {
                if (includedScenes.Contains(s))
                {
                    includedScenes.Remove(s);
                }
            }
        }
        
        dropMenu.AddOptions(includedScenes);
    }

    public void LoadSelectedLevel()
    {
        GetComponent<CallSceneChange>().ChangeLevel(dropMenu.options[dropMenu.value].text);
    }
}
