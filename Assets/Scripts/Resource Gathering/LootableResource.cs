using UnityEngine;

public class LootableResource : MonoBehaviour
{

    public ResourceScriptableObject resourceType;
    public GameObject textPrefab;

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameManager.manager.gatheredResources.Add(resourceType);

            GameObject textObj = Instantiate(textPrefab, transform.position, Quaternion.identity);
            textObj.transform.parent = null;

            TextPopup textScript = textObj.GetComponent<TextPopup>();
            textScript.textDisplayed = "Collected " + resourceType.name + " x1";
            textScript.timeAlive = 1000;
            textScript.fontSize = 1;

            Destroy(gameObject);
        }
    }

}