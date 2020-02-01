using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    //public string input;
    public float seed;

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
        seed = Random.Range(0, input.GetHashCode());
    }
}
