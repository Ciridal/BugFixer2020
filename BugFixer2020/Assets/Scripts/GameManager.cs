using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Sprite[] gridSprite;
    public float cellSize;
    public int columns = 20;
    public int rows = 10;
    
    [Range(0, 100)]
    public int randomFillPercent;
    public int smoothness = 5;

    public GameObject player;
    public int playerScore = 0;

    public EnemyManager enemyManager;
    public Grid gridManager;

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

    public void OnLevelLoad(string name)
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        gridManager.CreateGrid(columns, rows, gridSprite, cellSize, randomFillPercent, seed, smoothness);
        player.GetComponent<Player>().SetGridPosition(50, 50, true, gridManager);
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

    
