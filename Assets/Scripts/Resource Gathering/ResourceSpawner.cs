using UnityEngine;
using System.Collections.Generic;

public class ResourceSpawner : MonoBehaviour
{
    Vector2 screenSize;
    public int extraSpawns = 3;

    private void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        foreach(List<ResourceScriptableObject> resList in GameManager.manager.finishedFireworks)
            foreach (ResourceScriptableObject res in resList)
            {
                if (res.name != "Blunder")
                    SpawnResource(res);
            }

        int randomSpawnAmount = 0;

        for (int i = GameManager.manager.score; i > 0; i -= 1000)
            randomSpawnAmount++;

        for(int i = randomSpawnAmount + extraSpawns; i > 0; i--)
        {
            ResourceScriptableObject rolledResource = GameManager.manager.resources[Random.Range(0, GameManager.manager.resources.Count)];
            while (rolledResource.name == "Blunder")
                rolledResource = GameManager.manager.resources[Random.Range(0, GameManager.manager.resources.Count)];

            SpawnResource(rolledResource);
        }

        SpawnResource(GameManager.manager.resourceNames["VolitileCrystals"]);
    }

    void SpawnResource(ResourceScriptableObject res)
    {
        GameObject resource = new GameObject();

        LootableResource lootScript = resource.AddComponent<LootableResource>();
        SpriteRenderer resourceSpriteRenderer = resource.AddComponent<SpriteRenderer>();

        resourceSpriteRenderer.sprite = res.overrideSprite;
        resourceSpriteRenderer.sortingLayerName = "Sprites";

        lootScript.resourceType = res;

        resource.transform.position = new Vector2(Random.Range(-screenSize.x, screenSize.x), Random.Range(-screenSize.y, screenSize.y));
    }
}