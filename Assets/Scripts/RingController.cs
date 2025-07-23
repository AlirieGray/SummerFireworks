using System.Collections;
using UnityEngine;

public class RingController : MonoBehaviour
{
    private RocketController rocket;
    private Vector3 scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rocket = FindFirstObjectByType<RocketController>();
        scale = gameObject.transform.localScale;
        StartCoroutine(Shrink());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("colliding?");
        if (other.gameObject.CompareTag("OuterTarget"))
        {
            rocket.SetOkZone(true);
            Debug.Log("Setting ok zone T");
            
        } else if (other.gameObject.CompareTag("InnerTarget"))
        {
            rocket.SetPerfectZone(true);
            rocket.SetOkZone(false);
            Debug.Log("Setting ok zone F");
            Debug.Log("Setting PERFECT zone T");
        } else if (other.gameObject.CompareTag("InnerTarget"))
        {
            rocket.SetPerfectZone(false);
            rocket.SetOkZone(true);
            Debug.Log("Setting ok zone T");
        } 
    }

    private IEnumerator Shrink()
    {
        while (scale.x > 0)
        {
            gameObject.transform.localScale = new Vector3 (scale.x - 0.01f, scale.y - 0.01f, scale.z - 0.01f);
            scale = gameObject.transform.localScale;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
