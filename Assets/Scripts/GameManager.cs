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
    public CameraShake Camerashake;

    public Player Player;
    public Text CoinText;

    public int[] SubWID;

    SubWeapon[] SubWeapons;

    public void SetCoinText(int Coin) { CoinText.text = Coin.ToString(); }

    /*public void SlowGame()
    {
        Time.timeScale = 0.1f;
        Invoke("ReturnGameSpeed", 0.015f);
    }

    public void ReturnGameSpeed() { Time.timeScale = 1.0f; }*/

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
    }

    void Start()
    {
        SpawnEnemies();

        SetTexts();
    }

    void SpawnEnemies()
    {
        //Small
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
        }
    }

    void SetTexts()
    {
        for(int i = 0; i < 5; i++)
        {
            TxtManager.SetBLevels(i, UpgManager.GetBData(i).GetPowerLevel());
            TxtManager.SetBPrices(i, UpgManager.GetBData(i).GetPrice());
        }

        TxtManager.SetSLevel(UpgManager.GetSubWeaponLevel());
        TxtManager.SetSPrice(UpgManager.GetSubWeaponPrice());
    }
}
