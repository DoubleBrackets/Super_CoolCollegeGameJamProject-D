using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovingPlatformScript : MonoBehaviour
{
    //Movement points
    public Vector2[] positions;
    private float[] times;

    private Dictionary<Rigidbody2D,Transform> attachedRbs;

    public float speed;

    private int size;
    private float floorYLocal;
    /*Moving physics fields*/
    private int currentPoint = 0;
    private int nextPoint = 1;
    private float currentTimer= 0f;

    private void Awake()
    {
        size = positions.Length;
        times = new float[size];
        if (size < 2)
            this.enabled = false;
        attachedRbs = new Dictionary<Rigidbody2D, Transform>();
        for(int x = 0;x < size;x++)
        {
            times[x] = (positions[(x + 1) % size] - positions[x]).magnitude / speed;
        }
        Collider2D coll = GetComponent<Collider2D>();
        floorYLocal = coll.bounds.center.y + coll.bounds.extents.y - transform.position.y;
    }

    private void FixedUpdate()
    {
        currentTimer += Time.fixedDeltaTime;
        transform.position = Vector2.Lerp(positions[currentPoint],positions[nextPoint],currentTimer/times[currentPoint]);
        if(currentTimer > times[currentPoint])
        {
            MoveToNextPoint();
        }
    }

    void MoveToNextPoint()
    {
        currentPoint = (currentPoint + 1) % size;
        nextPoint = (currentPoint + 1) % size;
        currentTimer = 0f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if(rb!= null && GetLowerY(rb) > transform.position.y + floorYLocal)
        {
            attachedRbs.Add(rb, rb.transform.parent);
            rb.transform.SetParent(transform);
        }
    }

    float GetLowerY(Rigidbody2D rb)
    {
        Collider2D coll = rb.GetComponent<Collider2D>();
        return coll.bounds.center.y - coll.bounds.extents.y;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Transform returnTransform;
            bool valueFound = attachedRbs.TryGetValue(rb, out returnTransform);
            if(valueFound)
            {
                rb.transform.SetParent(returnTransform);
                attachedRbs.Remove(rb);
            }
        }
    }
}

[CustomEditor(typeof(MovingPlatformScript))]

public class MovingPlatformScriptEditor : Editor
{
    private void OnSceneGUI()
    {
        SerializedObject obj = new SerializedObject(target);
        SerializedProperty points = obj.FindProperty("positions");
        int len = points.arraySize;
        for(int x = 0;x < len;x++)
        {
            //Creates handles for each point
            SerializedProperty arrayElementProperty = points.GetArrayElementAtIndex(x);
            arrayElementProperty.vector2Value = Handles.PositionHandle(arrayElementProperty.vector2Value, Quaternion.identity);
            Handles.Label(arrayElementProperty.vector2Value, "Position" + (x + 1));
        }
        obj.ApplyModifiedProperties();
    }
}