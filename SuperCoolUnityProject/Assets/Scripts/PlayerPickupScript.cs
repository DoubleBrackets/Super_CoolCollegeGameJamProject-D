using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/*Handles picking up and throwing Throwable objects*/
public class PlayerPickupScript : MonoBehaviour
{
    //Singleton ref
    public static PlayerPickupScript instance;
    /*Pickup casting fields*/
    public LayerMask pickupMask;
    public float pickupDistance;
    /*Throwing fields*/
    public float throwVelocity;
    /*Picked object fields*/
    public float holdDist = 2f;
    private Rigidbody2D pickedRb;
    private Vector2 relativePos;
    private bool isCarryingObject = false;
    /*Events*/
    public event System.Action ItemThrown;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Subscribes pickup to proper keypress
        PlayerInputScript.instance.PickupThrowButtonPressed += PickupThrow;
    }

    private void FixedUpdate()
    {
        //Update picked up object position, circles player following mouse
        if(isCarryingObject)
        {
            relativePos = holdDist * PlayerInputScript.instance.vectorToMouseNormalized;
            pickedRb.transform.position = Vector2.Lerp(pickedRb.transform.position, PlayerInputScript.instance.movementScript.collCenter + relativePos, 0.2f) ;
        }
    }

    void PickupThrow(Vector2 pos)
    {
        if (!isCarryingObject)
        {
            Pickup();
        }
        else
        {
            ItemThrown?.Invoke();
            Invoke("Throw", 0.25f);
        }
    }

    void Pickup()
    {
        int dir = PlayerInputScript.instance.facing;
        Vector2 center = GetComponent<Collider2D>().bounds.center;
        Vector2 size = new Vector2(0.1f, 1f);

        //Casts for Throwables
        RaycastHit2D hit = Physics2D.BoxCast(center, size, 0f, new Vector2(dir, 0), pickupDistance,pickupMask);

        if(hit.collider != null)
        {
            Rigidbody2D pickupRb = hit.collider.GetComponent<Rigidbody2D>();
            if (pickupRb == null)
                return;
            /*Picks up the object*/
            relativePos = pickupRb.transform.position - transform.position;
            isCarryingObject = true;
            pickedRb = pickupRb;
            pickedRb.isKinematic = true;
        }

    }

    void Throw()
    {
        pickedRb.isKinematic = false;
        pickedRb.velocity = throwVelocity * PlayerInputScript.instance.vectorToMouseNormalized;
        isCarryingObject = false;
    }



}
