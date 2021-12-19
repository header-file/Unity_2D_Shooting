using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Bullet
{
    public SpriteRenderer Sprite;
    public AnimationCurve Curve;

    Rigidbody2D Rigidbody2D;
    Vector3 StartPos;
    Vector3 TargetPos;
    Vector3[] ControlPos;
    bool IsReturn;
    float Timer;
    float Duration;
    float Speed;
    float StopTime;

    public void SetTargetpos(Vector3 pos)
    {
        float rad = Mathf.Atan2(pos.y - StartPos.y, pos.x - StartPos.x);
        TargetPos.x = StartPos.x + (Duration * Mathf.Sin(Mathf.PI / 2 - rad) * 2);
        TargetPos.y = StartPos.y + (Duration * Mathf.Cos(Mathf.PI / 2 - rad) * 2);
    }
    public void SetStartPos(Vector3 pos) { StartPos = pos; }
    public void SetIsReturn(bool b) { IsReturn = b; }


    void Awake()
    {
        Type = BulletType.BOOMERANG;

        IsReturn = false;
        Timer = 0.0f;
        Duration = 0.0f;
        StopTime = 0.0f;

        StartPos = new Vector3(0.0f, 0.0f, 0.0f);
        TargetPos = new Vector3(0.0f, 0.0f, 0.0f);
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Speed = GameManager.Inst().UpgManager.BData[(int)BulletType.BOOMERANG].GetSpeed();

        ControlPos = new Vector3[2];
        ControlPos[0] = GameManager.Inst().Player.BoomerangPos[1].transform.position;
        ControlPos[1] = GameManager.Inst().Player.BoomerangPos[2].transform.position;
    }

    void Update()
    {
        Sprite.transform.RotateAround(Sprite.transform.position, Vector3.forward, Time.deltaTime * 1800.0f);
        Timer += Time.deltaTime;
        float speed = Curve.Evaluate(Timer) * Speed;
        Rigidbody2D.velocity = Vector3.zero;
        Rigidbody2D.AddForce(transform.up * speed, ForceMode2D.Impulse);

        if (Timer >= 2.0f)
        {
            GameManager.Inst().ShtManager.BoomerangCount--;
            gameObject.SetActive(false);
        }
    }

    public void SetStart()
    {
        IsReturn = false;
        Timer = 0.0f;
        StopTime = 0.0f;
        Duration = GameManager.Inst().UpgManager.BData[(int)BulletType.BOOMERANG].GetDuration();
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
