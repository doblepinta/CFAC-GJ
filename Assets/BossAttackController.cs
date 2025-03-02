using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{

    public CircleCollider2D groundCheck;
    public LayerMask groundMask;


    public CircleCollider2D playerCheck;
    public LayerMask playerMask;

    // Start is called before the first frame update
    void Start()
    {
        playerCheck = this.GetComponent<CircleCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        CheckGround();
        CheckPlayer();
    }

    void CheckGround()
    {
        if (Physics2D.OverlapCircleAll(transform.position, groundCheck.radius, groundMask).Length > 0)
        {
            Destroy(this.gameObject);
        }
    }
    void CheckPlayer()
    {
        if (Physics2D.OverlapCircleAll(transform.position, playerCheck.radius, playerMask).Length > 0)
        {
            //Damage to player
            Destroy(this.gameObject);
        }
    }


}
