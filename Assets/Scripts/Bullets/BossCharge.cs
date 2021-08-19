using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharge : Bullet
{
    GameObject ChargePos;

    float ChargeTime;
    Vector2 Dir;
    const float TickRate = 1.0f / 60.0f;
    float ReloadTime;
    float Scale;
    int Damage;

    
    void Awake()
    {
        Type = BulletType.CHARGE;
        Damage = 5 * GameManager.Inst().StgManager.Stage;
        ChargeTime = 0.0f;
    }

    public void StartCharge(GameObject pos)
    {
        ReloadTime = GameManager.Inst().UpgManager.BData[(int)Type].GetReloadTime();
        gameObject.transform.localScale = Vector3.one * 0.1f;

        InvokeRepeating("Charging", 0.0f, TickRate);
        ReloadTime = 1.0f;
        Scale = 3.0f;
        ChargePos = pos;
    }

    public void Charging()
    {
        if (ChargeTime >= ReloadTime)
        {
            ChargeTime = 0.0f;
            CancelInvoke("Charging");
            Release();
        }
        else
        {
            ChargeTime += ReloadTime / 60.0f;
            transform.localScale = Vector3.one * ChargeTime * Scale;

            transform.position = ChargePos.transform.position;
            transform.rotation = ChargePos.transform.rotation;

            if(ChargeTime <= ReloadTime / 2.0f)
                Dir = Vector3.Normalize(GameManager.Inst().Player.transform.position - ChargePos.transform.position);

            //Dir = ChargePos.transform.up;
        }
    }

    void Release()
    {
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        rig.AddForce(Dir * GameManager.Inst().UpgManager.BData[(int)Type].GetSpeed(), ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<Player>().Damage(Damage);
        }
    }

    void HitEffect(GameObject obj)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = obj.transform.position;
    }
}
