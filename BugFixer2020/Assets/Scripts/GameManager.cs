using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Sprite gridSprite;
    public float cellSize;

    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(20, 10, gridSprite, cellSize);
    }

    private void Update()
    {
        
    }
}

    
