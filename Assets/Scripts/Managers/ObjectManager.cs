using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject Enemy_SPref;
    public GameObject Enemy_MPref;
    public GameObject Enemy_LPref;

    public GameObject CoinPref;

    public GameObject NormalPref;
    public GameObject SpreadPref;
    public GameObject MissilePref;
    public GameObject LaserPref;
    public GameObject ChargePref;
    public GameObject BoomerangPref;
    public GameObject SplitPref;
    public GameObject PiecePref;

    public GameObject SubWeaponPref;

    public GameObject ExplosionPref;

    GameObject[] Enemies_S;
    GameObject[] Enemies_M;
    GameObject[] Enemies_L;

    GameObject[] Coins;

    GameObject[] Normals;
    GameObject[] Spreads;
    GameObject[] Missiles;
    GameObject[] Lasers;
    GameObject[] Charges;
    GameObject[] Boomerangs;
    GameObject[] Splits;
    GameObject[] Pieces;

    GameObject[] SubWeapons;

    GameObject[] Explosions;


    GameObject[] TargetPool;


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

            case "Normal":
                TargetPool = Normals;
                break;

            case "Spread":
                TargetPool = Spreads;
                break;

            case "Missile":
                TargetPool = Missiles;
                break;

            case "Laser":
                TargetPool = Lasers;
                break;

            case "Charge":
                TargetPool = Charges;
                break;

            case "Boomerang":
                TargetPool = Boomerangs;
                break;

            case "Split":
                TargetPool = Splits;
                break;

            case "Piece":
                TargetPool = Pieces;
                break;

            case "SubWeapon":
                TargetPool = SubWeapons;
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

        Coins = new GameObject[10];

        Normals = new GameObject[100];
        Spreads = new GameObject[100];
        Missiles = new GameObject[50];
        Lasers = new GameObject[10];
        Charges = new GameObject[20];
        Boomerangs = new GameObject[20];
        Splits = new GameObject[20];
        Pieces = new GameObject[100];

        SubWeapons = new GameObject[4];

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


        for (int i = 0; i < Normals.Length; i++)
        {
            Normals[i] = Instantiate(NormalPref);
            Normals[i].SetActive(false);
        }

        for (int i = 0; i < Spreads.Length; i++)
        {
            Spreads[i] = Instantiate(SpreadPref);
            Spreads[i].SetActive(false);
        }

        for (int i = 0; i < Missiles.Length; i++)
        {
            Missiles[i] = Instantiate(MissilePref);
            Missiles[i].SetActive(false);
        }

        for (int i = 0; i < Lasers.Length; i++)
        {
            Lasers[i] = Instantiate(LaserPref);
            Lasers[i].SetActive(false);
        }

        for (int i = 0; i < Charges.Length; i++)
        {
            Charges[i] = Instantiate(ChargePref);
            Charges[i].SetActive(false);
        }

        for (int i = 0; i < Boomerangs.Length; i++)
        {
            Boomerangs[i] = Instantiate(BoomerangPref);
            Boomerangs[i].SetActive(false);
        }

        for (int i = 0; i < Splits.Length; i++)
        {
            Splits[i] = Instantiate(SplitPref);
            Splits[i].SetActive(false);
        }

        for (int i = 0; i < Pieces.Length; i++)
        {
            Pieces[i] = Instantiate(PiecePref);
            Pieces[i].SetActive(false);
        }


        for (int i = 0; i < SubWeapons.Length; i++)
        {
            SubWeapons[i] = Instantiate(SubWeaponPref);
            SubWeapon sub = SubWeapons[i].gameObject.GetComponent<SubWeapon>();
            GameManager.Inst().SetSubWeapons(sub, i);
            GameManager.Inst().SubWID[i] = SubWeapons[i].GetInstanceID();
            SubWeapons[i].SetActive(false);
        }


        for(int i = 0; i < Explosions.Length; i++)
        {
            Explosions[i] = Instantiate(ExplosionPref);
            Explosions[i].SetActive(false);
        }
    }
}
