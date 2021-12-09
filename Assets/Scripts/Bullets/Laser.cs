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
    bool IsStopped;

    void Awake()
    {
        Type = BulletType.LASER;
        IsStopped = false;
        SizeGyesu = 1.0f;
        ColSize = Vector2.zero;
    }

    private void Start()
    {
        SetStartPos();
    }

    void Update()
    {
        ShootRay();
        if(!IsStopped)
            LineRender();
        SetCol();
    }

    public void SetStartPos()
    {
        //transform.parent = GameManager.Inst().Player.LaserPos.transform;

        SizeGyesu = 1.0f;
        Line.positionCount = 2;        
        Line.widthMultiplier = 0.0f;
    }

    void ShootRay()
    {
        Pos = GameManager.Inst().Player.LaserPos.transform.position;
        Dir = GameManager.Inst().Player.LaserPos.transform.up;
        Hit = Physics2D.Raycast(Pos, Dir, 20.0f, WallMask);

        Line.SetPosition(0, Pos);
        Line.SetPosition(1, Hit.point);
    }

    void LineRender()
    {
        Line.widthMultiplier += (Time.deltaTime * SizeGyesu);

        if (SizeGyesu > 0.0f && Line.widthMultiplier >= 0.25f)
        {
            IsStopped = true;
            SizeGyesu *= -1.0f;
            Invoke("Restart", GameManager.Inst().UpgManager.BData[(int)Type].GetDuration());
        }
        else if (Line.widthMultiplier <= 0.0f)
        {
            //transform.SetParent(GameManager.Inst().ObjManager.PBulletPool.transform);
            gameObject.SetActive(false);
            //SizeGyesu *= -1.0f;
        }
    }

    void SetCol()
    {
        transform.position = GameManager.Inst().Player.LaserPos.transform.position;
        transform.rotation = GameManager.Inst().Player.LaserPos.transform.rotation;
        transform.localScale = Vector3.one;

        ColSize.x = (Line.widthMultiplier * 2.0f);
        //ColSize.x = 1.25f;
        ColSize.y = Vector2.Distance(Line.GetPosition(0), Line.GetPosition(1));
        Col.size = ColSize;

        Vector2 offset = Col.offset;
        offset.y = (ColSize.y / 2.0f);
        Col.offset = offset;
    }

    void Restart()
    {
        IsStopped = false;
    }
}
