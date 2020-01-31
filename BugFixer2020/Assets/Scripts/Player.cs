using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp;
    public int damage;
    public float speed;

    void Update()
    {

    }

    public int TakeDamage(int dmg)
    {
        hp -= dmg;
        return hp;
    }

}
