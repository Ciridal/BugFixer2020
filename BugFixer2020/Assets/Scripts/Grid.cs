using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{  
    private int width;
    private int height;
    private int wallSize;
    private int[,] gridArray;
    private float cellSize;
    private GameObject grid;

    public List<GameObject> tiles;
    
    public List<PathNode> nodes;

    public void CreateGrid(int width, int height, Sprite[] gridSprite, float cellSize, int randomFillPercent, System.Random seed, int smoothness, int wallSize)
    {
        this.width = width + (wallSize * 2);
        this.height = height + (wallSize * 2);
        this.cellSize = cellSize;
        this.wallSize = wallSize;

        gridArray = new int[width + (wallSize * 2) , height + (wallSize * 2)];
        grid = new GameObject("Grid");
        nodes = new List<PathNode>();
        tiles = new List<GameObject>();

        if (seed == null)
            seed = new System.Random(System.DateTime.Now.ToString().GetHashCode());

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                //1 = wall, 0 = floor
                if (x == 0 || x == width - wallSize || y == 0 || y == height - wallSize)
                {
                    gridArray[x, y] = 1;
                }
                else
                    gridArray[x, y] = (seed.Next(0, 100) < randomFillPercent) ? 1 : 0;
            }
        }
        SmoothMap(smoothness, gridSprite);
    }

    private void SmoothMap(int smoothness, Sprite[] gridSprite)
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                if (neighbourWallTiles > smoothness - 1)
                    gridArray[x, y] = 1;
                else if (neighbourWallTiles < smoothness - 1)
                    gridArray[x, y] = 0;
                
                SpawnTile(x, y, gridSprite[gridArray[x, y]]);
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for(int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for(int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 + wallSize && neighbourX < width - wallSize && neighbourY >= 0 + wallSize && neighbourY < height - wallSize)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += gridArray[neighbourX, neighbourY];
                    }
                }
                else
                    wallCount++;
            }
        }
        return wallCount;
    }


    private void SpawnTile(int x, int y, Sprite gridSprite)
    {
        GameObject tile = new GameObject(x + "," + y);
        tile.transform.SetParent(grid.transform);
        tile.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
        var sprite = tile.AddComponent<SpriteRenderer>();
        
        sprite.sprite = gridSprite;

        tiles.Add(tile);
        nodes.Add(new PathNode(tile, x, y, gridArray[x, y] == 0));
           
    }

    public int[,] GetGridArray()
    {
        return gridArray;
    }

    public GameObject GetTileWithPosition(Vector3 position)
    {
        return tiles.Find(n => n.transform.position == position);
    }

    public List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        neighbours.Add(nodes.Find(n => n.gridX == node.gridX - 1 && n.gridY == node.gridY)); //LEFT
        neighbours.Add(nodes.Find(n => n.gridX == node.gridX && n.gridY == node.gridY - 1)); //BOTTOM
        neighbours.Add(nodes.Find(n => n.gridX == node.gridX + 1 && n.gridY == node.gridY)); //RIGHT
        neighbours.Add(nodes.Find(n => n.gridX == node.gridX && n.gridY == node.gridY + 1)); //TOP
        //Debug.Log(neighbours.Where(n => n != null).ToList().Count);

        return neighbours.Where(n => n != null).ToList();
    }

    public List<PathNode> GetWalkableNeighbours(PathNode node)
    {
        return GetNeighbours(node).Where(n => n.walkable).ToList();
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public PathNode GetNodePosition(float x, float y)
    {
        var node = nodes.Find(n => n.tile.transform.position == new Vector3(x, y));
        if (node != null)
            return node;
        else
            return FindNearestNode(new Vector3(x, y));
    }

    public PathNode GetNodePosition(Vector3 position)
    {
        var node = nodes.Find(n => n.tile.transform.position == position);
        if (node != null)
            return node;
        else
            return FindNearestNode(position);
    }

    public PathNode FindNearestNode(Vector3 position)
    {
        PathNode node = null;
        float minDist = cellSize;
        //Get list of nearest nodes
        List<PathNode> nearest = nodes.Where(n => n.tile.transform.position.x >= position.x - cellSize
            && n.tile.transform.position.x <= position.x + cellSize
            && n.tile.transform.position.y >= position.y - cellSize
            && n.tile.transform.position.y <= position.y + cellSize).ToList();

        //Calculate the nearest
        if(nearest.Count >= 1)
        {
            foreach (PathNode n in nearest)
            {
                float dist = Vector3.Distance(n.tile.transform.position, position);
                if (dist < minDist)
                {
                    node = n;
                    minDist = dist;
                }
            }
            return node;
        }

        //No nodes available (probably out of bounds)
        return null;
    }

    public PathNode FindNearestWalkable(Vector3 position)
    {
        PathNode node = null;
        float minDist = Mathf.Infinity;
        //Get list of nearest nodes
        List<PathNode> walkables = nodes.Where(n => n.walkable == true).ToList();

        //Calculate the nearest
        if (walkables.Count >= 1)
        {
            foreach (PathNode n in walkables)
            {
                float dist = Vector3.Distance(n.tile.transform.position, position);
                if (dist < minDist)
                {
                    node = n;
                    minDist = dist;
                }
            }
            return node;
        }

        //No nodes available (probably out of bounds)
        return null;
    }

    public PathNode FindNearestWalkable(PathNode pathNode)
    {
        return FindNearestWalkable(pathNode.tile.transform.position);
    }

    public PathNode FindNearestWalkable(int x, int y)
    {
        var position = new Vector3(x * cellSize, y * cellSize);
        return FindNearestWalkable(position);
    }

    public PathNode MoveTowardsCentre(PathNode node, bool onlyWalkables = false)
    {
        int x = node.gridX;
        int y = node.gridY;
        int centreX = (int)Mathf.Sqrt(nodes.Count) / 2;
        int centreY = (int)Mathf.Sqrt(nodes.Count) / 2;

        int distX = x - centreX;
        int distY = y - centreY;

        int dist = (Mathf.Abs(distX) + Mathf.Abs(distY)) / 2;

        if (onlyWalkables)
        {
            for(int i = 0; i < distX + distY / 2; i++)
            {
                if (distX < 0)
                    x++;
                else
                    x--;

                if (distY < 0)
                    y++;
                else
                    y--;

                if (GetNode(x, y).walkable)
                    return GetNode(x, y);
            }
            return FindNearestWalkable(GetNode(x, y));
        }

        else
        {
            if (distX < 0)
                x++;
            else
                x--;

            if (distY < 0)
                y++;
            else
                y--;
            return GetNode(x, y);
        }
        return null;
    }

    public PathNode GetNode(int x, int y)
    {
        return nodes.Find(n => n.gridX == x && n.gridY == y);
    }

    public PathNode GetWalkableNode(int x, int y)
    {
        var node = nodes.Find(n => n.gridX == x && n.gridY == y && n.walkable);
        if (node == null)
            return FindNearestWalkable(x,y);
        return node;
    }

    public float CellSize()
    {
        return cellSize;
    }

    public int GridHeight()
    {
        return height;
    }

    public int GridWidth()
    {
        return width;
    }
}
