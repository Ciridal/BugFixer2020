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
    //public GameObject enemy;

    public EnemyManager enemyManager;

    //public Grid grid;
    private System.Random seed = null;
    
    void Start()
    {
        var seedObject = GameObject.FindObjectOfType<Randomizer>();
        if(seedObject != null)
            seed = seedObject.seed;
        this.GetComponent<Grid>().CreateGrid(columns, rows, gridSprite, cellSize, randomFillPercent, seed, smoothness);
        player.GetComponent<Player>().SetGridPosition(50,50,true);
        if (enemyManager == null)
            enemyManager = this.GetComponent<EnemyManager>();
        enemyManager.Spawn();
        //enemy.GetComponent<Enemy>().SetGridPosition(true);
    }

    private void Update()
    {
        
    }
}

    
