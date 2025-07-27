using System.Collections;
using UnityEngine;

public class Firework : MonoBehaviour
{

    // gets a color and a shape from the Resource object and applies it to the prefab
    public GameObject explosion;
    ParticleSystem ps;

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(PlayExplosion());
    }

    IEnumerator PlayExplosion()
    {
        yield return new WaitForSeconds(ps.startLifetime);
        explosion.SetActive(true);
    }



}
