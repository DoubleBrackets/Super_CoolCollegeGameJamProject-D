using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    /*Component refs*/
    public BasicMovementScript playerMoveScript;
    private Animator anim;
    private Rigidbody2D rb;

    /*Anim logic fields*/
    private string currentAnim = "Idle";
    private int count;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        SweetMagicScript.instance.SweetMagicStrikeEvent += PlayAttackAnimation;
        SavorySprayScript.instance.SavorySprayEvent += PlayAttackAnimation;
        rb = playerMoveScript.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 vel = playerMoveScript.rbVelocity;
        float input = PlayerInputScript.instance.horizontalInput;
        //Flip to face direction
        if(rb.constraints != RigidbodyConstraints2D.FreezeAll && playerMoveScript.movementEnabled==0)
            transform.localScale = new Vector2(PlayerInputScript.instance.facing, 1);
        //Jump check
        anim.SetBool("IsGrounded", playerMoveScript.isGroundedRaw);
        //movement check
        anim.SetBool("IsMoving", Mathf.Abs(vel.x) > 0.5f);
        if (count > 0)
        {
            count--;
            if(count == 0)
                anim.SetBool("IsAttacking", false);
        }
    }

    void PlayAttackAnimation(int attackId,int facing)
    {
        anim.SetBool("IsAttacking", true);
        anim.SetInteger("AttackType", attackId);
        count = 5;
        transform.localScale = new Vector2(facing, 1);
    }

    void PlayAnim(string name)
    {
        if(!currentAnim.Equals(name))
        {
            anim.Play(name);
            currentAnim = name;
        }
    }
}
