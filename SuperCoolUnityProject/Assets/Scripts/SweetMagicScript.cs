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
    private LayerMask targetMask = 1 << 16;
    private float followThroughTime = 0.2f;
    /*Dash component fields*/
    public float dashVel;
    /*Visuals*/
    public ParticleSystem particles;
    public ParticleSystem dashParticles;
    /*Events*/
    public event System.Action<int,int,float> SweetMagicStrikeEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        instance = this;
    }
    private void Start()
    {
        PlayerInputScript.instance.SweetStrikeButtonPressed += SweetStrike;
        playerMoveScript = PlayerInputScript.instance.movementScript;
    }

    private void Update()
    {
        /*Timers*/
        cooldownTimer -= Time.deltaTime;
    }

    void SweetStrike(Vector2 dirNormalized)
    {
        if (cooldownTimer > 0 || !MagicManager.attackTimerCleared)
            return;
        MagicManager.SetStaticAttackTimer(windupTime);
        cooldownTimer = strikeCooldown;
        StartCoroutine(SweetStrikeCoroutine(dirNormalized));
        SweetMagicStrikeEvent?.Invoke(1,PlayerInputScript.instance.facing,windupTime+followThroughTime+0.2f);
    }

    private IEnumerator SweetStrikeCoroutine(Vector2 dirNormalized)
    {
        float horizontalInput = PlayerInputScript.instance.facing;
        float mouseDir = Mathf.Sign(PlayerInputScript.instance.vectorToMouseRaw.x);
        /*Dash*/
        rb.SetXVel(horizontalInput * dashVel);
        rb.SetYVel(0);
        playerMoveScript.frictionEnabled++;
        playerMoveScript.movementEnabled++;
        dashParticles.Play();
        for (int x = 1; x <= 5; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        playerMoveScript.frictionEnabled--;
        playerMoveScript.movementEnabled--;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        CineMachineImpulseManager.instance.Impulse(new Vector2(horizontalInput, 0) * -1f);
        yield return new WaitForSeconds(windupTime);
        /*Attack*/

        particles.SetParticleDirection(horizontalInput);

        particles.Play();
        CineMachineImpulseManager.instance.Impulse(new Vector2(horizontalInput,0) * 1.8f);
        /*Casting for targets*/
        Vector2 size = new Vector2(range, 1f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(playerMoveScript.collCenter + Vector2.right * horizontalInput * range / 2f, size, 0f, targetMask);
        foreach(Collider2D hitTarget in hits)
        {
            //Hit logic
            EnemyScript enemy = hitTarget.GetComponent<EnemyScript>();
            if(enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
        yield return new WaitForSeconds(followThroughTime);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.SetXVel(horizontalInput * playerMoveScript.maxVelocity);
    }
}
