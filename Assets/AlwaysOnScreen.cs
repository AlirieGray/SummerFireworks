using UnityEngine;

public class AlwaysOnScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var left = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        transform.position = new Vector3(Mathf.Min(left.x - (2), 8), transform.position.y, 0);
    }
}
