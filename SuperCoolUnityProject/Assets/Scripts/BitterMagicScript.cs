using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitterMagicScript : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float cooldownTime = 2;
    public float nextFireTime = 0;

    public PlayerAnimScript bitterSprayAnim;

    public ParticleSystem part;

    public LayerMask targetMask;

    float lastshot;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 dir = PlayerInputScript.instance.vectorToMouseRaw;
                int facing = (int) Mathf.Sign(dir.x);
                float angle = dir.Angle();
                transform.rotation = Quaternion.Euler(0, 0, angle);

                StartCoroutine(Hydropump());

                bitterSprayAnim.PlayAttackAnimation(3, facing, 0.75f);

            }

        }
    }

    IEnumerator Hydropump() 
    {
        for (var i = 0; i < 4; i++)
        {

            nextFireTime = Time.time + cooldownTime;
            Shoot();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1);
    
    
    }



    void Shoot()
    {
   
        firePoint.rotation = Quaternion.Euler(0, 0, PlayerInputScript.instance.vectorToMouseRaw.Angle());

        part.Play();
        
        //Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        //shooting logic

    }

    void OnParticleCollision(GameObject other)
    {
        if (targetMask.IsInMask(other.layer))
        {
            /*Damage logic*/
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy != null)
                Destroy(enemy.gameObject);
        }
    }

}
