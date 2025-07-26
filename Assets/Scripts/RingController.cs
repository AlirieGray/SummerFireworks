using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    private RocketController rocket;
    private Vector3 scale;
    private List<Collider2D> overlapping;
    private bool missedPerfectZone;
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
        CheckCollision();
    }
    private void CheckCollision()
    {
        if (!missedPerfectZone)
        {
            for (int i = 0; i < overlapping.Count; i++) {
                if (overlapping[i].gameObject.CompareTag("OuterTarget"))
                {
                    rocket.SetOkZone(true);
                    break;
            
                } else if (overlapping[i].gameObject.CompareTag("InnerTarget"))
                {
                    Debug.Log("in perfect zone");
                    rocket.SetPerfectZone(true);
                    rocket.SetOkZone(false);
                    break;
                } else if (overlapping[i].gameObject.CompareTag("InnerInnerTarget"))
                {
                    Debug.Log("MISSED perfect zone");
                    rocket.SetPerfectZone(false);
                    rocket.SetOkZone(false);
                    missedPerfectZone = true;
                    break;
                } 
            }
        }
    }

    private IEnumerator Shrink()
    {
        while (scale.x > 0)
        {
            Physics2D.OverlapCollider(gameObject.GetComponent<Collider2D>(), overlapping);
            gameObject.transform.localScale = new Vector3 (scale.x - 0.03f, scale.y - 0.03f, scale.z - 0.03f);
            scale = gameObject.transform.localScale;
            yield return new WaitForSeconds(0.1f);
        }
        rocket.SetPerfectZone(false);
        rocket.SetOkZone(false);
        rocket.SpawnNewRingAndTarget();
    }
}
