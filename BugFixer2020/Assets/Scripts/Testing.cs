using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Sprite gridSprite;

    // Start is called before the first frame update
    void Start()
    {
        Grid grid = new Grid(20, 10, gridSprite);
    }
}

    
