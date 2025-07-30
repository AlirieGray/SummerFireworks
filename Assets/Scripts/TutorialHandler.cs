using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHandler : MonoBehaviour
{
    public GameObject clickAndDragText;
    public GameObject arrowL;
    public GameObject arrowR;
    public GameObject releaseText;
    private RocketController rocketController;
    private Color defaultColor;
    private Color transparent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rocketController = FindFirstObjectByType<RocketController>();
    }

    public void StartTutorial()
    {
        // blink arrows and show text for a couple seconds
        // then show target and text
        // then start game!
        StartCoroutine(ShowTutorialText());
    }

    IEnumerator ShowTutorialText()
    {
        arrowL.SetActive(true);
        arrowR.SetActive(true);
        clickAndDragText.SetActive(true);
        // blink arrows
        StartCoroutine(BlinkArrows());

        yield return new WaitForSeconds(2f);
        arrowR.SetActive(false);
        arrowL.SetActive(false);
        clickAndDragText.SetActive(false);

        // 
        releaseText.SetActive(true);

        yield return new WaitForSeconds(2f);
        rocketController.StartLevel();
        releaseText.SetActive(false);
    }

    IEnumerator BlinkArrows()
    {
        float time = 3f;
        bool transparent = false;
        float alpha = 1f;
        Image L = arrowL.GetComponent<Image>();
        Image R = arrowR.GetComponent<Image>();
        while (time > 0f)
        {
            transparent = !transparent;
            alpha = transparent ? 1f : 0f;
            L.color = new Color(1,1,1,alpha);
            R.color = new Color(1, 1, 1, alpha);
            time -= 0.3f;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
