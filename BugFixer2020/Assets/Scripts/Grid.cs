using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int width;
    private int height;
    private int[,] gridArray;

    //public Sprite image;

    public Grid(int width, int height, Sprite gridSprite)
    {
        this.width = width;
        this.height = height;

        gridArray = new int[width, height];

        Debug.Log(width + " " + height);

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y<gridArray.GetLength(1); y++)
            {
                SpawnTile(x,y, gridSprite);
            }
        }

    }

    private void SpawnTile(int x, int y, Sprite gridSprite)
    {
        GameObject tile = new GameObject("x: " + x + "y: " + y);
        tile.transform.position = new Vector3(x, y);
        var sprite = tile.AddComponent<SpriteRenderer>();
        //tile.GetComponent<SpriteRenderer>().sprite = image;
        sprite.sprite = gridSprite;
        //sprite.color = new Color(0, 0, 0);
    }

    
}
