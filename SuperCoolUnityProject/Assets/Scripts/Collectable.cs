using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//public class CollectedEvent : UnityEvent<> { }

[RequireComponent(typeof(Collider2D))]
public class Collectable : MonoBehaviour
{
    public UnityEvent myEvent;
    public LayerMask targetMask;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collected)
            return;
        if(targetMask.IsInMask(collision.gameObject.layer))
        {
            myEvent?.Invoke();
            collected = true;
        }
    }
}
