using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Bullet
{
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
