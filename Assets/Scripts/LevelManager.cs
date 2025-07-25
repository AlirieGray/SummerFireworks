using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int cyclesPlayed;
    private List<string> levelOrder;
    void Start()
    {
        cyclesPlayed = 0;
        DontDestroyOnLoad(gameObject);
        levelOrder = new List<string> {}
    }

    public void LoadNextLevel()
    {

    }

    public int GetCurrentCycle()
    {
        return cyclesPlayed;
    }
}
