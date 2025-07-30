using UnityEngine;
using System.Collections.Generic;

public class ResourceSpawner : MonoBehaviour
{
    Vector2 screenSize;
    public int extraSpawns = 3;

    public GameObject textPrefab;

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

        SpawnResource(GameManager.manager.resourceNames["Volatile Crystals"]);
    }

    void SpawnResource(ResourceScriptableObject res)
    {
        GameObject resource = new GameObject();

        LootableResource lootScript = resource.AddComponent<LootableResource>();
        SpriteRenderer resourceSpriteRenderer = resource.AddComponent<SpriteRenderer>();
        BoxCollider2D bc = resource.AddComponent<BoxCollider2D>();
        Rigidbody2D rb = resource.AddComponent<Rigidbody2D>();

        resource.name = res.name;

        resourceSpriteRenderer.sprite = res.overrideSprite;
        resourceSpriteRenderer.sortingLayerName = "Sprites";

        lootScript.resourceType = res;
        lootScript.textPrefab = textPrefab;

        bc.isTrigger = true;
        bc.size = Vector2.one;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.bodyType = RigidbodyType2D.Static;

        resource.transform.position = new Vector2(Random.Range(-screenSize.x, screenSize.x), Random.Range(-screenSize.y, screenSize.y));
    }
}