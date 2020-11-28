﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        SMALL = 0,
        MEDIUM = 1,
        LARGE = 2
    };

    GameManager Manager;

    public Sprite[] Sprites;
    public Image HP_Bar;
    public GameObject Canvas;

    protected float Speed;
    protected float Health;
    protected float CurHP;
    protected float BeforeHP;
    protected Vector3 MidPoint;
    protected EnemyType Type;

    SpriteRenderer SpriteRenderer;
    HitArea HitArea;

    bool IsBarVisible;
    Vector3 TargetPosition;

    public int GetEnemyType() { return (int)Type; }
    public Vector3 GetTargetPosition() { return TargetPosition; }

    public void SetTargetPosition(Vector3 Pos) { TargetPosition = Pos; }
    
    
    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.Find("Player");
        MidPoint = player.transform.position;
        
        IsBarVisible = false;
        HP_Bar.fillAmount = 1.0f;
        Canvas.SetActive(false);

        TargetPosition = Vector3.zero;

        GameObject manager = GameObject.Find("GameManager");
        Manager = manager.gameObject.GetComponent<GameManager>();

        HitArea = transform.GetChild(1).GetComponent<HitArea>();
    }

    void FixedUpdate()
    {
        //transform.RotateAround(MidPoint, Vector3.forward, Time.deltaTime * Speed);
        Vector3 pos = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
        transform.position = pos;
    }

    void OnHit(float Damage)
    {
        if (!IsBarVisible)
            ShowHPBar();
        else
        {
            CancelInvoke("HideHPBar");
            Invoke("HideHPBar", 1.0f);
        }

        BeforeHP = CurHP;
        CurHP -= Damage;
        HP_Bar.fillAmount = CurHP / Health;

        //SlowGame();

        SpriteRenderer.sprite = Sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (CurHP <= 0)
        {
            GameObject coin = Manager.ObjManager.MakeObj("Coin");
            coin.transform.position = transform.position;
            Item_Coin ic = coin.gameObject.GetComponent<Item_Coin>();
            ic.SetValue((int)Random.Range(Health, Health * 5));

            CurHP = Health;
            gameObject.SetActive(false);

            GameManager.Inst().Camerashake.Vibrate(0.05f);
        }       
    }

    void ReturnSprite()
    {
        SpriteRenderer.sprite = Sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BlockBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float level = (float)GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetPowerLevel();
            float dmg = bullet.GetDamage() * level;
            collision.gameObject.SetActive(false);

            OnHit(dmg);
        }
        else if(collision.gameObject.tag == "Laser")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float level = (float)GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetPowerLevel();
            float dmg = bullet.GetDamage() * level;

            OnHit(dmg);
        }
        else if (collision.gameObject.tag == "PierceBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float level = (float)GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetPowerLevel();
            float dmg = bullet.GetDamage() * level;

            OnHit(dmg);
        }
        else if(collision.gameObject.name == "Bottom")
        {
            //GameManager.Inst().RedMask.gameObject.SetActive(true);
            GameManager.Inst().RedMask.gameObject.GetComponent<RedMask>().SetIsAlert(true);

            GameManager.Inst().Camerashake.Vibrate(0.05f);
            
            //아군 피격
            for(int i = 0; i < 5; i++)
            {
                if (HitArea.HitObjects[i] == null)
                    continue;
                Debug.Log(HitArea.HitObjects[i].gameObject.name);
                HitArea.HitObjects[i].GetComponent<SubWeapon>().Dead();
                HitArea.HitObjects[i] = null;
            }

            //적 사망 처리
            CurHP = Health;
            gameObject.SetActive(false);
        }
    }

    void ShowHPBar()
    {
        IsBarVisible = true;
        Canvas.SetActive(true);
        Invoke("HideHPBar", 1.0f);
    }

    void HideHPBar()
    {
        Canvas.SetActive(false);
        IsBarVisible = false;
    }

    private int SetCoinValue()
    {
        return (int)Random.Range(0.0f, Health * 3);
    }

    //void SlowGame()
    //{
    //    GameObject gm = GameObject.Find("GameManager");
    //    GameManager manager = gm.gameObject.GetComponent<GameManager>();
    //    manager.SlowGame();
    //}
}
