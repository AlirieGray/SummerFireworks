using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName = "Fireworks/Create New Recipe", fileName = "New Recipe", order = 1)]
public class RecipeScriptableObject : ScriptableObject
{
    public List<ResourceScriptableObject> ingredients = new List<ResourceScriptableObject>();
    public GameObject fireworkToSpawn;
}
