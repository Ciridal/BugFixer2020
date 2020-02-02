using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wipnemy : MonoBehaviour
{
    public float startX;
    public float startY;

    public Grid gridManager;

    void Start()
    {
        //this.transform.position = new Vector3(startX, startY, -0.5f) + new Vector3(1.3f, 1.3f) * .5f + new Vector3(.4f, .4f);
    }
    
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.F))
        //{
        //    Debug.Log(gridManager.GetNode(0, 1).tile.name);
        //    gridManager.GetNode(0, 1).tile.GetComponent<SpriteRenderer>().color = Color.red;
        //}
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
            this.transform.position = nearest.WorldPosition();
        }
        else
        {
            PathNode current = CurrentNode();
            if (current != null)
                this.transform.position = current.WorldPosition();
        }
    }
}
