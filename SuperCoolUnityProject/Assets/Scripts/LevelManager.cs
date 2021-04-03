using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    //Singleton ref
    public static LevelManager instance;
    /*Component refs*/
    public RawImage fade;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(BlackScreenFadeInCoroutine());
    }

    public void ChangeScene(string name)
    {
        //Add transition effects here 
        StartCoroutine(BlackScreenFadeOutCoroutine(name));
    }

    private IEnumerator BlackScreenFadeInCoroutine()
    {
        var c = fade.color;
        for (int x = 0; x <= 50; x++)
        {
            c.a = 1 - (x / 50f);
            fade.color = c;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator BlackScreenFadeOutCoroutine(string name)
    {
        var c = fade.color;
        for (int x = 0; x <= 50; x++)
        {
            c.a = (x / 50f);
            fade.color = c;
            yield return new WaitForFixedUpdate();
        }
        SceneManager.LoadSceneAsync(name);
    }
}
