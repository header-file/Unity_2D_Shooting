﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        NONE = -1,
        NORMAL = 0,
        SPREAD = 1,
        MISSILE = 2,
        LASER = 3,
        CHARGE = 4,
        BOOMERANG = 5,
        CHAIN = 6,
    };
    
    public Color GlowColor;

    //protected float Damage;
    protected BulletType Type;

    //public float GetDamage() { return Damage; }
    public int GetBulletType() { return (int)Type; }

    //public void SetDamage(int PowerLevel, int Power) { Damage = PowerLevel * Power; }
    public void SetBulletType(int t) { Type = (BulletType)t; }
    
    public void Shoot(Vector2 Direction)
    {
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        float spd = 1.0f + (GameManager.Inst().UpgManager.BData[(int)Type].GetSpd() / 500.0f);
        float speed = GameManager.Inst().UpgManager.BData[(int)Type].GetSpeed() * spd;
        rig.AddForce(Direction * speed, ForceMode2D.Impulse);
    }
}
