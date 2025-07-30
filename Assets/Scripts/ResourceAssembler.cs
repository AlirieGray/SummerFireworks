using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
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

    public LineRenderer dragLine;
    List<LineRenderer> dragLines = new List<LineRenderer>();

    public Material timerMaterial;
    public float timerMin = 4;
    public float timerMax = 5;
    float timer;
    float currentMax;

    public TextMeshPro whatToDo;

    bool hasMadeAFirework = false;

    bool isCrafting;

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
        dragLines.Add(dragLine);
        if (LevelManager.manager.GetCurrentLevel() == 0)
        {
            for(int i = 0; i < 4; i++)
            {
                var n = Instantiate(dragLine.gameObject);
                dragLines.Add(n.GetComponent<LineRenderer>());
            }
        }
        var difficultyEasing = Mathf.Max(8 - LevelManager.manager.GetCurrentLevel(), 0);
        timerMax += difficultyEasing;
        timerMin += difficultyEasing;
    }

    void Update()
    {
        fullscreenFadeMat.SetFloat("_alpha", Mathf.MoveTowards(fullscreenFadeMat.GetFloat("_alpha"), 0, Time.deltaTime));

        if(!hasMadeAFirework && isCrafting && LevelManager.manager.GetCurrentLevel() == 0)
        {
            timer = timerMax;
            for(int i = 0; i < dragLines.Count; i++)
            {
                if(i < linePuzzles.Count)
                {
                    dragLines[i].SetPosition(0, linePuzzles[i].transform.position);
                    dragLines[i].SetPosition(1, linePuzzles[i].socketPosition);
                }
                else
                {
                    dragLines[i].SetPosition(0, Vector3.zero);
                    dragLines[i].SetPosition(1, Vector3.zero);
                }
            }
        }
        else
        {
            for (int i = 0; i < dragLines.Count; i++)
            {
                    dragLines[i].SetPosition(0, Vector3.zero);
                    dragLines[i].SetPosition(1, Vector3.zero);
            }
        }

    }

    private void FixedUpdate()
    {
        if (isCrafting)
        {
            if(timer > 0)
            {
                timer = Mathf.MoveTowards(timer, 0, Time.fixedDeltaTime);
                timerMaterial.SetFloat("_progress", (timer / currentMax));
            }
            else if(timer == 0)
            {
                timerMaterial.SetFloat("_progress", -0.1f);
                YourTakingTooLong();
            }
        }
    }

    private void OnDestroy()
    {
        timerMaterial.SetFloat("_progress", -0.1f);
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

        CraftedFirework();
    }

    void CraftedFirework()
    {
        isCrafting = false;
        hasMadeAFirework = true;
        timerMaterial.SetFloat("_progress", -0.1f);



        List<ResourceScriptableObject> usedIngredients = new List<ResourceScriptableObject>();

        foreach (ResourceScriptableObject res in resDict.Keys)
        {
            for (int i = 0; i < resDict[res]; i++)
            {
                usedIngredients.Add(res);
            }
        }

        AddResourcePestel.pestel.GetComponent<BoxCollider2D>().enabled = true;

        Debug.Log("finished a firework with " + usedIngredients.Count + " resources in it");
        DisplayCompletedFireworks.instance.AddNewFirework(usedIngredients);

        //clear
        resDict.Clear();
        grindAmount.Clear();
        foreach (GameObject go in pile)
        {
            Destroy(go);
        }
        pile.Clear();

        fullscreenFadeMat.SetFloat("_alpha", 1);

        for (int i = 0; i < linePuzzles.Count; i++)
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
        //Invoke("HideLines", 1);
    }

    //??
    void YourTakingTooLong()
    {
        //make the lines freak out. maybe turn red?
        //add a Blunder resource
        Debug.Log("taking too long!");
        var blunder = Resources.Load<ResourceScriptableObject>("ResourceData/Blunder");
        foreach (DraggableLine line in linePuzzles)
        {
            if (!line.solved)
                AddResource(blunder);
        }
        whatToDo.text = "Ya gotta be faster than that!";

        CraftedFirework();
        StartCoroutine(FailureAnimation());
    }

    IEnumerator FailureAnimation()
    {
        float t = 0;
        while (t < 1)
        {
            foreach (LineRenderer rend in lrs)
            {
                //rend.startColor = (Color.red);
                //rend.endColor = (Color.red);
                rend.widthMultiplier =  1 - t;
                for(int i = 0; i < rend.positionCount; i++)
                {
                    var randomDir = Random.Range(-Mathf.PI, Mathf.PI) * Mathf.Rad2Deg;

                    Vector3 n = new Vector3(Mathf.Sin(randomDir) * Time.deltaTime * 5, Mathf.Cos(randomDir) * Time.deltaTime * 5, 0);
                    rend.SetPosition(i, rend.GetPosition(i) + n);
                }
            }
            t += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        HideLines();
        yield return null;
    }

    void HideLines()
    {
        for (int i = 0; i < lrs.Count; i++)
        {
            lrs[i].gameObject.SetActive(false);
            lrs[i].widthMultiplier = 1;
        }
        whatToDo.text = "Click and drag to add ingredients.";
    }

    public void FinishGrind()
    {
        if (pile.Count > 0 && resDict.ContainsKey(GameManager.manager.resourceNames["VolatileCrystals"]))
            SpawnWires(Mathf.Min(Mathf.Max(3, pile.Count), 5));
        else
        {
            AddResourcePestel.pestel.GetComponent<BoxCollider2D>().enabled = true;
            whatToDo.text = "Add a volatile crystal to finish this firework.";
        }
    }

    void SpawnWires(int lines = 3)
    {
        whatToDo.text = "Match the symbols quickly to finish the firework.";
        isCrafting = true;
        timer = Mathf.Lerp(timerMin, timerMax, lines / 5);
        currentMax = timer;
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
            l.solved = false;
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
        var keys = new List<ResourceScriptableObject>();
        foreach (ResourceScriptableObject res in new List<ResourceScriptableObject>(resDict.Keys))
        {
            for(int i = 0; i < resDict[res]; i++)
            {
                keys.Add(res);
            }
        }

        for(int i = 0; i < grindAmount.Count; i++)
        {
            var f = grindAmount[i];
            grindAmount[i] = Mathf.MoveTowards(f, 1, Time.fixedDeltaTime * Random.Range(1f, 2f));
            //pile[i].transform.localPosition = Vector3.Lerp(pile[i].transform.localPosition, Vector3.zero, grindAmount[i]);
            //pile[i].transform.localScale = Vector3.Lerp(pile[i].transform.localScale, Vector3.one * 1.5f, grindAmount[i]);



            Debug.Log("grindstone @ index " + i + "/" + keys.Count + " " + pile.Count);
            if (grindAmount[i] >= 1)
                if(i < pile.Count)
                    pile[i].GetComponent<SpriteRenderer>().sprite = keys[i].groundUpSprite;
        }
        foreach(ResourceScriptableObject res in resDict.Keys)
        {
            grindFXDict[res].Emit(1);
        }
    }

    public bool TryAddResource(ResourceScriptableObject res)
    {
        var screen = GameManager.manager.CursorWorldPosition();
        var mousePos2D = new Vector2(screen.x, screen.y);
        if(Vector2.Distance(mousePos2D, new Vector2(transform.position.x, transform.position.y) )<= range)
        {
            AddResource(res);
            return true;
        }
        else
        {
            return false;
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
        whatToDo.text = "Keep adding ingredients, or click the <color=#00FFFF>pestel</color> if you're done";
    }

    void AddItToThePile(ResourceScriptableObject res)
    {
        grindAmount.Add(0);

        var n = Instantiate(prefab);
        Destroy(n.GetComponent<DraggableResource>());
        Destroy(n.GetComponent<BoxCollider2D>());
        var img = n.GetComponent<SpriteRenderer>();

        n.transform.GetChild(0).gameObject.SetActive(false);
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
