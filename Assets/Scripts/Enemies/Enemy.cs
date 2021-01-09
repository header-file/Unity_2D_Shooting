using System.Collections;
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

    public Sprite[] Sprites;
    public Image HP_Bar;
    public GameObject Canvas;

    protected float Speed;
    protected float Health;
    protected float CurHP;
    protected float BeforeHP;
    protected int Atk;
    protected float AtkSpeed;
    protected Vector3 MidPoint;
    protected EnemyType Type;

    SpriteRenderer SpriteRenderer;
    HitArea HitArea;

    bool IsBarVisible;
    Vector3 TargetPosition;
    bool IsInvincible;

    public int GetEnemyType() { return (int)Type; }
    public Vector3 GetTargetPosition() { return TargetPosition; }

    public void SetTargetPosition(Vector3 Pos) { TargetPosition = Pos; }

    public void SetDatas(List<Dictionary<string, object>> data, int type)
    {
        Type = (EnemyType)type;

        BeforeHP = CurHP = Health = float.Parse(data[type]["HP"].ToString());
        Speed = float.Parse(data[type]["Speed"].ToString());
        Atk = int.Parse(data[type]["Atk"].ToString());
        AtkSpeed = float.Parse(data[type]["AtkSpd"].ToString());
    }
    
    
    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.Find("Player");
        MidPoint = player.transform.position;
        
        IsBarVisible = false;
        HP_Bar.fillAmount = 1.0f;
        Canvas.SetActive(false);

        TargetPosition = Vector3.zero;

        HitArea = transform.GetChild(1).GetComponent<HitArea>();

        IsInvincible = false;
    }

    void FixedUpdate()
    {
        //transform.RotateAround(MidPoint, Vector3.forward, Time.deltaTime * Speed);
        Vector3 pos = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
        transform.position = pos;
    }

    void OnHit(float Damage)
    {
        if (IsInvincible)
            return;

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

        //DamageText
        GameManager.Inst().TxtManager.ShowDmgText(gameObject.transform.position, Damage);

        //SlowGame();

        SpriteRenderer.sprite = Sprites[1];
        IsInvincible = true;
        Invoke("ReturnSprite", 0.1f);

        if (CurHP <= 0)
        {
            int rand = Random.Range(0, 5);

            if(rand >= 0)
            {
                MakeCoin();
            }
            //else
            {
                GameManager.Inst().MakeEquipment(-1, -1, transform);
            }

            GameObject explosion = GameManager.Inst().ObjManager.MakeObj("Explosion");
            explosion.transform.position = transform.position;
            
            CurHP = Health;
            gameObject.SetActive(false);

            GameManager.Inst().Camerashake.Vibrate(0.05f);
        }       
    }

    void MakeCoin()
    {
        int rand = Random.Range(2, 5);

        for(int i = 0; i <= rand; i++)
        {
            GameObject coin = GameManager.Inst().ObjManager.MakeObj("Coin");
            coin.transform.position = transform.position;

            Vector3 pos = transform.position;
            pos.x += Mathf.Cos(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;
            pos.y += Mathf.Sin(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;

            Item_Coin ic = coin.gameObject.GetComponent<Item_Coin>();
            ic.SetValue((int)Random.Range(Health * 0.1f, Health));
            ic.SetTargetPosition(pos);
            ic.SetIsScatter(true);
            ic.InvokeAbsorb();
        }
    }

    void ReturnSprite()
    {
        SpriteRenderer.sprite = Sprites[0];
        IsInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlockBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetDamage();
            float atk = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetAtk();
            float dmg = damage * (1 + (atk / 100.0f));
            collision.gameObject.SetActive(false);

            OnHit(dmg);
        }
        else if (collision.gameObject.tag == "Laser")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetDamage();
            float atk = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetAtk();
            float dmg = damage * (1 + (atk / 100.0f));

            OnHit(dmg);
        }
        else if (collision.gameObject.tag == "PierceBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetDamage();
            float atk = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetAtk();
            float dmg = damage * (1 + (atk / 100.0f));

            OnHit(dmg);
        }
        else if (collision.gameObject.tag == "Chain")
        {
            Chain bullet = collision.gameObject.GetComponent<Chain>();
            bullet.HitEnemy();
            float damage = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetDamage();
            float atk = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetAtk();
            float dmg = damage * (1 + (atk / 100.0f));

            OnHit(dmg);
        }
        else if(collision.gameObject.name == "Bottom")
        {
            GameManager.Inst().RedMask.gameObject.GetComponent<RedMask>().SetIsAlert(true);
            GameManager.Inst().Camerashake.Vibrate(0.05f);
            
            //아군 피격
            for(int i = 0; i < 5; i++)
            {
                if (HitArea.HitObjects[i] == null)
                    continue;
                HitArea.HitObjects[i].GetComponent<SubWeapon>().Damage(Atk);
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

    //void SlowGame()
    //{
    //    GameObject gm = GameObject.Find("GameManager");
    //    GameManager manager = gm.gameObject.GetComponent<GameManager>();
    //    manager.SlowGame();
    //}
}
