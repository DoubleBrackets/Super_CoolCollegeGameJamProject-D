using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    /*Rigidbody helpers*/

    public static void SetXVel(this Rigidbody2D v, float val)
    {
        v.velocity = new Vector2(val, v.velocity.y);
    }

    public static void SetYVel(this Rigidbody2D v, float val)
    {
        v.velocity = new Vector2(v.velocity.x, val);
    }

    /*Vector2 helpers*/

    //Gets angle of a vector in degrees
    public static float Angle(this Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x)*Mathf.Rad2Deg;
    }

}
