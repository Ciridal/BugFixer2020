using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int damage;
    public float speed;
    Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    int TakeDamage(int dmg)
    {
        hp -= dmg;
        return hp;
    }

    void DealDamage()
    {
        player.TakeDamage(damage);
    }
}
