using System.Collections.Generic;
using UnityEngine;

public class ResourceAssembler : MonoBehaviour
{
    public static ResourceAssembler instance;

    public GameObject prefab;

    public Dictionary<ResourceScriptableObject, int> resDict = new Dictionary<ResourceScriptableObject, int>();

    public List<GameObject> pile = new List<GameObject>();

    public float range = 2.5f;

    void Awake()
    {
        instance = this;
        //do not have more than one per scene!!!!!
    }

    void Update()
    {
        
    }

    public void TryAddResource(ResourceScriptableObject res)
    {
        var screen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePos2D = new Vector2(screen.x, screen.y);
        if(Vector2.Distance(mousePos2D, new Vector2(transform.position.x, transform.position.y) )<= range)
        {
            AddResource(res);
        }
    }

    public void AddResource(ResourceScriptableObject res)
    {
        if (resDict.ContainsKey(res))
        {
            resDict[res] += 1;
        }
        else
        {
            resDict.Add(res, 1);
        }
        AddItToThePile(res);
    }

    void AddItToThePile(ResourceScriptableObject res)
    {
        var n = Instantiate(prefab);
        Destroy(n.GetComponent<DraggableResource>());
        Destroy(n.GetComponent<BoxCollider2D>());
        var img = n.GetComponent<SpriteRenderer>();
        img.color = res.color;

        n.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),0);
        n.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180)));

        n.transform.SetParent(transform);

        if (res.overrideSprite != null)
        {
            img.sprite = res.overrideSprite;
        }

        pile.Add(n);

        for(int i = 0; i < pile.Count; i++)
        {
            var c = pile[i];
            c.GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }
}
