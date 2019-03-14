using UnityEngine;

public class ExitApplication : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        Debug.Log("Application.Quit() is ignored in editor, you dummy.");
        #endif
    }
}
