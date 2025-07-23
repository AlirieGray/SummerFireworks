using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;


    void FixedUpdate()
    {
        Vector2 inputDir = Vector2.zero;
        inputDir.x = Input.GetAxisRaw("Horizontal");
        inputDir.y = Input.GetAxisRaw("Vertical");

        inputDir = inputDir.normalized;

        transform.position += new Vector3(inputDir.x, inputDir.y) * Time.fixedDeltaTime * speed;
    }
}
