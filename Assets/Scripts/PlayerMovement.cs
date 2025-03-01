

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum PlayerState { Idle,Running, Airbone}

    public Animator anim;

    PlayerState state;
    bool stateComplete;

    public Rigidbody2D rigid;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;

    public float acceleration;
    [Range(0, 1f)]
    public float groundDecay;
    public float maxXSpeed;
    public float jumpSpeed;


    public bool grounded;
    float xInput;
    float yInput;

    // Update is called once per frame
    void Update()
    {

        CheckInput();
        HandleJump();

        if (stateComplete)
        {
            SelectState();
        }

        UpdateState();

    }
    private void FixedUpdate()
    {
        CheckGround();
        ApplyFriction();
        HandleXMovement();


    }


    void SelectState()
    {
        stateComplete = false;

        if (grounded)
        {
            if (xInput == 0)
            {
                state = PlayerState.Idle;
                StartIdle();
            }
            else
            {
                state = PlayerState.Running;
                StartRunning();
            }
        }
        else
        {
            state = PlayerState.Airbone;
            StartAirbone();
        }
    }

    void UpdateState()
    {
        switch(state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Running:
                UpdateRunning();
                break;
            case PlayerState.Airbone:
                UpdateAirbone();
                break;
        }
    }

    void StartIdle()
    {
        anim.Play("Idle");
    }

    void StartRunning()
    {
        if(xInput > 0)
        {
            anim.Play("Running");
        }
        else if(xInput < 0)
        {
            anim.Play("RunningLeft");
        }
    }

    void StartAirbone()
    {
        anim.Play("Airbone");
    }

    void UpdateIdle()
    {
        if (xInput != 0 || !grounded)
        {
            stateComplete = true;
        }
    }

    void UpdateRunning()
    {

        float velX = rigid.velocity.x;
        anim.speed = Mathf.Abs(velX) / maxXSpeed;
        if (!grounded || Mathf.Abs (velX) < 0.1f)
        {
            stateComplete = true;
        }
    }

    void UpdateAirbone()
    {

        float time = Map(rigid.velocity.y, jumpSpeed, -jumpSpeed, 0, 1, true);
        anim.Play("Airbone", 0, time);
        anim.speed = 0;

        if (grounded)
        {
            stateComplete = true;
        }
    }

    void HandleXMovement()
    {
        if (Mathf.Abs(xInput) > 0)
        {
            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(rigid.velocity.x + increment, -maxXSpeed, maxXSpeed);
            rigid.velocity = new Vector2(newSpeed, rigid.velocity.y);


            FaceInput();

        }

    }

    void FaceInput()
    {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);

    }
    void HandleJump()
    {

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rigid.velocity = new Vector2(rigid.velocity.x,jumpSpeed);
        }
    }
    void ApplyFriction()
    {
        if (grounded && xInput == 0 && rigid.velocity.y <= 0)
        {
            rigid.velocity *= groundDecay;
        }
    }

    void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }


    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
    {
        float val = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        return clamp ? Mathf.Clamp(value, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : val;
    }
}
