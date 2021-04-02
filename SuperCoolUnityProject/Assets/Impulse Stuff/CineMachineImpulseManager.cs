using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachineImpulseManager : MonoBehaviour
{
    //handles cinemachine camera effects
    public static CineMachineImpulseManager instance;
    CinemachineImpulseSource source;

    void Awake()
    {
        instance = this;
        source = GetComponent<CinemachineImpulseSource>();

    }

    public void Impulse(Vector3 velocity)
    {
        source.GenerateImpulse(Quaternion.Euler(0, 0, 45) * velocity);
    }
}