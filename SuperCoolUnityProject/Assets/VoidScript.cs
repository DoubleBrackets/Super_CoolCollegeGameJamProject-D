using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class VoidScript : MonoBehaviour
{
    public Vector2 returnPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerInputScript>() != null)
        {
            collision.transform.position = returnPos;
        }
        else if(collision.GetComponent<EnemyScript>() != null)
        {
            Destroy(collision.gameObject);
        }
    }
}

/*[CustomEditor(typeof(VoidScript))]
 class VoidScriptEditor : Editor
{
    private void OnSceneGUI()
    {
        SerializedObject obj = new SerializedObject(target);
        SerializedProperty property = obj.FindProperty("returnPos");
        property.vector2Value = Handles.PositionHandle(property.vector2Value, Quaternion.identity);
        Handles.Label(property.vector2Value, "RespawnPosition");
        obj.ApplyModifiedProperties();
    }
}*/
