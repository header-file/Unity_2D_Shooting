using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject Enemy_SPref;
    public GameObject Enemy_MPref;
    public GameObject Enemy_LPref;

    public GameObject CoinPref;
    public GameObject EqAttackPref;
    public GameObject EqRangePref;
    public GameObject EqSpeedPref;

    public GameObject[] NormalPref;
    public GameObject[] SpreadPref;
    public GameObject[] MissilePref;
    public GameObject[] LaserPref;
    public GameObject[] ChargePref;
    public GameObject[] BoomerangPref;
    public GameObject[] SplitPref;
    public GameObject[] PiecePref;

    public GameObject SubWeaponPref;

    public GameObject DmgTextPref;
    public GameObject InventorySlotPref;

    public GameObject ExplosionPref;

    GameObject[] Enemies_S;
    GameObject[] Enemies_M;
    GameObject[] Enemies_L;

    GameObject[] Coins;
    GameObject[] EqAttacks;
    GameObject[] EqRanges;
    GameObject[] EqSpeeds;

    GameObject[,] Normals;
    GameObject[,] Spreads;
    GameObject[,] Missiles;
    GameObject[,] Lasers;
    GameObject[,] Charges;
    GameObject[,] Boomerangs;
    GameObject[,] Splits;
    GameObject[,] Pieces;

    GameObject[] SubWeapons;

    GameObject[] DmgTexts;
    GameObject[] InventorySlots;

    GameObject[] Explosions;


    GameObject[] TargetPool;
    GameObject[,] TargetPools;


    public GameObject MakeBullet(string Type, int ColorIndex)
    {
        switch(Type)
        {
            case "Normal":
                TargetPools = Normals;
                break;

            case "Spread":
                TargetPools = Spreads;
                break;

            case "Missile":
                TargetPools = Missiles;
                break;

            case "Laser":
                TargetPools = Lasers;
                break;

            case "Charge":
                TargetPools = Charges;
                break;

            case "Boomerang":
                TargetPools = Boomerangs;
                break;

            case "Split":
                TargetPools = Splits;
                break;

            case "Piece":
                TargetPools = Pieces;
                break;
        }

        for (int i = 0; i < TargetPools.Length / GameManager.Inst().MAXCOLOR; i++)
        {
            if (!TargetPools[ColorIndex, i].activeSelf)
            {
                TargetPools[ColorIndex, i].SetActive(true);
                return TargetPools[ColorIndex, i];
            }

        }

        return null;
    }

    public GameObject MakeObj(string Type)
    {
        switch(Type)
        {
            case "EnemyS":
                TargetPool = Enemies_S;
                break;

            case "EnemyM":
                TargetPool = Enemies_M;
                break;

            case "EnemyL":
                TargetPool = Enemies_L;
                break;

            case "Coin":
                TargetPool = Coins;
                break;

            case "EqAttack":
                TargetPool = EqAttacks;
                break;

            case "EqRange":
                TargetPool = EqRanges;
                break;

            case "EqSpeed":
                TargetPool = EqSpeeds;
                break;
            
            case "SubWeapon":
                TargetPool = SubWeapons;
                break;

            case "DamageText":
                TargetPool = DmgTexts;
                break;

            case "InventorySlot":
                TargetPool = InventorySlots;
                break;

            case "Explosion":
                TargetPool = Explosions;
                break;
        }

        for (int i = 0; i < TargetPool.Length; i++)
        {
            if (!TargetPool[i].activeSelf)
            {
                TargetPool[i].SetActive(true);
                return TargetPool[i];
            }
                
        }

        return null;
    }

    void Awake()
    {
        Enemies_S = new GameObject[20];
        Enemies_M = new GameObject[10];
        Enemies_L = new GameObject[10];

        Coins = new GameObject[30];
        EqAttacks = new GameObject[10];
        EqRanges = new GameObject[10];
        EqSpeeds = new GameObject[10];

        Normals = new GameObject[8, 100];
        Spreads = new GameObject[8, 100];
        Missiles = new GameObject[8, 50];
        Lasers = new GameObject[8, 10];
        Charges = new GameObject[8, 20];
        Boomerangs = new GameObject[8, 10];
        Splits = new GameObject[8, 10];
        Pieces = new GameObject[8, 100];

        SubWeapons = new GameObject[4];

        DmgTexts = new GameObject[40];
        InventorySlots = new GameObject[200];

        Explosions = new GameObject[20];
        
        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < Enemies_S.Length; i++)
        {
            Enemies_S[i] = Instantiate(Enemy_SPref);
            Enemies_S[i].SetActive(false);
        }

        for (int i = 0; i < Enemies_M.Length; i++)
        {
            Enemies_M[i] = Instantiate(Enemy_MPref);
            Enemies_M[i].SetActive(false);
        }

        for (int i = 0; i < Enemies_L.Length; i++)
        {
            Enemies_L[i] = Instantiate(Enemy_LPref);
            Enemies_L[i].SetActive(false);
        }


        for (int i = 0; i < Coins.Length; i++)
        {
            Coins[i] = Instantiate(CoinPref);
            Coins[i].SetActive(false);
        }

        for (int i = 0; i < EqAttacks.Length; i++)
        {
            EqAttacks[i] = Instantiate(EqAttackPref);
            EqAttacks[i].SetActive(false);
        }

        for (int i = 0; i < EqRanges.Length; i++)
        {
            EqRanges[i] = Instantiate(EqRangePref);
            EqRanges[i].SetActive(false);
        }

        for (int i = 0; i < EqSpeeds.Length; i++)
        {
            EqSpeeds[i] = Instantiate(EqSpeedPref);
            EqSpeeds[i].SetActive(false);
        }


        int maxColor = GameManager.Inst().MAXCOLOR;
        
        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Normals.Length / maxColor; i++)
            {
                Normals[j, i] = Instantiate(NormalPref[j]);
                Normals[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().GetColors(j));
                Normals[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Spreads.Length / maxColor; i++)
            {
                Spreads[j, i] = Instantiate(SpreadPref[j]);
                Spreads[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().GetColors(j));
                Spreads[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Missiles.Length / maxColor; i++)
            {
                Missiles[j, i] = Instantiate(MissilePref[j]);
                Missiles[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().GetColors(j));
                Missiles[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Lasers.Length / maxColor; i++)
            {
                Lasers[j, i] = Instantiate(LaserPref[j]);
                Lasers[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().GetColors(j));
                Lasers[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Charges.Length / maxColor; i++)
            {
                Charges[j, i] = Instantiate(ChargePref[j]);
                Charges[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().GetColors(j));
                Charges[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Boomerangs.Length / maxColor; i++)
            {
                Boomerangs[j, i] = Instantiate(BoomerangPref[j]);
                Boomerangs[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().GetColors(j));
                Boomerangs[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Splits.Length / maxColor; i++)
            {
                Splits[j, i] = Instantiate(SplitPref[j]);
                Splits[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().GetColors(j));
                Splits[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Pieces.Length / maxColor; i++)
            {
                Pieces[j, i] = Instantiate(PiecePref[j]);
                Pieces[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().GetColors(j));
                Pieces[j, i].SetActive(false);
            }
        }


        for (int i = 0; i < SubWeapons.Length; i++)
        {
            SubWeapons[i] = Instantiate(SubWeaponPref);
            SubWeapon sub = SubWeapons[i].gameObject.GetComponent<SubWeapon>();
            GameManager.Inst().SetSubWeapons(sub, i);
            GameManager.Inst().SubWID[i] = SubWeapons[i].GetInstanceID();
            SubWeapons[i].SetActive(false);
        }


        for(int i = 0; i < DmgTexts.Length; i++)
        {
            DmgTexts[i] = Instantiate(DmgTextPref);
            DmgTexts[i].SetActive(false);
        }

        for(int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlots[i] = Instantiate(InventorySlotPref);
            InventorySlots[i].SetActive(false);
        }


        for (int i = 0; i < Explosions.Length; i++)
        {
            Explosions[i] = Instantiate(ExplosionPref);
            Explosions[i].SetActive(false);
        }
    }
}
