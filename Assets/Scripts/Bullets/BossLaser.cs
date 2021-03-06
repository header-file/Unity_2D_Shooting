﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : Bullet
{
    public int Damage;

    Animator Anim; 
    bool IsStopped;

    void Awake()
    {
        Type = BulletType.LASER;
        GetComponent<SpriteRenderer>().color = Color.white;
        Damage = 20;
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

        if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            Destroy();
    }

    void Restart()
    {
        Anim.speed = 1.0f;
    }

    void Destroy()
    {
        gameObject.SetActive(false);
        IsStopped = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
        {
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = collision.gameObject.GetComponent<BoxCollider2D>().ClosestPoint(transform.position);
        }
    }
}
