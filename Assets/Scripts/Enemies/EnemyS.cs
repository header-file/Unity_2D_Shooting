using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyS : Enemy
{
    void Start()
    {
        Type = EnemyType.SMALL;
    }

    public void Erase()
    {
        Rig.velocity = Vector2.zero;
        Invoke("Disappear", Time.deltaTime);
    }

    void Disappear()
    {
        Color color = SpriteRenderer.color;
        color.a -= (Time.deltaTime * 2.0f);
        SpriteRenderer.color = color;

        if (color.a < 0.0f)
            gameObject.SetActive(false);
        else
            Invoke("Disappear", Time.deltaTime);
    }
}
