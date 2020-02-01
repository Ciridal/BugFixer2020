using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{  
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    private GameObject grid;

    public List<GameObject> tiles;
    
    public List<PathNode> nodes;

    public void CreateGrid(int width, int height, Sprite[] gridSprite, float cellSize, int randomFillPercent, System.Random seed, int smoothness)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width , height];
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
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
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
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
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
        return tiles.FirstOrDefault(n => n.transform.position == position);
    }

    public List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        neighbours.Add(nodes.FirstOrDefault(n => n.gridX == node.gridX - 1 && n.gridY == node.gridY)); //LEFT
        neighbours.Add(nodes.FirstOrDefault(n => n.gridX == node.gridX && n.gridY == node.gridY - 1)); //BOTTOM
        neighbours.Add(nodes.FirstOrDefault(n => n.gridX == node.gridX + 1 && n.gridY == node.gridY)); //RIGHT
        neighbours.Add(nodes.FirstOrDefault(n => n.gridX == node.gridX && n.gridY == node.gridY + 1)); //TOP

        return neighbours;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public PathNode GetNodePosition(float x, float y)
    {
        return nodes.FirstOrDefault(n => n.tile.transform.position == new Vector3(x, y));
    }

    public PathNode GetNode(int x, int y)
    {
        return nodes.Find(n => n.gridX == x && n.gridY == y);
    }

    public GameObject GetTile(PathNode node)
    {
        return tiles.Find(t => t.gameObject.name == node.tile.name);
    }

    public GameObject GetTile(int x, int y)
    {
        return tiles.Find(t => t.gameObject.name == x + "," + y);
    }

    public GameObject GetTile(string name)
    {
        return tiles.Find(t => t.gameObject.name == name);
    }
}
