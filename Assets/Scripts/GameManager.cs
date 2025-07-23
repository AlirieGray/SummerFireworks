using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public List<Resource> inventory = new List<Resource> ();

    public List<ResourceScriptableObject> resources = new List<ResourceScriptableObject>();
    public Dictionary<ResourceScriptableObject, int> resDict = new Dictionary<ResourceScriptableObject, int>();

    public class Resource
    {
        public ResourceScriptableObject resource;
        public int count = 0;
    }

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
