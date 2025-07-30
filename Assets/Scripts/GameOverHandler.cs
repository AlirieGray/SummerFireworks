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

    void BackToMainMenu()
    {
        levelManager.ResetGame();
        GameManager.manager.ResetScore();
    }
}
