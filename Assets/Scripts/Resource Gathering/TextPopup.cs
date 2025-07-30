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


        var w = 2.65f;
        var CameraTopLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        var CameraBotRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, CameraTopLeft.x + w, CameraBotRight.x - w),
            Mathf.Clamp(transform.position.y, CameraTopLeft.y - w, CameraBotRight.y + w),
            transform.position.z);

    }

    private void FixedUpdate()
    {
        if (timeAlive <= 0)
            Destroy(gameObject);

        textField.fontSize = fontSize * ((float)timeAlive / (float)baseTimeAlive);
        timeAlive--;
    }
}