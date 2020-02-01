using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public Scene ActiveScene;

    void start()
    {
        ActiveScene = SceneManager.GetActiveScene();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        ActiveScene = SceneManager.GetActiveScene();
    }
}
