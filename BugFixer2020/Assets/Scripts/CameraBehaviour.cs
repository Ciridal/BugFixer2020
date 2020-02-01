using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject player;
    private float zValue = -10;

    public float minZValue = -10;
    public float maxZValue = -30;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            zValue += Input.GetAxis("Mouse ScrollWheel");

        if (zValue >= minZValue)
            zValue = minZValue;

        if (zValue <= maxZValue)
            zValue = maxZValue;

    }

    public void Move()
    {
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zValue);
    }
}
