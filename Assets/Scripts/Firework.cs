using System.Collections;
using UnityEngine;

public class Firework : MonoBehaviour
{

    // gets a color and a shape from the Resource object and applies it to the prefab
    public GameObject explosion;
    ParticleSystem ps;
    private Color explosionColor;

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(PlayExplosion());
        explosionColor = new Color(1, 1, 1f);
    }

    public void SetColor(Color color)
    {
        explosionColor = color;
        explosion.GetComponent<ParticleSystem>().startColor = explosionColor;
    }

    IEnumerator PlayExplosion()
    {
        yield return new WaitForSeconds(ps.startLifetime);
        explosion.SetActive(true);
    }



}
