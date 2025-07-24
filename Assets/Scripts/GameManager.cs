using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<ResourceScriptableObject> resources = new List<ResourceScriptableObject>();
    public Dictionary<ResourceScriptableObject, int> resDict = new Dictionary<ResourceScriptableObject, int>();

    public List<RecipeScriptableObject> craftedRecipe = new List<RecipeScriptableObject> ();
    public int score;

    public void RegisterResource(ResourceScriptableObject res)
    {
        resources.Add(res);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
