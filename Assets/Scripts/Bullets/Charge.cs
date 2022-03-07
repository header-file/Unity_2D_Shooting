using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Bullet
{
    public CircleCollider2D Col;

    GameObject ChargePos;

    float ChargeTime;
    Vector2 Dir;
    const float TickRate = 1.0f / 60.0f;
    Player Player;
    float ReloadTime;
    float Scale;

    public void SetChargePos(GameObject obj) { ChargePos = obj; }

    void Awake()
    {
        //Damage = 3.0f;
        Type = BulletType.CHARGE;
        GetComponent<SpriteRenderer>().color = Color.white;
        //Damage *= (float)GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
        ChargeTime = 0.0f;

        Player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void StartCharge(float scale)
    {
        ReloadTime = GameManager.Inst().UpgManager.BData[(int)Type].GetReloadTime();
        gameObject.transform.localScale = Vector3.one * 0.1f;

        InvokeRepeating("Charging", 0.0f, TickRate);
        Scale = scale * 0.5f;
        Col.enabled = false;
    }

    public void Charging()
    {
        if (ChargeTime >= ReloadTime)
        {
            ChargeTime = 0.0f;
            CancelInvoke("Charging");
            Col.enabled = true;
            Release();
        }
        else
        {
            ChargeTime += ReloadTime / 60.0f;
            transform.localScale = Vector3.one * ChargeTime * Scale;

            transform.position = ChargePos.transform.position;
            transform.rotation = ChargePos.transform.rotation;
            
            Dir = ChargePos.transform.up;
        }
    }

    void Release()
    {
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        rig.AddForce(Dir * GameManager.Inst().UpgManager.BData[(int)Type].GetSpeed(), ForceMode2D.Impulse);
    }
}
