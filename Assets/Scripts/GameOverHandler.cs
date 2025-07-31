using TMPro;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = GameManager.manager.GetScore().ToString();
    }

    public void BackToMainMenu()
    {
        GameManager.manager.ResetGame();
        LevelManager.manager.ResetGame();
    }

    public void PlayAgain()
    {
        GameManager.manager.ResetGame();
        LevelManager.manager.PlayAgain();
    }
}
