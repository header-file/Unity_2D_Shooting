using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissile : Bullet
{
    public GameObject Target;
    public GameObject SearchArea;
    public TrailRenderer Trail;

    Vector3 InitPos;
    Rigidbody2D Rig;

    float RotateSpeed;
    float Speed;
    int Damage;


    void Awake()
    {
        Type = BulletType.MISSILE;
        Damage = 5 * GameManager.Inst().StgManager.Stage;

        InitPos = new Vector3(0.0f, 0.0f, 0.0f);
        Rig = GetComponent<Rigidbody2D>();
        Speed = GameManager.Inst().UpgManager.BData[(int)Type].GetSpeed();
        RotateSpeed = 200.0f;
    }

    void Update()
    {
        if (Target != null)
        {
            Vector2 pointTarget = (Vector2)transform.position - (Vector2)Target.transform.position;
            pointTarget.Normalize();

            float val = Vector3.Cross(pointTarget, transform.up).z;

            Rig.angularVelocity = RotateSpeed * val;

            Rig.velocity = -transform.up * Speed;
        }
        else if (Target == null)
        {
            Vector2 pointTarget = (Vector2)transform.position - (Vector2)InitPos;
            pointTarget.Normalize();

            float val = Vector3.Cross(pointTarget, transform.up).z;

            Rig.velocity = -transform.up * Speed;
            Rig.angularVelocity = 0.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SubWeapon")
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

    void ResetTarget()
    {
        Target = null;
    }

    void OnDisable()
    {
        ResetTarget();
        Trail.Clear();
    }
}
