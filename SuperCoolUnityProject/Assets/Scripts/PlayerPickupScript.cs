using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/*Handles picking up and throwing Throwable objects*/
public class PlayerPickupScript : MonoBehaviour
{
    //Singleton ref
    public static PlayerPickupScript instance;
    /*Component refs*/
    private Collider2D playerCollider;
    /*Pickup casting fields*/
    public LayerMask pickupMask;
    public float pickupDistance;
    /*Throwing fields*/
    //Static action cooldown
    public float throwVelocity;
    private float throwCooldown = 0.2f;
    private float throwTimer = 0f;
    /*Picked object fields*/
    public float holdDist = 2f;
    public int maxObjects;

    List<PickedObject> pickedObjects;
    int count = 0;
    /*Events*/
    public event System.Action ItemThrown;

    struct PickedObject
    {
        public Rigidbody2D pickedRb;
        public Collider2D pickedColl;
    }


    private void Awake()
    {
        instance = this;
        playerCollider = GetComponent<Collider2D>();
        pickedObjects = new List<PickedObject>();
    }

    private void Start()
    {
        //Subscribes pickup to proper keypress
        PlayerInputScript.instance.PickupThrowButtonPressed += PickupThrow;
    }

    private void Update()
    {
        /*Timer updates*/
        throwTimer -= Time.deltaTime;
        //Update picked up objects position, circles player following mouse
        if(count != 0)
        {
            int index = 0;
            float angleOffset = 360f / count;
            foreach (PickedObject obj in pickedObjects)
            {
                float targetAngle = index*angleOffset + PlayerInputScript.instance.vectorToMouseNormalized.Angle();
                float cAngle = ((Vector2)(obj.pickedRb.transform.position - playerCollider.bounds.center)).Angle();
                float diff = Mathf.DeltaAngle(cAngle, targetAngle);
                float newAngle = Mathf.Lerp(cAngle, cAngle + diff, 7.5f * Time.deltaTime);
                obj.pickedRb.transform.position = playerCollider.bounds.center + Quaternion.Euler(0, 0, newAngle) * Vector2.right * holdDist;
                index++;
            }
        }
 
    }

    void PickupThrow(Vector2 pos)
    {
        bool objectFound = false;
        if (count < maxObjects && throwTimer <= 0)
        {
            objectFound = Pickup();
            if(objectFound)
            {
                throwTimer = throwCooldown;
            }
        }
        //Throw if no spaces left or no object found to pick up
        if(!objectFound && count > 0 && throwTimer <= 0)
        {
            ItemThrown?.Invoke();
            Throw(pickedObjects[0]);
        }
    }

    bool Pickup()
    {
        int dir = PlayerInputScript.instance.facing;
        Vector2 center = GetComponent<Collider2D>().bounds.center;
        Vector2 size = new Vector2(0.1f, 1f);

        //Casts for Throwables
        RaycastHit2D hit = Physics2D.BoxCast(center, size, 0f, new Vector2(dir, 0), pickupDistance,pickupMask);

        if(hit.collider != null)
        {
            Rigidbody2D pickupRb = hit.collider.GetComponent<Rigidbody2D>();
            if (pickupRb == null || pickupRb.isKinematic)
                return false;
            /*Picks up the object*/
            var newObject = new PickedObject { pickedColl = hit.collider, pickedRb = pickupRb };
            count++;
            //Disables rb and collider of picked object
            newObject.pickedRb.isKinematic = true;
            Physics2D.IgnoreCollision(newObject.pickedColl, playerCollider, true);
            newObject.pickedColl.enabled = false;
            pickedObjects.Add(newObject);

            return true;
        }
        return false;

    }

    void Throw(PickedObject obj)
    {
        StartCoroutine(ThrowCoroutine(obj, PlayerInputScript.instance.vectorToMouseNormalized));
        throwTimer = throwCooldown;
    }

    private IEnumerator ThrowCoroutine(PickedObject obj,Vector2 dirNormalized)
    {
        yield return new WaitForSeconds(0.15f);
        //Throw object
        obj.pickedRb.transform.position = (Vector2)playerCollider.bounds.center + dirNormalized * holdDist;
        obj.pickedRb.velocity = throwVelocity * dirNormalized;
        pickedObjects.Remove(obj);
        count--;
        //Reenables rb and collider
        obj.pickedRb.isKinematic = false;
        obj.pickedColl.enabled = true;
        Physics2D.IgnoreCollision(obj.pickedColl, playerCollider, false);
    }

}
