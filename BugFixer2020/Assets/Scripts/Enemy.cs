﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int damage;
    public float speed;
    public EnemyManager enemyManager;
    public GameManager gameManager;

    //Time stuff
    public float damageDelay;
    public float moveDelay;

    private GameObject player;
    private float lastHit;
    private float lastMoved;

    //Pathfinding stuff
    public Grid gridManager;
    public Pathfinding pathfinding;
    public List<PathNode> path;
    public bool outOfBounds;

    private PathNode currentNode;
    private PathNode nextNode;

    void Start()
    {
        if(gridManager == null)
            gridManager = GameObject.FindObjectOfType<Grid>();

        if (enemyManager == null)
            enemyManager = GameObject.FindObjectOfType<EnemyManager>();

        if (pathfinding == null)
            pathfinding = this.gameObject.GetComponent<Pathfinding>();

        if (gameManager == null)
            gameManager = GameObject.FindObjectOfType<GameManager>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(path == null || path.IndexOf(player.GetComponent<Player>().CurrentNode()) < 0)
            path = pathfinding.DoPathFinding(this.transform, player.transform);

        currentNode = CurrentNode();
        if (currentNode != null && currentNode != player.GetComponent<Player>().CurrentNode())
            currentNode.inhabited = true;
        outOfBounds = pathfinding.outOfBound;

        if (hp <= 0)
            Death();

        if (player)
        {
            if (this.currentNode != null && this.currentNode == player.GetComponent<Player>().CurrentNode())
            {
                pathfinding.StopPathFinding();
                if (Time.time > lastHit + damageDelay)
                {
                    DealDamage();
                    lastHit = Time.time;
                }
            }
        }

        if (!outOfBounds && enemyManager.isReady)
        {
            if (path != null && Time.time > lastMoved + moveDelay)
                Move();
        }
    }

    public int TakeDamage(int dmg)
    {
        hp -= dmg;
        return hp;
    }

    void DealDamage()
    {
        player.GetComponent<Player>().TakeDamage(damage);
        Debug.Log("Monster hit player");
    }

    void Death()
    {
        gameManager.AddScore(1);
        enemyManager.OnEnemyDeath(this.gameObject);
        if (this.path != null)
            this.pathfinding.StopPathFinding();
        CurrentNode().inhabited = false;
        Destroy(gameObject);
    }
    public PathNode CurrentNode()
    {
        return gridManager.GetNodePosition(this.transform.position);
    }

    public void SetGridPosition(bool walkable = false)
    {
        if (walkable)
        {
            PathNode nearest = gridManager.FindNearestWalkable(this.transform.position);
            MoveToNode(nearest);
        }
        else
        {
            PathNode current = CurrentNode();
            if (current != null)
                MoveToNode(current);
        }
    }

    public void SetGridPosition(int x, int y, bool walkable = false)
    {
        if (walkable)
        {
            PathNode nearest = gridManager.FindNearestWalkable(gridManager.GetNode(x, y));
            MoveToNode(nearest);
        }
        else
        {
            PathNode target = gridManager.GetNode(x, y);
            MoveToNode(target);
        }
    }

    public void MoveToNode(PathNode node)
    {
        if(currentNode != null)
            currentNode.inhabited = false;
        this.transform.position = new Vector3(node.WorldPosition().x, node.WorldPosition().y, this.transform.position.z);
    }

    private void Move()
    {
        if (path.Count == 0 || currentNode == path[path.Count - 1])
        {
            pathfinding.StopPathFinding();
        }
        else if (currentNode != path[path.Count - 1])
        {
            nextNode = path[path.IndexOf(currentNode) + 1];
            if(!nextNode.inhabited)
                MoveToNode(nextNode);
        }
        lastMoved = Time.time;
    }

    public void SetGrid(Grid grid)
    {
        this.gridManager = grid;
    }

    public void CorrectZPosition(Vector3 position)
    {
        this.transform.position = new Vector3(position.x, position.y, -0.1f);
    }
}
