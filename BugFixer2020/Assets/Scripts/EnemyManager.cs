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
    SceneManagement sceneManagement;
    Grid grid;
    public bool isReady = false;

    //(0,0) , (0, 99) , (99, 99), (99, 0)

    int lastPos = 1;
   
    void Start()
    {
        if (gameManager == null)
            gameManager = this.GetComponent<GameManager>();

        if (sceneManagement == null)
            sceneManagement = this.GetComponent<SceneManagement>();
    }

    void Update()
    {
        if (deathAmount >= enemyCount)
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

        for (int i = 0; i < enemyCount; i++)
        {
            

            var newEnemy = Instantiate(enemy);

            if (newEnemy.active == false)
                newEnemy.SetActive(true);
            enemies.Add(newEnemy);


            if (lastPos == 1)
            {
                newEnemy.GetComponent<Enemy>().SetGridPosition(0, 0, true);
                lastPos++;
            }
            else if (lastPos == 2)
            {
                newEnemy.GetComponent<Enemy>().SetGridPosition(0, gameManager.rows - 1, true);
                lastPos++;
            }
            else if (lastPos == 3)
            {
                newEnemy.GetComponent<Enemy>().SetGridPosition(gameManager.columns - 1 ,gameManager.rows - 1 , true);
                lastPos++;
            }     
            else if (lastPos >= 4)
            {
                newEnemy.GetComponent<Enemy>().SetGridPosition(gameManager.columns - 1, 0, true);
                lastPos = 1;
            }
                

            if (newEnemy.GetComponent<Enemy>().outOfBounds)
            {
                grid.MoveTowardsCentre(grid.FindNearestWalkable(transform.position));
            }
            
        }
        isReady = true;
    }

    Vector3 RandomPosition()
    {
        var randX = Random.Range(0, gameManager.columns - 1);
        var randY = Random.Range(1, gameManager.rows - 1);


        Vector3 randPos = new Vector3(randX, randY, -0.1f);

        return randPos;
    }

    public void OnEnemyDeath()
    {
        deathAmount++;
    }

   

}
