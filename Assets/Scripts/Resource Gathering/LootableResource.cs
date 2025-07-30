using UnityEngine;

public class LootableResource : MonoBehaviour
{

    public ResourceScriptableObject resourceType;
    public GameObject textPrefab;

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameManager.manager.resDict[resourceType]++;

            GameObject textObj = Instantiate(textPrefab, transform.position, Quaternion.identity);
            textObj.transform.parent = null;

            TextPopup textScript = textObj.GetComponent<TextPopup>();
            textScript.textDisplayed = "Collected <color=#" + ColorUtility.ToHtmlStringRGB(resourceType.color) + ">" + resourceType.name + "</color> x1";
            textScript.timeAlive = 100;
            textScript.fontSize = 3;

            Destroy(gameObject);
        }
    }

}