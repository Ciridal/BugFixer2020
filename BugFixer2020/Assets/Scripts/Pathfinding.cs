using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;
    public Transform seeker, target;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    
    
    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        PathNode startNode = grid.GetWorldPosition(startPos.x, startPos.y);
        PathNode targetNode = grid.GetWorldPosition(targetPos.x, targetPos.y);

        List<PathNode> openSet = new List<PathNode>();
        HashSet<PathNode> closedSet = new HashSet<PathNode>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            PathNode node = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() < node.fCost() || openSet[i].fCost() == node.fCost())
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if(node==targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
        }

    }

    void RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            
        }

        path.Reverse();

        grid.path = path;
    }

    int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY); // wth is going on here??
        return 14 * dstX + 10 * (dstY - dstX);     // seriously
    }
}
