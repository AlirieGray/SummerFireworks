using System.Collections.Generic;
using UnityEngine;

public class ResourceAssembler : MonoBehaviour
{
    public static ResourceAssembler instance;

    public GameObject linePrefab;

    public GameObject prefab;

    public Dictionary<ResourceScriptableObject, int> resDict = new Dictionary<ResourceScriptableObject, int>();
    public List<float> grindAmount = new List<float>();

    public List<GameObject> pile = new List<GameObject>();

    public List<DraggableLine> linePuzzles = new List<DraggableLine>();

    public float range = 2.5f;

    Vector2 prevMouseClickPosition;

    public Material fullscreenFadeMat;
    public GameObject completedLinePrefab;

    List<LineRenderer> lrs = new List<LineRenderer>();

    public ParticleSystem ps;
    List<ParticleSystem> grindStonePS = new List<ParticleSystem>();
    Dictionary<ResourceScriptableObject, ParticleSystem> grindFXDict = new Dictionary<ResourceScriptableObject, ParticleSystem>();

    void Awake()
    {
        instance = this;
        //do not have more than one per scene!!!!!
        linePrefab.SetActive(false);


        for(int i = 0; i < 5; i++)
        {
            var n = Instantiate(completedLinePrefab);
            n.SetActive(true);
            n.transform.position += Vector3.left * 10;
            lrs.Add(n.GetComponent<LineRenderer>());
        }
    }

    private void Start()
    {
        grindStonePS.Add(ps);
        while (grindStonePS.Count < GameManager.manager.resources.Count)
        {
            var n = Instantiate(ps);
            n.transform.parent = ps.transform.parent;
            grindStonePS.Add(n);
        }

        for (int i = 0; i < GameManager.manager.resources.Count; i++)
        {
            ResourceScriptableObject res = GameManager.manager.resources[i];
            var xoff = res.overrideSprite.textureRectOffset.x;
            grindFXDict.Add(res, grindStonePS[i]);
            var mat = grindStonePS[i].GetComponent<ParticleSystemRenderer>().material;
            mat.SetFloat("_offset", xoff);
            //add this to the shader of each material grindstone.
        }
    }

    void Update()
    {
        fullscreenFadeMat.SetFloat("_alpha", Mathf.MoveTowards(fullscreenFadeMat.GetFloat("_alpha"), 0, Time.deltaTime));
    }

