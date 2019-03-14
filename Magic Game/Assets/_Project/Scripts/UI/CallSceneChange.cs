using UnityEngine;

public class CallSceneChange : MonoBehaviour
{
    public void ChangeLevel(string levelName)
    {
        if (levelName == "Reload")
        {
            levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        GlobalVariables.loadLevel = levelName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }
}
