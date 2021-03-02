using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public ShootingManager ShtManager;
    public ObjectManager ObjManager;
    public UpgradeManager UpgManager;
    public InputManager IptManager;
    public TextManager TxtManager;
    public UIManager UiManager;
    public StageManager StgManager;
    public QuestManager QstManager;

    public CameraShake Camerashake;
    public GameObject RedMask;
    public GameObject Inventory;

    public Player Player;
    public Text CoinText;

    public int[] SubWID;
    public int[] Resources;

    SubWeapon[] SubWeapons;
    
    int UIDCount;

    List<Dictionary<string, object>> DropRateData;


    public static GameManager Inst() { return Instance; }
    public SubWeapon GetSubweapons(int index) { return SubWeapons[index]; }
    public int GetDropRate(int stage, string grade) { return int.Parse(DropRateData[stage][grade].ToString()); }

    public void SetSubWeapons(SubWeapon Sub, int index) { SubWeapons[index] = Sub; }
    public void SetCoinText(int Coin) { CoinText.text = Coin.ToString(); }
    public void AddResource(int stage, int value) { Resources[stage - 1] += value; TxtManager.Resources[stage - 1].text = Resources[stage - 1].ToString(); }
    public void SubtractResource(int index, int value) { Resources[index] -= value; TxtManager.Resources[index].text = Resources[index].ToString(); }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Resources = new int[StageManager.MAXSTAGES];

        SubWeapons = new SubWeapon[4];
        SubWID = new int[4];

        UIDCount = 0;
        StgManager.Stage = 1;
    }

    void Start()
    {
        SetTexts();
        SetInventory();
        SetData();
        SetResources();
        
        StgManager.BeginStage();
    }

    void Update()
    {
        
    }

    void SetTexts()
    {
        for (int i = 0; i < Bullet.MAXBULLETS; i++)
        {
            TxtManager.SetBLevels(i, UpgManager.BData[i].GetPowerLevel());
            TxtManager.SetBPrices(i, UpgManager.BData[i].GetPrice());
        }

        TxtManager.SetSPrice(UpgManager.GetSubWeaponPrice(0));
    }

    void SetData()
    {
        DropRateData = CSVReader.Read("Datas/DropRate");


    }

    void SetResources()
    {
        for (int i = 0; i < StageManager.MAXSTAGES; i++)
        {
            Resources[i] = 10000;
            TxtManager.Resources[i].text = Resources[i].ToString();
        }
    }

    public void MakeEquipment(int type, int grade, Transform transform)
    {
        int rand = type;
        if (rand == -1)
            rand = (int)(Random.value * 3.0f);

        switch (rand)
        {
            case 0:
                GameObject eqAtk = ObjManager.MakeObj("EqAttack");
                eqAtk.transform.position = transform.position;
                Item_Equipment eqpAtk = eqAtk.GetComponent<Item_Equipment>();
                eqpAtk.StartAbsorb(0.5f);

                eqpAtk.SetValues(grade, UIDCount++);
                break;
            case 1:
                GameObject eqRng = ObjManager.MakeObj("EqRange");
                eqRng.transform.position = transform.position;
                Item_Equipment eqpRng = eqRng.GetComponent<Item_Equipment>();
                eqpRng.StartAbsorb(0.5f);

                eqpRng.SetValues(grade, UIDCount++);
                break;
            case 2:
                GameObject eqSpd = ObjManager.MakeObj("EqSpeed");
                eqSpd.transform.position = transform.position;
                Item_Equipment eqpSpd = eqSpd.GetComponent<Item_Equipment>();
                eqpSpd.StartAbsorb(0.5f);

                eqpSpd.SetValues(grade, UIDCount++);
                break;
        }
    }

    public Item_Equipment MakeEuipData(int num, int grade)
    {
        int rand = num;
        if (rand == -1)
            rand = (int)(Random.value * 3.0f);

        GameObject eq;
        Item_Equipment ieq = null;

        switch (rand)
        {
            case 0:
                eq = ObjManager.MakeObj("EqAttack");
                ieq = eq.GetComponent<Item_Equipment>();
                ieq.StartAbsorb(0.0f);
                ieq.SetValues(grade, UIDCount++);
                ieq.gameObject.SetActive(false);
                break;

            case 1:
                eq = ObjManager.MakeObj("EqRange");
                ieq = eq.GetComponent<Item_Equipment>();
                ieq.StartAbsorb(0.0f);
                ieq.SetValues(grade, UIDCount++);
                ieq.gameObject.SetActive(false);
                break;

            case 2:
                eq = ObjManager.MakeObj("EqSpeed");
                ieq = eq.GetComponent<Item_Equipment>();
                ieq.StartAbsorb(0.0f);
                ieq.SetValues(grade, UIDCount++);
                ieq.gameObject.SetActive(false);
                break;
        }

        return ieq;
    }

    void SetInventory()
    {
        InventoryScroll inventory = Inventory.GetComponent<InventoryScroll>();
        for (int i = 0; i < Player.MAXINVENTORY; i++)
        {
            GameObject inventorySlot = GameManager.Inst().ObjManager.MakeObj("InventorySlot");
            inventorySlot.transform.SetParent(inventory.Contents.transform, false);
            InventorySlot slot = inventorySlot.GetComponent<InventorySlot>();
            slot.SetIndex(i);
            slot.SetType(-1);
            inventory.SetInventory(i, slot);
            inventorySlot.name = i.ToString();
        }
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
