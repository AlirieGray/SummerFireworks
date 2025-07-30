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
        textField.fontSize = fontSize * ((float)timeAlive / (float)baseTimeAlive);
    }

    private void FixedUpdate()
    {
        if (timeAlive <= 0)
            Destroy(gameObject);

        textField.fontSize = fontSize * ((float)timeAlive / (float)baseTimeAlive);
        timeAlive--;
    }
}