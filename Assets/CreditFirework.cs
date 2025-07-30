using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Rendering.Universal;
using System.Net.Sockets;

public class CreditFirework : MonoBehaviour
{
    bool displayInfo;

    public CustomShapedFirework custom;
    public TextMeshPro n;

    public Transform moveWithParticle;

    public Light2D flare;
    public bool hasPlayed = false;
    Coroutine display;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<ParticleSystem>().particleCount == 0 && GetComponent<ParticleSystem>().time > 0.1f && hasPlayed)
        {
            if (!displayInfo)
            {
                displayInfo = true;
                display = StartCoroutine(Display());
                //custom.stopEmitting = false;
            }
        }
        else
        {
            displayInfo = false;
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
            GetComponent<ParticleSystem>().GetParticles(particles, 1, 0);
            moveWithParticle.localPosition = new Vector3(particles[0].position.x, particles[0].position.y);
            custom.stopEmitting = true;

            n.color = Color.clear;
        }
    }

    public void Hide()
    {
        if (display != null)
            StopCoroutine(display);
        moveWithParticle.localPosition = Vector3.zero;
        custom.stopEmitting = true;
        flare.pointLightOuterRadius = 0;
        n.color = Color.clear;
        hasPlayed = false;
    }

    public void Play()
    {
        GetComponent<ParticleSystem>().Play();
        moveWithParticle.localPosition = Vector3.zero;
        hasPlayed = true;
    }

    IEnumerator Display()
    {
        //fade in text
        //start emitting
        Debug.Log("display credit for " + gameObject.name);
        custom.stopEmitting = false;
        float t = 0;
        while(t < 1)
        {
            flare.pointLightOuterRadius = 2 - (t * 2f);
            n.color = new Color(1, 1, 1, t);
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        flare.pointLightOuterRadius = 2 - (1 * 2f);
        n.color = new Color(1, 1, 1, 1);

        yield return null;
    }
}
