using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMirror : Bullet
{
    public LineRenderer Line;
    public int Damage;
    public Rigidbody2D Rig;
    public LayerMask WallMask;
    public LayerMask NothingMask;

    Vector3[] Poses;
    Vector3 Pos;
    Vector3 Dir;
    bool IsAiming;
    int BounceCount;
    float Speed;
    RaycastHit2D Hit;
    GameObject[] HitWalls;

    void Start()
    {
        Type = BulletType.NORMAL;
        IsAiming = true;
        Rig = gameObject.GetComponent<Rigidbody2D>();
        Damage = 10 * GameManager.Inst().StgManager.Stage;
        BounceCount = 3;
        Speed = 20.0f;
        Poses = new Vector3[3];
        HitWalls = new GameObject[4];
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
            Hit = Physics2D.Raycast(Pos, Dir, 20.0f, WallMask);

            if (HitWalls[i - 1] != null)
                HitWalls[i - 1].layer = 8;

            Line.positionCount++;
            Line.SetPosition(i, Hit.point);

            Pos = Hit.point;
            Poses[3 - i] = Hit.point;
            Dir = Vector3.Reflect(Dir, Hit.normal);

            HitWalls[i] = Hit.collider.gameObject;
            HitWalls[i].layer = NothingMask;
        }

        for (int i = 0; i < HitWalls.Length; i++)
        {
            if (HitWalls[i] == null)
                continue;

            HitWalls[i].layer = 8;
            HitWalls[i] = null;
        }            

        Line.widthMultiplier -= Time.deltaTime;

        if (Line.widthMultiplier <= 0.0f)
        {
            Line.widthMultiplier = 0.0f;
            IsAiming = false;
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, Poses[BounceCount - 1], Time.deltaTime * Speed);
    }

    public void Rotate()
    {
        int rand = Random.Range(-12, 12);
        rand *= 5;
        transform.rotation = Quaternion.AngleAxis(rand, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.GetComponent<BossMirror>())
            return;

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
                IsAiming = true;
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == "SubWeapon")
        {
            
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
            
            Line.widthMultiplier = 1.0f;
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<Player>().Damage(Damage);

            Line.widthMultiplier = 1.0f;
            gameObject.SetActive(false);
        }
    }

    void HitEffect(Vector2 pos)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = pos;
    }
}
