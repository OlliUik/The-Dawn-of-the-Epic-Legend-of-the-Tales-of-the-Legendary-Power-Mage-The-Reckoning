using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverObject = null;

    void Start()
    {
        gameOverObject.SetActive(false);
    }

    public void Activate()
    {
        gameOverObject.SetActive(true);
    }
}
