using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TextPopup : MonoBehaviour
{
    public TextMeshPro textField;

    public string textDisplayed;
    public int timeAlive = 100; //this is in frames :skull:
    int baseTimeAlive; //set in start to keep track of it as the og number is reduced;
    public float fontSize = 1;

    private void Start()
    {
        textField = GetComponent<TextMeshPro>();
        textField.text = textDisplayed;
        baseTimeAlive = timeAlive;
    }

    private void FixedUpdate()
    {
        if (timeAlive <= 0)
            Destroy(gameObject);

        transform.localScale = Vector3.one * (timeAlive / baseTimeAlive);
        timeAlive--;
    }
}