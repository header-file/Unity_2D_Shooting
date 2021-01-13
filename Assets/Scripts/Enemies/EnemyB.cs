using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    bool IsAbleAttack;
    Vector3 NowPosition;
    float Gyesu;
    bool IsReady;

    void Start()
    {
        Type = EnemyType.BOSS;
        Gyesu = 0.1f;
        IsReady = false;
    }

    void FixedUpdate()
    {
        if (IsAbleAttack)
            return;

        if(IsReady)
        {
            Vector3 pos = transform.position;
            pos.x += Speed * Gyesu;
            transform.position = pos;

            if (transform.position.x >= 3.0f || transform.position.x <= -3.0f)
                Gyesu *= -1.0f;
        }
        else
        {
            Vector3 pos = transform.position;
            pos.y -= Speed;
            transform.position = pos;

            if (transform.position.y <= 8.5f)
                IsReady = true;
        }
    }

    void Update()
    {
        if (!IsAbleAttack)
            return;

        IsAbleAttack = false;

        int rand = Random.Range(0, 4);
        if (CurHP / Health > 0.25f)
        {
            switch (rand)
            {
                case 0:
                    Spread();
                    break;
                case 1:
                    Laser();
                    break;
                case 2:
                    BigBullet();
                    break;
                case 3:
                    ShotOneWay();
                    break;
            }
        }
        else
            LastSpurt();

        Invoke("AbleAttack", 2.0f);
    }

    void Spread()
    {
        Debug.Log("Spread");
    }

    void Laser()
    {
        Debug.Log("Laser");
    }

    void BigBullet()
    {
        Debug.Log("BigBullet");
    }

    void ShotOneWay()
    {
        Debug.Log("ShotOneWay");
    }

    void LastSpurt()
    {
        Debug.Log("LastSpurt");
    }

    void AbleAttack()
    {
        IsAbleAttack = true;
    }
}
