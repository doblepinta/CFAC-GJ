using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    enum PlayerState { Idle, Chase, Attack }

    public Animator anim;
    public Transform player;

    PlayerState state;
    bool stateComplete;


    public float speed;
    public float attackSpeed;

    public float rangeChase;
    public float rangeAttack;

    public bool onRangeChase;
    public bool onRangeAttack;

    public int index;

    public GameObject attack1Prefab;
    public bool canAttack1;
    public Transform attack1Holder;


    // Update is called once per frame
    void Update()
    {

        if (stateComplete)
        {
            SelectState();
        }

        UpdateState();

    }
    private void FixedUpdate()
    {
        HandleMovement();
        SelectState();
    }
    void HandleMovement()
    {

        if (!canAttack1)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2 (player.transform.position.x, transform.position.y), speed * 0.5f * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, BossWaypoints.route[index].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, BossWaypoints.route[0].position) < 0.05f)
            {
                index = 1;
            }
            else if (Vector2.Distance(transform.position, BossWaypoints.route[1].position) < 0.05f)
            {
                index = 0;
            }
        }
    }

    void SelectState()
    {
        stateComplete = false;

        if (Vector2.Distance (transform.position, player.position) <= rangeChase)
        {
            if (Vector2.Distance(transform.position, player.position) <= rangeAttack)
            {
                state = PlayerState.Attack;
                StartAttack();
            }
            else
            {
                state = PlayerState.Chase;
                StartChasing();
            }
        }
        else
        {
            state = PlayerState.Idle;
            StartIdle();
        }
    }

    void UpdateState()
    {
        switch (state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Chase:
                UpdateChasing();
                break;
            case PlayerState.Attack:
                UpdateAttacking();
                break;
        }
    }

    void StartIdle()
    {
        anim.Play("Idle");
    }

    void StartChasing()
    {
        anim.Play("Chase");
    }

    void StartAttack()
    {
        if (canAttack1)
        {
            StartCoroutine("Attack1Coroutine");
        }
        anim.Play("Attack");
    }

    void UpdateIdle()
    {
        onRangeAttack = false;
        onRangeChase = false;
        stateComplete = true;
    }

    void UpdateChasing()
    {
        onRangeChase = true;
        stateComplete = true;
    }

    void UpdateAttacking()
    {
        onRangeAttack = true;
        stateComplete = true;
    }


    public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
    {
        float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        return clamp ? Mathf.Clamp(value, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
        Gizmos.DrawWireSphere(transform.position, rangeChase);
    }

    IEnumerator Attack1Coroutine()
    {
        canAttack1 = false;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 10; i++)
        {
            GameObject attack1 = (GameObject)Instantiate(attack1Prefab, transform.position, transform.rotation, attack1Holder);
            yield return new WaitForSeconds(1f);
        }
        canAttack1 = true;
    }
}
