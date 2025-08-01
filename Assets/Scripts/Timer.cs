using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timerTime;
    
    void Start()
    {
        StartCoroutine(Countdown(GameManager.manager.GetResourceGatheringTime()));
    }

    public void Stop()
    {
        StopAllCoroutines();
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1f);
        if (LevelManager.manager.GetCurrentLevel() == 3)
        {
            LevelManager.manager.LoadNextLevel();
        }
    }

    IEnumerator Countdown(float timeSeconds)
    {
        timerTime = timeSeconds;
        while (timerTime > 0f) {
            if (timerTime == 60f)
            {
                timerText.text = "1:00";

            } else
            {
                string str = timerTime.ToString();
                if (timerTime < 10)
                    str = "0" + str;
                timerText.text = "0: " + str;
            }
            timerTime -= 1f;
            yield return new WaitForSeconds(1f);
        }
        LevelManager.manager.LoadNextLevel();
    }
}
