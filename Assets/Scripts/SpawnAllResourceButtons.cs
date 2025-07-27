using UnityEngine;
using System.Collections.Generic;
public class SpawnAllResourceButtons : MonoBehaviour
{
    public GameObject drawerPrefab;

    public Vector3 startPosition;

    public Vector2 cellSize = Vector2.one;

    public int width = 2;

    public List<GameObject> spawnedPrefabs = new List<GameObject>();


    private void Update()
    {
        var left = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        transform.position = new Vector3(Mathf.Max(left.x + (2), -8), transform.position.y, 0);
        for (int i = 0; i < spawnedPrefabs.Count; i++)
        {
            spawnedPrefabs[i].transform.localPosition = startPosition + (new Vector3(cellSize.x * (Mathf.Repeat(i, width)), -cellSize.y * (Mathf.FloorToInt(i / 2))));
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int skippedAmount = 0;
        for(int i =0; i < GameManager.manager.resources.Count; i++)
        {
            ResourceScriptableObject res = GameManager.manager.resources[i];
            if (res.name == "Blunder")
            {
                skippedAmount++;
                continue;
            }
            var n = Instantiate(drawerPrefab);
            n.name = res.name;
            n.transform.SetParent(transform);
            n.transform.localPosition = startPosition + (new Vector3(cellSize.x * (Mathf.Repeat(i - skippedAmount, width)), -cellSize.y * (Mathf.FloorToInt((i - skippedAmount) / 2))));
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
        var local = startPosition + transform.position;
        Gizmos.DrawLine(local - Vector3.right, local + Vector3.right);
        Gizmos.DrawLine(local - Vector3.up, local + Vector3.up);
    }
}
