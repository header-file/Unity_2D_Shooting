using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    SpriteRenderer Renderer;
    bool IsStart;
    Vector3 MaxScale;

    void Awake()
    {
        IsStart = false;
        Renderer = GetComponent<SpriteRenderer>();
        MaxScale = new Vector3(15.0f, 15.0f, 1.0f);
    }

    public void BombStart()
    {
        IsStart = true;
        transform.localScale = Vector3.one;
        Renderer.material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(GameManager.Inst().ShtManager.GetColorSelection(2)));
    }

    void Update()
    {
        if (!IsStart)
            return;

        transform.localScale = Vector3.Lerp(transform.localScale, MaxScale, Time.deltaTime * 0.5f);

        if (transform.localScale.x > 13.0f)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Enemy.EnemyType type = (Enemy.EnemyType)enemy.GetEnemyType();
            if (type == Enemy.EnemyType.BOSS)
                enemy.OnHit(100.0f);
            else
                enemy.OnHit(999999.0f);
        }
    }
}
