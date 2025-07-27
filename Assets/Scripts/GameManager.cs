using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<ResourceScriptableObject> resources = new List<ResourceScriptableObject>();

    public Dictionary<string, ResourceScriptableObject> resourceNames = new Dictionary<string, ResourceScriptableObject>();

    public Dictionary<ResourceScriptableObject, int> resDict = new Dictionary<ResourceScriptableObject, int>();

    public List<RecipeScriptableObject> craftedRecipe = new List<RecipeScriptableObject> ();
    public List<List<ResourceScriptableObject>> finishedFireworks = new List<List<ResourceScriptableObject>>();
    public int score;

    // fireworks mini-game numbers
    public float fireworksTargetSpeed;

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
    }

    public void IncreaseScore(int value)
    {
        score += value;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddFirework(List<ResourceScriptableObject> firework)
    {
        finishedFireworks.Add(firework);
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
