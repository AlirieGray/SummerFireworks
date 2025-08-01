using UnityEngine;
using UnityEngine.UI;

public class RenderTextureCameraUpdate : MonoBehaviour
{
    Transform monster;
    public Image enemyHighlight;

    Vector2 screenSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        monster = FindFirstObjectByType<Monster>().gameObject.transform;
        screenSize = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {

        if(screenSize.x != Screen.width || screenSize.y != Screen.height)
        {
            GetComponent<Camera>().targetTexture.Release();
            GetComponent<Camera>().targetTexture.width = Screen.width;
            GetComponent<Camera>().targetTexture.height = Screen.height;
            GetComponent<Camera>().targetTexture.Create();

            screenSize = new Vector2(Screen.width, Screen.height);
        }




        /*transform.localPosition = new Vector3(monster.position.x, monster.position.y, 0);
        var screenBorder = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        var percentX = transform.localPosition.x / screenBorder.x;
        var percentY = transform.localPosition.y / screenBorder.y;

        enemyHighlight.material.SetVector("_Offset", new Vector2(-percentX/2, -percentY/2));*/
    }
}
