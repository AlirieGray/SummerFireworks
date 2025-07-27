using UnityEngine;
using System.Collections.Generic;
public class DisplayCompletedFireworks : MonoBehaviour
{
    List<List<ResourceScriptableObject>> fireworksDisplayed = new List<List<ResourceScriptableObject>>();

    List<RecipeInstance> recipeInstances = new List<RecipeInstance>();

    public static DisplayCompletedFireworks instance;

    public GameObject prefab;

    public Transform mostRecentObject;

    private void Awake()
    {
        instance = this;
        mostRecentObject = transform;
    }

    public void AddNewFirework(List<ResourceScriptableObject> newList)
    {
        fireworksDisplayed.Add(newList);
        var n = Instantiate(prefab, new Vector3(transform.position.x, 3.5f, 0), Quaternion.identity);
        n.transform.SetParent(mostRecentObject);
        
        n.transform.SetAsFirstSibling();
        if (mostRecentObject != transform)
        {
            //n.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            n.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -mostRecentObject.GetComponent<RectTransform>().sizeDelta.y);
        }
        else
        {
            
        }
        n.GetComponent<RecipeInstance>().ingredientsUsed = newList;
        recipeInstances.Add(n.GetComponent<RecipeInstance>());

        mostRecentObject = n.transform;


    }

    private void Update()
    {
        float total = 0;
        foreach (RecipeInstance ri in recipeInstances)
        {
            total += ri.background.rectTransform.sizeDelta.y;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, Mathf.Max(total, 7.25f));
    }
}
