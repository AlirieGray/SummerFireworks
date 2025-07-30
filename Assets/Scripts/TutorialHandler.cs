using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public TextMeshProUGUI clickAndDragText;
    public GameObject arrowL;
    public GameObject arrowR;
    private RocketController rocketController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartTutorial()
    {
        // blink arrows and show text for a couple seconds
        // then show target and text
        // then start game!
    }

    IEnumerator showTutorialText()
    {
        yield return new WaitForSeconds(1f);
        rocketController.StartLevel();
    }
}
