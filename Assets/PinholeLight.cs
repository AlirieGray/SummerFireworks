using UnityEngine;
using UnityEngine.UI;

public class PinholeLight : MonoBehaviour
{
    public Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        var aspectRatio = Screen.width / Screen.height;
        image.material.SetVector("_mouseCoords", new Vector2((Input.mousePosition.x/ (float)Screen.width), Input.mousePosition.y / (float)Screen.height));
    }
}
