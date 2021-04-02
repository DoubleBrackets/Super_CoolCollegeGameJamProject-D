using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavorySprayScript : MonoBehaviour
{
    //Singleton
    public static SavorySprayScript instance;
    /*Component refs*/
    public ParticleSystem part;
    private BasicMovementScript playerMovementScript;
    private Rigidbody2D rb;
    /*Spray fields*/
    public float attackCooldown;
    public float windupTime;
    private float attackCooldownTimer = 0f;
    public LayerMask targetMask;
    /*Events*/
    public event System.Action<int,int> SavorySprayEvent;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        PlayerInputScript.instance.SavorySprayButtonPressed += Spray;
        playerMovementScript = PlayerInputScript.instance.movementScript;
        rb = playerMovementScript.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
    }

    void Spray(Vector2 dirNormalized)
    {
        if (attackCooldownTimer > 0)
            return;
        attackCooldownTimer = attackCooldown;
        transform.rotation = Quaternion.Euler(0, 0, dirNormalized.Angle());
        StartCoroutine(SprayCoroutine());
        SavorySprayEvent?.Invoke(2,(int)Mathf.Sign(dirNormalized.x));
    }

    private IEnumerator SprayCoroutine()
    {
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        playerMovementScript.movementEnabled++;
        CineMachineImpulseManager.instance.Impulse(Vector2.up * -0.5f);
        yield return new WaitForSeconds(windupTime);
        part.Play();
        CineMachineImpulseManager.instance.Impulse(Vector2.up * 1f);
        yield return new WaitForSeconds(0.3f);
        playerMovementScript.movementEnabled--;
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }    


    void OnParticleCollision(GameObject other)
    {
        if(targetMask.IsInMask(other.layer))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            /*Damage logic*/
            
        }
    }
}
