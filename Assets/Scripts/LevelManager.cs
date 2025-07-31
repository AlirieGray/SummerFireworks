using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;
    private int cyclesPlayed;
    private List<string> levelOrder;
    private int currentLevel;
    void Start()
    {
        if (manager == null)
        {
            manager = this;
            cyclesPlayed = 0;
            DontDestroyOnLoad(gameObject);
            blackout = transform.GetChild(0).GetComponent<RectTransform>();
        }
        else
        {
            if (manager != this)
            {
                Destroy(gameObject);
            }
        }

        levelOrder = new List<string>
        {
            "MainMenu", "MixResources", "Launch", "ResourceGathering", "GameOver"
        };
        blackout.anchoredPosition = Vector2.left * Screen.width;
    }


    public void LoadNextLevel()
    {
        if (currentLevel == 3) { 
            cyclesPlayed++;
            
            if (cyclesPlayed == 3) // three levels
            {
                StartCoroutine(FadeIn(levelOrder[4]));
            } else
            {
                currentLevel = 1;
                AdjustDifficulty();
                StartCoroutine(FadeIn(levelOrder[currentLevel]));
            }
        } else 
        {
            currentLevel++;
            StartCoroutine(FadeIn(levelOrder[currentLevel]));
            //SceneManager.LoadScene(levelOrder[currentLevel]);
        }
    }

    private void AdjustDifficulty()
    {
        // adjust ring shrinking rate in game manager
        GameManager.manager.SetFireworksSpeed(GameManager.manager.GetFireworksSpeed() - 0.05f);
        GameManager.manager.SetResourceGatheringTime(GameManager.manager.GetResourceGatheringTime() - 5f);
    }

    public void ResetGame()
    {
        currentLevel = 0;
        cyclesPlayed = 0;
        SceneManager.LoadScene(levelOrder[currentLevel]);
    }

    public void PlayAgain()
    {
        currentLevel = 1;
        cyclesPlayed = 0;
        SceneManager.LoadScene(levelOrder[currentLevel]);
    }

    public void LoadMainMenu()
    {
        currentLevel = 0;
        SceneManager.LoadScene(levelOrder[currentLevel]);
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetCurrentCycle()
    {
        return cyclesPlayed;
    }

    public RectTransform blackout;

    private void Update()
    {
        blackout.sizeDelta = Mathf.Max(Screen.width, Screen.height) * Vector2.one;
        if(!fadein)
            blackout.anchoredPosition = Vector2.left * Screen.width;
    }
    bool fadein = false;
    IEnumerator FadeIn(string levelName)
    {
        fadein = true;
        float rotation = Random.Range(-180f, 180f);
        var screenMax = Mathf.Max(Screen.width, Screen.height);
        blackout.anchoredPosition = new Vector2(Mathf.Sin(rotation) * screenMax, Mathf.Cos(rotation) * screenMax) * 1.4142f;
        float t = 0;
        while (t < 1)
        {
            blackout.anchoredPosition = Vector2.Lerp(blackout.anchoredPosition, Vector2.zero, t);
            t += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }

        StartCoroutine(WaitForSceneLoad(rotation, levelName));
        yield return null;
    }

    IEnumerator WaitForSceneLoad(float rotation, string levelName)
    {
        Debug.Log("starting async load of scene " + levelName);
        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);
        Task task = Task.Run(async () => await op);
        while (op.progress < 1f)
        {
            Debug.Log("load %: " + op.progress);
            yield return new WaitForSeconds(0);
        }
        //yield return new WaitUntil(() => task.IsCompleted);
        StartCoroutine(FadeOut(rotation));
    }

    IEnumerator FadeOut(float rotation)
    {
        float t = 0;

        var screenMax = Mathf.Max(Screen.width, Screen.height);
        var end = - new Vector2(Mathf.Sin(rotation) * screenMax, Mathf.Cos(rotation) * screenMax) * 1.4142f;
        while (t < 1)
        {
            blackout.anchoredPosition = Vector2.Lerp(blackout.anchoredPosition, end, t);
            t += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        fadein = false;
        yield return null;
    }
}
