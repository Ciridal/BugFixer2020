using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    public GameObject tile;
    public int gCost, hCost;
    public int gridX, gridY;
    public bool walkable;

    public PathNode(GameObject tile, int x, int y, bool _walkable)
    {
        this.tile = tile;

        gridX = x;
        gridY = y;
        walkable = _walkable;
    }

    public int fCost()
    {
        return gCost + hCost;
    }

    public void SetColour(Color color)
    {
        tile.GetComponent<SpriteRenderer>().color = color;
    }

    public Vector3 GetWorldPosition()
    {
        return tile.transform.position;
    }
}
