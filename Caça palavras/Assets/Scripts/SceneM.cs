using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneM : MonoBehaviour
{

    int Next;

    public static SceneM manager;

    void Awake()
    {
        Next = SceneManager.GetActiveScene().buildIndex + 1;
        if (manager != this)
        {
            if (manager == null)
            {
                manager = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void LoadScene(int sceneID)
    {
        Next = sceneID + 1;
        SceneManager.LoadScene(sceneID);
    }

    public void NextScene()
    {
        if(SceneManager.sceneCountInBuildSettings <= Next)
        {
            SceneManager.LoadScene(0);
            Next = 1;
        }
        else
        {
            SceneManager.LoadScene(Next);
            Next++;
        }
        
    }
}
