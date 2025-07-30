using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public void GetNextLevel()
    {
        LevelManager.manager.LoadNextLevel();
    }
}
