using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{

    public ParticleSystem[] credits = new ParticleSystem[7];

    public Button play;
    public Button creditsButton;
    public Button back;
    public Button quit;
    public GameObject settingsPopup;

    void Start()
    {

        back.gameObject.SetActive(false);
    }

    public void Play()
    {
        gameObject.SetActive(false);
        LevelManager.manager.LoadNextLevel();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        settingsPopup.SetActive(true);
    }
    Coroutine creditsAnim;
    public void Credits()
    {
        back.gameObject.SetActive(true);
        play.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);

        LaunchRandomFirework.randomFireworkDisplay.stop = true;
        StopAllCoroutines();
        creditsAnim = StartCoroutine(AnimateCredits());
    }

    IEnumerator AnimateCredits()
    {
        //move camera to the left?

        float t = 0;
        while(t < 1)
        {
            t += Time.fixedDeltaTime;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0,0,-10) + (Vector3.left * 30), t);
            yield return new WaitForFixedUpdate();
        }

        foreach (ParticleSystem ps in credits)
        {
            if (ps == null)
                continue;
            //ps.Play();
            ps.GetComponent<CreditFirework>().Play();
            yield return new WaitForSecondsRealtime(0.25f);
        }

        yield return null;
    }

    IEnumerator MoveBack()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.fixedDeltaTime;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, 0, -10), t);
            yield return new WaitForFixedUpdate();
        }
    }

    public void BackToMain()
    {
        if (creditsAnim != null)
        {
            StopCoroutine(creditsAnim);
        }

        foreach (ParticleSystem ps in credits)
        {
            ps.GetComponent<CreditFirework>().Hide();
        }
        LaunchRandomFirework.randomFireworkDisplay.stop = false;
        StartCoroutine(MoveBack());
        back.gameObject.SetActive(false);
        play.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);

    }

    public void GetNextLevel()
    {
        LevelManager.manager.LoadNextLevel();
    }

}
