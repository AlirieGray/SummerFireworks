using UnityEngine;
using TMPro;
using System.Collections;

public class DisplayText : MonoBehaviour
{
    private TextMeshProUGUI textMP;
    RectTransform rt;
    float originalY;
    void Start()
    {
        textMP = GetComponent<TextMeshProUGUI>();
        rt = GetComponent<RectTransform>();
        originalY = rt.transform.position.y;
    }

    public void DisplayWithShrink()
    {
        textMP.color = new Color(1, 1, 1, 1);
        StartCoroutine(Shrink());
    }

    public void DisplayWithShake()
    {
        textMP.color = new Color(1, 0, 0, 1);
        StartCoroutine(Shake());
    }

    public void Reset()
    {
        StopAllCoroutines();
        textMP.color = new Color(1, 1, 1, 0f);
        rt.transform.position = new Vector3 (
            rt.transform.position.x, originalY, rt.transform.position.z);
    }

    IEnumerator FadeOut()
    {
        float color = 1f;
        yield return new WaitForSeconds(0.3f);

        while (color > 0f) {
            textMP.color = new Color(1,1,1,color);
            color -= .1f;
            yield return new WaitForSeconds(0.1f);
        }
        textMP.color = new Color(1, 1, 1, 0);
    }

    IEnumerator Shrink()
    {
        float font = 56;
        while (font > 26)
        {
            textMP.fontSize = font;
            font -= 2;
            yield return new WaitForSeconds(0.02f);
        }
        StartCoroutine(FadeOut());
    }

    IEnumerator Shake()
    {
        
        float time = .1f;
        int multiplier = -1;
        while (time > 0f) {
            multiplier *= -1;
            time -= .01f;
            rt.position = new Vector3(
                rt.position.x,
                rt.position.y + 5 * multiplier,
                rt.position.z);
            yield return new WaitForSeconds(time);
        }
        StartCoroutine(FadeOut());
    }

}
