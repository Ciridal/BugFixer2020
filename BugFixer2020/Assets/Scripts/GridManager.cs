using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float[,] Grid;
    int vertical, horizontal, columns, rows;

    void Start()
    {
        vertical = (int)Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);
        columns = horizontal * 2;
        rows = vertical * 2;
        Grid = new float[columns, rows];
        for(int i = 0; i < columns; i++)
        {
            for(int j = 0; j < rows; j++)
            {
                Grid[i, j] = Random.Range(0.0f, 1.0f);
                SpawnTile(i, j, Grid[i, j]);
            }
        }
    }

    private void SpawnTile(int x, int y, float value)
    {
        GameObject tile = new GameObject("x: " + x + " y: " + y);
        tile.transform.position = new Vector3(x - (horizontal - 0.5f), y - (vertical - 0.5f));
        var sprite = tile.AddComponent<SpriteRenderer>();
        sprite.color = new Color(value, value, value);
    }
}
