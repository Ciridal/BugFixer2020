using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    Text text;
    GameObject player;
    public GameManager gameManager;
    public int currentScore;

    private void Start()
    {
        text = gameObject.GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        text.text = "Score: " + currentScore;

        if (gameManager == null)
            gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(gameManager != null)
        {
            currentScore = gameManager.GetScore();
            text.text = "Score: " + currentScore;
        }
        
    }


}
