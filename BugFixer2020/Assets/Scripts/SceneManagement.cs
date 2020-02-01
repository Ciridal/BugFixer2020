using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public int Level = 1;
    public Randomizer seedObject;
    public string LevelName;

    void start()
    {
        if (seedObject == null)
            seedObject = GameObject.FindObjectOfType<Randomizer>();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLevel(LevelName);
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void NextLevel(string name)
    {
        Level++;
        seedObject.IncreaseSeed(Level);
        SceneManager.LoadScene(name);
    }
}
