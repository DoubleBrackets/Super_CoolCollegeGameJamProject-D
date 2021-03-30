using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEditor;

[RequireComponent(typeof(TextMeshPro))]
public class DialogueScript : MonoBehaviour
{
    /*Dialogue fields*/
    //public static float textSpeed = 1.5f;
    [Multiline] public string[] displayText;
    public float interactDistance;
    private bool isShowing = false;
    //Page tracker
    private int currentIndex = 0;
    /*Component&Coroutine ref*/
    private Coroutine currentCoroutine;
    private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        PlayerInputScript.instance.InteractButtonPressed += OnInteract;
    }

    private void OnInteract(Vector2 playerPos)
    {
        if(((Vector2)transform.position - playerPos).sqrMagnitude <= interactDistance*interactDistance)
        {
            if (isShowing)
                HideText();
            else
            {
                ShowText(displayText[0]);
                currentIndex=1;
            }
        }
    }

    private void ShowText(string text)
    {
        if(currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(ShowTextCoroutine(text));
        }
    }

    IEnumerator ShowTextCoroutine(string text)
    {
        isShowing = true;
        int len = text.Length;
        for(int x = 0;x <= len;x++)
        {
            textMesh.text = text.Substring(0, x);
            yield return new WaitForFixedUpdate();
        }
        currentCoroutine = null;
    }

    private void HideText()
    {
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(HideTextCoroutine());
        }
    }

    IEnumerator HideTextCoroutine()
    {
        int l = textMesh.text.Length;
        for(int x = l-1;x >= 0;x--)
        {
            textMesh.text = textMesh.text.Substring(0, x);
            yield return new WaitForFixedUpdate();
        }
        int index = currentIndex % displayText.Length;
        if(index == 0)//Back to page 1, dont display new pages
        {
            isShowing = false;
        }
        else//Otherwise display next page
        {
            yield return ShowTextCoroutine(displayText[index]);
            currentIndex++;
        }
        currentCoroutine = null;
    }
}

[CustomEditor(typeof(DialogueScript))]
class DialogueEditor : Editor
{
    SerializedProperty interactDist;

    private void OnSceneGUI()
    {
        SerializedObject script = new SerializedObject(target);
        Vector2 pos = (target as DialogueScript).transform.position;
        interactDist = script.FindProperty("interactDistance");

        interactDist.floatValue = Handles.RadiusHandle(Quaternion.identity,pos, interactDist.floatValue);
        Handles.Label(pos, "Interact distance");
        script.ApplyModifiedProperties();
    }
}
