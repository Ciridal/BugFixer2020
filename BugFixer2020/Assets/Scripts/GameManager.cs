using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Sprite[] gridSprite;
    public float cellSize;
    public int columns = 10;
    public int rows = 10;
    public int wallSize = 8;
    
    [Range(0, 100)]
    public int randomFillPercent;
    public int smoothness = 5;

    public GameObject player;
    public int playerScore = 0;

    public EnemyManager enemyManager;
    public Grid gridManager;
    public GameObject loadingScreen;

    //public Grid grid;
    private System.Random seed = null;
    
    void Start()
    {
        var seedObject = GameObject.FindObjectOfType<Randomizer>();
        if(seedObject != null)
            seed = seedObject.seed;
        if (enemyManager == null)
            enemyManager = this.GetComponent<EnemyManager>();
        if (gridManager == null)
            gridManager = this.GetComponent<Grid>();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (enemyManager.isReady && loadingScreen.activeSelf)
            loadingScreen.SetActive(false);
    }

    public void OnLevelLoad(string name)
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        loadingScreen.SetActive(true);
        gridManager.CreateGrid(columns, rows, gridSprite, cellSize, randomFillPercent, seed, smoothness, wallSize);
        player.GetComponent<Player>().SetGridPosition(columns / 2, rows / 2, true, gridManager);
        enemyManager.Spawn(gridManager);
    }
    public int AddScore(int points)
    {
        playerScore += points;
        return playerScore;
    }

    public int GetScore()
    {
        return playerScore;
    }
        
}

    
