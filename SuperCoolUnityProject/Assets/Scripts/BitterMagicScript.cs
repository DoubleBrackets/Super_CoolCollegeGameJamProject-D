using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitterMagicScript : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public float cooldownTime = 2;
    public float nextFireTime = 0;



    float lastshot;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(1))
            {

                StartCoroutine(Hydropump());

            }

        }
    }

    IEnumerator Hydropump() 
    {
        for (var i = 0; i < 4; i++)
        {

            nextFireTime = Time.time + cooldownTime;
            Shoot();
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1);
    
    
    }



    void Shoot()
    {
   
        firePoint.rotation = Quaternion.Euler(0, 0, PlayerInputScript.instance.vectorToMouseRaw.Angle());

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        //shooting logic

    }

}
