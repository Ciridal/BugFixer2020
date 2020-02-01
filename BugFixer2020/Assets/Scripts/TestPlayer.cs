using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float nodeLength = 1;
    public GameObject camera;

    void Start()
    {
        this.transform.position += new Vector3(nodeLength, nodeLength) * .5f;
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + nodeLength, this.transform.position.z);

        if (Input.GetKeyUp(KeyCode.A))
            this.transform.position = new Vector3(this.transform.position.x - nodeLength, this.transform.position.y, this.transform.position.z);

        if (Input.GetKeyUp(KeyCode.S))
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - nodeLength, this.transform.position.z);

        if (Input.GetKeyUp(KeyCode.D))
            this.transform.position = new Vector3(this.transform.position.x + nodeLength, this.transform.position.y, this.transform.position.z);

        camera.GetComponent<CameraBehaviour>().Move();
    }
}
