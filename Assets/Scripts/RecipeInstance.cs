using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecipeInstance : MonoBehaviour
{
    public TextMeshProUGUI label;

    public RecipeScriptableObject recipe;

    public List<ResourceScriptableObject> ingredientsUsed = new List<ResourceScriptableObject>();

    public Image background;

    public Image ingredientIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (recipe != null)
        {
            label.text = recipe.name;
            foreach(ResourceScriptableObject res in recipe.ingredients)
            {
                ingredientsUsed.Add(res);
            }
        }
        else
        {
            label.text = "Custom Firework";
        }

        var amountY = Mathf.Max(Mathf.CeilToInt((ingredientsUsed.Count * 1f) / 3f), 1);
        background.rectTransform.sizeDelta = new Vector3(3, (amountY) + 0.25f, 1);
        Debug.Log(amountY + " rows of ingredients used");
        //background.transform.localPosition = new Vector3(-0.25f,0.125f - ((amountY - 1) / 2f),0);

        ingredientIcon.color = Color.white;
        //GameManager.manager.MixColor(Color.yellow, Color.green);

        for (int i =0; i < ingredientsUsed.Count; i++)
        {
            var n = Instantiate(ingredientIcon.gameObject);
            n.SetActive(true);
            n.transform.SetParent(ingredientIcon.transform.parent);

            n.GetComponent<Image>().sprite = ingredientsUsed[i].overrideSprite;

            var y = Mathf.CeilToInt((i * 1f) / 3f);
            var x = Mathf.Repeat(i, 3);

            n.transform.localPosition = new Vector3(-1.25f + x, -(y));
        }

    }

}
