using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    //씬 유지
    public UpgradeManager UpgManager;
    public StageManager StgManager;
    public DataManager DatManager;
    public ResourceManager ResManager;
    public AdvertiseManager AdsManager;
    public SoundManager SodManager;
    public BuffManager BufManager;
    public Login Login;

    //씬마다 생성
    public ShootingManager ShtManager;
    public ObjectManager ObjManager;
    public InputManager IptManager;
    public TextManager TxtManager;
    public UIManager UiManager;
    public QuestManager QstManager;
    public ShakeManager ShkManager;
    public EquipManager EquManager;

    //정가 구매
    public bool IsFullPrice;

    //튜토리얼용
    public Tutorials Tutorials;
    public bool IsTutorial;

    public Player Player;

    public int[,] SubWID;
    public int Jewel;
    public int[] Resources;
    public int[,,] EquipDatas;

    SubWeapon[] SubWeapons;
    
    List<Dictionary<string, object>> DropRateData;

    public static GameManager Inst() { return Instance; }
    public SubWeapon GetSubweapons(int index) { return SubWeapons[index] != null ? SubWeapons[index] : null; }
    public int GetDropRate(int stage, string grade) { return int.Parse(DropRateData[stage][grade].ToString()); }

    public void SetSubWeapons(SubWeapon Sub, int index) { SubWeapons[index] = Sub; }
    public void SetJewel(int value) { Jewel = value; UiManager.MainUI.JewelText.text = string.Format("{0:#,###}", Jewel); }
    public void AddJewel(int value) { Jewel += value; UiManager.MainUI.JewelText.text = string.Format("{0:#,###}", Jewel); }
    public void SubtractJewel(int value) { Jewel -= value; UiManager.MainUI.JewelText.text = string.Format("{0:#,###}", Jewel); }
    public void SetResource(int stage, int value) { Resources[stage - 1] = value; UiManager.MainUI.Resources[stage - 1].text = string.Format("{0:#,###}", Resources[stage - 1]); }
    public void AddResource(int stage, int value) { Resources[stage - 1] += value; UiManager.MainUI.Resources[stage - 1].text = string.Format("{0:#,###}", Resources[stage - 1]); }
    public void SubtractResource(int stage, int value) { Resources[stage] -= value; UiManager.MainUI.Resources[stage].text = string.Format("{0:#,###}", Resources[stage]); }

    void Awake()
    {
        GameManager[] objs = FindObjectsOfType<GameManager>();
        if (objs.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

//#if UNITY_EDITOR
//        Debug.unityLogger.logEnabled = true;
//#else
//        Debug.unityLogger.logEnabled=false;
//#endif

        Jewel = 0;
        Resources = new int[Constants.MAXSTAGES];

        SubWeapons = new SubWeapon[Constants.MAXSUBWEAPON];
        SubWID = new int[Constants.MAXSTAGES, 4];

        EquipDatas = new int[Constants.MAXEQUIPTYPE, Constants.MAXRARITY, Constants.MAXEQUIPDATAS];

        if(GameObject.Find("LoginManager") != null)
            Login =  GameObject.Find("LoginManager").GetComponent<Login>();

        IsFullPrice = false;
    }

    void Start()
    {
        SetDropRateData();
        SetEquipDatas();
        SetResources();
    }

    public void ResetSetManagers()
    {
        UpgManager.ResetTutorial();
        StgManager.Stage = 1;
        StgManager.ReachedStage = 1;
        QstManager.ResetQuestData();
        BufManager.BuffTimers[0] = 0.0f;
        for (int i = 0; i < Constants.MAXSTAGES; i++)
            ResManager.IsStartCount[i] = false;
        IsFullPrice = false;

        Player.SetCoin(0);
        Player.ResetInventories();
        Player.SetBulletType(0);
        Jewel = 0;
        for (int i = 1; i <= Constants.MAXRESOURCETYPES; i++)
            SetResource(i, 0);
    }

    void SetDropRateData()
    {
        DropRateData = CSVReader.Read("Datas/DropRate");
    }

    void SetEquipDatas()
    {
        List<Dictionary<string, object>> datas = CSVReader.Read("Datas/ZzinEquipData");

        for (int i = 0; i < Constants.MAXEQUIPTYPE; i++)
        {
            for (int j = 0; j < Constants.MAXRARITY; j++)
            {
                EquipDatas[i, j, 0] = int.Parse(datas[i * Constants.MAXRARITY + j]["COOLTIME"].ToString());
                EquipDatas[i, j, 1] = int.Parse(datas[i * Constants.MAXRARITY + j]["MIN"].ToString());
                EquipDatas[i, j, 2] = int.Parse(datas[i * Constants.MAXRARITY + j]["MAX"].ToString());
            }
        }
    }

    void SetResources()
    {
        for (int i = 0; i < Constants.MAXRESOURCETYPES; i++)
            UiManager.MainUI.Resources[i].text = Resources[i].ToString();
    }

    public void MakeEquip(int type, int grade, Transform transform)
    {
        int rand = type;
        if (rand == -1)
            rand = Random.Range(0, Constants.MAXEQUIPTYPE);

        int uid = 0;
        switch (rand)
        {
            case 0:
                GameObject eq = ObjManager.MakeObj("EqMagnet");
                eq.transform.position = transform.position;
                Item_ZzinEquipment eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                int rarity = eqp.SetGrade(grade);
                int val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.MAGNET, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.MAGNET, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.MAGNET, rarity, 0], val, uid);
                break;

            case 1:
                eq = ObjManager.MakeObj("EqHoming");
                eq.transform.position = transform.position;
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.HOMING, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.HOMING, rarity, 2]);                

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.HOMING, rarity, 0], val, uid);
                break;

            case 2:
                eq = ObjManager.MakeObj("EqHeal");
                eq.transform.position = transform.position;
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.HEAL, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.HEAL, rarity, 2]);                

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.HEAL, rarity, 0], val, uid);
                break;

            case 3:
                eq = ObjManager.MakeObj("EqVamp");
                eq.transform.position = transform.position;
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.VAMP, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.VAMP, rarity, 2]);                

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.VAMP, rarity, 0], val, uid);
                break;

            case 4:
                eq = ObjManager.MakeObj("EqShield");
                eq.transform.position = transform.position;
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.SHIELD, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.SHIELD, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.SHIELD, rarity, 0], val, uid);
                break;

            case 5:
                eq = ObjManager.MakeObj("EqRevive");
                eq.transform.position = transform.position;
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.REVIVE, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.REVIVE, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.REVIVE, rarity, 0], val, uid);
                break;

            case 6:
                eq = ObjManager.MakeObj("EqReinforce");
                eq.transform.position = transform.position;
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.REINFORCE, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.REINFORCE, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.REINFORCE, rarity, 0], val, uid);
                break;

            case 7:
                eq = ObjManager.MakeObj("EqBack");
                eq.transform.position = transform.position;
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.KNOCKBACK, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.KNOCKBACK, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.KNOCKBACK, rarity, 0], val, uid);
                break;

            case 8:
                eq = ObjManager.MakeObj("EqSlower");
                eq.transform.position = transform.position;
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.SLOW, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.SLOW, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.SLOW, rarity, 0], val, uid);
                break;
        }
    }

    public int MakeEquipData(int type, int grade)
    {
        int rand = type;
        if (rand == -1)
            rand = Random.Range(0, Constants.MAXEQUIPTYPE);

        int uid = 0;
        Item_ZzinEquipment eqp = null;
        switch (rand)
        {
            case 0:
                GameObject eq = ObjManager.MakeObj("EqMagnet");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                int rarity = eqp.SetGrade(grade);
                int val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.MAGNET, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.MAGNET, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.MAGNET, rarity, 0], val, uid);
                break;

            case 1:
                eq = ObjManager.MakeObj("EqHoming");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.HOMING, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.HOMING, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.HOMING, rarity, 0], val, uid);
                break;

            case 2:
                eq = ObjManager.MakeObj("EqHeal");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.HEAL, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.HEAL, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.HEAL, rarity, 0], val, uid);
                break;

            case 3:
                eq = ObjManager.MakeObj("EqVamp");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.VAMP, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.VAMP, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.VAMP, rarity, 0], val, uid);
                break;

            case 4:
                eq = ObjManager.MakeObj("EqShield");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.SHIELD, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.SHIELD, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.SHIELD, rarity, 0], val, uid);
                break;

            case 5:
                eq = ObjManager.MakeObj("EqRevive");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.REVIVE, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.REVIVE, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.REVIVE, rarity, 0], val, uid);
                break;

            case 6:
                eq = ObjManager.MakeObj("EqReinforce");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.REINFORCE, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.REINFORCE, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.REINFORCE, rarity, 0], val, uid);
                break;

            case 7:
                eq = ObjManager.MakeObj("EqBack");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.KNOCKBACK, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.KNOCKBACK, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.KNOCKBACK, rarity, 0], val, uid);
                break;

            case 8:
                eq = ObjManager.MakeObj("EqSlower");
                eqp = eq.GetComponent<Item_ZzinEquipment>();
                eqp.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.EQUIPMENT + (grade + 1) * 10 + (int)Item.UIDCombination.ETC;
                rarity = eqp.SetGrade(grade);
                val = Random.Range(EquipDatas[(int)Item_ZzinEquipment.EquipType.SLOW, rarity, 1], EquipDatas[(int)Item_ZzinEquipment.EquipType.SLOW, rarity, 2]);

                eqp.SetValues(EquipDatas[(int)Item_ZzinEquipment.EquipType.SLOW, rarity, 0], val, uid);
                break;
        }

        int index = Player.AddItem(eqp);
        eqp.gameObject.SetActive(false);
        
        return index;
    }

    public void MakeReinforce(int type, int grade, Transform transform)
    {
        int rand = type;
        if (rand == -1)
            rand = Random.Range(0, 3);

        int uid = 0;
        grade = 0;

        switch (rand)
        {
            case 0:
                GameObject eqAtk = ObjManager.MakeObj("EqAttack");
                eqAtk.transform.position = transform.position;
                Item_Equipment eqpAtk = eqAtk.GetComponent<Item_Equipment>();
                eqpAtk.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.REINFORCE + (grade + 1) * 10 + (int)Item.UIDCombination.ATK;

                eqpAtk.SetValues(grade, uid, rand);
                break;
            case 1:
                GameObject eqRng = ObjManager.MakeObj("EqRange");
                eqRng.transform.position = transform.position;
                Item_Equipment eqpRng = eqRng.GetComponent<Item_Equipment>();
                eqpRng.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.REINFORCE + (grade + 1) * 10 + (int)Item.UIDCombination.HP;

                eqpRng.SetValues(grade, uid, rand);
                break;
            case 2:
                GameObject eqSpd = ObjManager.MakeObj("EqSpeed");
                eqSpd.transform.position = transform.position;
                Item_Equipment eqpSpd = eqSpd.GetComponent<Item_Equipment>();
                eqpSpd.StartAbsorb(0.5f);
                uid = (int)Item.UIDCombination.REINFORCE + (grade + 1) * 10 + (int)Item.UIDCombination.SPD;

                eqpSpd.SetValues(grade, uid, rand);
                break;
        }
    }

    public int MakeReinforceData(int type, int grade)
    {
        int rand = type;
        if (rand == -1)
            rand = Random.Range(0, 3);

        GameObject eq;
        Item_Equipment ieq = null;
        int uid = 0;
        grade = 0;

        switch (rand)
        {
            case 0:
                eq = ObjManager.MakeObj("EqAttack");
                ieq = eq.GetComponent<Item_Equipment>();
                ieq.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.REINFORCE + (grade + 1) * 10 + (int)Item.UIDCombination.ATK;
                ieq.SetValues(grade, uid, rand);
                ieq.gameObject.SetActive(false);
                break;

            case 1:
                eq = ObjManager.MakeObj("EqRange");
                ieq = eq.GetComponent<Item_Equipment>();
                ieq.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.REINFORCE + (grade + 1) * 10 + (int)Item.UIDCombination.HP;
                ieq.SetValues(grade, uid, rand);
                ieq.gameObject.SetActive(false);
                break;

            case 2:
                eq = ObjManager.MakeObj("EqSpeed");
                ieq = eq.GetComponent<Item_Equipment>();
                ieq.StartAbsorb(0.0f);
                uid = (int)Item.UIDCombination.REINFORCE + (grade + 1) * 10 + (int)Item.UIDCombination.SPD;
                ieq.SetValues(grade, uid, rand);
                ieq.gameObject.SetActive(false);
                break;
        }

        int index = Player.AddItem(ieq);
        ieq.gameObject.SetActive(false);

        return index;
    }

    public void SetInventory()
    {
        InventoryScroll inventory = UiManager.InventoryScroll.GetComponent<InventoryScroll>();
        for (int i = 0; i < Player.MaxInventory; i++)
        {
            GameObject inventorySlot = ObjManager.MakeObj("InventorySlot");
            inventory.SetPhysicalInventory(i, inventorySlot);
            inventorySlot.transform.SetParent(inventory.Contents.transform, false);
            InventorySlot slot = inventorySlot.GetComponent<InventorySlot>();
            slot.SetIndex(i);
            slot.SetType(-1);
            inventory.SetInventory(i, slot);
            inventorySlot.name = i.ToString();
        }

        inventory.Contents.SetActive(false);
    }

    public void AddInventory(int count)
    {
        InventoryScroll inventory = UiManager.InventoryScroll.GetComponent<InventoryScroll>();

        for (int i = Player.MaxInventory - count; i < Player.MaxInventory; i++)
        {
            GameObject inventorySlot = Inst().ObjManager.MakeObj("InventorySlot");
            inventory.SetPhysicalInventory(i, inventorySlot);
            inventorySlot.transform.SetParent(inventory.Contents.transform, false);
            InventorySlot slot = inventorySlot.GetComponent<InventorySlot>();
            slot.SetIndex(i);
            slot.SetType(-1);
            inventory.SetInventory(i, slot);
            inventorySlot.name = i.ToString();
        }

        inventory.ResetInventory();
        inventory.ShowInventory();
    }
}

