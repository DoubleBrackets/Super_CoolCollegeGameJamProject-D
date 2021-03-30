using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    //Singleton ref
    public static PlayerInputScript instance;
    /*Component refs*/
    private BasicMovementScript movementScript;
    /*Movement input fields*/
    [HideInInspector] public float horizontalInput;

    /*Keypress events*/

    public event System.Action<Vector2> InteractButtonPressed;

    private void Awake()
    {
        instance = this;
        movementScript = GetComponent<BasicMovementScript>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        movementScript.horizontalInput = horizontalInput;
        //Jump
        if(Input.GetKeyDown(KeyCode.Space))
        {
            movementScript.Jump();
        }
        //Interact
        if(Input.GetKeyDown(KeyCode.E))
        {
            InteractButtonPressed?.Invoke(transform.position);
        }
    }
}
