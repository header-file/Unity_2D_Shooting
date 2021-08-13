using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //Pool
    public GameObject EnemyPool;
    public GameObject ItemPool;
    public GameObject PBulletPool;
    public GameObject EBulletPool;
    public GameObject SubWeaponPool;
    public GameObject UIPool;
    public GameObject EffectPool;

    //Enemy
    public GameObject Enemy_SPref;
    public GameObject Enemy_MPref;
    public GameObject Enemy_LPref;
    public GameObject Enemy_BPref;

    //Item
    public GameObject CoinPref;
    public GameObject EqAttackPref;
    public GameObject EqRangePref;
    public GameObject EqSpeedPref;
    public GameObject ShieldPref;
    public GameObject ItemBombPref;
    public GameObject BombPref;
    public GameObject EqMagnetPref;
    public GameObject EqHomingPref;
    public GameObject EqHealPref;
    public GameObject EqVampPref;
    public GameObject EqShieldPref;
    public GameObject EqRevivePref;
    public GameObject EqReinforcePref;
    public GameObject EqBackPref;
    public GameObject EqSlowerPref;
    public GameObject ResourcePref;

    //Player Bullet
    public GameObject[] NormalPref;
    public GameObject[] SpreadPref;
    public GameObject[] MissilePref;
    public GameObject[] LaserPref;
    public GameObject[] ChargePref;
    public GameObject[] BoomerangPref;
    public GameObject[] ChainPref;
    public GameObject[] GatlingPref;
    public GameObject[] ExplodePref;
    public GameObject[] DotPref;
    public GameObject EqMissilePref;
    public GameObject EqKnockBackPref;
    public GameObject EqSlowPref;

    //BossBullet
    public GameObject BossNormalPref;
    public GameObject BossLaserPref;
    public GameObject BossOneWayPref;
    public GameObject BossBigBulletPref;
    public GameObject BossBouncePref;

    //SubWeapon
    public GameObject SubWeaponPref;

    //UI
    public GameObject DmgTextPref;
    public GameObject InventorySlotPref;
    public GameObject LinePref;
    public GameObject QuestSlotPref;
    public GameObject SideMenuSlotNowPref;
    public GameObject SideMenuSlotNotYetPref;
    public GameObject SideMenuSlotClearedPref;

    //Effect
    public GameObject ExplosionPref;
    public GameObject HitPref;
    public GameObject[] EquipPopPref;
    public GameObject EquipActionPref;
    public GameObject MagnetActionPref;
    public GameObject HealActionPref;

    GameObject[] Enemies_S;
    GameObject[] Enemies_M;
    GameObject[] Enemies_L;
    GameObject Enemy_B;

    GameObject[] Coins;
    GameObject[] EqAttacks;
    GameObject[] EqRanges;
    GameObject[] EqSpeeds;
    GameObject[] Shields;
    GameObject[] ItemBombs;
    GameObject[] Bombs;
    GameObject[] EqMagnets;
    GameObject[] EqHomings;
    GameObject[] EqHeals;
    GameObject[] EqVamps;
    GameObject[] EqShields;
    GameObject[] EqRevives;
    GameObject[] EqReinforces;
    GameObject[] EqBacks;
    GameObject[] EqSlowers;
    GameObject[] Resources;

    GameObject[,] Normals;
    GameObject[,] Spreads;
    GameObject[,] Missiles;
    GameObject[,] Lasers;
    GameObject[,] Charges;
    GameObject[,] Boomerangs;
    GameObject[,] Chains;
    GameObject[,] Gatlings;
    GameObject[,] Explodes;
    GameObject[,] Dots;
    GameObject[] EqMissiles;
    GameObject[] EqKnockbacks;
    GameObject[] EqSlows;

    GameObject[] BossNormals;
    GameObject[] BossLasers;
    GameObject[] BossOneWays;
    GameObject[] BossBigBullets;
    GameObject[] BossBounces;

    GameObject[] SubWeapons;

    GameObject[] DmgTexts;
    GameObject[] InventorySlots;
    GameObject[] Lines;
    GameObject[] QuestSlots;
    GameObject[] SideMenuSlotNows;
    GameObject[] SideMenuSlotNotYets;
    GameObject[] SideMenuSlotCleareds;


    GameObject[] Explosions;
    GameObject[] Hits;
    GameObject[] EquipPopsW;
    GameObject[] EquipPopsG;
    GameObject[] EquipPopsB;
    GameObject[] EquipPopsP;
    GameObject[] EquipPopsY;
    GameObject[] EquipActions;
    GameObject[] MagnetActions;
    GameObject[] HealActions;


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

            case "Chain":
                TargetPools = Chains;
                break;

            case "Gatling":
                TargetPools = Gatlings;
                break;

            case "Explosion":
                TargetPools = Explodes;
                break;

            case "Dot":
                TargetPools = Dots;
                break;
        }

        for (int i = 0; i < TargetPools.Length / GameManager.Inst().ShtManager.MAXCOLOR; i++)
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

            case "EnemyB":
                Enemy_B.SetActive(true);
                return Enemy_B;

            case "BossNormal":
                TargetPool = BossNormals;
                break;

            case "BossLaser":
                TargetPool = BossLasers;
                break;

            case "BossOneWay":
                TargetPool = BossOneWays;
                break;

            case "BossBigBullet":
                TargetPool = BossBigBullets;
                break;

            case "BossBounce":
                TargetPool = BossBounces;
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

            case "Shield":
                TargetPool = Shields;
                break;

            case "ItemBomb":
                TargetPool = ItemBombs;
                break;

            case "Bomb":
                TargetPool = Bombs;
                break;

            case "EqMagnet":
                TargetPool = EqMagnets;
                break;

            case "EqHoming":
                TargetPool = EqHomings;
                break;

            case "EqMissile":
                TargetPool = EqMissiles;
                break;

            case "EqHeal":
                TargetPool = EqHeals;
                break;

            case "EqVamp":
                TargetPool = EqVamps;
                break;

            case "EqShield":
                TargetPool = EqShields;
                break;

            case "EqRevive":
                TargetPool = EqRevives;
                break;

            case "EqReinforce":
                TargetPool = EqReinforces;
                break;

            case "EqBack":
                TargetPool = EqBacks;
                break;

            case "EqKnockback":
                TargetPool = EqKnockbacks;
                break;

            case "EqSlower":
                TargetPool = EqSlowers;
                break;

            case "EqSlow":
                TargetPool = EqSlows;
                break;

            case "Resource":
                TargetPool = Resources;
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

            case "Line":
                TargetPool = Lines;
                break;

            case "QuestSlot":
                TargetPool = QuestSlots;
                break;

            case "SideNow":
                TargetPool = SideMenuSlotNows;
                break;

            case "SideNotYet":
                TargetPool = SideMenuSlotNotYets;
                break;

            case "SideCleared":
                TargetPool = SideMenuSlotCleareds;
                break;

            case "Explosion":
                TargetPool = Explosions;
                break;

            case "Hit":
                TargetPool = Hits;
                break;

            case "EquipPopW":
                TargetPool = EquipPopsW;
                break;

            case "EquipPopG":
                TargetPool = EquipPopsG;
                break;

            case "EquipPopB":
                TargetPool = EquipPopsB;
                break;

            case "EquipPopP":
                TargetPool = EquipPopsP;
                break;

            case "EquipPopY":
                TargetPool = EquipPopsY;
                break;

            case "EquipAction":
                TargetPool = EquipActions;
                break;

            case "MagnetAction":
                TargetPool = MagnetActions;
                break;

            case "HealAction":
                TargetPool = HealActions;
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
        GameManager.Inst().ObjManager = gameObject.GetComponent<ObjectManager>();

        Enemies_S = new GameObject[100];
        Enemies_M = new GameObject[10];
        Enemies_L = new GameObject[10];
        Enemy_B = new GameObject();

        Coins = new GameObject[100];
        EqAttacks = new GameObject[10];
        EqRanges = new GameObject[10];
        EqSpeeds = new GameObject[10];
        Shields = new GameObject[5];
        ItemBombs = new GameObject[5];
        Bombs = new GameObject[5];
        EqMagnets = new GameObject[10];
        EqHomings = new GameObject[10];
        EqHeals = new GameObject[10];
        EqVamps = new GameObject[10];
        EqShields = new GameObject[10];
        EqRevives = new GameObject[10];
        EqReinforces = new GameObject[10];
        EqBacks = new GameObject[10];
        EqSlowers = new GameObject[10];
        Resources = new GameObject[40];

        Normals = new GameObject[8, 100];
        Spreads = new GameObject[8, 100];
        Missiles = new GameObject[8, 50];
        Lasers = new GameObject[8, 10];
        Charges = new GameObject[8, 20];
        Boomerangs = new GameObject[8, 10];
        Chains = new GameObject[8, 10];
        Gatlings = new GameObject[8, 100];
        Explodes = new GameObject[8, 20];
        Dots = new GameObject[8, 30];
        EqMissiles = new GameObject[10];
        EqKnockbacks = new GameObject[10];
        EqSlows = new GameObject[10];

        BossNormals = new GameObject[60];
        BossLasers = new GameObject[12];
        BossOneWays = new GameObject[15];
        BossBigBullets = new GameObject[3];
        BossBounces = new GameObject[10];

        SubWeapons = new GameObject[4];

        DmgTexts = new GameObject[40];
        InventorySlots = new GameObject[Constants.MAXINVENTORY];
        Lines = new GameObject[3];
        QuestSlots = new GameObject[10];
        SideMenuSlotNows = new GameObject[4];
        SideMenuSlotNotYets = new GameObject[4];
        SideMenuSlotCleareds = new GameObject[4];

        Explosions = new GameObject[50];
        Hits = new GameObject[50];
        EquipPopsW = new GameObject[25];
        EquipPopsG = new GameObject[25];
        EquipPopsB = new GameObject[25];
        EquipPopsP = new GameObject[25];
        EquipPopsY = new GameObject[25];
        EquipActions = new GameObject[10];
        MagnetActions = new GameObject[10];
        HealActions = new GameObject[10];

        Generate();
    }

    void Generate()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Datas/EnemyData");
        GameManager.Inst().StgManager.SetTimeData(data);

        for (int i = 0; i < Enemies_S.Length; i++)
        {
            Enemies_S[i] = Instantiate(Enemy_SPref);
            Enemies_S[i].GetComponent<Enemy>().SetDatas(data, 0);
            Enemies_S[i].transform.SetParent(EnemyPool.transform, false);
            Enemies_S[i].SetActive(false);
        }

        for (int i = 0; i < Enemies_M.Length; i++)
        {
            Enemies_M[i] = Instantiate(Enemy_MPref);
            Enemies_M[i].GetComponent<Enemy>().SetDatas(data, 1);
            Enemies_M[i].transform.SetParent(EnemyPool.transform, false);
            Enemies_M[i].SetActive(false);
        }

        for (int i = 0; i < Enemies_L.Length; i++)
        {
            Enemies_L[i] = Instantiate(Enemy_LPref);
            Enemies_L[i].GetComponent<Enemy>().SetDatas(data, 2);
            Enemies_L[i].transform.SetParent(EnemyPool.transform, false);
            Enemies_L[i].SetActive(false);
        }

        Enemy_B = Instantiate(Enemy_BPref);
        Enemy_B.GetComponent<Enemy>().SetDatas(data, 3);
        Enemy_B.transform.SetParent(EnemyPool.transform, false);
        Enemy_B.SetActive(false);

        for(int i = 0; i < BossNormals.Length; i++)
        {
            BossNormals[i] = Instantiate(BossNormalPref);
            BossNormals[i].transform.SetParent(EBulletPool.transform, false);
            BossNormals[i].SetActive(false);
        }

        for (int i = 0; i < BossLasers.Length; i++)
        {
            BossLasers[i] = Instantiate(BossLaserPref);
            BossLasers[i].transform.SetParent(EBulletPool.transform, false);
            BossLasers[i].SetActive(false);
        }

        for (int i = 0; i < BossOneWays.Length; i++)
        {
            BossOneWays[i] = Instantiate(BossOneWayPref);
            BossOneWays[i].transform.SetParent(EBulletPool.transform, false);
            BossOneWays[i].SetActive(false);
        }

        for (int i = 0; i < BossBigBullets.Length; i++)
        {
            BossBigBullets[i] = Instantiate(BossBigBulletPref);
            BossBigBullets[i].transform.SetParent(EBulletPool.transform, false);
            BossBigBullets[i].SetActive(false);
        }

        for (int i = 0; i < BossBounces.Length; i++)
        {
            BossBounces[i] = Instantiate(BossBouncePref);
            BossBounces[i].transform.SetParent(EBulletPool.transform, false);
            BossBounces[i].SetActive(false);
        }

        for (int i = 0; i < Coins.Length; i++)
        {
            Coins[i] = Instantiate(CoinPref);
            Coins[i].transform.SetParent(ItemPool.transform, false);
            Coins[i].SetActive(false);
        }

        for (int i = 0; i < EqAttacks.Length; i++)
        {
            EqAttacks[i] = Instantiate(EqAttackPref);
            EqAttacks[i].transform.SetParent(ItemPool.transform, false);
            EqAttacks[i].SetActive(false);
        }

        for (int i = 0; i < EqRanges.Length; i++)
        {
            EqRanges[i] = Instantiate(EqRangePref);
            EqRanges[i].transform.SetParent(ItemPool.transform, false);
            EqRanges[i].SetActive(false);
        }

        for (int i = 0; i < EqSpeeds.Length; i++)
        {
            EqSpeeds[i] = Instantiate(EqSpeedPref);
            EqSpeeds[i].transform.SetParent(ItemPool.transform, false);
            EqSpeeds[i].SetActive(false);
        }

        for(int i = 0; i < Shields.Length; i++)
        {
            Shields[i] = Instantiate(ShieldPref);
            Shields[i].transform.SetParent(ItemPool.transform, false);
            Shields[i].SetActive(false);
        }

        for (int i = 0; i < ItemBombs.Length; i++)
        {
            ItemBombs[i] = Instantiate(ItemBombPref);
            ItemBombs[i].transform.SetParent(ItemPool.transform, false);
            ItemBombs[i].SetActive(false);
        }

        for (int i = 0; i < Bombs.Length; i++)
        {
            Bombs[i] = Instantiate(BombPref);
            Bombs[i].transform.SetParent(ItemPool.transform, false);
            Bombs[i].SetActive(false);
        }

        for (int i = 0; i < EqMagnets.Length; i++)
        {
            EqMagnets[i] = Instantiate(EqMagnetPref);
            EqMagnets[i].transform.SetParent(ItemPool.transform, false);
            EqMagnets[i].SetActive(false);
        }

        for (int i = 0; i < EqHomings.Length; i++)
        {
            EqHomings[i] = Instantiate(EqHomingPref);
            EqHomings[i].transform.SetParent(ItemPool.transform, false);
            EqHomings[i].SetActive(false);
        }

        for (int i = 0; i < EqMissiles.Length; i++)
        {
            EqMissiles[i] = Instantiate(EqMissilePref);
            EqMissiles[i].transform.SetParent(PBulletPool.transform, false);
            EqMissiles[i].SetActive(false);
        }

        for (int i = 0; i < EqHeals.Length; i++)
        {
            EqHeals[i] = Instantiate(EqHealPref);
            EqHeals[i].transform.SetParent(ItemPool.transform, false);
            EqHeals[i].SetActive(false);
        }

        for (int i = 0; i < EqVamps.Length; i++)
        {
            EqVamps[i] = Instantiate(EqVampPref);
            EqVamps[i].transform.SetParent(ItemPool.transform, false);
            EqVamps[i].SetActive(false);
        }

        for (int i = 0; i < EqShields.Length; i++)
        {
            EqShields[i] = Instantiate(EqShieldPref);
            EqShields[i].transform.SetParent(ItemPool.transform, false);
            EqShields[i].SetActive(false);
        }

        for (int i = 0; i < EqRevives.Length; i++)
        {
            EqRevives[i] = Instantiate(EqRevivePref);
            EqRevives[i].transform.SetParent(ItemPool.transform, false);
            EqRevives[i].SetActive(false);
        }

        for(int i = 0; i < EqReinforces.Length; i++)
        {
            EqReinforces[i] = Instantiate(EqReinforcePref);
            EqReinforces[i].transform.SetParent(ItemPool.transform, false);
            EqReinforces[i].SetActive(false);
        }

        for (int i = 0; i < EqBacks.Length; i++)
        {
            EqBacks[i] = Instantiate(EqBackPref);
            EqBacks[i].transform.SetParent(ItemPool.transform, false);
            EqBacks[i].SetActive(false);
        }

        for (int i = 0; i < EqKnockbacks.Length; i++)
        {
            EqKnockbacks[i] = Instantiate(EqKnockBackPref);
            EqKnockbacks[i].transform.SetParent(PBulletPool.transform, false);
            EqKnockbacks[i].SetActive(false);
        }

        for (int i = 0; i < EqSlowers.Length; i++)
        {
            EqSlowers[i] = Instantiate(EqSlowerPref);
            EqSlowers[i].transform.SetParent(ItemPool.transform, false);
            EqSlowers[i].SetActive(false);
        }

        for (int i = 0; i < EqSlows.Length; i++)
        {
            EqSlows[i] = Instantiate(EqSlowPref);
            EqSlows[i].transform.SetParent(PBulletPool.transform, false);
            EqSlows[i].SetActive(false);
        }

        for (int i = 0; i < Resources.Length; i++)
        {
            Resources[i] = Instantiate(ResourcePref);
            Resources[i].transform.SetParent(ItemPool.transform, false);
            Resources[i].SetActive(false);
        }


        int maxColor = GameManager.Inst().ShtManager.MAXCOLOR;
        
        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Normals.Length / maxColor; i++)
            {
                Normals[j, i] = Instantiate(NormalPref[j]);
                Normals[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Normals[j, i].transform.SetParent(PBulletPool.transform, false);
                Normals[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Spreads.Length / maxColor; i++)
            {
                Spreads[j, i] = Instantiate(SpreadPref[j]);
                Spreads[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Spreads[j, i].transform.SetParent(PBulletPool.transform, false);
                Spreads[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Missiles.Length / maxColor; i++)
            {
                Missiles[j, i] = Instantiate(MissilePref[j]);
                Missiles[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Missiles[j, i].transform.SetParent(PBulletPool.transform, false);
                Missiles[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Lasers.Length / maxColor; i++)
            {
                Lasers[j, i] = Instantiate(LaserPref[j]);
                Lasers[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Lasers[j, i].transform.SetParent(PBulletPool.transform, false);
                Lasers[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Charges.Length / maxColor; i++)
            {
                Charges[j, i] = Instantiate(ChargePref[j]);
                Charges[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Charges[j, i].transform.SetParent(PBulletPool.transform, false);
                Charges[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Boomerangs.Length / maxColor; i++)
            {
                Boomerangs[j, i] = Instantiate(BoomerangPref[j]);
                Boomerangs[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Boomerangs[j, i].transform.SetParent(PBulletPool.transform, false);
                Boomerangs[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Chains.Length / maxColor; i++)
            {
                Chains[j, i] = Instantiate(ChainPref[j]);
                Chains[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Chains[j, i].transform.SetParent(PBulletPool.transform, false);
                Chains[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Gatlings.Length / maxColor; i++)
            {
                Gatlings[j, i] = Instantiate(GatlingPref[j]);
                Gatlings[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Gatlings[j, i].transform.SetParent(PBulletPool.transform, false);
                Gatlings[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Explodes.Length / maxColor; i++)
            {
                Explodes[j, i] = Instantiate(ExplodePref[j]);
                Explodes[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Explodes[j, i].transform.SetParent(PBulletPool.transform, false);
                Explodes[j, i].SetActive(false);
            }
        }

        for (int j = 0; j < maxColor; j++)
        {
            for (int i = 0; i < Dots.Length / maxColor; i++)
            {
                Dots[j, i] = Instantiate(DotPref[j]);
                Dots[j, i].GetComponent<SpriteRenderer>().material.SetColor("_GlowColor", GameManager.Inst().ShtManager.GetColors(j));
                Dots[j, i].transform.SetParent(PBulletPool.transform, false);
                Dots[j, i].SetActive(false);
            }
        }


        for (int i = 0; i < SubWeapons.Length; i++)
        {
            SubWeapons[i] = Instantiate(SubWeaponPref);
            //SubWeapon sub = SubWeapons[i].gameObject.GetComponent<SubWeapon>();
            //GameManager.Inst().SetSubWeapons(sub, i);
            //GameManager.Inst().SubWID[i] = SubWeapons[i].GetInstanceID();
            SubWeapons[i].transform.SetParent(SubWeaponPool.transform, false);
            SubWeapons[i].SetActive(false);
        }


        for(int i = 0; i < DmgTexts.Length; i++)
        {
            DmgTexts[i] = Instantiate(DmgTextPref);
            DmgTexts[i].transform.SetParent(UIPool.transform, false);
            DmgTexts[i].SetActive(false);
        }

        for(int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlots[i] = Instantiate(InventorySlotPref);
            InventorySlots[i].transform.SetParent(UIPool.transform, false);
            InventorySlots[i].SetActive(false);
        }

        for(int i = 0; i < Lines.Length; i++)
        {
            Lines[i] = Instantiate(LinePref);
            Lines[i].transform.SetParent(UIPool.transform, false);
            Lines[i].SetActive(false);
        }

        for(int i = 0; i < QuestSlots.Length; i++)
        {
            QuestSlots[i] = Instantiate(QuestSlotPref);
            QuestSlots[i].transform.SetParent(UIPool.transform, false);
            QuestSlots[i].SetActive(false);
        }

        for (int i = 0; i < SideMenuSlotNows.Length; i++)
        {
            SideMenuSlotNows[i] = Instantiate(SideMenuSlotNowPref);
            SideMenuSlotNows[i].transform.SetParent(UIPool.transform, false);
            SideMenuSlotNows[i].SetActive(false);
        }

        for (int i = 0; i < SideMenuSlotNotYets.Length; i++)
        {
            SideMenuSlotNotYets[i] = Instantiate(SideMenuSlotNotYetPref);
            SideMenuSlotNotYets[i].transform.SetParent(UIPool.transform, false);
            SideMenuSlotNotYets[i].SetActive(false);
        }

        for (int i = 0; i < SideMenuSlotCleareds.Length; i++)
        {
            SideMenuSlotCleareds[i] = Instantiate(SideMenuSlotClearedPref);
            SideMenuSlotCleareds[i].transform.SetParent(UIPool.transform, false);
            SideMenuSlotCleareds[i].SetActive(false);
        }


        for (int i = 0; i < Explosions.Length; i++)
        {
            Explosions[i] = Instantiate(ExplosionPref);
            Explosions[i].transform.SetParent(EffectPool.transform, false);
            Explosions[i].SetActive(false);
        }

        for (int i = 0; i < Hits.Length; i++)
        {
            Hits[i] = Instantiate(HitPref);
            Hits[i].transform.SetParent(EffectPool.transform, false);
            Hits[i].SetActive(false);
        }

        for(int i = 0; i < EquipPopsW.Length; i++)
        {
            EquipPopsW[i] = Instantiate(EquipPopPref[0]);
            EquipPopsW[i].transform.SetParent(EffectPool.transform, false);
            EquipPopsW[i].SetActive(false);
        }

        for (int i = 0; i < EquipPopsG.Length; i++)
        {
            EquipPopsG[i] = Instantiate(EquipPopPref[1]);
            EquipPopsG[i].transform.SetParent(EffectPool.transform, false);
            EquipPopsG[i].SetActive(false);
        }

        for (int i = 0; i < EquipPopsB.Length; i++)
        {
            EquipPopsB[i] = Instantiate(EquipPopPref[2]);
            EquipPopsB[i].transform.SetParent(EffectPool.transform, false);
            EquipPopsB[i].SetActive(false);
        }

        for (int i = 0; i < EquipPopsP.Length; i++)
        {
            EquipPopsP[i] = Instantiate(EquipPopPref[3]);
            EquipPopsP[i].transform.SetParent(EffectPool.transform, false);
            EquipPopsP[i].SetActive(false);
        }

        for (int i = 0; i < EquipPopsY.Length; i++)
        {
            EquipPopsY[i] = Instantiate(EquipPopPref[4]);
            EquipPopsY[i].transform.SetParent(EffectPool.transform, false);
            EquipPopsY[i].SetActive(false);
        }

        for (int i = 0; i < EquipActions.Length; i++)
        {
            EquipActions[i] = Instantiate(EquipActionPref);
            EquipActions[i].transform.SetParent(EffectPool.transform, false);
            EquipActions[i].SetActive(false);
        }

        for (int i = 0; i < MagnetActions.Length; i++)
        {
            MagnetActions[i] = Instantiate(MagnetActionPref);
            MagnetActions[i].transform.SetParent(EffectPool.transform, false);
            MagnetActions[i].SetActive(false);
        }

        for (int i = 0; i < HealActions.Length; i++)
        {
            HealActions[i] = Instantiate(HealActionPref);
            HealActions[i].transform.SetParent(EffectPool.transform, false);
            HealActions[i].SetActive(false);
        }
    }
}
