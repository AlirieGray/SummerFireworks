using TMPro;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private GameManager gameManager;
    private LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = gameManager.GetScore().ToString();
    }

    void BackToMainMenu()
    {
        levelManager.ResetGame();
        gameManager.ResetScore();
    }
}
