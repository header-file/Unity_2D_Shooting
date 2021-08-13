using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBigBullet : MonoBehaviour
{
    public GameObject[] ShotDir;

    Vector3 TargetPos;
    float HP;
    bool IsFinish;
    int ShotCount;
    bool IsInvincible;
    bool IsShrink;

    public void SetTargetPos(Vector3 pos) { TargetPos = pos; }
    public void SetHP(float hp) { HP = hp; }

    void Start()
    {
        IsFinish = false;
        IsInvincible = false;
        ShotCount = 0;
        IsShrink = false;
    }

    void Update()
    {
        if(!IsFinish)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * 0.25f);

            if(!IsShrink)
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 10.0f, Time.deltaTime * 0.5f);
            else
                transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale - Vector3.one, Time.deltaTime * 0.5f);

            ShotCount++;
            if (ShotCount / 50 % 2 == 0)
                IsShrink = false;
            else
                IsShrink = true;

            if (Vector3.Distance(transform.position, TargetPos) <= 1.2f)
            {
                ShotCount = 0;
                IsFinish = true;
                Shot();
            }
        }
    }

    void Shot()
    {
        switch(GameManager.Inst().StgManager.Stage)
        {
            case 1:
                for (int i = 0; i < 12; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossNormal");
                    obj.transform.position = ShotDir[i].transform.position;
                    obj.transform.rotation = ShotDir[i].transform.rotation;
                    
                    BossNormal bullet = obj.gameObject.GetComponent<BossNormal>();
                    bullet.Shoot(ShotDir[i].transform.up);
                }
                break;
            case 2:
                for (int i = 0; i < 2; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeObj("BossLaser");
                    obj.transform.position = ShotDir[ShotCount].transform.position;
                    obj.transform.rotation = ShotDir[ShotCount + 6].transform.rotation;

                    BossLaser bullet = obj.GetComponent<BossLaser>();
                    bullet.StopTime = 0.0f;
                }
                break;
            case 3:
                break;
            case 4:
                break;
        }

        ShotCount++;
        if (ShotCount >= 6)
            Die();
        else
            Invoke("Shot", 0.1f);
    }

    void Damage(float dmg, bool isReinforced)
    {
        if(!IsInvincible)
            HP -= dmg;

        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = gameObject.transform.position;

        GameManager.Inst().TxtManager.ShowDmgText(gameObject.transform.position, dmg, (int)TextManager.DamageType.BYPLYAER, isReinforced);

        IsInvincible = true;
        gameObject.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(1.0f, 0.0f, 0.0f));
        Invoke("ReturnColor", 0.1f);

        if (HP <= 0.0f)
            Die();
    }

    void ReturnColor()
    {
        gameObject.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));
        IsInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsFinish)
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
            collision.gameObject.SetActive(false);

            GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
            hit.transform.position = bullet.transform.position;

            Damage(dmg, bullet.IsReinforce);
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

            Damage(dmg, bullet.IsReinforce);
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

            Damage(dmg, bullet.IsReinforce);
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

            Damage(dmg, bullet.IsReinforce);
        }
    }

    void Die()
    {
        IsFinish = false;
        ShotCount = 0;
        transform.localScale = Vector3.one * 2.0f;
        gameObject.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", new Color(0.5f, 0.5f, 0.5f));
        gameObject.SetActive(false);
    }
}
