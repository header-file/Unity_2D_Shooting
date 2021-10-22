using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMirror : Bullet
{
    public LineRenderer Line;
    public int Damage;
    public Rigidbody2D Rig;

    Vector3 StartPos;
    bool IsAiming;
    int BounceCount;


    void Start()
    {
        IsAiming = true;
        Rig = gameObject.GetComponent<Rigidbody2D>();
        Damage = 10 * GameManager.Inst().StgManager.Stage;
        BounceCount = 3;
    }

    void Update()
    {
        if(IsAiming)
            RenderLine();
    }

    void RenderLine()
    {
        Vector3 Pos = transform.position;
        Vector3 Dir = -transform.up;
        Line.positionCount = 1;
        Line.SetPosition(0, transform.position);

        for (int i = 1; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(Pos, Dir, 40.0f, 8);

            Line.positionCount++;
            Line.SetPosition(i, hit.point);

            Pos = hit.point;
            Dir = Vector3.Reflect(Dir, hit.normal);
        }

        Line.widthMultiplier -= Time.deltaTime;

        if (Line.widthMultiplier <= 0.0f)
            Line.widthMultiplier = 1.0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            BounceCount--;

            if (BounceCount >= 0)
            {
                Rig.velocity = Vector2.zero;
                Rig.angularVelocity = 0.0f;

                Vector3 dir = gameObject.transform.position - StartPos;
                Shoot(Vector3.Reflect(dir, -collision.gameObject.transform.up).normalized);
                StartPos = gameObject.transform.position;
            }
            else
            {
                BounceCount = 3;
                Rig.velocity = Vector2.zero;
                Rig.angularVelocity = 0.0f;
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);

            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.gameObject);
            collision.gameObject.GetComponent<Player>().Damage(Damage);

            gameObject.SetActive(false);
        }
    }

    void HitEffect(GameObject obj)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = obj.transform.position;
    }
}
