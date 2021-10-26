using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLasers : Bullet
{
    Vector3 Scale;
    float Gyesu;
    bool IsGaro;
    bool IsAttack;
    int Damage;


    void Awake()
    {
        IsAttack = false;
        IsGaro = false;
        Gyesu = 1.0f;
    }

    void Start()
    {
        Damage = 10 * GameManager.Inst().StgManager.Stage;
    }

    void Update()
    {
        Scale = transform.localScale;
        if (IsGaro)
        {
            Scale.y -= (Time.deltaTime * Gyesu * 0.5f);
            if (Scale.y <= 0.0f)
            {
                Gyesu *= -10.0f;
                IsAttack = true;
            }
            else if (Scale.y >= 0.75f)
                ResetData();
        }
        else
        {
            Scale.x -= (Time.deltaTime * Gyesu * 0.5f);
            if (Scale.x <= 0.0f)
            {
                Gyesu *= -10.0f;
                IsAttack = true;
            }
            else if (Scale.x >= 0.75f)
                ResetData();
        }
        transform.localScale = Scale;
    }

    public void StartLine(int Dir)
    {
        if (Dir == 0)
            IsGaro = true;

        if (IsGaro)
        {
            int rand = Random.Range(0, 6);
            transform.position = new Vector3(0.0f, rand, 0.0f);
            transform.localScale = new Vector3(7.5f, 0.5f, 1.0f);
        }
        else
        {
            int rand = Random.Range(-2, 3);
            transform.position = new Vector3(rand, 4.2f, 0.0f);
            transform.localScale = new Vector3(0.5f, 11.0f, 1.0f);
        }

        
    }

    void ResetData()
    {
        IsAttack = false;
        IsGaro = false;
        Gyesu = 1.0f;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsAttack)
            return;

        if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<Player>().Damage(Damage);
        }
    }

    void HitEffect(GameObject obj)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = obj.transform.position;
    }
}
