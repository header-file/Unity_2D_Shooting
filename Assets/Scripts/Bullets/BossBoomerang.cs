using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBoomerang : Bullet
{
    public AnimationCurve Curve2;

    Vector3 StartPos;
    Vector3 TargetPos;
    Vector3[] ControlPos;
    bool IsReturn;
    float Timer;
    float Duration;
    float Speed;

    int Damage;
    
    
    void Awake()
    {
        Type = BulletType.BOOMERANG;
        Damage = 10 * GameManager.Inst().StgManager.Stage;

        IsReturn = false;
        Timer = 0.0f;
        Speed = 2.0f;

        StartPos = new Vector3(0.0f, 0.0f, 0.0f);
        TargetPos = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * 500.0f);
        Timer += Time.deltaTime;
        float linearT = Timer * Speed;
        float up = Curve2.Evaluate(linearT);

        transform.position = Vector3.Lerp(StartPos, TargetPos, up);

        if (!IsReturn && linearT >= 1.0f)
            IsReturn = true;
        //if (IsReturn && Vector3.Distance(transform.position, StartPos) < 0.0001f)
        if(linearT >= 2.0f)
            gameObject.SetActive(false);
    }

    public void StartMove(Vector3 pos)
    {
        StartPos = pos;
        SetTargetpos();

        IsReturn = false;
        Timer = 0.0f;
    }

    void SetTargetpos()
    {
        Duration = 3.0f;
        Vector3 pos = StartPos + transform.up;

        float rad = Mathf.Atan2(pos.y - StartPos.y, pos.x - StartPos.x);
        TargetPos.x = StartPos.x + (Duration * Mathf.Sin(Mathf.PI / 2 - rad));
        TargetPos.y = StartPos.y + (Duration * Mathf.Cos(Mathf.PI / 2 - rad));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<SubWeapon>().Damage(Damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            HitEffect(collision.ClosestPoint(transform.position));
            collision.gameObject.GetComponent<Player>().Damage(Damage);
        }
    }

    void HitEffect(Vector2 pos)
    {
        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = pos;
    }
}
