using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int Stage = 0;
    public int ReachedStage = 1;
    public int[] BossCount;
    public bool IsBoss;
    public float BossTimer;
    public EnemyB Boss;
    public int[] UnlockBulletStages;
    public int[] BossDeathCounts;
    public float[] MinFever;
    public float[] MaxFever;
    public int[] FullFever;

    bool[,] BulletUnlockData;
    float SmallTime;
    float MediumTime;
    float LargeTime;
    int BossMax;
    bool IsFeverMode;

    
    void StartEnemy() { Invoke("SpawnEnemies", 2.0f); }

    void Awake()
    {
        StageManager[] objs = FindObjectsOfType<StageManager>();
        if (objs.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        BossCount = new int[Constants.MAXSTAGES];
        BossDeathCounts = new int[Constants.MAXSTAGES];
        for (int i = 0; i < Constants.MAXSTAGES; i++)
        {
            BossCount[i] = 0;
            BossDeathCounts[i] = 0;
        }
            
        GameManager.Inst().UiManager.BossHPBarCanvas.SetActive(false);
        BossMax = 100;
        GameManager.Inst().UiManager.BossGauge.SetActive(true);
        IsFeverMode = false;

        UnlockBulletStages = new int[Constants.MAXBULLETS];
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            UnlockBulletStages[i] = 0;

        Stage = 1;

        MinFever = new float[3 * Constants.MAXSTAGES];
        MaxFever = new float[3 * Constants.MAXSTAGES];
        FullFever = new int[Constants.MAXSTAGES];

        SetUnlockData();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        FillGauge();
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
            BulletUnlockData[i, (int)Bullet.BulletType.GATLING] = bool.Parse(data[i]["Gatling"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.EXPLOSION] = bool.Parse(data[i]["Explosion"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.DOT] = bool.Parse(data[i]["Dot"].ToString());

            if (i != 0)
                for (int j = 0; j < Constants.MAXBULLETS; j++)
                    if (!BulletUnlockData[i, j])
                        UnlockBulletStages[j] = i;
        }
    }

    public void AddBossCount()
    {
        if (!IsBoss)
        {
            BossCount[Stage - 1]++;
            FillGauge();

            if (BossCount[Stage - 1] >= BossMax)
                StartBossMode();
        }
    }

    public void SetBossCount(int stage, int count)
    {
        BossCount[stage - 1] = count;
        FillGauge();

        if (BossCount[Stage - 1] >= BossMax)
            StartBossMode();
    }

    void StartBossMode()
    {
        IsBoss = true;
        GameManager.Inst().UiManager.BgAnim.SetTrigger("toBoss");
        BossTimer = Constants.MAXBOSSTIME;
        GameManager.Inst().UiManager.BossGauge.SetActive(false);
        BossCount[Stage - 1] = 0;
        GameManager.Inst().UiManager.BossGaugeBar.fillAmount = (float)BossCount[Stage - 1] / BossMax;
        CancelEnemies();
        Invoke("SpawnBoss", 2.5f);
        GameManager.Inst().Player.BossMode();

        GameObject bomb = GameManager.Inst().ObjManager.MakeObj("Bomb");
        bomb.transform.position = Vector3.zero;
        bomb.GetComponent<Bomb>().BombStart();

        GameManager.Inst().SodManager.StopBGM();
        GameManager.Inst().SodManager.PlayBGM("Stage" + Stage.ToString() + " Boss");
    }

    public void FillGauge()
    {
        float percent = (float)BossCount[Stage - 1] / BossMax;
        GameManager.Inst().UiManager.BossGaugeBar.fillAmount = percent;

        for (int i = 0; i < 3; i++)
        {
            if (!IsFeverMode && percent >= MinFever[i] && percent < MaxFever[i])
            {
                Debug.Log("Min " + i.ToString() + " : " + MinFever[i].ToString());
                FeverMode();
                return;
            }
            else if (IsFeverMode && percent > MaxFever[i])
            {
                if (i < 2 && percent > MinFever[i + 1])
                    continue;

                Debug.Log("Max " + i.ToString() + " : " + MaxFever[i].ToString());
                EndFeverMode();
                return;
            }
        }

        Debug.Log("CurrentGauge : " + percent.ToString());
    }

    public void SetFever(int stage, int index, float min, float max)
    {
        MinFever[stage * 3 + index] = min / 100.0f;
        MaxFever[stage * 3 + index] = max / 100.0f;
        float mid = (MinFever[stage * 3 + index] + MaxFever[stage * 3 + index]) / 2.0f;

        GameManager.Inst().UiManager.MainUI.GetComponent<MainUI>().BossGauge.SetFeverZones(index, mid, (MaxFever[stage * 3 + index] - MinFever[stage * 3 + index]));

        if(MinFever[stage * 3 + index] > 0)
            FullFever[stage] = index + 1;
    }

    void RandFever()
    {
        for(int i = 0; i < 3; i++)
        {
            MinFever[(Stage - 1) * 3 + i] = 0.0f;
            MaxFever[(Stage - 1) * 3 + i] = 0.0f;
        }

        FullFever[Stage - 1] = Random.Range(1, 4);
        float mid = 0.0f;
        switch(FullFever[Stage - 1])
        {
            case 1:
                mid = Random.Range(0.25f, 0.75f);
                int rng = Random.Range(1, 4);
                MinFever[(Stage - 1) * 3] = mid - (rng * 0.05f);
                MaxFever[(Stage - 1) * 3] = mid + (rng * 0.05f);

                GameManager.Inst().UiManager.MainUI.GetComponent<MainUI>().BossGauge.SetFeverZones(0, mid, (MaxFever[0] - MinFever[0]));
                break;
            case 2:
                for(int i = 0; i < FullFever[Stage - 1]; i++)
                {
                    mid = Random.Range(0.25f + 0.5f * i, 0.3f + 0.5f * i);
                    rng = Random.Range(1, 3);
                    MinFever[(Stage - 1) * 3 + i] = mid - (rng * 0.05f);
                    MaxFever[(Stage - 1) * 3 + i] = mid + (rng * 0.05f);

                    GameManager.Inst().UiManager.MainUI.GetComponent<MainUI>().BossGauge.SetFeverZones(i, mid, (MaxFever[i] - MinFever[i]));
                }
                break;
            case 3:
                for (int i = 0; i < FullFever[Stage - 1]; i++)
                {
                    mid = Random.Range(0.15f + 0.33f * i, 0.22f + 0.33f * i);
                    rng = Random.Range(1, 3);
                    MinFever[(Stage - 1) * 3 + i] = mid - (rng * 0.05f);
                    MaxFever[(Stage - 1) * 3 + i] = mid + (rng * 0.05f);

                    GameManager.Inst().UiManager.MainUI.GetComponent<MainUI>().BossGauge.SetFeverZones(i, mid, (MaxFever[i] - MinFever[i]));
                }
                break;
        }
    }

    public void SetTimeData(List<Dictionary<string, object>> data)
    {
        SmallTime = float.Parse(data[0]["SpawnTime"].ToString());
        MediumTime = float.Parse(data[1]["SpawnTime"].ToString());
        LargeTime = float.Parse(data[2]["SpawnTime"].ToString());
    }

    void SpawnEnemies()
    {
        CancelEnemies();

        GameManager.Inst().UiManager.BossGauge.SetActive(true);

        InvokeRepeating("SpawnSmall", 0.0f, SmallTime);
        InvokeRepeating("SpawnMedium", 0.0f, MediumTime);
        InvokeRepeating("SpawnLarge", 0.0f, LargeTime);
    }

    void SpawnSmall()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyS").gameObject.GetComponent<Enemy>();
        SetTransform(enemy, new Vector2(0.0f, 0.0f));
    }

    public void SpawnSmall(Vector2 pos)
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyS").gameObject.GetComponent<Enemy>();
        enemy.transform.position = pos;
        SetTransform(enemy, pos);
    }

    public void SpawnMedium()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyM").gameObject.GetComponent<Enemy>();
        SetTransform(enemy, new Vector2(0.0f, 0.0f));
    }

    public void SpawnMedium(Vector2 pos)
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyM").gameObject.GetComponent<Enemy>();
        enemy.transform.position = pos;
        SetTransform(enemy, pos);
    }

    public void SpawnLarge()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyL").gameObject.GetComponent<Enemy>();
        SetTransform(enemy, new Vector2(0.0f, 0.0f));
    }

    public void SpawnLarge(Vector2 pos)
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyL").gameObject.GetComponent<Enemy>();
        enemy.transform.position = pos;
        SetTransform(enemy, pos);
    }

    void SpawnBoss()
    {
        //EraseCurEnemies();

        Boss = GameManager.Inst().ObjManager.MakeObj("EnemyB").gameObject.GetComponent<EnemyB>();
        Vector3 pos = Vector3.zero;
        pos.y = 13.0f;
        Boss.transform.position = pos;
        Boss.ResetData();

        GameManager.Inst().UiManager.BossHPBarCanvas.SetActive(true);
    }

    public void RestartStage()
    {
        GameManager.Inst().UiManager.BgAnim.SetTrigger("toNormal");
        GameManager.Inst().UiManager.BossHPBarCanvas.SetActive(false);
        GameManager.Inst().Player.EndBossMode();

        GameObject bomb = GameManager.Inst().ObjManager.MakeObj("Bomb");
        bomb.transform.position = Vector3.zero;
        bomb.GetComponent<Bomb>().BombStart();

        RandFever();
        SpawnEnemies();

        GameManager.Inst().SodManager.StopBGM();
        GameManager.Inst().SodManager.PlayBGM("Stage" + Stage.ToString());
    }

    public void CancelEnemies()
    {
        CancelInvoke("SpawnSmall");
        CancelInvoke("SpawnMedium");
        CancelInvoke("SpawnLarge");
    }

    void SetTransform(Enemy Enemy, Vector2 pos)
    {
        switch(Stage)
        {
            case 1:
                FallDown(Enemy, pos);
                break;
            case 2:
                Zigzag(Enemy, pos);
                break;
            case 3:
                Break(Enemy, pos);
                break;
            case 4:
                break;
        }
    }

    void FallDown(Enemy Enemy, Vector2 fixedPos)
    {
        Vector3 pos = fixedPos;
        if (fixedPos == Vector2.zero)
        {
            pos.x = Random.Range(-2.5f, 2.5f);
            pos.y = Random.Range(11.0f, 15.0f);
            Enemy.transform.position = pos;
        }

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
        Enemy.StartMove(Time.deltaTime);
    }

    void Zigzag(Enemy Enemy, Vector2 fixedPos)
    {
        Vector3 pos = fixedPos;
        if (fixedPos == Vector2.zero)
        {            
            pos.x = Random.Range(-2.0f, 2.0f);
            pos.y = Random.Range(9.0f, 11.0f);
            Enemy.transform.position = pos;
        }

        float angleZ = Random.Range(45, 76);
        int r = Random.Range(0, 2);
        if (r == 1)
            angleZ *= -1.0f;
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, angleZ);
        Enemy.transform.rotation = rot;
        Enemy.StartMove(Time.deltaTime);
    }

    void Break(Enemy Enemy, Vector2 fixedPos)
    {
        Vector3 pos = fixedPos;
        if (fixedPos == Vector2.zero)
        {
            pos.x = Random.Range(3.5f, 5.0f);
            int r = Random.Range(0, 2);
            if (r == 1)
                pos.x *= -1;
            pos.y = Random.Range(7.0f, 9.0f);
            Enemy.transform.position = pos;
        }

        Vector3 target = Vector3.zero;
        target.x = Random.Range(-2.5f, 2.5f);
        target.y = pos.y;
        Enemy.SetTargetPosition(target);

        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Enemy.transform.rotation = rot;
        Enemy.StartMove(Time.deltaTime);
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

        EraseCurEnemies();
        CancelEnemies();

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 66)
        {
            EraseCurEnemies();
            return;
        }            

        SpawnEnemies();
    }

    public void BeginStage()
    {
        //Bullet 처리
        UnlockBullet(Stage);

        //Stage 처리
        for (int i = 0; i < Stage; i++)
            GameManager.Inst().UiManager.UnlockStage(i);

        if(MinFever[3 * (Stage - 1)] == 0)
           RandFever();
        StartEnemy();

        GameManager.Inst().SodManager.PlayBGM("Stage" + Stage.ToString());
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
            GameManager.Inst().UiManager.SetSlotsActive(i, BulletUnlockData[stage, i], UnlockBulletStages[i]);
        }
    }

    public bool CheckBulletUnlocked(int type)
    {
        return BulletUnlockData[GameManager.Inst().DatManager.GameData.ReachedStage, type];
    }

    void EraseCurEnemies()
    {
        EnemyS[] enemies = new EnemyS[100];
        enemies = FindObjectsOfType<EnemyS>();
        for (int i = 0; i < enemies.Length; i++)
            if (enemies != null)
                enemies[i].Erase();
    }
}
