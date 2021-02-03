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
        LARGE = 2,
        BOSS = 3,
    };

    public Sprite[] Sprites;
    public Image HP_Bar;
    public GameObject Canvas;

    protected float Speed;
    protected float Health;
    protected float CurHP;
    protected float BeforeHP;
    protected int Atk;
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
    }
    
    
    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.Find("Player");
        MidPoint = player.transform.position;
        
        IsBarVisible = false;
        if(HP_Bar != null)
        {
            HP_Bar.fillAmount = 1.0f;
            Canvas.SetActive(false);
            HitArea = transform.GetChild(1).GetComponent<HitArea>();
        }

        TargetPosition = Vector3.zero;
        IsInvincible = false;
    }

    void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
        transform.position = pos;
    }

    public void OnHit(float Damage)
    {
        if (IsInvincible)
            return;

        BeforeHP = CurHP;
        CurHP -= Damage;

        if (Type != EnemyType.BOSS)
        {
            if (!IsBarVisible)
                ShowHPBar();
            else
            {
                CancelInvoke("HideHPBar");
                Invoke("HideHPBar", 1.0f);
            }

            HP_Bar.fillAmount = CurHP / Health;

            SpriteRenderer.sprite = Sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }
        else
        {
            GameManager.Inst().StgManager.HPBar.fillAmount = CurHP / Health;
            GameManager.Inst().StgManager.HPBarText.text = CurHP.ToString() + "/" + Health.ToString();

            gameObject.GetComponent<EnemyB>().HitEffect();
            Invoke("ReturnInvincible", 0.1f);
        }

        //DamageText
        GameManager.Inst().TxtManager.ShowDmgText(gameObject.transform.position, Damage);

        IsInvincible = true;

        if (CurHP <= 0)
        {
            int rand = Random.Range(0, 5);

            if(rand >= 1)
                MakeCoin();
            else
            {
                if (Type == EnemyType.BOSS)
                    MakeCoin();
            }

            if(Type == EnemyType.LARGE)
            {
                rand = Random.Range(0, 2);
                GameObject item;

                switch (rand)
                {
                    case 0:
                        item = GameManager.Inst().ObjManager.MakeObj("Shield");
                        item.transform.position = transform.position;
                        item.GetComponent<Item>().StartAbsorb(1.0f);
                        break;
                    case 1:
                        item = GameManager.Inst().ObjManager.MakeObj("ItemBomb");
                        item.transform.position = transform.position;
                        item.GetComponent<Item>().StartAbsorb(1.0f);
                        break;
                }
            }

            GameObject explosion = GameManager.Inst().ObjManager.MakeObj("Explosion");
            explosion.transform.position = transform.position;
            
            CurHP = Health;

            GameManager.Inst().Camerashake.Vibrate(0.05f);
            if (Type != EnemyType.BOSS)
            {
                GameManager.Inst().StgManager.AddBossCount();
                gameObject.SetActive(false);

                rand = Random.Range(0, 2);
                if(rand > 0)
                    GameManager.Inst().MakeEquipment(-1, -1, transform);
            }
            else
            {
                gameObject.GetComponent<EnemyB>().Die();
                GameManager.Inst().MakeEquipment(-1, -1, transform);

                GameManager.Inst().StgManager.IsBoss = false;
                GameManager.Inst().StgManager.RestartStage();
            }
        }       
    }

    void MakeCoin()
    {
        int rand = Random.Range(2 + (int)Type, 5 + (int)Type);
        if (Type == EnemyType.BOSS)
            rand = Random.Range(10, 20);

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
        ReturnInvincible();
    }

    void ReturnInvincible()
    {
        IsInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitArea")
            return;

        if (collision.gameObject.tag == "BlockBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetDamage();
            float atk = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetAtk();
            float dmg = damage * (1 + (atk / 100.0f));
            collision.gameObject.SetActive(false);

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = bullet.transform.position;

            OnHit(dmg);
        }
        else if (collision.gameObject.tag == "Laser")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetDamage();
            float atk = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetAtk();
            float dmg = damage * (1 + (atk / 100.0f));

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            /*RaycastHit rayHit;
            if (Physics.Raycast(bullet.transform.position, bullet.transform.up, out rayHit))
                hit.transform.position = rayHit.point;*/
            hit.transform.position = collision.gameObject.GetComponent<BoxCollider2D>().ClosestPoint(transform.position);

            OnHit(dmg);
        }
        else if (collision.gameObject.tag == "PierceBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetDamage();
            float atk = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetAtk();
            float dmg = damage * (1 + (atk / 100.0f));

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = bullet.transform.position;

            OnHit(dmg);
        }
        else if (collision.gameObject.tag == "Chain")
        {
            Chain bullet = collision.gameObject.GetComponent<Chain>();
            bullet.HitEnemy(gameObject);

            float damage = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetDamage();
            float atk = GameManager.Inst().UpgManager.GetBData(bullet.GetBulletType()).GetAtk();
            float dmg = damage * (1 + (atk / 100.0f));

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = bullet.transform.position;

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

            GameManager.Inst().Camerashake.Vibrate(0.05f);
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
