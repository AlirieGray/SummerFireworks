using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int cyclesPlayed;
    void Start()
    {
        cyclesPlayed = 0;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextLevel()
    {

    }

    public int GetCurrentCycle()
    {
        return cyclesPlayed;
    }
}