/*//Small
        for (int i = 0; i < 12; i++)
        {
            float x = 5.0f * Mathf.Cos(Mathf.Deg2Rad * 30.0f * i);
            float y = 5.0f * Mathf.Sin(Mathf.Deg2Rad * 30.0f * i);
            Vector3 pos = new Vector3(x, y, 0.0f);
            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, 30.0f * (i - 3));

            GameObject enemy = ObjManager.MakeObj("EnemyS");
            enemy.transform.position = pos;
            enemy.transform.rotation = rot;
        }

        //Medium
        for (int i = 0; i < 8; i++)
        {
            float x = 6.0f * Mathf.Cos(Mathf.Deg2Rad * 45.0f * i);
            float y = 6.0f * Mathf.Sin(Mathf.Deg2Rad * 45.0f * i);
            Vector3 pos = new Vector3(x, y, 0.0f);
            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, 45.0f * (i - 2));

            GameObject enemy = ObjManager.MakeObj("EnemyM");
            enemy.transform.position = pos;
            enemy.transform.rotation = rot;
        }

        //Large
        for (int i = 0; i < 6; i++)
        {
            float x = 7.5f * Mathf.Cos(Mathf.Deg2Rad * 60.0f * i);
            float y = 7.5f * Mathf.Sin(Mathf.Deg2Rad * 60.0f * i);
            Vector3 pos = new Vector3(x, y, 0.0f);
            Quaternion rot = Quaternion.Euler(0.0f, 0.0f, 30 * (2 * i - 3));

            GameObject enemy = ObjManager.MakeObj("EnemyL");
            enemy.transform.position = pos;
            enemy.transform.rotation = rot;
        }*/
