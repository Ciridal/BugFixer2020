using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float nodeLength = 1;
    public GameObject camera;
    public Grid GridManager;
    public int hp;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float lastFire;
    public float fireDelay;

    private PathNode currentNode;
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
            Move("up");

        if (Input.GetKeyUp(KeyCode.A))
            Move("left");

        if (Input.GetKeyUp(KeyCode.S))
            Move("down");

        if (Input.GetKeyUp(KeyCode.D))
            Move("right");

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

    public PathNode CurrentNode()
    {
        return GridManager.GetNodePosition(this.transform.position);
    }

    private PathNode NextNode(Vector3 position)
    {
        return GridManager.GetNodePosition(position);
    }

    private void Move(string direction)
    {
        Vector3 nextPos;
        PathNode next;
        switch (direction)
        {
            case "up":
                nextPos = new Vector3(this.transform.position.x, this.transform.position.y + nodeLength);
                next = NextNode(nextPos);
                if (next != null && next.walkable)
                    MoveToNode(next);
                break;
            case "down":
                nextPos = new Vector3(this.transform.position.x, this.transform.position.y - nodeLength);
                next = NextNode(nextPos);
                if (next != null && next.walkable)
                    MoveToNode(next);
                break;
            case "left":
                nextPos = new Vector3(this.transform.position.x - nodeLength, this.transform.position.y);
                next = NextNode(nextPos);
                if (next != null && next.walkable)
                    MoveToNode(next);
                break;
            case "right":
                nextPos = new Vector3(this.transform.position.x + nodeLength, this.transform.position.y);
                next = NextNode(nextPos);
                if (next != null && next.walkable)
                    MoveToNode(next);
                break;
        }
    }

    public void SetGridPosition(bool walkable = false)
    {
        if (walkable)
        {
            PathNode nearest = GridManager.FindNearestWalkable(this.transform.position);
            MoveToNode(nearest);
        }
        else
        {
            PathNode current = CurrentNode();
            if (current != null)
                MoveToNode(current);
        }
    }

    public void SetGridPosition(int x, int y, bool walkable = false, Grid grid = null)
    {
        if (grid != null)
            this.GridManager = grid;

        if(walkable)
        {
            PathNode nearest = GridManager.FindNearestWalkable(GridManager.GetNode(x,y));
            MoveToNode(nearest);
        }
        else
        {
            PathNode target = GridManager.GetNode(x, y);
            MoveToNode(target);
        }
    }

    private void MoveToNode(PathNode node)
    {
        node.inhabited = false;
        this.transform.position = new Vector3(node.WorldPosition().x, node.WorldPosition().y, this.transform.position.z);
    }

    public void SetGrid(Grid grid)
    {
        this.GridManager = grid;
    }
}
