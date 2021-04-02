using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetMagicScript : MonoBehaviour
{
    //Short dash and strike
    public static SweetMagicScript instance;
    /*Component refs*/
    private Rigidbody2D rb;
    private BasicMovementScript playerMoveScript;
    /*Attack fields*/
    public float strikeCooldown;
    public float range;
    private float cooldownTimer = 0f;
    public float windupTime;
    private LayerMask targetMask = 1 << 12;
    /*Dash component fields*/
    public float dashVel;
    /*Visuals*/
    public ParticleSystem particles;
    /*Events*/
    public event System.Action SweetMagicStrikeEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        instance = this;
    }
    private void Start()
    {
        PlayerInputScript.instance.AttackButtonPressed += SweetStrike;
        playerMoveScript = PlayerInputScript.instance.movementScript;
    }

    private void Update()
    {
        /*Timers*/
        cooldownTimer -= Time.deltaTime;
    }

    void SweetStrike(Vector2 dirNormalized)
    {
        if (cooldownTimer > 0)
            return;
        cooldownTimer = strikeCooldown;
        StartCoroutine(SweetStrikeCoroutine(dirNormalized));
        SweetMagicStrikeEvent?.Invoke();
    }

    private IEnumerator SweetStrikeCoroutine(Vector2 dirNormalized)
    {
        float horizontalInput = PlayerInputScript.instance.facing;
        float mouseDir = Mathf.Sign(PlayerInputScript.instance.vectorToMouseRaw.x);
        /*Dash*/
        rb.SetXVel(horizontalInput * dashVel);
        playerMoveScript.frictionEnabled++;
        playerMoveScript.movementEnabled++;
        for (int x = 1; x <= 3; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        particles.Stop();
        playerMoveScript.frictionEnabled--;
        playerMoveScript.movementEnabled--;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(windupTime);
        /*Attack*/
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        particles.transform.localScale = new Vector2(horizontalInput, 1);
        particles.Play();
        CineMachineImpulseManager.instance.Impulse(new Vector2(horizontalInput,0) * 2f);
        /*Casting for targets*/
        Vector2 size = new Vector2(range, 1f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(playerMoveScript.collCenter + Vector2.right * horizontalInput * range / 2f, size, 0f, targetMask);
        foreach(Collider2D hitTarget in hits)
        {
            //Hit logic
        }

    }
}
