using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    public LineRenderer Line;
    public GameObject Ball;
    public BoxCollider2D Col;
    public LayerMask WallMask;
    public GameObject LaserPos;

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
        IsReinforce = false;
        SizeGyesu = 1.0f;
        ColSize = Vector2.zero;
    }

    void Update()
    {
        ShootRay();
        if(!IsStopped)
            LineRender();
        SetCol();
    }

    public void SetStartPos(GameObject Pos)
    {
        LaserPos = Pos;

        SizeGyesu = 1.0f;
        if (GameManager.Inst().Player.GetBossMode())
            SizeGyesu = 0.5f;
        if (IsReinforce)
            SizeGyesu *= 1.5f;
        Line.positionCount = 2;
        Line.widthMultiplier = 0.0f;
    }

    void ShootRay()
    {
        Pos = LaserPos.transform.position;
        Dir = LaserPos.transform.up;
        Hit = Physics2D.Raycast(Pos, Dir, 20.0f, WallMask);

        Line.SetPosition(0, Pos);
        Line.SetPosition(1, Hit.point);
    }

    void LineRender()
    {
        Line.widthMultiplier += (Time.deltaTime * SizeGyesu);

        if (SizeGyesu > 0.0f && Line.widthMultiplier >= (0.25f * SizeGyesu))
        {
            IsStopped = true;
            SizeGyesu *= -1.0f;
            Invoke("Restart", GameManager.Inst().UpgManager.BData[(int)Type].GetDuration());
        }
        else if (Line.widthMultiplier <= 0.0f)
            gameObject.SetActive(false);
    }

    void SetCol()
    {
        Ball.transform.position = LaserPos.transform.position;

        Col.transform.position = LaserPos.transform.position;
        Col.transform.rotation = LaserPos.transform.rotation;

        ColSize.x = Line.widthMultiplier;
        ColSize.y = Vector3.Distance(Line.GetPosition(0), Line.GetPosition(1));

        if (GameManager.Inst().Player.GetBossMode())
            ColSize.y *= 2.0f;
        Col.size = ColSize;

        Vector2 offset = Col.offset;
        offset.y = ColSize.y / 2.0f;
        Col.offset = offset;
    }

    void Restart()
    {
        IsStopped = false;
    }
}
