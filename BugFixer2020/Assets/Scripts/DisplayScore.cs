using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    Text text;
    GameObject player;
    public int currentScore;

    private void Start()
    {
        text = gameObject.GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        text.text = "Score: " + currentScore;
    }

    private void Update()
    {

        currentScore = player.GetComponent<TestPlayer>().GetScore();
        text.text = "Score: " + currentScore;
    }


}
