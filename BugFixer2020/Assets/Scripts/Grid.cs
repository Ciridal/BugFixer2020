using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int width;
    private int height;
    private int[,] gridArray;
    private float cellSize;
    //private GameObject[,] tiles;

    //public Sprite image;

    public Grid(int width, int height, Sprite gridSprite, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];
       // tiles = new GameObject[width, height];

        Debug.Log(width + " " + height);

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y<gridArray.GetLength(1); y++)
            {
                SpawnTile(x,y, gridSprite);
                
            }
        }

        SetValue(2, 1, 1);
    }

    private void SpawnTile(int x, int y, Sprite gridSprite)
    {
        GameObject tile = new GameObject("x: " + x + "y: " + y);
        tile.transform.position = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
        var sprite = tile.AddComponent<SpriteRenderer>();
        //tile.GetComponent<SpriteRenderer>().sprite = image;
        
        sprite.sprite = gridSprite;
        //sprite.color = new Color(0, 0, 0);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    private void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPos.x / cellSize);
        y = Mathf.FloorToInt(worldPos.y / cellSize);
    }
    
    public void SetValue(int x, int y, int value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
           gridArray[x, y] = value;
           Debug.Log("x: " + x + " y: " + y + " value set to " + value);
        }   
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);

        Debug.Log("x: " + x + " y: " + y + " value set to " + value);
    }

}
