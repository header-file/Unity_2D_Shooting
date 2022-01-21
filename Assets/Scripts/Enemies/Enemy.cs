using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.U2D.Animation;

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
    public HitArea HitArea;
    public SpriteRenderer SpriteRenderer;
    public GameObject Body;
    public CircleCollider2D Collider;
    public SpriteResolver Skin;
    public AnimationCurve Curve;
    public ParticleSystem FallEffect;

    public float SpeedMultiplier;
    public float RotGyesu;
    public float CurveTime;
    public bool IsDot;

    protected float Speed;
    protected float Health;
    protected float CurHP;
    protected float BeforeHP;
    protected int Atk;
    protected EnemyType Type;
    protected Rigidbody2D Rig;    

    bool IsBarVisible;
    Vector3 TargetPosition;
    bool IsInvincible;
    bool IsReflected;
    bool IsStop;
    float SizeTimer;
    float BaseRad;
    Vector3 BodyScale;


    public int GetEnemyType() { return (int)Type; }
    public Vector3 GetTargetPosition() { return TargetPosition; }

    public void SetTargetPosition(Vector3 Pos) { TargetPosition = Pos; }

    public void SetDatas(List<Dictionary<string, object>> data, int type)
    {
        Type = (EnemyType)type;
        BeforeHP = CurHP = Health = float.Parse(data[type + (GameManager.Inst().StgManager.Stage - 1) * 4]["HP"].ToString());
        Speed = float.Parse(data[type + (GameManager.Inst().StgManager.Stage - 1) * 4]["Speed"].ToString());
        Atk = int.Parse(data[type + (GameManager.Inst().StgManager.Stage - 1) * 4]["Atk"].ToString());
    }
    
    void Awake()
    {
        Rig = GetComponent<Rigidbody2D>();

        SpeedMultiplier = 1.0f;
        RotGyesu = 1.0f;
        SizeTimer = 0.0f;
        if(Collider != null)
            BaseRad = Collider.radius;
        BodyScale = Vector3.one;
        
        IsBarVisible = false;
        if(HP_Bar != null)
        {
            HP_Bar.fillAmount = 1.0f;
            Canvas.SetActive(false);
            HitArea = transform.GetChild(1).GetComponent<HitArea>();
        }

        TargetPosition = Vector3.zero;
        IsInvincible = false;

        IsReflected = false;
        CurveTime = 0.0f;
        IsDot = false;
    }

    void Start()
    {
        if (FallEffect.gameObject != null)
            FallEffect.gameObject.SetActive(false);
    }

    void Update()
    {
        Canvas.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        if (GameManager.Inst().StgManager.Stage == 4)
            Bloom();
    }

    void Bloom()
    {
        SizeTimer += Time.deltaTime;

        if (SizeTimer <= 0.1f)
        {
            Body.transform.localScale = Vector3.one * (0.5f + SizeTimer * 5.0f) * 0.75f;
            Collider.radius = BaseRad * (1.0f + SizeTimer * 10.0f) * 0.75f;
        }
        else if (SizeTimer > 1.5f && SizeTimer <= 1.6f)
        {
            Body.transform.localScale = Vector3.one * (1.0f + (1.5f - SizeTimer) * 5.0f) * 0.75f;
            Collider.radius = BaseRad * (2.0f + (1.5f - SizeTimer) * 10.0f) * 0.75f;
        }
        else if (SizeTimer >= 3.0f)
            SizeTimer = 0.0f;
    }

    public void StartMove()
    {
        switch(GameManager.Inst().StgManager.Stage)
        {
            case 1:
                Rig.velocity = -transform.up * Speed * SpeedMultiplier;
                break;
            case 2:
                Rig.velocity = -transform.up * Speed * Curve.Evaluate(CurveTime) * SpeedMultiplier;
                CurveTime += Time.deltaTime;
                break;
            case 3:
                if (IsReflected)
                    Rig.velocity = -transform.up * 5.0f * SpeedMultiplier ;
                else
                {
                    if(transform.position.x > TargetPosition.x)
                        Rig.velocity = -transform.right * Speed * SpeedMultiplier;
                    else if(transform.position.x < TargetPosition.x)
                        Rig.velocity = transform.right * Speed * SpeedMultiplier;

                    if (Mathf.Abs(transform.position.x - TargetPosition.x) < 0.05f)
                    {
                        IsReflected = true;
                        FallEffect.gameObject.SetActive(true);
                        FallEffect.Play();
                    }
                }
                break;
            case 4:
                if (transform.rotation.eulerAngles.z > 60.0f &&
                    transform.rotation.eulerAngles.z < 300.0f)
                    RotGyesu *= -1.0f;

                transform.Rotate(0.0f, 0.0f, 0.5f * RotGyesu);

                Rig.velocity = -transform.up * Speed * SpeedMultiplier;
                break;
            case 5:
                BodyScale.x += (Time.deltaTime * Speed * 0.5f);
                Body.transform.localScale = BodyScale;

                Rig.velocity = -transform.up * Speed * SpeedMultiplier;
                break;
        }
    }

    public void StartMove(float time)
    {
        if (IsStop)
            return;

        InvokeRepeating("StartMove", 0.0f, time);
    }

    public void StopMove(float time)
    {
        IsStop = true;
        CancelInvoke("StartMove");
        Rig.velocity = Vector2.zero;
        Invoke("ReturnMove", time);
    }

    void ReturnMove()
    {
        IsStop = false;
        StartMove(0.0f);
    }

    public void OnHit(float Damage, bool isReinforced, Vector2 HitPoint, bool IsTrueDgm = false)
    {
        if (IsInvincible && !IsTrueDgm)
            return;

        GameManager.Inst().SodManager.PlayEffect("Enemy hit");

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

            GetComponent<Animator>().SetTrigger("hit");
            Invoke("ReturnInvincible", 0.1f);
        }
        else
        {
            GameManager.Inst().UiManager.MainUI.BossHPBar.fillAmount = CurHP / Health;
            GameManager.Inst().UiManager.MainUI.BossHPBarText.text = CurHP.ToString() + "/" + Health.ToString();

            gameObject.GetComponent<EnemyB>().HitEffect();
            Invoke("ReturnInvincible", 0.1f);
        }

        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = HitPoint;

        GameManager.Inst().TxtManager.ShowDmgText(HitPoint, Damage, (int)TextManager.DamageType.BYPLYAER, isReinforced);

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

            //폭발 이펙트
            GameObject explosion = GameManager.Inst().ObjManager.MakeObj("Explosion");
            explosion.transform.position = transform.position;

            IsReflected = false;
            IsDot = false;
            DotCheck();
            CurHP = Health;
            BodyScale = Vector3.one;
            CancelInvoke("StartMove");

            if (Type != EnemyType.BOSS)
            {
                GameManager.Inst().StgManager.AddBossCount();
                gameObject.SetActive(false);

                if (SceneManager.GetActiveScene().name == "Stage0" && (GameManager.Inst().Tutorials.Step == 1 || GameManager.Inst().Tutorials.Step == 67))
                {
                    GameManager.Inst().Tutorials.Step++;
                    return;
                }

                GameManager.Inst().SodManager.PlayEffect("Enemy die");

#if UNITY_EDITOR
                rand = Random.Range(0, 2);
                if (rand == 0)
                    GameManager.Inst().MakeReinforce(-1, -1, transform);
                else
                    GameManager.Inst().MakeEquip(-1, -1, transform);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
                rand = Random.Range(0, 20);
                if (rand == 0)
                    GameManager.Inst().MakeReinforce(-1, -1, transform);
                else if (rand == 1)
                    GameManager.Inst().MakeEquip(-1, -1, transform);
#endif
            }
            else
            {
                gameObject.GetComponent<EnemyB>().Die();
                rand = Random.Range(0, 2);
                if (rand > 0)
                    GameManager.Inst().MakeReinforce(-1, -1, transform);
                else
                    GameManager.Inst().MakeEquip(-1, -1, transform);

                if (GameManager.Inst().StgManager.BossDeathCounts[GameManager.Inst().StgManager.Stage - 1] < 10)
                    GameManager.Inst().StgManager.BossDeathCounts[GameManager.Inst().StgManager.Stage - 1]++;
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
            resource.InvokeDisappear();
        }
    }

    void ReturnSprite()
    {
        ReturnInvincible();
    }

    void ReturnInvincible()
    {
        IsInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetDamage();
            float atk = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetAtk();
            float dmg = damage * 9.0f + atk;
            if (bullet.IsReinforce)
                dmg *= 2;
            bullet.BloodSuck(dmg);

            Vector2 hitPoint = collision.ClosestPoint(gameObject.transform.position);

            OnHit(dmg, bullet.IsReinforce, hitPoint, true);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
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

            Vector2 hitPoint = collision.ClosestPoint(gameObject.transform.position);

            OnHit(dmg, bullet.IsReinforce, hitPoint);

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

            Vector2 hitPoint = collision.ClosestPoint(gameObject.transform.position);

            OnHit(dmg, bullet.IsReinforce, hitPoint);
        }
        else if (collision.gameObject.tag == "PierceBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet == null)
                bullet = collision.transform.parent.GetComponent<Bullet>();
            float damage = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetDamage();
            float atk = GameManager.Inst().UpgManager.BData[bullet.GetBulletType()].GetAtk();
            float dmg = damage + atk;
            if (bullet.IsReinforce)
                dmg *= 2;
            bullet.BloodSuck(dmg);

            Vector2 hitPoint = collision.ClosestPoint(gameObject.transform.position);

            OnHit(dmg, bullet.IsReinforce, hitPoint);
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

            Vector2 hitPoint = collision.ClosestPoint(gameObject.transform.position);

            OnHit(dmg, bullet.IsReinforce, hitPoint);
        }
        else if(collision.gameObject.tag == "EquipBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            float dmg = GameManager.Inst().Player.GetItem(bullet.InventoryIndex).Value;
            
            Vector2 hitPoint = collision.ClosestPoint(gameObject.transform.position);

            OnHit(dmg, bullet.IsReinforce, hitPoint);
        }
        else if(collision.gameObject.name == "Bottom")
        {
            GameManager.Inst().UiManager.RedMask.gameObject.GetComponent<RedMask>().SetIsAlert(true);
            
            //아군 피격
            for(int i = 0; i < 5; i++)
            {
                if(HitArea == null)
                    HitArea = transform.GetChild(0).GetComponent<HitArea>();
                
                if (HitArea.HitObjects[i] == null)
                    continue;

                SubWeapon sub = HitArea.HitObjects[i].GetComponent<SubWeapon>();
                if(sub == null)
                    HitArea.HitObjects[i].GetComponent<Player>().Damage(Atk);
                else
                    sub.Damage(Atk);
                HitArea.HitObjects[i] = null;
            }

            if (GameManager.Inst().StgManager.Stage == 3)
                FallEffect.gameObject.SetActive(false);

            //적 사망 처리
            CurHP = Health;
            IsReflected = false;
            IsInvincible = false;
            IsDot = false;
            DotCheck();
            BodyScale = Vector3.one;
            CancelInvoke("StartMove");
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.name == "Right" || collision.gameObject.name == "Left")
        {
            if(GameManager.Inst().StgManager.Stage == 2)
            {
                if (IsReflected)
                    return;

                float rotZ = transform.rotation.eulerAngles.z;
                Quaternion rot = Quaternion.Euler(0.0f, 0.0f, -rotZ);
                transform.rotation = rot;
                
                CurveTime = 0.0f;
                IsReflected = true;
                Invoke("ReturnReflected", 1.0f);
            }
        }
    }

    void ReturnReflected()
    {
        IsReflected = false;
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

    void DotCheck()
    {
        Dot[] dots = GetComponentsInChildren<Dot>();
        for(int i = 0; i < dots.Length; i++)
        {
            if (dots[i])
                dots[i].Die();
        }
        
    }

    //void SlowGame()
    //{
    //    GameObject gm = GameObject.Find("GameManager");
    //    GameManager manager = gm.gameObject.GetComponent<GameManager>();
    //    manager.SlowGame();
    //}
}
