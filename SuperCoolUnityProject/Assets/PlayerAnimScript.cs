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

    private void Update()
    {
        Vector2 vel = playerMoveScript.rbVelocity;
        float input = PlayerInputScript.instance.horizontalInput;
        //Flip to face direction
        if(input > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if(input < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        //Jump check
        anim.SetBool("IsGrounded", playerMoveScript.isGroundedRaw);
        //movement check
        anim.SetBool("IsMoving", Mathf.Abs(vel.x) > 0.5f);
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
