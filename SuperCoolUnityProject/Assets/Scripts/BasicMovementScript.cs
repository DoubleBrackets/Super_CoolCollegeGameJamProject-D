using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BasicMovementScript : MonoBehaviour
{
    /*Component references*/
    private Rigidbody2D rigidBody;
    public Collider2D _collider;
    public Vector2 collCenter{ get => _collider.bounds.center; }
    public Vector2 rbVelocity { get => rigidBody.velocity; }
    /*Gravity manipulation*/
    public float fallingGravityFactor;
    private float fallGravity;
    private float defaultGravity;
    /*Grounded check fields*/
    public LayerMask groundedCheckMask;
    private float groundedSizeOffset = 0.05f;
    private Vector2 groundedBoxcastSize;
    /*Movement fields*/
    public float maxVelocity;
    public float moveForce;
    public float frictionFactorGrounded;
    public float frictionFactorAir;
    private float accMagnitudePerPhysicsStep;
    /*Jump fields*/
    public float jumpVelocity;

    private float jumpStaticCooldown = 0.2f;
    private float jumpStaticCooldownTimer = 0f;
    /*Control fields*/
    [HideInInspector] public float horizontalInput;
    /*Movement state fields*/
    //Extra "grounded" time for easier jumps
    private float groundedBufferTime = 0.1f;
    private float groundedBufferTimer = 0f;
    private bool groundedRaw;
    public bool isGrounded { get => groundedBufferTimer > 0; }
    public bool isGroundedRaw { get => groundedRaw; }

    [HideInInspector] public int frictionEnabled = 0;
    [HideInInspector] public int movementEnabled = 0;
    private void Awake()
    {
        UpdateFields();
    }

    private void OnValidate()
    {
        UpdateFields();
    }

    private void UpdateFields()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        //Calculates the magnitude of the acceleration per fixed update
        accMagnitudePerPhysicsStep = moveForce / rigidBody.mass * Time.fixedDeltaTime;
        //Lower gravity when falling than when rising
        defaultGravity = rigidBody.gravityScale;
        fallGravity = defaultGravity * fallingGravityFactor;
        //Grounded boxcast uses collider bounds
        _collider = GetComponent<Collider2D>();
        groundedBoxcastSize = (Vector2)_collider.bounds.size - Vector2.up*groundedSizeOffset - Vector2.right*groundedSizeOffset;
    }

    private void Update()
    {
        /*Update timers*/
        jumpStaticCooldownTimer -= Time.deltaTime;
        groundedBufferTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        GroundedCheck();
        HorizontalMovement();
        //Gravity adjustment
        if(rigidBody.velocity.y >= 0)
        {
            rigidBody.gravityScale = defaultGravity;
        }
        else
        {
            rigidBody.gravityScale = fallGravity;
        }
    }

    private void GroundedCheck()
    {
        //Boxcasts down to check for a floor
        RaycastHit2D hit = Physics2D.BoxCast(_collider.bounds.center, groundedBoxcastSize, 0f, Vector2.down, groundedSizeOffset * 1f,groundedCheckMask);
        if(hit.collider != null)
        {
            groundedBufferTimer = groundedBufferTime;
            groundedRaw = true;
        }
        else
        {
            groundedRaw = false;
        }
    }
     
    private void HorizontalMovement()
    {
        float horizontalAcceleration = accMagnitudePerPhysicsStep * horizontalInput;
        float currentXVel = rigidBody.velocity.x;
        float xVelocityAfterAcceleration = currentXVel + horizontalAcceleration;
        bool hasForceBeenApplied = false;

        float currentXDir = Mathf.Sign(currentXVel);
        float movementXDir = Mathf.Sign(horizontalAcceleration);
        if(horizontalAcceleration != 0 && movementEnabled == 0)
        {
            //Movement only apply only if current velocity is under the max velocity or movement is opposite to the current velocity
            if ((currentXDir == movementXDir && Mathf.Abs(currentXVel) <= maxVelocity)  || currentXVel == 0)
            {
                //Apply force as acceleration
                if (Mathf.Abs(xVelocityAfterAcceleration) < maxVelocity)
                {
                   rigidBody.SetXVel(xVelocityAfterAcceleration);
                    hasForceBeenApplied = true;
                }
                //Goes over max speed, so set velocity to max speed directly
                else
                {
                    rigidBody.SetXVel(maxVelocity * movementXDir);
                    hasForceBeenApplied = true;
                }
            }
            else if( currentXDir != movementXDir)
            {
                //Apply force as acceleration
                rigidBody.SetXVel(xVelocityAfterAcceleration);
                //hasForceBeenApplied = true;
            }

        }

        //Horizontal friction
        if (!hasForceBeenApplied && frictionEnabled == 0)
        {
            if(isGrounded && jumpStaticCooldownTimer <= 0)
                rigidBody.SetXVel(rigidBody.velocity.x * frictionFactorGrounded);
            else
                rigidBody.SetXVel(rigidBody.velocity.x * frictionFactorAir);
        }

        
    }

    public void Jump()
    {
        if (jumpStaticCooldownTimer > 0 || !isGrounded)
            return;
        rigidBody.SetYVel(jumpVelocity);
        jumpStaticCooldownTimer = jumpStaticCooldown;
    }

}
//Custom movement editor, mostly for visualizing jumps and whatnot for level design
[CustomEditor(typeof(BasicMovementScript))]
public class BasicMovementScriptCustomEditor : Editor
{
    [DrawGizmo(GizmoType.Active)]
    private static void GizmoDraw(BasicMovementScript moveScript, GizmoType aGizmoType)
    {
        SimulateJump(moveScript);
    }
    
    private static void SimulateJump(BasicMovementScript moveScript)
    {
        float timeInterval = 0.1f;
        float timeDuration = 3f;
        int count = (int)(timeDuration / timeInterval);

        Rigidbody2D rb = moveScript.GetComponent<Rigidbody2D>();


        Vector2 startVel = new Vector2(moveScript.maxVelocity, moveScript.jumpVelocity);
        Vector2 prev = moveScript.transform.position;
        for (int x = 1; x <= count; x++)
        {
            Vector2 current = (Vector2)moveScript.transform.position + rb.CalculateJumpArcPoint(startVel, moveScript.fallingGravityFactor, x * timeInterval);
            Gizmos.DrawLine(prev, current);
            prev = current;
        }
    }
}
