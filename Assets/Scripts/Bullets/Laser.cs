﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    Animator Anim;
    bool IsStopped;

    void Awake()
    {
        //Damage = 8.0f;
        Type = BulletType.LASER;
        GetComponent<SpriteRenderer>().color = Color.white;
        //Damage *= (float)GameManager.Inst().UpgManager.GetBData((int)Type).GetPowerLevel();
        IsStopped = false;
    }

    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsStopped && Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            Anim.speed = 0.0f;
            IsStopped = true;

            float stopTime = GameManager.Inst().UpgManager.BData[(int)Type].GetDuration();
            Invoke("Restart", stopTime);
        }

        if(Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            Destroy();
    }

    void Restart()
    {
        Anim.speed = 1.0f;
    }

    void Destroy()
    {
        GameManager.Inst().IptManager.SetIsAbleControl(true);
        IsStopped = false;
        gameObject.SetActive(false);
    }
}
