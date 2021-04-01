using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject HPBarCanvas;
    public Image HPBar;
    public Text HPBarText;
    public GameObject BossGauge;
    public Image BossGaugeBar;
    public Animator WarningAnim;
    public GameObject Ground;
    public GameObject Turrets;

    public int Stage = 0;
    public int[] BossCount;
    public bool IsBoss;
    public float BossTimer;
    public EnemyB Boss;

    Vector3 Ground_Up;
    Vector3 Ground_Down;
    bool[,] BulletUnlockData;
    float SmallTime;
    float MediumTime;
    float LargeTime;
    int BossMax;
    bool IsFeverMode;
    
    void StartEnemy() { Invoke("SpawnEnemies", 2.0f); }

    void Awake()
    {
        BossCount = new int[Constants.MAXSTAGES];
        for(int i = 0; i < Constants.MAXSTAGES; i++)
            BossCount[i] = 0;
        HPBarCanvas.SetActive(false);
        BossMax = 50;
        BossGauge.SetActive(true);
        Ground_Up = new Vector3(0.0f, -0.8f, 0.0f);
        Ground_Down = new Vector3(0.0f, -6.0f, 0.0f);
        IsFeverMode = false;

        SetUnlockData();

        DontDestroyOnLoad(gameObject);
    }

    void SetUnlockData()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Datas/BulletUnlockData");

        BulletUnlockData = new bool[Constants.MAXSTAGES + 1, Constants.MAXBULLETS];
        for (int i = 0; i <= Constants.MAXSTAGES; i++)
        {
            BulletUnlockData[i, (int)Bullet.BulletType.NORMAL] = bool.Parse(data[i]["Normal"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.SPREAD] = bool.Parse(data[i]["Spread"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.MISSILE] = bool.Parse(data[i]["Missile"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.LASER] = bool.Parse(data[i]["Laser"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.CHARGE] = bool.Parse(data[i]["Charge"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.BOOMERANG] = bool.Parse(data[i]["Boomerang"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.CHAIN] = bool.Parse(data[i]["Chain"].ToString());
        }
    }

    public void AddBossCount()
    {
        if (!IsBoss)
        {
            BossCount[Stage]++;
            FillGauge();

            if (BossCount[Stage] >= BossMax)
            {
                IsBoss = true;
                BossTimer = Constants.MAXBOSSTIME;
                BossGauge.SetActive(false);
                BossCount[Stage] = 0;
                BossGaugeBar.fillAmount = (float)BossCount[Stage] / BossMax;
                CancelEnemies();
                WarningAnim.SetTrigger("Start");
                Invoke("SpawnBoss", 2.5f);
                GameManager.Inst().Player.BossMode();
                GroundDown();
            }
        }
    }

    public void FillGauge()
    {
        float percent = (float)BossCount[Stage] / BossMax;
        BossGaugeBar.fillAmount = percent;

        if (!IsFeverMode && percent >= 0.4f && percent < 0.6f)
            FeverMode();
        if (IsFeverMode && percent > 0.6f)
            EndFeverMode();
    }

    public void SetTimeData(List<Dictionary<string, object>> data)
    {
        SmallTime = float.Parse(data[0]["SpawnTime"].ToString());
        MediumTime = float.Parse(data[1]["SpawnTime"].ToString());
        LargeTime = float.Parse(data[2]["SpawnTime"].ToString());
    }

    void SpawnEnemies()
    {
        BossGauge.SetActive(true);

        InvokeRepeating("SpawnSmall", 0.0f, SmallTime);
        InvokeRepeating("SpawnMedium", 0.0f, MediumTime);
        InvokeRepeating("SpawnLarge", 0.0f, LargeTime);
    }

    void SpawnSmall()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyS").gameObject.GetComponent<Enemy>();
        SetTransform(enemy);
    }

    void SpawnMedium()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyM").gameObject.GetComponent<Enemy>();
        SetTransform(enemy);
    }

    void SpawnLarge()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyL").gameObject.GetComponent<Enemy>();
        SetTransform(enemy);
    }

    void SpawnBoss()
    {
        Boss = GameManager.Inst().ObjManager.MakeObj("EnemyB").gameObject.GetComponent<EnemyB>();
        Vector3 pos = Vector3.zero;
        pos.y = 13.0f;
        Boss.transform.position = pos;
        Boss.ResetData();

        HPBarCanvas.SetActive(true);
    }

    public void RestartStage()
    {
        HPBarCanvas.SetActive(false);
        GameManager.Inst().Player.EndBossMode();
        GroundUp();

        SpawnEnemies();
    }

    void CancelEnemies()
    {
        CancelInvoke("SpawnSmall");
        CancelInvoke("SpawnMedium");
        CancelInvoke("SpawnLarge");
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

    void FeverMode()
    {
        IsFeverMode = true;
        CancelEnemies();

        InvokeRepeating("SpawnSmall", 0.0f, 0.25f);
    }

    void EndFeverMode()
    {
        IsFeverMode = false;
        CancelEnemies();

        SpawnEnemies();
    }

    void GroundDown()
    {
        Ground.transform.position = Vector3.Lerp(Ground.transform.position, Ground_Down, Time.deltaTime * 5.0f);

        Turrets.transform.position = Vector3.Lerp(Turrets.transform.position, new Vector3(0.0f, 1.8f, 90.0f), Time.deltaTime * 5.0f);

        if (Vector3.Distance(Ground.transform.position, Ground_Down) > 0.001f)
            Invoke("GroundDown", Time.deltaTime);
    }

    void GroundUp()
    {
        Ground.transform.position = Vector3.Lerp(Ground.transform.position, Ground_Up, Time.deltaTime * 5.0f);

        Turrets.transform.position = Vector3.Lerp(Turrets.transform.position, new Vector3(0.0f, 4.0f, 90.0f), Time.deltaTime * 5.0f);

        if (Vector3.Distance(Ground.transform.position, Ground_Up) > 0.001f)
            Invoke("GroundUp", Time.deltaTime);
    }

    public void BeginStage()
    {
        //Bullet 처리
        UnlockBullet(Stage);

        //Stage 처리
        for (int i = 0; i < Stage; i++)
            GameManager.Inst().UiManager.UnlockStage(i);

        StartEnemy();
    }

    public void UnlockStages(int stage)
    {
        //Bullet 처리
        UnlockBullet(stage);

        //Stage 처리
        for (int i = 0; i < stage; i++)
            GameManager.Inst().UiManager.UnlockStage(i);
    }

    public void UnlockBullet(int stage)
    {
        for (int i = 0; i < Constants.MAXBULLETS; i++)
        {
            GameManager.Inst().UpgManager.BData[i].SetActive(BulletUnlockData[stage, i]);
            GameManager.Inst().UiManager.SetSlotsActive(i, BulletUnlockData[stage, i]);
        }
    }
}
