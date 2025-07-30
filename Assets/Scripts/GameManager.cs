using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<ResourceScriptableObject> resources = new List<ResourceScriptableObject>();
    private InputSystem_Actions inputActions;

    public Dictionary<string, ResourceScriptableObject> resourceNames = new Dictionary<string, ResourceScriptableObject>();

    public Dictionary<ResourceScriptableObject, int> resDict = new Dictionary<ResourceScriptableObject, int>();

    public List<RecipeScriptableObject> craftedRecipe = new List<RecipeScriptableObject> ();
    public List<List<ResourceScriptableObject>> finishedFireworks = new List<List<ResourceScriptableObject>>();
    public int score;

    public List<ResourceScriptableObject> gatheredResources = new List<ResourceScriptableObject>();

    // fireworks mini-game numbers
    private float fireworksTargetSpeed;
    public bool playedFireworksTutorial;

    public Texture2D clickCursor_0; 
    public Texture2D clickCursor_1; 
    public Texture2D defaultCursor; 

    public void RegisterResource(ResourceScriptableObject res)
    {
        resources.Add(res);
        resourceNames.Add(res.name, res);
    }

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (manager != this)
            {
                Destroy(gameObject);
            }
        }

        foreach (ResourceScriptableObject res in Resources.LoadAll<ResourceScriptableObject>("ResourceData/"))
        {
            RegisterResource(res);
        }

        foreach(ResourceScriptableObject res in resources)
        {
            if (res.name != "Blunder")
            {
                resDict.Add(res, 0);
            }
        }

        // TODO: testing only
        // remove for production
        finishedFireworks.Add(new List<ResourceScriptableObject>() { resourceNames["Orb"], resourceNames["Dragonscale"], resourceNames["Yellow"] });
        finishedFireworks.Add(new List<ResourceScriptableObject>() { resourceNames["Heart"], resourceNames["Blue"] });
        finishedFireworks.Add(new List<ResourceScriptableObject>() { resourceNames["Starfruit"], resourceNames["Yellow"] });
        finishedFireworks.Add(new List<ResourceScriptableObject>() { resourceNames["Orb"], resourceNames["Dragonscale"] });
        finishedFireworks.Add(new List<ResourceScriptableObject>() { resourceNames["Heart"], resourceNames["Blue"] });
        finishedFireworks.Add(new List<ResourceScriptableObject>() { resourceNames["Starfruit"], resourceNames["Yellow"] });



    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Cursor.SetCursor(clickCursor_0, Vector2.zero, CursorMode.Auto);
        }
        if (Input.GetMouseButtonUp(0)) {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    public void IncreaseScore(int value)
    {
        score += value;
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public float GetResourceGatheringTime()
    {
        return 60f;
    }

    public float GetFireworksSpeed()
    {
        if (fireworksTargetSpeed == 0)
        {
            fireworksTargetSpeed = 0.1f;
        }
        return fireworksTargetSpeed;
    }

    public void SetFireworksSpeed(float value)
    {
        fireworksTargetSpeed = value;
    }

    public void AddFirework(List<ResourceScriptableObject> firework)
    {
        finishedFireworks.Add(firework);
    }

    public List<List<ResourceScriptableObject>> GetFinishedFireworks()
    {
        return finishedFireworks;
    }

    public void ClearFireworks()
    {
        finishedFireworks.Clear();
    }

    public Color MixColor(Color color1, Color color2)
    {
        var key = 1f - Mathf.Max(color1.r, color1.g, color1.b);
        var c = (1f - color1.r - key) / (1f - key);
        var m = (1f - color1.g - key) / (1f - key);
        var y = (1f - color1.b - key) / (1f - key);

        var key1 = 1f - Mathf.Max(color1.r, color1.g, color1.b);
        var c1 = (1f - color2.r - key1) / (1f - key1);
        var m1 = (1f - color2.g - key1) / (1f - key1);
        var y1 = (1f - color2.b - key1) / (1f - key1);


        var mixedColor = Color.Lerp(
            new Color((1f - c) * (1f - key),(1f - m) * (1f - key), (1f - y) * (1f - key)),
            new Color((1f - c1) * (1f - key1), (1f - m1) * (1f - key1), (1f - y1) * (1f - key1)),
            0.5f);

        return mixedColor;
    }

    public Vector3 CursorWorldPosition()
    {
        var aspectRatio = Screen.width / Screen.height;
        var world = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        world.z = 0;
        return world;
    }



}
