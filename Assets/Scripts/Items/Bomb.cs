using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    SpriteRenderer Renderer;
    LineRenderer LineRenderer;
    bool IsStart;
    Vector3 MaxScale;
    int Segment;

    void Awake()
    {
        IsStart = false;
        Renderer = GetComponent<SpriteRenderer>();
        MaxScale = new Vector3(10.0f, 10.0f, 1.0f);
        Segment = 60;
    }

    public void BombStart()
    {
        IsStart = true;
        transform.localScale = Vector3.one;
        Renderer.material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(GameManager.Inst().ShtManager.GetColorSelection(2)));

        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(GameManager.Inst().ShtManager.GetColorSelection(2)));
        LineRenderer.positionCount = Segment;
        LineRenderer.widthMultiplier = 0.2f;
    }

    void Update()
    {
        if (!IsStart)
            return;

        transform.localScale = Vector3.Lerp(transform.localScale, MaxScale, Time.deltaTime * 0.5f);
        DrawRing(transform.localScale.x);

        if (transform.localScale.x > 8.0f)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Enemy.EnemyType type = (Enemy.EnemyType)enemy.GetEnemyType();
            if (type == Enemy.EnemyType.BOSS)
                enemy.OnHit(100.0f, false);
            else
                enemy.OnHit(999999.0f, false);
        }
    }

    void DrawRing(float radius)
    {
        float x = 0.0f;
        float y = 0.0f;
        float angle = 180.0f;

        for (int i = 0; i < Segment; i++)
        {
            x = transform.position.x + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = transform.position.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            LineRenderer.SetPosition(i, new Vector3(x, y, 0.0f));
            angle += (360.0f / Segment);
        }
    }
}
