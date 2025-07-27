using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[ExecuteAlways]
public class CustomShapedFirework : MonoBehaviour
{
    ParticleSystem ps;

    public List<Transform> childObjects = new List<Transform>();
    private float lifetime;
    private bool stopEmitting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        lifetime = ps.startLifetime;
        StartCoroutine(Die());
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
        if (!stopEmitting) {
            EmitParticle();
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
        particleParams.velocity = (particleParams.position - transform.position).normalized * 0.2f;

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
        yield return new WaitForSeconds(lifetime);
        stopEmitting = true;
    }
}
