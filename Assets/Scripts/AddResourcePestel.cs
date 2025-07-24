using UnityEngine;

public class AddResourcePestel : MonoBehaviour
{
    Transform pivot;

    Vector2 direction;

    bool held;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pivot = transform.parent;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (held)
        {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 position = new Vector2(world.x, world.y);
            var newDir = (new Vector3(position.x, position.y) - pivot.position).normalized;
            pivot.rotation = Quaternion.Euler(pivot.rotation.eulerAngles + (Vector3.forward * 2));

            ResourceAssembler.instance.Grind();
        }
    }

    private void OnMouseDown()
    {
        held = true;
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
