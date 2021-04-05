 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    
    [HideInInspector]
    public bool mustPatrol;
    private bool mustTurn;
    public bool melee;

    [HideInInspector]
    public bool playerInRange;

    public float walkSpeed, range;
    private float distToPlayer;
    public float cooldownTime;

    public float turnCD = 0.5f;

    private float turnCDtimer = 0;

    [HideInInspector]
    public float nextFireTime = 0;

    public Rigidbody2D rb;
    public Transform groundCheckPos;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;
    public Collider2D bodyCollider, playerCollider;
    public Transform player, firePoint;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        mustPatrol = true;
    }

    // Update is called once per frame
    void Update()
    {
        turnCDtimer -= Time.deltaTime;

        if (mustPatrol)
        {
            Patrol();
        }

        distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer <= range)
        {
            playerInRange = true;
            
            if ((player.position.x > transform.position.x && transform.localScale.x < 0) || (player.position.x < transform.position.x && transform.localScale.x > 0))
            {
                Flip();
            }

            mustPatrol = false;
            rb.velocity = Vector2.zero;

            if (melee)
            {
                Attack();
            }
            
            else
            {
                Shoot();
            }     
        }

        else
        {
            mustPatrol = true;
            playerInRange = false;
        }

        if (bodyCollider.IsTouching(playerCollider))
        {
            print("Player death");
        }

    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
        }
    }

    void Patrol()
    {
        if (mustTurn || bodyCollider.IsTouchingLayers(wallLayer))
        {
            Flip();
        }

        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        if (turnCDtimer <= 0)
        {
            turnCDtimer = turnCD;

            mustPatrol = false;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            walkSpeed *= -1;
            mustPatrol = true;
        }
    }

    void Shoot()
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + cooldownTime;

            float angle = ((Vector2)(player.position - transform.position)).Angle();
            firePoint.rotation = Quaternion.Euler(0,0,angle);

            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    void Attack()
    {
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);

        if (bodyCollider.IsTouching(playerCollider))
        {
            rb.velocity = Vector2.zero;
        }
    }

}
