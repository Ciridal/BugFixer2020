using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public GameObject enemy;
    public int enemyCount = 5;
    public GameManager gameManager;
    SceneManagement sceneManagement;
    Grid grid;

   
    void Start()
    {
        Spawn();
    }

    void Update()
    {
        if(enemies.Count <= 0)
        {
            sceneManagement.NextLevel("Kim");
            enemyCount += 5;
        }
    }

    public void Spawn()
    {
        bool walkable = true;

        for (int i = 0; i < enemyCount; i++)
        {
            
            var newEnemy = Instantiate(enemy);
            enemies.Add(newEnemy);

            newEnemy.GetComponent<Enemy>().SetGridPosition(gameManager.columns, gameManager.rows, walkable);
            
            if(newEnemy.GetComponent<Enemy>().pathfinding.path == null)
            {
                grid.MoveTowardsCentre(grid.FindNearestWalkable(transform.position));
            }
        }

        
    }

    Vector3 RandomPosition()
    {
        var randX = Random.Range(1, 50);
        var randY = Random.Range(1, 50);


        Vector3 randPos = new Vector3(randX, randY, -0.1f);

        return randPos;
    }

   
   

}
