using System.Collections;
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
        CHARGE = 4
    };

    public static int MAXBULLETS = 5;

    protected float Damage;

    protected BulletType Type;

    public float GetDamage() { return Damage; }
    public int GetBulletType() { return (int)Type; }

    public void SetDamage(int PowerLevel) { Damage *= (float)PowerLevel; }
    public void SetBulletType(int t) { Type = (BulletType)t; }

    
    void Start()
    {
        
    }

    public void Shoot(Vector2 Direction)
    {
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        rig.AddForce(Direction * GameManager.Inst().UpgManager.GetBData((int)Type).GetSpeed(), ForceMode2D.Impulse);
    }
}
