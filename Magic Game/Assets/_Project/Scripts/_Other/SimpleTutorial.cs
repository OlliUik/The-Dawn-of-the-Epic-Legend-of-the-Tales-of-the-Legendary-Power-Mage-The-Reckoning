//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class SimpleTutorial : CallSceneChange
{
    public string levelName = null;
    public GameObject tutorialScreen = null;

    public void Tutorial() 
    {
        if (QualityManager.DATA.SHOW_TUTORIAL)
        {
            if (tutorialScreen != null)
            {
                tutorialScreen.SetActive(true);
            }
            else 
            {
                Debug.LogError(this + " variable 'tutorialScreen' is null!");
            }
        }
        else
        {
            ChangeLevel();
        }
    }

    public void ChangeLevel()
    {
        if (levelName != null)
        {
            ChangeLevel(levelName);
        }
        else
        {
            Debug.LogError(this + " variable 'levelName' is null!");
        }
    }
}
