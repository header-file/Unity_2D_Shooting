using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    GameManager Manager;

    public Sprite[] Sprites;
    public Image HP_Bar;
    public GameObject Canvas;

    protected float Speed;
    protected float Health;
    protected float CurHP;
    protected float BeforeHP;
    protected Vector3 MidPoint;

    SpriteRenderer SpriteRenderer;

    bool IsBarVisible;
    
    
    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.Find("Player");
        MidPoint = player.transform.position;
        
        IsBarVisible = false;
        HP_Bar.fillAmount = 1.0f;
        Canvas.SetActive(false);

        GameObject manager = GameObject.Find("GameManager");
        Manager = manager.gameObject.GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        transform.RotateAround(MidPoint, Vector3.forward, Time.deltaTime * Speed);

        //Vector3 pos = transform.position;
        //pos.x -= Speed * 00.01f;

        //HP_Bar.fillAmount = Mathf.Lerp(BeforeHP / Health, CurHP / Health, Time.deltaTime);
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
            ic.SetValue((int)Random.Range(Health, Health * 3));

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
