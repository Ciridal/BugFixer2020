﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;
    //public Transform seeker, target;
    public GameObject gameManager;
    public EnemyManager enemyManager;
    public List<PathNode> path;
    public bool walkableOnly = true;

    //If no path to player is available
    public bool outOfBound = false;

    private void Start()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (enemyManager == null)
            enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        grid = GameObject.FindObjectOfType<Grid>();
    }

    private void Update()
    {
        //EmptyPath(this.path);
        //DrawPath(this.path);
    }

    public List<PathNode> DoPathFinding(Transform seeker, Transform target)
    {
        if (this.path != null)
            outOfBound = false;

        if (outOfBound)
            Debug.Log("Path not found");

        //Empty old path and retrace new one
        if (this.path != null && this.path.IndexOf(grid.GetNodePosition(target.position)) != this.path.Count - 1)
            EmptyPath(this.path);

        if (!this.outOfBound && (this.path == null || this.path.Count <= 0 || this.path.IndexOf(grid.GetNodePosition(target.position)) != this.path.Count - 1))
            this.path = FindPath(seeker.position, target.position, walkableOnly);

        return this.path;
    }
    
    public List<PathNode> FindPath(Vector3 startPos, Vector3 targetPos, bool onlyWalkable)
    {
        if (grid == null)
            grid = FindObjectOfType<Grid>();

        if (this.path != null)
            EmptyPath(this.path);

        PathNode startNode = grid.GetNodePosition(startPos);
        PathNode targetNode = grid.GetNodePosition(targetPos);
        //startNode.SetColour(Color.yellow);

        List<PathNode> openSet = new List<PathNode>();
        var cameFrom = new Dictionary<PathNode, PathNode>();

        if(startNode != null && targetNode != null)
        {
            openSet.Add(startNode);
            var gScore = new Dictionary<PathNode, int>();
            var fScore = new Dictionary<PathNode, int>();
            gScore[startNode] = 0;
            fScore[startNode] = GetDistance(startNode, targetNode);

            while (openSet.Count > 0)
            {
                openSet.Sort((n1, n2) =>
                {
                    int fScore1, fScore2;
                    if (!fScore.TryGetValue(n1, out fScore1))
                    {
                        fScore1 = int.MaxValue;
                    }
                    if (!fScore.TryGetValue(n2, out fScore2))
                    {
                        fScore2 = int.MaxValue;
                    }
                    return fScore1 - fScore2;
                });
                PathNode node = openSet[0];
                if (node == targetNode)
                {
                    return RetracePath(cameFrom, targetNode);
                }

                openSet.Remove(node);
                List<PathNode> neighbours;
                if (onlyWalkable)
                    neighbours = grid.GetWalkableNeighbours(node);
                else
                    neighbours = grid.GetNeighbours(node);
                foreach (var neighbor in neighbours)
                {
                    var tentativeScore = gScore[node] + 1;
                    int neighborScore;
                    if (!gScore.TryGetValue(neighbor, out neighborScore)) neighborScore = int.MaxValue;
                    if (tentativeScore < neighborScore)
                    {
                        cameFrom[neighbor] = node;
                        gScore[neighbor] = tentativeScore;
                        fScore[neighbor] = tentativeScore + GetDistance(neighbor, targetNode);
                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
        }
        outOfBound = true;
        return null;
    }

    List<PathNode> RetracePath(Dictionary<PathNode,PathNode> cameFrom, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;
        PathNode fromNode;

        while(cameFrom.TryGetValue(currentNode, out fromNode))
        {
            path.Add(currentNode);
            currentNode = fromNode;
        }

        path.Reverse();

        if (enemyManager == null)
            enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        if (enemyManager.isReady)
            DrawPath(path);

        outOfBound = false;
        return path;
    }

    private int GetDistance(PathNode nodeA, PathNode nodeB)
    { 
        var cellSize = grid.CellSize();
        return Mathf.CeilToInt(Vector3.Distance(nodeA.WorldPosition(), nodeB.WorldPosition()) / cellSize); //eii lamo
    }

    private void EmptyPath(List<PathNode> path)
    {
        if(path != null)
        {
            if (enemyManager == null)
                enemyManager = GameObject.FindObjectOfType<EnemyManager>();
            var paths = enemyManager.Paths();

            foreach (PathNode node in path)
            {
                //Does not work, drawing should be in enemy manager / controller
                //if(paths.Find(p => p.IndexOf(node) > 0) != null && paths.Where(p => p.IndexOf(node) > 0).ToList().Count <= 1)
                node.ResetColour();
            }
        }
        path = null;
    }

    public void StopPathFinding()
    {
        EmptyPath(this.path);
    }

    public void DrawPath(List<PathNode> path)
    {
        foreach (var node in path)
        {
            //Startnode
            if (path.IndexOf(node) == 0)
                node.SetColour(Color.green);
            //Targetnode
            else if (path.IndexOf(node) == path.Count - 1)
                node.SetColour(Color.red);
            else
                node.SetColour(Color.blue);
        }
    }
}
