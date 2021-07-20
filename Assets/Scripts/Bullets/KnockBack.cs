using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : Bullet
{
    public Vector3 Direction;

    void Awake()
    {
        Type = BulletType.KNOCKBACK;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy.GetEnemyType() == (int)Enemy.EnemyType.BOSS)
                return;

            enemy.StopMove(0.5f);
            enemy.GetComponent<Rigidbody2D>().AddForce(Direction * 5.0f, ForceMode2D.Impulse);
        }
    }
}
