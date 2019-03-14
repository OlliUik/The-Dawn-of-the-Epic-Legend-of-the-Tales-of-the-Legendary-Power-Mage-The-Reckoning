using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseObject = null;
    private bool isPaused = false;
    private PlayerCore caller;

    void Start()
    {
        pauseObject.SetActive(isPaused);
    }
    
    public bool FlipPauseState(PlayerCore pc)
    {
        caller = pc;
        isPaused = !isPaused;
        pauseObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        return isPaused;
    }

    //Use previous caller if no PlayerInput is specified
    //(example: pressing an UI button resumes the game)
    public void FlipPauseState()
    {
        if (caller != null)
        {
            caller.EnableControls(!FlipPauseState(caller));
        }
    }
}
