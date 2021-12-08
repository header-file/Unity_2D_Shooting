using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    public LineRenderer Line;
    public BoxCollider2D Col;
    public LayerMask WallMask;

    Vector3 Pos;
    Vector3 Dir;
    RaycastHit2D Hit;
    float SizeGyesu;
    Vector2 ColSize;
    //Animator Anim;
    bool IsStopped;

    void Awake()
    {
        Type = BulletType.LASER;
        GetComponent<SpriteRenderer>().color = Color.white;
        IsStopped = false;
        SizeGyesu = 1.0f;
        ColSize = Vector2.zero;
    }

    //void Start()
    //{
    //    Anim = GetComponent<Animator>();
    //}

    //void Update()
    //{
    //    if (!IsStopped && Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
    //    {
    //        Anim.speed = 0.0f;
    //        IsStopped = true;

    //        float stopTime = GameManager.Inst().UpgManager.BData[(int)Type].GetDuration();
    //        Invoke("Restart", stopTime);
    //    }

    //    if(Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
    //        Destroy();
    //}

    //void Restart()
    //{
    //    Anim.speed = 1.0f;
    //}

    //void Destroy()
    //{
    //    IsStopped = false;
    //    gameObject.SetActive(false);
    //}

    void Start()
    {
        SetStartPos();
    }

    void Update()
    {
        ShootRay();
        LineRender();
        SetCol();
    }

    public void SetStartPos()
    {
        transform.parent = GameManager.Inst().Player.LaserPos.transform;        

        Line.positionCount = 2;
        Line.SetPosition(0, Pos);
        Line.widthMultiplier = 0.0f;
    }

    void ShootRay()
    {
        Pos = GameManager.Inst().Player.LaserPos.transform.position;
        Dir = GameManager.Inst().Player.LaserPos.transform.up;
        Hit = Physics2D.Raycast(Pos, Dir, 20.0f, WallMask);

        Line.SetPosition(1, Hit.point);
    }

    void LineRender()
    {
        Line.widthMultiplier += (Time.deltaTime * SizeGyesu);

        if (Line.widthMultiplier >= 0.25f)
            SizeGyesu *= -1.0f;
        else if (Line.widthMultiplier <= 0.0f)
            SizeGyesu *= -1.0f;
    }

    void SetCol()
    {
        ColSize.x = Line.widthMultiplier;
        ColSize.y = Vector2.Distance(Line.GetPosition(0), Line.GetPosition(1));
        Col.size = ColSize;
    }
}
