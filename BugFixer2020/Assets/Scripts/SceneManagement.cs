using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public int Level = 1;
    public Randomizer seedObject;
    public string LevelName;
    public GameManager gameManager;

    void Start()
    {
        if (seedObject == null)
            seedObject = this.gameObject.GetComponent<Randomizer>();

        if (gameManager == null)
            gameManager = this.gameObject.GetComponent<GameManager>();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel(string name)
    {
        Level++;
        seedObject.IncreaseSeed(Level);
        SceneManager.LoadScene(name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Death")
        {
            SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
        }

        if(scene.name == LevelName)
        {
            Debug.Log(LevelName + " was loaded!");
            gameManager.OnLevelLoad(name);
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public Scene CurrentScene()
    {
        return SceneManager.GetActiveScene();
    }
}
