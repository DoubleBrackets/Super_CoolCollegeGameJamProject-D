using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    
    /*Layer tests*/
    public static bool IsInMask(this LayerMask mask, int layer)
    {
        return mask == (mask | 1 << layer);
    }

    /*Particle stuff*/
    public static void SetParticleDirection(this ParticleSystem particles,float dir)
    {
        dir = Mathf.Sign(dir);
        particles.transform.localScale = new Vector3(dir, 1, 1);
        int count = particles.transform.childCount;
        for (int x = 0; x < count; x++)
        {
            particles.transform.GetChild(x).localScale = new Vector3(dir, 1, 1);
        }
    }

    /*Rigidbody helpers*/

    public static void SetXVel(this Rigidbody2D v, float val)
    {
        v.velocity = new Vector2(val, v.velocity.y);
    }

    public static void SetYVel(this Rigidbody2D v, float val)
    {
        v.velocity = new Vector2(v.velocity.x, val);
    }

    public static Vector2 CalculateJumpArcPoint(this Rigidbody2D rb,Vector2 startVelocity,float fallRatio,float time)
    {
        float gravity = rb.gravityScale*-Physics2D.gravity.y;
        float x = startVelocity.x * time;
        float maxHTime = startVelocity.y / gravity;
        float y;

        if (time < maxHTime)
        {
            y = startVelocity.y * time - 0.5f * gravity * time * time;
        }
        else
        {
            y = startVelocity.y * maxHTime - 0.5f * gravity * maxHTime * maxHTime - 0.5f * gravity * fallRatio  * (time-maxHTime) * (time-maxHTime);
        }
        return new Vector2(x, y);
    }

    /*Vector2 helpers*/

    //Gets angle of a vector in degrees
    public static float Angle(this Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x)*Mathf.Rad2Deg;
    }

}
