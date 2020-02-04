﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int damage;
    public float speed;

    //Time stuff
    public float damageDelay;
    public float moveDelay;

    private GameObject player;
    private float dist;
    private float lastHit;
    private float lastMoved;

    //Pathfinding stuff
    public Grid gridManager;
    public Pathfinding pathfinding;
    public List<PathNode> path;
    public bool outOfBounds;

    private List<PathNode> remainingPath;
    private PathNode currentNode;
    private PathNode nextNode;

    void Start()
    {
        if(gridManager == null)
            gridManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Grid>();
        Debug.Log(gridManager);

        if (pathfinding == null)
            pathfinding = this.gameObject.GetComponent<Pathfinding>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        pathfinding.DoPathFinding(this.transform, player.transform);

        currentNode = CurrentNode();

        path = pathfinding.path;
        outOfBounds = pathfinding.outOfBound;

        if (!outOfBounds)
        {
            if (path != null && Time.time > lastMoved + moveDelay)
                Move();

            if (hp <= 0)
                Death();

            if (player)
            {
                if (this.currentNode == player.GetComponent<Player>().CurrentNode() && Time.time > lastHit + damageDelay)
                {
                    DealDamage();
                    lastHit = Time.time;
                }
            }
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
        player.GetComponent<Player>().AddScore(1);
        if (this.path != null)
            this.pathfinding.StopPathFinding();
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
            Debug.Log("x: " + x + " y: " + y);
            Debug.Log(gridManager.GetNode(x, y));
            PathNode nearest = gridManager.FindNearestWalkable(gridManager.GetNodePosition(transform.position));
            MoveToNode(nearest);
        }
        else
        {
            PathNode target = gridManager.GetNode(x, y);
            MoveToNode(target);
        }
    }

    private void MoveToNode(PathNode node)
    {
        this.transform.position = new Vector3(node.WorldPosition().x, node.WorldPosition().y, this.transform.position.z);
    }

    private void Move()
    {
        if(currentNode != path[path.Count - 1])
        {
            nextNode = path[path.IndexOf(currentNode) + 1];
            MoveToNode(nextNode);
        }
        else if(currentNode == path[path.Count - 1])
        {
            pathfinding.StopPathFinding();
        }
        lastMoved = Time.time;
    }
}
