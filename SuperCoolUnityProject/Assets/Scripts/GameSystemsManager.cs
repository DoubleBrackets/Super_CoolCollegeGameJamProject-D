using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemsManager : MonoBehaviour
{
    public int objectsCollected;

    public void ObjectCollected()
    {
        objectsCollected++;
    }
}
