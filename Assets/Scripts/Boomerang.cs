using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Bullet
{
    public AnimationCurve Curve;
    public AnimationCurve Curve2;

    Vector3 StartPos;
    Vector3 TargetPos;
    Vector3[] ControlPos;
    bool IsReturn;
    float Timer;
    float Duration;
    float Speed;


    public void SetTargetpos(Vector3 pos)
    {
        Duration = GameManager.Inst().UpgManager.GetBData((int)BulletType.BOOMERANG).GetDuration();

        float rad = Mathf.Atan2(pos.y - StartPos.y, pos.x - StartPos.x);
        TargetPos.x = StartPos.x + (Duration * Mathf.Sin(Mathf.PI / 2 - rad));
        TargetPos.y = StartPos.y + (Duration * Mathf.Cos(Mathf.PI / 2 - rad));
    }
    public void SetStartPos(Vector3 pos) { StartPos = pos; }
    public void SetIsReturn(bool b) { IsReturn = b; }


    void Awake()
    {
        Damage = 2.0f;
        Type = BulletType.BOOMERANG;
        IsReturn = false;
        Timer = 0.0f;
        Duration = 2.0f;

        StartPos = new Vector3(0.0f, 1.5f, 0.0f);
        TargetPos = new Vector3(1.0f, 7.5f, 0.0f);
    }

    void Start()
    {
        Speed = GameManager.Inst().UpgManager.GetBData((int)BulletType.BOOMERANG).GetSpeed();

        ControlPos = new Vector3[2];
        ControlPos[0] = GameManager.Inst().Player.BoomerangPos[1].transform.position;
        ControlPos[1] = GameManager.Inst().Player.BoomerangPos[2].transform.position;
    }

    void FixedUpdate()
    {
        transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * 500.0f);
        Timer += Time.deltaTime;//(1.0f / 60.0f);
        float linearT = Timer * Speed;
        float right = Curve.Evaluate(linearT);
        float up = Curve2.Evaluate(linearT);


        if (!IsReturn)
            transform.position = Vector3.Lerp(StartPos, TargetPos, up)/* + new Vector3(up, 0.0f, 0.0f)*/;
            //transform.position = Bezier(StartPos, TargetPos, ControlPos[0], Timer);
        else
            transform.position = Vector3.Lerp(StartPos, TargetPos, up)/* - new Vector3(up, 0.0f, 0.0f)*/;
            //transform.position = Bezier(TargetPos, StartPos, ControlPos[1], Timer);

        if (!IsReturn && Timer >= 1.0f)
        {
            Timer = 1.0f;
            IsReturn = true;
            Debug.Log("Change");
            //TargetPos = gameObject.transform.position;
        }
        //Vector3.Distance(transform.position, StartPos) < 0.0001f
        if (IsReturn && Timer >= 1.5f)
        {
            IsReturn = false;
            Timer = 0.0f;
            gameObject.SetActive(false);
        }
            
    }

    public void SetStart()
    {
        IsReturn = false;
        Timer = 0.0f;
    }

    Vector3 Bezier(Vector3 now, Vector3 target, Vector3 control, float t)
    {
        /*Vector2 nc = Vector2.Lerp(now, control, t);
        Vector2 ct = Vector2.Lerp(control, target, t);

        return Vector2.Lerp(nc, ct, t);*/

        float u = 1.0f - t;
        float t2 = t * t;
        float u2 = u * u;

        Vector3 result = (u2) * now + 2.0f * (t - t2) * control + (t2) * target;

        return result;
    }
}
