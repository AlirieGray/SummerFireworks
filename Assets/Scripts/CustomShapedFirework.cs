using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[ExecuteAlways]
public class CustomShapedFirework : MonoBehaviour
{
    ParticleSystem ps;

    public List<Transform> childObjects = new List<Transform>();
    private float lifetime;

    public bool dontDestroy;

    public float speed = 0.1f;

    public float emission = 1;
    float em = 0;
    public bool stopEmitting;
    bool burst = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        lifetime = ps.startLifetime;
        stopEmitting = true;
    }

    public void UpdateChildObjects()
    {
        childObjects.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {
            childObjects.Add(transform.GetChild(i));
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(!Application.isPlaying)
            return;
#endif
        if (transform.parent.GetComponent<ParticleSystem>())
        {
            var rocket = transform.parent.GetComponent<ParticleSystem>();
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
            rocket.GetParticles(particles, 1, 0);
            if (rocket.particleCount > 0)
            {
                transform.localPosition = new Vector3(particles[0].position.x, particles[0].position.y);
                //stopEmitting = true;
                //transform.localScale = particles[0].GetCurrentSize3D(ps);
                //transform.parent.localScale = Vector3.one * 0.2f;
            }
            else if(rocket.time > 0.1f)
            {
                if (!burst)
                {
                    burst = true;
                    //transform.localScale = Vector3.one;
                    stopEmitting = false;
                    StartCoroutine(Die());
                }
            }
        }
        em += emission * Time.deltaTime;
        if (!stopEmitting) {
            var loops = Mathf.FloorToInt(em);
            for (int i = 0; i < loops; i++)
            {
                EmitParticle();
                em -= 1;
            }
        }
    }

    void EmitParticle()
    {
        //pick a random point
        //pick forward or backward
        // pick a random lerp value
        var pointID = Random.Range(0, childObjects.Count);
        if(pointID < 0 || pointID >= childObjects.Count)
        {
            UpdateChildObjects();
        }
        
        var fwdOrBck = Random.Range(0, 2) * 2 - 1;

        var otherPointID = Mathf.RoundToInt(Mathf.Repeat(pointID + fwdOrBck, childObjects.Count));

        if(otherPointID< 0)
            otherPointID = 0;

        //Debug.Log("emitting between point " + pointID + " and " + otherPointID);
        var lerp = Random.Range(0, 1f);
        ParticleSystem.EmitParams particleParams = new ParticleSystem.EmitParams();
        particleParams.position = Vector3.Lerp(childObjects[pointID].position, childObjects[otherPointID].position, lerp);
        var dir = (particleParams.position - transform.position).normalized;
        dir.z = 0;
        particleParams.velocity = dir * speed;
        ps.Emit(particleParams, 1);
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        if (childObjects.Count > 1)
        {
            for(int i = 1; i < childObjects.Count; i++)
            {
                Transform tf = childObjects[i];
                Gizmos.DrawLine(tf.position, childObjects[i-1].position);
            }
        
        }
        if(childObjects.Count > 0)
        {
            Gizmos.DrawLine(childObjects[0].position, childObjects[transform.childCount - 1].position);
        }
    }

    IEnumerator Die()
    {
        if (!dontDestroy)
        {
            yield return new WaitForSeconds(lifetime);
            stopEmitting = true;


            while(ps.particleCount > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