    private void OnGUI()
    {
        var aspectRatio = Screen.width / Screen.height;
        var world = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x / (aspectRatio), Input.mousePosition.y / 1));
        GUI.Box(new Rect(10, 10, 200, 24), "cursor @ :" + world.x + ", " + world.y);
    }

    public void CheckCompletion()
    {
        //are we done?
        foreach (DraggableLine line in linePuzzles)
        {
            if (!line.solved)
                return;
        }
        //check recipe here

        List<ResourceScriptableObject> usedIngredients = new List<ResourceScriptableObject>();

        foreach(ResourceScriptableObject res in resDict.Keys)
        {
            for(int i = 0; i < resDict[res]; i++)
            {
                usedIngredients.Add(res);
            }
        }
        Debug.Log("finished a firework with " + usedIngredients.Count + " resources in it");
        DisplayCompletedFireworks.instance.AddNewFirework(usedIngredients);

        //clear
        resDict.Clear();
        grindAmount.Clear();
        foreach(GameObject go in pile)
        {
            Destroy(go);
        }
        pile.Clear();

        fullscreenFadeMat.SetFloat("_alpha", 1);

        for(int i = 0; i < linePuzzles.Count; i++)
        {
            DraggableLine line = linePuzzles[i];
            lrs[i].gameObject.SetActive(true);
            lrs[i].transform.position = line.line.transform.position;
            lrs[i].positionCount = line.line.positionCount;
            for (int x = 0; x < line.line.positionCount; x++)
            {
                lrs[i].SetPosition(x, line.line.GetPosition(x));
            }

            Destroy(line.gameObject);
        }
        linePuzzles.Clear();
        Invoke("HideLines", 1);
    }

    void HideLines()
    {
        for (int i = 0; i < lrs.Count; i++)
        {
            lrs[i].gameObject.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //craft
            if(pile.Count > 0)
                SpawnWires(Mathf.Min(Mathf.Max(3, pile.Count), 5));
        }
    }

    void SpawnWires(int lines = 3)
    {
        float incrimentPerStep = 180.0f / lines;

        for(int i = 0; i < lines; i++)
        {
            var currentRot = (incrimentPerStep * i) * Mathf.Deg2Rad;
            var oppositeRot = ((incrimentPerStep * i) + 180) * Mathf.Deg2Rad;
            //spawn 
            var position = new Vector3(Mathf.Sin(currentRot) * range, Mathf.Cos(currentRot) * range);
            var opPosition = new Vector3(Mathf.Sin(oppositeRot) * range, Mathf.Cos(oppositeRot) * range);
            var n = Instantiate(linePrefab);
            n.transform.position = position;
            n.SetActive(true);
            var l = n.GetComponent<DraggableLine>();
            l.resourceIconID = i;
            linePuzzles.Add(l);

            l.originPosition = position;
            l.socketPosition = opPosition;

        }

        //assign opPosition to each one randomly.

        List<int> usedNumbers = new List<int>();
        List<Vector3> newSocketPositions = new List<Vector3>();
        for (int i = 0; i < lines; i++)
        {
            var randomOpPos = Random.Range(0, lines);

            while (usedNumbers.Contains(randomOpPos))
            {
                randomOpPos = Random.Range(0, lines);
            }

            usedNumbers.Add(randomOpPos);

            var l = linePuzzles[i];
            newSocketPositions.Add(linePuzzles[randomOpPos].socketPosition);
        }
        for (int i = 0; i < newSocketPositions.Count; i++)
        {
            var l = linePuzzles[i];
            l.socketPosition = newSocketPositions[i];
        }
    }

    public void Grind()
    {
        //once every fixed update;
        for(int i = 0; i < grindAmount.Count; i++)
        {
            var f = grindAmount[i];
            grindAmount[i] = Mathf.MoveTowards(f, 1, Time.fixedDeltaTime * 0.1f);
            pile[i].transform.localPosition = Vector3.Lerp(pile[i].transform.localPosition, Vector3.zero, grindAmount[i]);
            pile[i].transform.localScale = Vector3.Lerp(pile[i].transform.localScale, Vector3.one * 1.5f, grindAmount[i]);

        }
        foreach(ResourceScriptableObject res in resDict.Keys)
        {
            grindFXDict[res].Emit(1);
        }
    }

    public void TryAddResource(ResourceScriptableObject res)
    {
        var screen = GameManager.manager.CursorWorldPosition();
        var mousePos2D = new Vector2(screen.x, screen.y);
        if(Vector2.Distance(mousePos2D, new Vector2(transform.position.x, transform.position.y) )<= range)
        {
            AddResource(res);
        }
    }

    public void AddResource(ResourceScriptableObject res)
    {
        Debug.Log("added " + res.name + " to mortar");
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
        grindAmount.Add(0);

        var n = Instantiate(prefab);
        Destroy(n.GetComponent<DraggableResource>());
        Destroy(n.GetComponent<BoxCollider2D>());
        var img = n.GetComponent<SpriteRenderer>();
        

        n.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f),0);
        n.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-180, 180)));

        n.transform.SetParent(transform);

        if (res.overrideSprite != null)
        {
            img.sprite = res.overrideSprite;
            img.color = Color.white;
        }
        else
            img.color = res.color;

        pile.Add(n);

        for(int i = 0; i < pile.Count; i++)
        {
            var c = pile[i];
            c.GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }
}
