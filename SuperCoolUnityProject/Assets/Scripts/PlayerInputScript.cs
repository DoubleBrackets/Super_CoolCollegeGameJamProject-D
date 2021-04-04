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
    public event System.Action<Vector2> SweetStrikeButtonPressed;
    public event System.Action<Vector2> SavorySprayButtonPressed;
    /*Platform dropdown*/
    private float timer;
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
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            movementScript.Jump();
        }
        //Drop down
        if(Input.GetKeyDown(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(14, 13,true);
            transform.position += Vector3.up * 0.01f;
            timer = 0.2f;
        }
        else if(timer > 0 && !Input.GetKey(KeyCode.S))
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
                Physics2D.IgnoreLayerCollision(14, 13, false);
        }

        //Interact
        if(Input.GetKeyDown(KeyCode.E))
        {
            InteractButtonPressed?.Invoke(transform.position);
        }
        if(Input.GetMouseButton(0))
        {
            SweetStrikeButtonPressed?.Invoke(vectorToMouseNormalized);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            SavorySprayButtonPressed?.Invoke(vectorToMouseNormalized);
        }
    }
}
