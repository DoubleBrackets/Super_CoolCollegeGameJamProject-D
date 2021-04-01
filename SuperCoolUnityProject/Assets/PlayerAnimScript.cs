using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : MonoBehaviour
{
    /*Component refs*/
    public BasicMovementScript playerMoveScript;
    private Animator anim;

    /*Anim logic fields*/
    private string currentAnim = "Idle";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //PlayerPickupScript.instance.ItemThrown += PlayThrowAnimation;
    }

    private void Update()
    {
        Vector2 vel = playerMoveScript.rbVelocity;
        float input = PlayerInputScript.instance.horizontalInput;
        //Flip to face direction
        transform.localScale = new Vector2(PlayerInputScript.instance.facing, 1);
        //Jump check
        anim.SetBool("IsGrounded", playerMoveScript.isGroundedRaw);
        //movement check
        anim.SetBool("IsMoving", Mathf.Abs(vel.x) > 0.5f);
        anim.SetBool("IsAttacking", false);
    }

    void PlayThrowAnimation()
    {
        anim.SetBool("IsAttacking", true);
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
