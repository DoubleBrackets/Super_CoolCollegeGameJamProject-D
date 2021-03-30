using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshPro))]
public class DialogueScript : MonoBehaviour
{
    public static float textSpeed = 1.5f;
    [Multiline]
    public string displayText;

    private Coroutine currentCoroutine;
    private TextMeshPro textMesh;

    private bool isShowing = false;

    public float interactDistance = 1f;

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
        if(((Vector2)transform.position - playerPos).magnitude <= interactDistance)
        {
            if (isShowing)
                HideText();
            else
                ShowText(displayText);
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
            yield return new WaitForSeconds(0.1f / textSpeed);
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
        isShowing = false;
        int l = textMesh.text.Length;
        for(int x = l-1;x >= 0;x--)
        {
            textMesh.text = textMesh.text.Substring(0, x);
            yield return new WaitForSeconds(0.1f/textSpeed);
        }
        currentCoroutine = null;
    }
}
