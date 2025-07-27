using UnityEngine;
using System.Collections.Generic;

public class ResourceSpawner : MonoBehaviour
{
    Vector2 screenSize;

    private void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        foreach(List<ResourceScriptableObject> resList in GameManager.manager.finishedFireworks)
            foreach (ResourceScriptableObject res in resList)
            {
                if (res.name != "Blunder")
                    SpawnResource(res);
            }
    }

    void SpawnResource(ResourceScriptableObject res)
    {
        GameObject resource = new GameObject();

        LootableResource lootScript = resource.AddComponent<LootableResource>();
        SpriteRenderer resourceSpriteRenderer = resource.AddComponent<SpriteRenderer>();

        resourceSpriteRenderer.sprite = res.overrideSprite;
        lootScript.resourceType = res;

        resource.transform.position = new Vector2(Random.Range(-screenSize.x, screenSize.x), Random.Range(-screenSize.y, screenSize.y));
    }
}