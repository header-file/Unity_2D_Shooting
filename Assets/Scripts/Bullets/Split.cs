using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : Bullet
{
    public GameObject[] SplitPositions;

    int ShooterID;


    public void SetShooterID(int id) { ShooterID = id; }

    void Awake()
    {
        //Damage = 2.0f;
        //Type = BulletType.SPLIT;
    }

    public void OnSplit()
    {
        int level = GameManager.Inst().UpgManager.BData[(int)Type].GetPowerLevel();
        int colorIndex = GameManager.Inst().ShtManager.GetColorSelection(ShooterID);

        switch (level)
        {
            case 1:
            case 2:
                for(int i = 0; i < 2; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeBullet("Piece", colorIndex);
                    obj.transform.position = SplitPositions[i].transform.position;
                    obj.transform.rotation = SplitPositions[i].transform.rotation;
                    //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", color);

                    Piece bullet = obj.GetComponent<Piece>();
                    bullet.Shoot(SplitPositions[i].transform.up);
                }
                break;
            case 3:
            case 4:
                for (int i = 2; i < 6; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeBullet("Piece", colorIndex);
                    obj.transform.position = SplitPositions[i].transform.position;
                    obj.transform.rotation = SplitPositions[i].transform.rotation;
                    //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", color);

                    Piece bullet = obj.GetComponent<Piece>();
                    bullet.Shoot(SplitPositions[i].transform.up);
                }
                break;
            case 5:
                for (int i = 0; i < 6; i++)
                {
                    GameObject obj = GameManager.Inst().ObjManager.MakeBullet("Piece", colorIndex);
                    obj.transform.position = SplitPositions[i].transform.position;
                    obj.transform.rotation = SplitPositions[i].transform.rotation;
                    //obj.GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", color);

                    Piece bullet = obj.GetComponent<Piece>();
                    bullet.Shoot(SplitPositions[i].transform.up);
                }
                break;
        }
    }
}
