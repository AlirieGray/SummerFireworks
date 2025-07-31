using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Monster : MonoBehaviour
{
    public SpriteRenderer sr;
    public Rigidbody2D rb;

    public Sprite[] monsterSprites;
    public SpriteAnimation[] animations;
    public int currentAnimation;
    public int currentFrame;

    public float wanderSpeed;
    public float chaseSpeed;

    public float wanderTime;
    public float wanderTimeRandom;

    public float sightRange;

    public float idleTime;
    public float idleTimeRandom;

    public float eatTime;
    public float eatRange;
    public bool eating;

    public monsterStates MonsterState = monsterStates.Idle;

    public Vector2 direction;

    GameObject[] resources;
    public GameObject closestResource = null;
    Animator anim;
    [System.Serializable] public class SpriteAnimation
    {
        public string animationName;
        public float animationSpeed;
        public int[] spriteID_Order;
    }

    public enum monsterStates
    {
        Idle,
        Wandering,
        Chasing,
        Eating
    }


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(StartIdle());
    }

    public void FixedUpdate()
    {
        if (rb.linearVelocity.x < 0)
            sr.flipX = true;
        if (rb.linearVelocity.x > 0)
            sr.flipX = false;

        if (MonsterState == monsterStates.Idle)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (MonsterState == monsterStates.Wandering)
        {
            WanderAI();
        }

        resources = GameObject.FindGameObjectsWithTag("LootableResource");
        float closestResourceDistance = Mathf.Infinity;

        foreach(GameObject res in resources)
        {
            float distance = Vector2.Distance(transform.position, res.transform.position);

            if(distance < closestResourceDistance)
            {
                closestResource = res;
                closestResourceDistance = distance;
            }
        }

        if(closestResourceDistance < sightRange)
        {
            MonsterState = monsterStates.Chasing;
            
            ChaseAI();
        }

        if(closestResourceDistance < eatRange && !eating)
        {
            MonsterState = monsterStates.Eating;
            rb.linearVelocity = Vector2.zero;
            if (!eating)
            {
                StopAllCoroutines();
                StartCoroutine(Eat());
            }
        }
    }
    public IEnumerator StartWander()
    {
        var screenBorder = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        if (transform.position.x > screenBorder.x ||
            transform.position.x < -screenBorder.x ||
            transform.position.y > screenBorder.y ||
            transform.position.y < -screenBorder.y
            )
        {
            //wander back into view
            var target = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2f, Screen.height/2f));
            direction = (target - transform.position).normalized;
        }
        else
        {
            direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        }

        MonsterState = monsterStates.Wandering;

        yield return new WaitForSeconds(wanderTime + Random.Range(-wanderTimeRandom, wanderTimeRandom));
        StartCoroutine(StartIdle());
    }

    void WanderAI()
    {
        if(!eating)
            rb.linearVelocity = direction * wanderSpeed;
    }

    public IEnumerator StartIdle()
    {
        direction = Vector2.zero;
        MonsterState = monsterStates.Idle;
        anim.SetTrigger("StartIdle");
        yield return new WaitForSeconds(idleTime + Random.Range(-idleTimeRandom, idleTimeRandom));
        StartCoroutine(StartWander());
        anim.SetTrigger("StartWalk");
    }

    public void ChaseAI()
    {
        if (!eating)
        {
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "MonsterWalk")
                anim.SetTrigger("StartWalk");
        StopAllCoroutines();
        direction = (closestResource.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
        }
    }

    public IEnumerator Eat()
    {
        //StopAllCoroutines();

        eating = true;

        anim.SetTrigger("StartEat");

        yield return new WaitForSeconds(eatTime);
        Debug.Log("finished eating");
        Destroy(closestResource);
        eating = false;
        StartCoroutine(StartIdle());
    }

}