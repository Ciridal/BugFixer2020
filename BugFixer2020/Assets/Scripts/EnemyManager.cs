using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject enemy;
    public int enemyCount = 5;
    public int deathAmount = 0;
    public GameManager gameManager;
    public SceneManagement sceneManagement;
    Grid grid;

    public bool isReady = false;

    void Start()
    {
        if (gameManager == null)
            gameManager = this.GetComponent<GameManager>();

        if (sceneManagement == null)
            sceneManagement = this.GetComponent<SceneManagement>();
    }

    void Update()
    {
        if(deathAmount >= enemyCount)
        {
            if (sceneManagement != null)
                sceneManagement.NextLevel("Kim");
            else
                Debug.Log("Congratulations!");
            enemyCount += 5;
        }
    }

    public void Spawn()
    {
        bool walkable = true;

        for (int i = 0; i < enemyCount; i++)
        {
            var newEnemy = Instantiate(enemy);
            if(newEnemy.active == false)
                newEnemy.SetActive(true);
            enemies.Add(newEnemy);

            newEnemy.GetComponent<Enemy>().SetGridPosition(gameManager.columns - 1, gameManager.rows -1, walkable);
            
            if(newEnemy.GetComponent<Enemy>().outOfBounds)
            {
                grid.MoveTowardsCentre(grid.FindNearestWalkable(transform.position));
            }
        }

        isReady = true;
    }

    Vector3 RandomPosition()
    {
        var randX = Random.Range(1, 50);
        var randY = Random.Range(1, 50);

        Vector3 randPos = new Vector3(randX, randY, -0.1f);

        return randPos;
    }

    public void OnEnemyDeath()
    {
        deathAmount++;
    }
}
