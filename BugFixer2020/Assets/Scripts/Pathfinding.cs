using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;
    public Transform seeker, target;
    public GameObject gameManager;
    public List<PathNode> path;

    private void Start()
    {
        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("GameManager");
        grid = GameObject.FindObjectOfType<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }
    
    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        PathNode startNode = grid.GetNodePosition(startPos);
        PathNode targetNode = grid.GetNodePosition(targetPos);

        startNode.SetColour(Color.green);
        targetNode.SetColour(Color.red);
        //Debug.Log(GetDistance(startNode, targetNode));

        List<PathNode> openSet = new List<PathNode>();
        var cameFrom = new Dictionary<PathNode, PathNode>();

        openSet.Add(startNode);
        var gScore = new Dictionary<PathNode, int>();
        var fScore = new Dictionary<PathNode, int>();
        gScore[startNode] = 0;
        fScore[startNode] = GetDistance(startNode, targetNode);

        //Everything underneath this point is still WIP and probably doesn't work
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
                RetracePath(cameFrom, targetNode);
                return;
            }

            openSet.Remove(node);
            foreach(var neighbor in grid.GetWalkableNeighbours(node))
            {
                var tentativeScore = gScore[node] + 1;
                int neighborScore;
                if (!gScore.TryGetValue(neighbor, out neighborScore)) neighborScore = int.MaxValue;
                if(tentativeScore < neighborScore)
                {
                    cameFrom[neighbor] = node;
                    gScore[neighbor] = tentativeScore;
                    fScore[neighbor] = tentativeScore + GetDistance(neighbor, targetNode);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        Debug.Log("Path not found");
    }

    void RetracePath(Dictionary<PathNode,PathNode> cameFrom, PathNode endNode)
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

        foreach(var node in path)
        {
            node.SetColour(Color.blue);
        }
        this.path = path;
    }

    int GetDistance(PathNode nodeA, PathNode nodeB)
    { 
        var cellSize = grid.CellSize();
        return Mathf.CeilToInt(Vector3.Distance(nodeA.WorldPosition(), nodeB.WorldPosition()) / cellSize); //eii lamo
    }
}
