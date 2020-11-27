using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Bullet
{
    float ChargeTime;
    Vector2 Dir;
    const float TickRate = 1.0f / 60.0f;
    Player Player;
    float ReloadTime;
    float Scale;

    void Awake()
    {
        Damage = 3.0f;
        Type = BulletType.CHARGE;
        //Damage *= (float)GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
        ChargeTime = 0.0f;

        Player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void StartCharge(float scale)
    {
        ReloadTime = GameManager.Inst().UpgManager.GetBData((int)Type).GetReloadTime();
        gameObject.transform.localScale = Vector3.one * 0.1f;

        InvokeRepeating("Charging", 0.0f, TickRate);
        Scale = scale;
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

            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 MPos = new Vector2(MousePos.x, MousePos.y);

            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 norm = (MPos - pos) / Vector2.Distance(MPos, pos);
            float angle = Vector2.Angle(Vector2.up, norm);
            if (MPos.x > transform.position.x)
                angle *= -1;

            transform.position = Player.GetChargePos().gameObject.transform.position;
            
            Dir = norm;
        }
    }

    void Release()
    {
        Rigidbody2D rig = GetComponent<Rigidbody2D>();
        rig.AddForce(Dir * GameManager.Inst().UpgManager.GetBData((int)Type).GetSpeed(), ForceMode2D.Impulse);
    }
}
