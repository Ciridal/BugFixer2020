using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    public GameObject tile;
    public int gCost, hCost;
    public int gridX, gridY;

    public PathNode(GameObject tile, int x, int y)
    {
        this.tile = tile;

        gridX = x;
        gridY = y;
    }

    public int fCost()
    {
        return gCost + hCost;
    }
}
