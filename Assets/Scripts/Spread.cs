using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : Bullet
{
    public float Duration;


    public float GetDuration() { return Duration; }

    public void SetDuration(float time) { Duration = time; }


    void Awake()
    {
        Damage = 2.0f;
        Type = BulletType.SPREAD;
        //Damage *= (float)GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
        Duration = GameManager.Inst().UpgManager.GetBData((int)Type).GetDuration();
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
