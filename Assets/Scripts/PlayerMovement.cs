

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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

    }
    private void FixedUpdate()
    {
        CheckGround();
        ApplyFriction();
        HandleXMovement();


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
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }


    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }
}
