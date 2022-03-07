using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Bullet
{
    public SpriteRenderer SpriteRenderer;

    void Awake()
    {
        Type = BulletType.EXPLOSION;
    }

    public void SetData(Bullet bullet)
    {
        IsReinforce = bullet.IsReinforce;
        IsVamp = bullet.IsVamp;
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
}
