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
            Destroy(gameObject);
        }
    }

}