using TMPro;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = GameManager.manager.GetScore().ToString();
    }

    public void BackToMainMenu()
    {
        GameManager.manager.ResetGame();
        levelManager.ResetGame();
    }

    public void PlayAgain()
    {
        GameManager.manager.ResetGame();
        levelManager.PlayAgain();
    }
}
