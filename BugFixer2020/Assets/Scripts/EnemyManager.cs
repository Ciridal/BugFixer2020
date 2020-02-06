using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject enemy;
    public GameObject player;
    public int enemyCount = 5;
    public int initialEnemyCount;
    public int deathAmount = 0;
    public GameManager gameManager;
    SceneManagement sceneManagement;
    Grid grid;
    public bool isReady = false;

    [Range(0,100)]
    public int minDistanceToPlayer = 0;

    //(0,0) , (0, 99) , (99, 99), (99, 0)
    private int realColumns;
    private int realRows;
   
    void Start()
    {
        initialEnemyCount = enemyCount;

        if (gameManager == null)
            gameManager = this.GetComponent<GameManager>();

        if (sceneManagement == null)
            sceneManagement = this.GetComponent<SceneManagement>();

        if (grid == null)
            grid = this.GetComponent<Grid>();

        realColumns = gameManager.columns + (2 * gameManager.wallSize);
        realRows = gameManager.rows + (2 * gameManager.wallSize);
    }

    void Update()
    {
        if (sceneManagement.CurrentScene().name == "Kim" && enemies.Count <= 0)
        {
            if (sceneManagement != null)
                sceneManagement.NextLevel("Kim");
            else
                Debug.Log("Congratulations!");
            enemyCount += 5;
        }
    }

    public void Spawn(Grid grid = null)
    {
        if (grid != null)
            this.grid = grid;

        StartCoroutine("SpawnEnemies");
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            var newEnemy = Instantiate(enemy);

            if (newEnemy.activeSelf == false)
                newEnemy.SetActive(true);
            var _enemy = newEnemy.GetComponent<Enemy>();
            _enemy.CorrectZPosition(newEnemy.transform.position);
            enemies.Add(newEnemy);

            if (grid == null)
                _enemy.SetGrid(this.grid);
            else
                _enemy.SetGrid(grid);


            //if (i % 4 == 0 && i != 0)
            //    _enemy.SetGridPosition(realColumns - 1, 0, true);
            //else if (i % 2 == 0)
            //    _enemy.SetGridPosition(0, 0, true);
            //else if (i % 3 == 0)
            //    _enemy.SetGridPosition(0, realRows - 1, true);
            //else
            //    _enemy.SetGridPosition(realColumns - 1, realRows - 1, true);

            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            var newPath = newEnemy.GetComponent<Pathfinding>().FindPath(newEnemy.transform.position, player.transform.position, true);

            //Cancerous fix
            if (newPath == null)
                for (int j = 0; j < 30; j++)
                {
                    if (newPath == null || _enemy.CurrentNode().inhabited || Vector3.Distance(_enemy.CurrentNode().tile.transform.position, player.transform.position) < minDistanceToPlayer)
                    {
                        _enemy.MoveToNode(RandomPosition());
                        newPath = newEnemy.GetComponent<Pathfinding>().FindPath(newEnemy.transform.position, player.transform.position, true);
                    }
                }
        }
        isReady = true;
        yield return null;
    }

    PathNode RandomPosition()
    {
        var randX = Random.Range(0, realColumns - 1);
        var randY = Random.Range(0, realRows - 1);

        PathNode randPos = grid.GetWalkableNode(randX, randY);

        return randPos;
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
