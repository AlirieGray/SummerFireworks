using UnityEngine;

public class Mouse : MonoBehaviour
{
    void Update()
    {
        transform.position = GameManager.manager.CursorWorldPosition();
    }
}
