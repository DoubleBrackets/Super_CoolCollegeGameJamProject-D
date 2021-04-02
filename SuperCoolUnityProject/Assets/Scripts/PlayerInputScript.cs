using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    public static Camera mainCamera;
    //Singleton ref
    public static PlayerInputScript instance;
    /*Component refs*/
    public BasicMovementScript movementScript;
    /*Movement input fields*/
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public int facing = 1;
    /*Mouse position fields*/
    [HideInInspector] public Vector2 mousePosScreen;
    [HideInInspector] public Vector2 mousePosWorld;
    [HideInInspector] public Vector2 vectorToMouseRaw;
    [HideInInspector] public Vector2 vectorToMouseNormalized;
    /*Keypress events*/

    public event System.Action<Vector2> InteractButtonPressed;
    public event System.Action<Vector2> AttackButtonPressed;

    private void Awake()
    {
        instance = this;
        movementScript = GetComponent<BasicMovementScript>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0)
        {
            facing = (int)horizontalInput;
        }

        movementScript.horizontalInput = horizontalInput;
        //Mouseposition
        mousePosScreen = Input.mousePosition;
        mousePosWorld = mainCamera.ScreenToWorldPoint(mousePosScreen);
        vectorToMouseRaw = mousePosWorld - movementScript.collCenter;
        vectorToMouseNormalized = vectorToMouseRaw.normalized;
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
        if(Input.GetMouseButton(0))
        {
            AttackButtonPressed?.Invoke(vectorToMouseNormalized);
        }
    }
}
