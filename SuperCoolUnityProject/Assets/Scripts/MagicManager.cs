using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour
{
    private static float staticAttackTimer;
    public static bool attackTimerCleared { get=>staticAttackTimer <= 0;}

    void Update()
    {
        staticAttackTimer -= Time.deltaTime;
    }

    public static void SetStaticAttackTimer(float val)
    {
        if (staticAttackTimer < val)
            staticAttackTimer = val;
    }
}
