using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMirror : Bullet
{
    public LineRenderer Line;
    public int Damage;
    public Rigidbody2D Rig;
    public LayerMask LayerMask;

    Vector3[] Poses;
    Vector3 Pos;
    Vector3 Dir;
    bool IsAiming;
    int BounceCount;
    float Speed;

    
    void Start()
    {
        Type = BulletType.NORMAL;
        IsAiming = true;
        Rig = gameObject.GetComponent<Rigidbody2D>();
        Damage = 10 * GameManager.Inst().StgManager.Stage;
        BounceCount = 3;
        Speed = 20.0f;
        Poses = new Vector3[3];
    }

    void Update()
    {
        if (IsAiming)
            RenderLine();
        else
            Move();
    }

    void RenderLine()
    {
        Pos = transform.position;
        Dir = -transform.up;
        Line.positionCount = 1;
        Line.SetPosition(0, transform.position);

        for (int i = 1; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(Pos, Dir, 10.0f, LayerMask);

            Line.positionCount++;
            Line.SetPosition(i, hit.point);

            Pos = hit.point;
            Poses[3 - i] = hit.point;
            Dir = Vector3.Reflect(Dir, hit.normal);
        }

        Line.widthMultiplier -= Time.deltaTime;

        if (Line.widthMultiplier <= 0.0f)
        {
            Line.widthMultiplier = 0.0f;
            IsAiming = false;
            //Shoot(-transform.up, Speed);
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Poses[BounceCount - 1], Time.deltaTime * Speed);
    }

    public void Rotate()
    {
        int rand = Random.Range(-60, 61);
        transform.rotation = Quaternion.AngleAxis(rand, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            BounceCount--;

            if(BounceCount <= 0)
            {
                BounceCount = 3;
                Rig.velocity = Vector2.zero;
                Rig.angularVelocity = 0.0f;
                Line.widthMultiplier = 1.0f;
                Line.positionCount = 0;
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
            
            Line.widthMultiplier = 1.0f;
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<Player>().Damage(Damage);

            Line.widthMultiplier = 1.0f;
            gameObject.SetActive(false);
        }
    }

    void HitEffect(GameObject obj)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = obj.transform.position;
    }
}
