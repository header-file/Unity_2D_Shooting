using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.U2D.Animation;

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
    public GameObject FeverPref;

    FeverMode Fever;
    bool[,] BulletUnlockData;
    float SmallTime;
    float MediumTime;
    float LargeTime;
    int BossMax;
    bool IsFeverMode;
    int EnemyCount;
    float BossGaugeBarSize;
    
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
            
        GameManager.Inst().UiManager.MainUI.BossHPBarCanvas.SetActive(false);
        BossMax = 100;
        GameManager.Inst().UiManager.MainUI.BossGauge.gameObject.SetActive(true);
        IsFeverMode = false;

        UnlockBulletStages = new int[Constants.MAXBULLETS];
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            UnlockBulletStages[i] = 0;

        Stage = 1;

        MinFever = new float[3 * Constants.MAXSTAGES];
        MaxFever = new float[3 * Constants.MAXSTAGES];
        FullFever = new int[Constants.MAXSTAGES];

        EnemyCount = 0;

        BossGaugeBarSize = GameManager.Inst().UiManager.MainUI.BossGauge.GetComponent<RectTransform>().sizeDelta.x;

        SetUnlockData();
    }

    void Start()
    {
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
            BulletUnlockData[i, (int)Bullet.BulletType.GATLING] = bool.Parse(data[i]["Gatling"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.EXPLOSION] = bool.Parse(data[i]["Explosion"].ToString());
            BulletUnlockData[i, (int)Bullet.BulletType.DOT] = bool.Parse(data[i]["Dot"].ToString());

            if (i != 0)
                for (int j = 0; j < Constants.MAXBULLETS; j++)
                    if (!BulletUnlockData[i, j])
                        UnlockBulletStages[j] = i;
        }
    }

    public void AddBossCount(int count)
    {
        if (!IsBoss)
        {
            BossCount[Stage - 1] += count;
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
        GameManager.Inst().UiManager.Background.BgAnim.SetTrigger("toBoss");
        BossTimer = Constants.MAXBOSSTIME;
        GameManager.Inst().UiManager.MainUI.BossGauge.gameObject.SetActive(false);
        BossCount[Stage - 1] = 0;
        GameManager.Inst().UiManager.MainUI.BossGaugeBar.fillAmount = (float)BossCount[Stage - 1] / BossMax;
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
        GameManager.Inst().UiManager.MainUI.BossGaugeBar.fillAmount = percent;

        Vector2 herePos = GameManager.Inst().UiManager.MainUI.BossGauge.Here.anchoredPosition;
        herePos.x = BossGaugeBarSize * percent - (BossGaugeBarSize * 0.5f);
        GameManager.Inst().UiManager.MainUI.BossGauge.Here.anchoredPosition = herePos;

        for (int i = 0; i < FullFever[Stage - 1]; i++)
        {
            if (!IsFeverMode && percent >= MinFever[(Stage - 1) * 3 + i] && percent < MaxFever[(Stage - 1) * 3 + i])
            {
                FeverMode();
                return;
            }
            else if (IsFeverMode && percent > MaxFever[i])
            {
                if (i < FullFever[Stage - 1] - 1 && percent < MinFever[(Stage - 1) * 3 + i + 1])
                {
                    EndFeverMode();
                    return;
                }
                else if (i == FullFever[Stage - 1] - 1)
                    EndFeverMode();
            }
        }
    }

    public void SetFever(int stage, int index, float min, float max)
    {
        MinFever[stage * 3 + index] = min / 100.0f;
        MaxFever[stage * 3 + index] = max / 100.0f;
        float mid = (MinFever[stage * 3 + index] + MaxFever[stage * 3 + index]) / 2.0f;

        if(MinFever[stage * 3 + index] > 0)
            FullFever[stage] = index + 1;

        if (Stage == stage + 1)
        {
            SetFeverGauge();
            FillGauge();
        }
    }

    public void SetFeverGauge()
    {
        for (int i = 0; i < 3; i++)
        {
            float mid = (MinFever[(Stage - 1) * 3 + i] + MaxFever[(Stage - 1) * 3 + i]) / 2.0f;
            GameManager.Inst().UiManager.MainUI.GetComponent<MainUI>().BossGauge.SetFeverZones(i, mid, (MaxFever[(Stage - 1) * 3 + i] - MinFever[(Stage - 1) * 3 + i]), BossGaugeBarSize);
        }
    }

    void RandFever()
    {
        if (SceneManager.GetActiveScene().name == "Stage0")
        {
            GameManager.Inst().Tutorials.SetFeverGauge();
            return;
        }

        if (MinFever[(Stage - 1)] != 0.0f)
            return;

        for (int i = 0; i < 3; i++)
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

                GameManager.Inst().UiManager.MainUI.GetComponent<MainUI>().BossGauge.SetFeverZones(0, mid, (MaxFever[0] - MinFever[0]), BossGaugeBarSize);
                break;
            case 2:
                for(int i = 0; i < FullFever[Stage - 1]; i++)
                {
                    mid = Random.Range(0.25f + 0.5f * i, 0.3f + 0.5f * i);
                    rng = Random.Range(1, 3);
                    MinFever[(Stage - 1) * 3 + i] = mid - (rng * 0.05f);
                    MaxFever[(Stage - 1) * 3 + i] = mid + (rng * 0.05f);

                    GameManager.Inst().UiManager.MainUI.GetComponent<MainUI>().BossGauge.SetFeverZones(i, mid, (MaxFever[i] - MinFever[i]), BossGaugeBarSize);
                }
                break;
            case 3:
                for (int i = 0; i < FullFever[Stage - 1]; i++)
                {
                    mid = Random.Range(0.15f + 0.33f * i, 0.22f + 0.33f * i);
                    rng = Random.Range(1, 3);
                    MinFever[(Stage - 1) * 3 + i] = mid - (rng * 0.05f);
                    MaxFever[(Stage - 1) * 3 + i] = mid + (rng * 0.05f);

                    GameManager.Inst().UiManager.MainUI.GetComponent<MainUI>().BossGauge.SetFeverZones(i, mid, (MaxFever[i] - MinFever[i]), BossGaugeBarSize);
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

    void CheckEnemyCount()
    {
        if (EnemyCount >= 120)
            EnemyCount = 0;
    }

    void SpawnEnemies()
    {
        CancelEnemies();

        GameManager.Inst().UiManager.MainUI.BossGauge.gameObject.SetActive(true);

        if (SceneManager.GetActiveScene().name == "Stage0")
            return;

        AddBossCount(0);

        InvokeRepeating("SpawnSmall", 0.0f, SmallTime);
        InvokeRepeating("SpawnMedium", 0.0f, MediumTime);
        InvokeRepeating("SpawnLarge", 0.0f, LargeTime);
    }

    void SpawnSmall()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyS").GetComponent<Enemy>();
        SetTransform(enemy, new Vector3(0.0f, 0.0f, 0.05f * EnemyCount++));

        CheckEnemyCount();
    }

    public void SpawnSmall(Vector3 pos)
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyS").GetComponent<Enemy>();
        pos.z = 0.05f * EnemyCount++;
        enemy.transform.position = pos;
        SetTransform(enemy, pos);

        CheckEnemyCount();
    }

    public void SpawnMedium()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyM").GetComponent<Enemy>();
        SetTransform(enemy, new Vector3(0.0f, 0.0f, 0.05f * EnemyCount++));

        CheckEnemyCount();
    }

    public void SpawnMedium(Vector3 pos)
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyM").GetComponent<Enemy>();
        pos.z = 0.05f * EnemyCount++;
        enemy.transform.position = pos;
        SetTransform(enemy, pos);

        CheckEnemyCount();
    }

    public void SpawnLarge()
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyL").GetComponent<Enemy>();
        SetTransform(enemy, new Vector3(0.0f, 0.0f, 0.05f * EnemyCount++));

        CheckEnemyCount();
    }

    public void SpawnLarge(Vector3 pos)
    {
        Enemy enemy = GameManager.Inst().ObjManager.MakeObj("EnemyL").GetComponent<Enemy>();
        pos.z = 0.05f * EnemyCount++;
        enemy.transform.position = pos;
        SetTransform(enemy, pos);

        CheckEnemyCount();
    }

    void SpawnBoss()
    {
        //EraseCurEnemies();

        Boss = GameManager.Inst().ObjManager.MakeObj("EnemyB").GetComponent<EnemyB>();
        Vector3 pos = Vector3.zero;
        pos.y = 13.0f;
        pos.z = 0.05f * EnemyCount++;
        Boss.transform.position = pos;
        Boss.ResetData();
        GameManager.Inst().UiManager.MainUI.BossLevel.text = ("LV " + (BossDeathCounts[Stage - 1] + 1).ToString()).ToString();

        CheckEnemyCount();

        GameManager.Inst().UiManager.MainUI.BossHPBarCanvas.SetActive(true);
    }

    public void RestartStage()
    {
        GameManager.Inst().UiManager.Background.BgAnim.SetTrigger("toNormal");
        GameManager.Inst().UiManager.MainUI.BossHPBarCanvas.SetActive(false);
        GameManager.Inst().Player.EndBossMode();

        GameObject bomb = GameManager.Inst().ObjManager.MakeObj("Bomb");
        bomb.transform.position = Vector3.zero;
        bomb.GetComponent<Bomb>().BombStart();

        RandFever();
        SpawnEnemies();

        GameManager.Inst().SodManager.StopBGM();
        if(Stage >= 1)
            GameManager.Inst().SodManager.PlayBGM("Stage" + Stage.ToString());
    }

    public void CancelEnemies()
    {
        CancelInvoke("SpawnSmall");
        CancelInvoke("SpawnMedium");
        CancelInvoke("SpawnLarge");
    }

    void SetTransform(Enemy Enemy, Vector3 pos)
    {
        Enemy.SpeedMultiplier = 1.0f;

        switch (Stage)
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
                Flower(Enemy, pos);
                break;
            case 5:
                Meteor(Enemy, pos);
                break;
        }
    }

    void FallDown(Enemy Enemy, Vector3 fixedPos)
    {
        Vector3 pos = fixedPos;
        if (fixedPos.y == 0)
        {
            pos.x = Random.Range(-2.5f, 2.5f);
            pos.y = Random.Range(11.0f, 15.0f);
            pos.z = 0.05f * EnemyCount++;
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

    void Zigzag(Enemy Enemy, Vector3 fixedPos)
    {
        Vector3 pos = fixedPos;
        if (fixedPos.y == 0)
        {            
            pos.x = Random.Range(-2.0f, 2.0f);
            pos.y = Random.Range(9.0f, 11.0f);
            pos.z = 0.05f * EnemyCount++;
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

    void Break(Enemy Enemy, Vector3 fixedPos)
    {
        Vector3 pos = fixedPos;
        if (fixedPos.y == 0)
        {
            pos.x = Random.Range(3.5f, 5.0f);
            int r = Random.Range(0, 2);
            if (r == 1)
                pos.x *= -1;
            pos.y = Random.Range(7.0f, 9.0f);
            pos.z = 0.05f * EnemyCount++;
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

    void Flower(Enemy Enemy, Vector3 fixedPos)
    {
        Vector3 pos = fixedPos;
        if (fixedPos.y == 0)
        {
            pos.x = Random.Range(-2.0f, 2.0f);
            pos.y = Random.Range(9.0f, 11.0f);
            pos.z = 0.05f * EnemyCount++;
            Enemy.transform.position = pos;
        }

        Enemy.transform.localScale = Vector3.one;
        Enemy.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Enemy.RotGyesu = 1.0f;
        Enemy.StartMove(Time.deltaTime);
    }

    void Meteor(Enemy Enemy, Vector3 fixedPos)
    {
        Vector3 pos = fixedPos;
        if (fixedPos.y == 0)
        {
            pos.x = Random.Range(-2.0f, 2.0f);
            pos.y = Random.Range(9.0f, 10.0f);
            pos.z = 0.05f * EnemyCount++;
            Enemy.transform.position = pos;
        }

        int rand = Random.Range(1, 4);

        if (!Enemy.name.Contains("05"))
        {
            Enemy.gameObject.SetActive(false);
            return;
        }

        if (!Enemy.Skin)
            Enemy.Skin = Enemy.transform.Find("Enemy05").Find("skin").GetComponent<SpriteResolver>();
        Enemy.Skin.SetCategoryAndLabel("color", "skin" + rand.ToString());
        if (!Enemy.Body)
            Enemy.Body = Enemy.transform.Find("Enemy05").Find("bone_1").Find("bone_2").gameObject;
        Enemy.Body.transform.localScale = Vector3.one;
        Enemy.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Enemy.StartMove(Time.deltaTime);
    }

    void FeverMode()
    {
        if (IsFeverMode)
            return;

        IsFeverMode = true;
        CancelEnemies();

        if(Fever == null)
            Fever = Instantiate(FeverPref).GetComponent<FeverMode>();
        Fever.transform.position = GameManager.Inst().gameObject.transform.position;
        Fever.Anim.Play();

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

        if (Fever == null)
            Fever = Instantiate(FeverPref).GetComponent<FeverMode>();
        Fever.ToEnd();

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

        if(Stage >= 1)
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
            GameManager.Inst().UpgManager.BData[i].SetActive(BulletUnlockData[stage, i]);
    }

    public bool CheckBulletUnlocked(int type)
    {
        return BulletUnlockData[GameManager.Inst().DatManager.GameData.ReachedStage, type];
    }

    public void EraseCurEnemies()
    {
        EnemyS[] enemies = new EnemyS[100];
        enemies = FindObjectsOfType<EnemyS>();
        for (int i = 0; i < enemies.Length; i++)
            if (enemies != null)
                enemies[i].Erase();
    }
}
