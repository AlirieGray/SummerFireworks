using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class TextHandler : MonoBehaviour
{
    private DisplayText[] texts;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;
    private int scoreDisplay;
    void Start()
    {
        texts = gameObject.GetComponentsInChildren<DisplayText>();
        scoreDisplay = 0;
        scoreText.text = scoreDisplay.ToString();
    }

    public void ResetAll()
    {
        foreach (var text in texts)
        {
            text.Reset();
        }
    }

    public void UpdateScore(int newScore)
    {
        StartCoroutine(StepUpScore(newScore));
    }

    public void Countdown()
    {
        StartCoroutine(ShowCountdownNumbers());
    }

    IEnumerator ShowCountdownNumbers()
    {
        countdownText.text = "3...";
        yield return new WaitForSeconds(.7f);
        countdownText.text = "2...";
        yield return new WaitForSeconds(.7f);
        countdownText.text = "1...";
        yield return new WaitForSeconds(.7f);
        countdownText.text = "GO!!";
        float color = 1f;
        yield return new WaitForSeconds(0.8f);

        while (color > 0f)
        {
            countdownText.color = new Color(1, 1, 1, color);
            color -= .1f;
            yield return new WaitForSeconds(0.1f);
        }
        countdownText.color = new Color(1, 1, 1, 0);
    }

    IEnumerator StepUpScore(int newValue)
    {
        while (scoreDisplay < newValue)
        {
            scoreDisplay += 10;
            scoreText.text = scoreDisplay.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
