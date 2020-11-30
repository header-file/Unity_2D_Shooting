using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public ObjectManager ObjManager;
    public UpgradeManager UpgManager;
    public ShootingManager ShtManager;
    public InputManager IptManager;
    public TextManager TxtManager;
    public UIManager UiManager;
    public CameraShake Camerashake;
    public GameObject RedMask;

    public Player Player;
    public Text CoinText;

    public int[] SubWID;

    SubWeapon[] SubWeapons;

    float SmallTime;
    float MediumTime;
    float LargeTime;


    public void SetCoinText(int Coin) { CoinText.text = Coin.ToString(); }

    public static GameManager Inst() { return Instance; }
    public SubWeapon GetSubweapons(int index) { return SubWeapons[index]; }

    public void SetSubWeapons(SubWeapon Sub, int index) { SubWeapons[index] = Sub; }


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        SubWeapons = new SubWeapon[4];
        SubWID = new int[4];

        SmallTime = 3.0f;
        MediumTime = 9.0f;
        LargeTime = 15.0f;
    }

    void Start()
    {
        //SpawnEnemies();
        InvokeRepeating("SpawnSmall", 0.0f, SmallTime);
        InvokeRepeating("SpawnMedium", 0.0f, MediumTime);
        InvokeRepeating("SpawnLarge", 0.0f, LargeTime);

        SetTexts();
    }

    void Update()
    {
        //SpawnEnemies();
    }

    //void SpawnEnemies()
    //{
    //    int r
    //}

    void SpawnSmall()
    {
        Enemy enemy = ObjManager.MakeObj("EnemyS").gameObject.GetComponent<Enemy>();
        SetTransform(enemy);
    }

    void SpawnMedium()
    {
        Enemy enemy = ObjManager.MakeObj("EnemyM").gameObject.GetComponent<Enemy>();
        SetTransform(enemy);
    }

    void SpawnLarge()
    {
        Enemy enemy = ObjManager.MakeObj("EnemyL").gameObject.GetComponent<Enemy>();
        SetTransform(enemy);
    }

    void SetTransform(Enemy Enemy)
    {
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(-2.5f, 2.5f);
        pos.y = Random.Range(11.0f, 15.0f);
        Enemy.transform.position = pos;

        Vector3 target = Vector3.zero;
        target.x = Random.Range(-2.5f, 2.5f);
        target.y = -1.0f;
        Enemy.SetTargetPosition(target);

        Vector2 pos2 = Enemy.transform.position;
        Vector2 tPos = target;
        Vector2 norm = (pos2 - tPos) / Vector2.Distance(tPos, pos2);
        float angle = Vector2.Angle(Vector2.up, norm);
        if (tPos.x < pos2.x)
            angle *= -1;
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, angle);
        Enemy.transform.rotation = rot;
    }

    void SetTexts()
    {
        for(int i = 0; i < 5; i++)
        {
            TxtManager.SetBLevels(i, UpgManager.GetBData(i).GetPowerLevel());
            TxtManager.SetBPrices(i, UpgManager.GetBData(i).GetPrice());
        }

        //TxtManager.SetSLevel(UpgManager.GetSubWeaponLevel());
        TxtManager.SetSPrice(UpgManager.GetSubWeaponPrice());
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
