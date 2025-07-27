using UnityEngine;

public class Cursor : MonoBehaviour
{
    void Update()
    {
        transform.position = GameManager.manager.CursorWorldPosition();
    }
}
