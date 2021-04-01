using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Singleton ref
    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void ChangeScene(string name)
    {
        //Add transition effects here 
        SceneManager.LoadSceneAsync(name);
    }
}
