using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private bool walking;
    private bool eating;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("velocity", 0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetBool("eating", true);
            animator.SetFloat("velocity", 0f);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {

            animator.SetFloat("velocity", 1f);
            animator.SetBool("eating", false);
        }
    }
}
