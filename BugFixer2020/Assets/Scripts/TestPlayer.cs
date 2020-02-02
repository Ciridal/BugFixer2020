using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float nodeLength = 1;
    public GameObject camera;
    public int hp;
    //public Sprite bullet;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;
    public int score;

    void Start()
    {
        this.transform.position += new Vector3(nodeLength, nodeLength) * .5f;
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + nodeLength, this.transform.position.z);

        if (Input.GetKeyUp(KeyCode.A))
            this.transform.position = new Vector3(this.transform.position.x - nodeLength, this.transform.position.y, this.transform.position.z);

        if (Input.GetKeyUp(KeyCode.S))
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - nodeLength, this.transform.position.z);

        if (Input.GetKeyUp(KeyCode.D))
            this.transform.position = new Vector3(this.transform.position.x + nodeLength, this.transform.position.y, this.transform.position.z);

        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");

        if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVert);
            lastFire = Time.time;
        }

        if(hp <= 0)
        {
            Death();
        }

        camera.GetComponent<CameraBehaviour>().Move();
    }

    public int TakeDamage(int dmg)
    {
        hp -= dmg;
        return hp;
    }

    void Death()
    {
        SceneManager.LoadScene("Death");
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3((x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed, (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed);
    }

    public int AddScore(int points)
    {
        score += points;
        return score;
    }

    public int GetScore()
    {
        return score;
    }

}
