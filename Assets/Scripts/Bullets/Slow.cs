using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Bullet
{
    public CircleCollider2D Col;
    public GameObject Area;

    void Awake()
    {
        Type = BulletType.SLOW;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy.SpeedMultiplier >= 1.0f)
            {
                enemy.SpeedMultiplier = 0.5f;

                if (enemy.GetEnemyType() == (int)Enemy.EnemyType.BOSS)
                    enemy.SpeedMultiplier = 0.75f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy.SpeedMultiplier < 1.0f)
                enemy.SpeedMultiplier = 1.0f;
        }
    }
}
