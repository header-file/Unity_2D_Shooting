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

    //public Sprite[] Sprites;
    public Image HP_Bar;
    public GameObject Canvas;

    public float SpeedMultiplier;

    protected float Speed;
    protected float Health;
    protected float CurHP;
    protected float BeforeHP;
    protected int Atk;
    protected Vector3 MidPoint;
    protected EnemyType Type;

    SpriteRenderer SpriteRenderer;
    Rigidbody2D Rig;
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
        Rig = GetComponent<Rigidbody2D>();

        SpeedMultiplier = 1.0f;

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

    public void StartMove()
    {
        Rig.velocity = Vector3.zero;
        Rig.AddForce(-transform.up * Speed, ForceMode2D.Impulse);
    }

    public void StartMove(float time)
    {
        Invoke("StartMove", time);
    }

    void FixedUpdate()
    {
        //Vector3 pos = Vector3.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime * SpeedMultiplier);
        //transform.position = pos;

    }

    public void OnHit(float Damage, bool isReinforced)
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

            //SpriteRenderer.sprite = Sprites[1];
            GetComponent<Animator>().SetTrigger("hit");
            Invoke("ReturnInvincible", 0.1f);
        }
        else
        {
            GameManager.Inst().UiManager.BossHPBar.fillAmount = CurHP / Health;
            GameManager.Inst().UiManager.BossHPBarText.text = CurHP.ToString() + "/" + Health.ToString();

            gameObject.GetComponent<EnemyB>().HitEffect();
            Invoke("ReturnInvincible", 0.1f);
        }

        //DamageText
        GameManager.Inst().TxtManager.ShowDmgText(gameObject.transform.position, Damage, (int)TextManager.DamageType.BYPLYAER, isReinforced);

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
            MakeResource();

            //퀘스트 처리
            GameManager.Inst().QstManager.QuestProgress((int)QuestManager.QuestType.KILL, (int)Type, 1);

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

            if (Type != EnemyType.BOSS)
            {
                GameManager.Inst().StgManager.AddBossCount();
                gameObject.SetActive(false);

                rand = Random.Range(0, 2);
                if (rand > 0)
                    GameManager.Inst().MakeReinforce(-1, -1, transform);
                else
                    GameManager.Inst().MakeEquip(-1, -1, transform);
            }
            else
            {
                gameObject.GetComponent<EnemyB>().Die();
                rand = Random.Range(0, 2);
                if (rand > 0)
                    GameManager.Inst().MakeReinforce(-1, -1, transform);
                else
                    GameManager.Inst().MakeEquip(-1, -1, transform);

                //Boss 사망 카운트 추가
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
            ic.SetIsAbsorb(false);
            //ic.InvokeAbsorb();
        }
    }

    void MakeResource()
    {
        int rand = Random.Range(1 + (int)Type, 3 + (int)Type);
        if (Type == EnemyType.BOSS)
            rand = Random.Range(5, 8);

        for(int i = 0; i < rand; i++)
        {
            GameObject res = GameManager.Inst().ObjManager.MakeObj("Resource");
            if (res == null)
                return;

            Item_Resource resource = res.GetComponent<Item_Resource>();
            resource.transform.position = transform.position;

            Vector3 pos = transform.position;
            pos.x += Mathf.Cos(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;
            pos.y += Mathf.Sin(Mathf.Deg2Rad * Random.Range(0.0f, 180.0f)) * 1.0f;

            resource.SetValue(Random.Range(1, 3 + (int)Type));
            resource.SetColor();
            resource.TargetPosition = pos;
            resource.IsScatter = true;
        }
    }

    void ReturnSprite()
    {
        //SpriteRenderer.sprite = Sprites[0];
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
            float damage = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetDamage();
            float atk = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetAtk();
            float dmg = damage + atk;
            if (bullet.IsReinforce)
                dmg *= 2;
            bullet.BloodSuck(dmg);

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = bullet.transform.position;

            OnHit(dmg, bullet.IsReinforce);

            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Laser")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetDamage();
            float atk = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetAtk();
            float dmg = damage + atk;
            if (bullet.IsReinforce)
                dmg *= 2;
            bullet.BloodSuck(dmg);

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = collision.gameObject.GetComponent<BoxCollider2D>().ClosestPoint(transform.position);

            OnHit(dmg, bullet.IsReinforce);
        }
        else if (collision.gameObject.tag == "PierceBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetDamage();
            float atk = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetAtk();
            float dmg = damage + atk;
            if (bullet.IsReinforce)
                dmg *= 2;
            bullet.BloodSuck(dmg);

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = bullet.transform.position;

            OnHit(dmg, bullet.IsReinforce);
        }
        else if (collision.gameObject.tag == "Chain")
        {
            Chain bullet = collision.gameObject.GetComponent<Chain>();
            bullet.HitEnemy(gameObject);

            float damage = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetDamage();
            float atk = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetAtk();
            float dmg = damage + atk;
            if (bullet.IsReinforce)
                dmg *= 2;
            bullet.BloodSuck(dmg);

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = bullet.transform.position;

            OnHit(dmg, bullet.IsReinforce);
        }
        else if(collision.gameObject.tag == "EquipBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().Player.GetItem(bullet.InventoryIndex).Value;
            //float damage = 0;

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = bullet.transform.position;

            OnHit(damage, bullet.IsReinforce);
        }
        else if(collision.gameObject.name == "Bottom")
        {
            GameManager.Inst().UiManager.RedMask.gameObject.GetComponent<RedMask>().SetIsAlert(true);
            //GameManager.Inst().Camerashake.Vibrate(0.05f);
            
            //아군 피격
            for(int i = 0; i < 5; i++)
            {
                if (HitArea.HitObjects[i] == null)
                    continue;
                SubWeapon sub = HitArea.HitObjects[i].GetComponent<SubWeapon>();
                if(sub == null)
                    HitArea.HitObjects[i].GetComponent<Player>().Damage(Atk);
                else
                sub.Damage(Atk);
                HitArea.HitObjects[i] = null;
            }

            //적 사망 처리
            CurHP = Health;
            gameObject.SetActive(false);

            //GameManager.Inst().Camerashake.Vibrate(0.05f);
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
