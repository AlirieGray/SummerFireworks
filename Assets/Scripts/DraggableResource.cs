using UnityEngine;

public class DraggableResource : MonoBehaviour
{
    public bool held;

    //display a ghost for when the player is dragging.

    SpriteRenderer ghost;

    public ResourceScriptableObject resource;

    bool isBroke = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Ghost
        GameObject n = Instantiate(gameObject);
        Destroy(n.GetComponent<DraggableResource>());
        Destroy(n.GetComponent<BoxCollider2D>());
        ghost = n.GetComponent<SpriteRenderer>();
        ghost.transform.GetChild(0).gameObject.SetActive(false);
        ghost.color = new Color(ghost.color.r, ghost.color.g, ghost.color.b, ghost.color.a / 2.0f);
        ghost.sortingOrder = 99;
        ghost.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (held)
        {
            ghost.transform.position = GameManager.manager.CursorWorldPosition();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            held = true;
            ghost.gameObject.SetActive(true);
            
            ghost.transform.position = GameManager.manager.CursorWorldPosition();
            //for specifics get an offset from center based on where you clicked
            //but thats for later
        }
    }

    private void OnMouseUp()
    {
        if (held)
        {
            //kill ghost if not at mortar
            ResourceAssembler.instance.TryAddResource(resource);
            ghost.gameObject.SetActive(false);
        }
    }
}
