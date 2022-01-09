using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Bullet
{
    public SpriteRenderer Sprite;
    public AnimationCurve Curve;

    Rigidbody2D Rigidbody2D;
    float Timer;
    float Duration;
    float Speed;


    void Awake()
    {
        Type = BulletType.BOOMERANG;

        Timer = 0.0f;
        Duration = 0.0f;
        
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Sprite.transform.RotateAround(Sprite.transform.position, Vector3.forward, Time.deltaTime * 1800.0f);
        Timer += Time.deltaTime;
        float speed = Curve.Evaluate(Timer) * Speed * Duration;
        if (Timer > 1.0f)
            speed *= -1.0f;
        Rigidbody2D.velocity = Vector3.zero;
        Rigidbody2D.AddForce(transform.up * speed, ForceMode2D.Impulse);

        if (Timer >= 2.0f)
           gameObject.SetActive(false);
    }

    public void SetStart()
    {
        Timer = 0.0f;
        Speed = GameManager.Inst().UpgManager.BData[(int)BulletType.BOOMERANG].GetSpeed();
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
