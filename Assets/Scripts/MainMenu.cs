using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private LevelManager levelManager;
    void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    public void Play()
    {
        levelManager.LoadNextLevel();
    }

}
