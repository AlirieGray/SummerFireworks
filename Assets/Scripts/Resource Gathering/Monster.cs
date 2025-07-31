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
    GameObject closestResource = null;

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

        StartCoroutine(StartIdle());
    }

    public void Update()
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

        if(closestResourceDistance < eatRange)
        {
            MonsterState = monsterStates.Eating;
            if (!eating)
                StartCoroutine(Eat());
        }
    }
    public IEnumerator StartWander()
    {
        direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        MonsterState = monsterStates.Wandering;

        yield return new WaitForSeconds(wanderTime + Random.Range(-wanderTimeRandom, wanderTimeRandom));
        StartCoroutine(StartIdle());
    }

    void WanderAI()
    {
        rb.linearVelocity = direction * wanderSpeed;
    }

    public IEnumerator StartIdle()
    {
        direction = Vector2.zero;
        MonsterState = monsterStates.Idle;

        yield return new WaitForSeconds(idleTime + Random.Range(-idleTimeRandom, idleTimeRandom));
        StartCoroutine(StartWander());
    }

    public void ChaseAI()
    {
        StopAllCoroutines();
        direction = Vector2.MoveTowards(transform.position, closestResource.transform.position, 1.0f);
        rb.linearVelocity = direction * chaseSpeed;
    }

    public IEnumerator Eat()
    {
        StopAllCoroutines();
        eating = true;

        yield return new WaitForSeconds(eatTime);

        Destroy(closestResource);
        eating = false;
        StartCoroutine(StartIdle());
    }

}