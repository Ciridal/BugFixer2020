using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    public GameObject tile;
    public int gridX, gridY;
    public bool walkable;

    public PathNode(GameObject tile, int x, int y, bool _walkable)
    {
        this.tile = tile;

        gridX = x;
        gridY = y;
        walkable = _walkable;
    }

    public void SetColour(Color color)
    {
        tile.GetComponent<SpriteRenderer>().color = color;
    }

    public void ResetColour()
    {
        tile.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public Vector3 WorldPosition()
    {
        return tile.transform.position;
    }
}
