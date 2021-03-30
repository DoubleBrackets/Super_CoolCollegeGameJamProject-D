using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TestCinemachineScript : MonoBehaviour
{
    public CinemachineVirtualCamera v1;
    public CinemachineVirtualCamera v2;

    bool toggle = false;
    private void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            toggle = !toggle;
            v1.enabled = toggle;
            v2.enabled = !toggle;
        }
    }
}
