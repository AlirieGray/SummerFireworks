using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    private RocketController rocket;
    private Vector3 scale;
    private List<Collider2D> overlapping;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        overlapping = new List<Collider2D>();
        rocket = FindFirstObjectByType<RocketController>();
        scale = gameObject.transform.localScale;
        StartCoroutine(Shrink());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CheckCollision()
    {
        for (int i = 0; i < overlapping.Count; i++) {
            if (overlapping[i].gameObject.CompareTag("OuterTarget"))
            {
                rocket.SetOkZone(true);
            
            } else if (overlapping[i].gameObject.CompareTag("InnerTarget"))
            {
                rocket.SetPerfectZone(true);
                rocket.SetOkZone(false);
            } else if (overlapping[i].gameObject.CompareTag("InnerInnerTarget"))
            {
                rocket.SetPerfectZone(false);
                rocket.SetOkZone(true);
            } 
        }
    }

    private IEnumerator Shrink()
    {
        while (scale.x > 0)
        {
            Physics2D.OverlapCollider(gameObject.GetComponent<Collider2D>(), overlapping);
            CheckCollision();
            gameObject.transform.localScale = new Vector3 (scale.x - 0.01f, scale.y - 0.01f, scale.z - 0.01f);
            scale = gameObject.transform.localScale;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
