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
        CHARGE = 4,
        BOOMERANG = 5,
        SPLIT = 6,
    };

    public static int MAXBULLETS = 7;

    public Color GlowColor;

    //protected float Damage;
    protected BulletType Type;

    //public float GetDamage() { return Damage; }
    public int GetBulletType() { return (int)Type; }

    //public void SetDamage(int PowerLevel, int Power) { Damage = PowerLevel * Power; }
    public void SetBulletType(int t) { Type = (BulletType)t; }
    
    void Start()
    {
        
    }

    public void Shoot(Vector2 Direction)
    {
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        float spd = (1.0f + (GameManager.Inst().UpgManager.GetBData((int)Type).GetSpd() / 100.0f));
        float speed = GameManager.Inst().UpgManager.GetBData((int)Type).GetSpeed() * spd;
        rig.AddForce(Direction * speed, ForceMode2D.Impulse);
    }
}
