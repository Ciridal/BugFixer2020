using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    public bool walkable;
    public Vector3 worldPos;

    public PathNode(bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        worldPos = _worldPos;

    }
}
