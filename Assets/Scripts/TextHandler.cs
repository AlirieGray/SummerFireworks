using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class TextHandler : MonoBehaviour
{
    private DisplayText[] texts;
    public TextMeshProUGUI scoreText;
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

    IEnumerator StepUpScore(int newValue)
    {
        while (scoreDisplay < newValue)
        {
            scoreDisplay++;
            scoreText.text = scoreDisplay.ToString();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
