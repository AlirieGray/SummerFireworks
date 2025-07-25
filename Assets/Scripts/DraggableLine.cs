using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class DraggableLine : MonoBehaviour
{
    public Vector3 position; //the line's endpoint
    public Vector3 socketPosition; //what you drag it to
    public Vector3 originPosition; //where the line comes from

    public int resourceIconID = 0;
    public List<Sprite> icons = new List<Sprite>();

    public GameObject socket;
    public GameObject origin;
    public LineRenderer line;

    //shoulda made a draggable base class to inheirit from, but i didnt :D
    bool held;
    public bool solved;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originPosition = transform.position;

        socket.transform.position = socketPosition;
        origin.transform.position = originPosition;

        line.SetPosition(0, origin.transform.localPosition);
        line.SetPosition(1, origin.transform.localPosition/2);
        line.SetPosition(2, Vector3.zero);
        foreach(SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sprite = icons[resourceIconID];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (held && !solved)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            socket.transform.position = socketPosition;
            origin.transform.position = originPosition;

            line.SetPosition(0, origin.transform.localPosition);
            line.SetPosition(1, origin.transform.localPosition / 2);
            line.SetPosition(2, Vector3.zero);

            if(Vector3.Distance(transform.position, socketPosition) < 0.6f)
            {
                transform.position = socketPosition;

                socket.transform.position = socketPosition;
                origin.transform.position = originPosition;

                line.SetPosition(0, origin.transform.localPosition);
                line.SetPosition(1, origin.transform.localPosition / 2);
                line.SetPosition(2, Vector3.zero);

                solved = true;
                ResourceAssembler.instance.CheckCompletion();
            }

        }
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            held = true;

        }
    }

    private void OnMouseUp()
    {
        if (held)
        {
            held = false;
        }
    }
}
