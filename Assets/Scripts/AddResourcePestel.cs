using UnityEngine;
using System.Collections;

public class AddResourcePestel : MonoBehaviour
{
    Transform pivot;

    Vector2 direction;

    public static AddResourcePestel pestel;

    bool held;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pivot = transform.parent;
        pestel = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (held)
        {
            //var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 position = new Vector2(world.x, world.y);
            //var newDir = (new Vector3(position.x, position.y) - pivot.position).normalized;
            

            //ResourceAssembler.instance.Grind();
            
        }
    }

    IEnumerator AnimateGrind()
    {
        float t = 0;


        while(t < 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.fixedDeltaTime);

            pivot.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Lerp(0, 360, t)));
            ResourceAssembler.instance.Grind();
            yield return new WaitForFixedUpdate();
        }
        ResourceAssembler.instance.FinishGrind();

        yield return null;
    }

    private void OnMouseDown()
    {
        held = true;
        GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(AnimateGrind());
    }

    void StopStirring()
    {
        held=false;
    }

    private void OnMouseExit()
    {
        if (held)
        {
            StopStirring();
        }
    }

    private void OnMouseUp()
    {
        if (held)
        {
            StopStirring();
        }
    }
}
