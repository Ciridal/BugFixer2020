using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Randomizer : MonoBehaviour
{
    public System.Random seed;

    void Start()
    {
        //seed = Random.Range(0, input.GetHashCode());
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void SetSeed(string input)
    {
        seed = new System.Random(input.GetHashCode());
    }

    public void SetSeed(GameObject obj)
    {
        seed = new System.Random(obj.GetComponent<Text>().text.GetHashCode());
    }
}
