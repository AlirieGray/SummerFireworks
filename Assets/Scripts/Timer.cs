using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    void Start()
    {
        StartCoroutine(Countdown(GameManager.manager.GetResourceGatheringTime()));
    }

    IEnumerator Countdown(float timeSeconds)
    {
        while (timeSeconds > 0f) {
            if (timeSeconds == 60f)
            {
                timerText.text = "1:00";

            } else
            {
                timerText.text = "0: " + timeSeconds.ToString();
            }
            timeSeconds -= 1f;
            yield return new WaitForSeconds(1f);
        }
        LevelManager.manager.LoadNextLevel();
    }
}
