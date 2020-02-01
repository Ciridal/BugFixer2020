using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;

    //public System.Random seed;

    public Grid(int width, int height, Sprite[] gridSprite, float cellSize, int randomFillPercent, System.Random seed, int smoothness)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];

        if (seed == null)
            seed = new System.Random(System.DateTime.Now.ToString().GetHashCode());

        //Debug.Log(width + " " + height);

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y<gridArray.GetLength(1); y++)
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
        for(int i = 0; i < smoothness; i++)
        {
            SmoothMap(smoothness, gridSprite);
        }
        //SetValue(2, 1, 1);
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
        GameObject tile = new GameObject("x: " + x + "y: " + y);
        tile.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
        var sprite = tile.AddComponent<SpriteRenderer>();
        
        sprite.sprite = gridSprite;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    //private void GetXY(Vector3 worldPos, out int x, out int y)
    //{
    //    x = Mathf.gridSpriteToInt(worldPos.x / cellSize);
    //    y = Mathf.gridSpriteToInt(worldPos.y / cellSize);
    //}
    
    //public void SetValue(int x, int y, int value)
    //{
    //    if(x >= 0 && y >= 0 && x < width && y < height)
    //    {
    //       gridArray[x, y] = value;
    //       Debug.Log("x: " + x + " y: " + y + " value set to " + value);
    //    }   
    //}

    //public void SetValue(Vector3 worldPosition, int value)
    //{
    //    int x, y;
    //    //GetXY(worldPosition, out x, out y);
    //    //SetValue(x, y, value);

    //    //Debug.Log("x: " + x + " y: " + y + " value set to " + value);
    //}

}
