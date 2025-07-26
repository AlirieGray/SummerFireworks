using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int cyclesPlayed;
    private List<string> levelOrder;
    private int currentLevel;
    void Start()
    {
        cyclesPlayed = 0;
        DontDestroyOnLoad(gameObject);
        // TODO: update with resource collection level
        levelOrder = new List<string>
        {
            "MainMenu", "GatherResources", "Launch", "Placeholder", "GameOver"
        };
    }

    public void LoadNextLevel()
    {
        if (currentLevel == 3) { 
            cyclesPlayed++;
            
            if (cyclesPlayed == 6)
            {
                currentLevel = 3;
                SceneManager.LoadScene("GameOver");
            } else
            {
                currentLevel = 1;
                AdjustDifficulty();
                SceneManager.LoadScene("GatherResources");
            }
        } else 
        {
            currentLevel++;
            
            SceneManager.LoadScene(levelOrder[currentLevel]);
        }
    }

    private void AdjustDifficulty()
    {
        // adjust ring shrinking rate in game manager
    }

    public void LoadMainMenu()
    {
        currentLevel = 0;
        SceneManager.LoadScene(levelOrder[currentLevel]);
    }

    public int GetCurrentCycle()
    {
        return cyclesPlayed;
    }
}
