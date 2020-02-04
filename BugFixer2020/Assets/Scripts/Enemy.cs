using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public int damage;
    public float speed;
    GameObject player;
    float dist;
    float lastHit;
    public int damageDelay;
    Grid gridManager;
    public Pathfinding pathfinding;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (hp <= 0)
            Death();

        if(player)
        {
            dist = Vector3.Distance(player.transform.position, transform.position);

            if(dist < 1 && Time.time > lastHit + damageDelay)
            {
                DealDamage();
                lastHit = Time.time;
            }
        }
    }

    public int TakeDamage(int dmg)
    {
        hp -= dmg;
        return hp;
    }

    void DealDamage()
    {

        player.GetComponent<TestPlayer>().TakeDamage(damage);
        Debug.Log("Monster hit player");
    }

    void Death()
    {
        player.GetComponent<TestPlayer>().AddScore(1);
        Destroy(gameObject);
    }

    public void SetGridPosition(int x, int y, bool walkable = false)
    {
        if (walkable)
        {
            PathNode nearest = gridManager.FindNearestWalkable(gridManager.GetNode(x, y));
            this.transform.position = new Vector3(nearest.WorldPosition().x, nearest.WorldPosition().y, this.transform.position.z);
        }
        else
        {
            PathNode target = gridManager.GetNode(x, y);
            this.transform.position = new Vector3(target.WorldPosition().x, target.WorldPosition().y, this.transform.position.z);
        }
    }
}
