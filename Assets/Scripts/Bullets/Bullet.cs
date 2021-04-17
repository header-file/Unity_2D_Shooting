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
        CHAIN = 6,
        EQUIP = 100,
    };
    
    public Color GlowColor;
    public bool IsVamp;
    public GameObject Vamp;
    public bool IsReinforce;

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

    public void BloodSuck(float dmg)
    {
        if (!IsVamp)
            return;

        if (Vamp.GetComponent<Player>())
        {
            float heal = dmg * GameManager.Inst().Player.GetItem(GameManager.Inst().UpgManager.BData[GameManager.Inst().Player.GetBulletType()].GetEquipIndex()).Value;

            GameManager.Inst().Player.Heal((int)heal);
        }
        else
        {
            SubWeapon sub = Vamp.GetComponent<SubWeapon>();
            float heal = dmg * GameManager.Inst().Player.GetItem(GameManager.Inst().UpgManager.BData[sub.GetBulletType()].GetEquipIndex()).Value;

            sub.Heal((int)heal);
        }
    }

    void OnDisable()
    {
        IsVamp = false;
        Vamp = null;
        IsReinforce = false;
    }
}
