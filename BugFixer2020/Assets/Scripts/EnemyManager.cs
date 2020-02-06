using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject enemy;
    public int enemyCount = 5;
    public int initialEnemyCount;
    public int deathAmount = 0;
    public GameManager gameManager;
    SceneManagement sceneManagement;
    Grid grid;
    public bool isReady = false;

    //(0,0) , (0, 99) , (99, 99), (99, 0)

    public int lastPos = 1;
   
    void Start()
    {
        initialEnemyCount = enemyCount;

        if (gameManager == null)
            gameManager = this.GetComponent<GameManager>();

        if (sceneManagement == null)
            sceneManagement = this.GetComponent<SceneManagement>();

        if (grid == null)
            grid = this.GetComponent<Grid>();
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


            if (i % 4 == 0 && i != 0)
                _enemy.SetGridPosition(gameManager.columns - 1, 0, true);
            else if (i % 2 == 0)
                _enemy.SetGridPosition(0, 0, true);
            else if (i % 3 == 0)
                _enemy.SetGridPosition(0, gameManager.rows - 1, true);
            else
                _enemy.SetGridPosition(gameManager.columns - 1, gameManager.rows - 1, true);

            if (enemies.Where(e => e.GetComponent<Enemy>().CurrentNode() == _enemy.CurrentNode()/* && e != newEnemy*/).ToList().Count() > 0)
            {
                _enemy.GetComponent<Enemy>().MoveToNode(RandomPosition());
                Debug.Log("Node Randomized");
            }

            newEnemy.GetComponent<Pathfinding>().DoPathFinding(newEnemy.transform, GameObject.FindGameObjectWithTag("Player").transform);

            //MIGHT WORK NOW MAYBE?
            if (_enemy.path == null)
            {
                Debug.Log(i + " is out of bounds!");
                var newNode = grid.MoveTowardsCentre(grid.FindNearestWalkable(newEnemy.transform.position));
                _enemy.SetGridPosition(newNode.gridX, newNode.gridY);
            }
        }
        isReady = true;
    }

    PathNode RandomPosition()
    {
        var randX = Random.Range(0, gameManager.columns - 1);
        var randY = Random.Range(0, gameManager.rows - 1);


        PathNode randPos = grid.GetNode(randX, randY);

        return randPos;
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
