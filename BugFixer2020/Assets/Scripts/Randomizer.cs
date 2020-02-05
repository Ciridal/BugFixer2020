using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Randomizer : MonoBehaviour
{
    public System.Random seed;
    public string seedString = "";

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void SetSeed(string input)
    {
        seedString = input;
        seed = new System.Random(input.GetHashCode());
    }

    public void SetSeed(GameObject obj)
    {
        seedString = obj.GetComponent<Text>().text;
        seed = new System.Random(seedString.GetHashCode());
    }

    public void IncreaseSeed(int level)
    {
        seed = new System.Random(seedString.GetHashCode() + level);
    }
}
