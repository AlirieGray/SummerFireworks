using UnityEngine;
using System.Collections.Generic;
public class SpawnAllResourceButtons : MonoBehaviour
{
    public GameObject drawerPrefab;

    public Vector3 startPosition;

    public Vector2 cellSize = Vector2.one;

    public int width = 2;

    public List<GameObject> spawnedPrefabs = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i =0; i < GameManager.manager.resources.Count; i++)
        {
            ResourceScriptableObject res = GameManager.manager.resources[i];
            if (res.name == "Blunder")
                continue;
            var n = Instantiate(drawerPrefab);
            n.name = res.name;
            n.transform.position = startPosition + (new Vector3(cellSize.x * (Mathf.Repeat(i, width)), -cellSize.y * (Mathf.FloorToInt(i / 2))));
            spawnedPrefabs.Add(n);

            n.GetComponent<DraggableResource>().resource = res;

            var img = n.GetComponent<SpriteRenderer>();
            if(res.overrideSprite!= null)
            {
                img.sprite = res.overrideSprite;
                img.color = Color.white;
            }
            else
                img.color = res.color;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPosition - Vector3.right, startPosition + Vector3.right);
        Gizmos.DrawLine(startPosition - Vector3.up, startPosition + Vector3.up);
    }
}
