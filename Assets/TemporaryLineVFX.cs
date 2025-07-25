using UnityEngine;

public class TemporaryLineVFX : MonoBehaviour
{

    LineRenderer line;
    public float t = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        t = Mathf.MoveTowards(t, 0, Time.deltaTime);
        line.endColor = new Color(1, 1, 1, Mathf.Clamp01(t));
        line.startColor = new Color(1, 1, 1, Mathf.Clamp01(t));
    }
}
