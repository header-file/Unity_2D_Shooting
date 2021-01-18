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

    public int Stage = 0;
    public int BossCount;

    float SmallTime;
    float MediumTime;
    float LargeTime;
    bool IsBoss;
    int BossMax;
    
    public void StartEnemy() { Invoke("SpawnEnemies", 2.0f); }

    void Awake()
    {
        BossCount = 0;
        HPBarCanvas.SetActive(false);
        BossMax = 3;
        BossGauge.SetActive(true);
    }

    void Update()
    {
        if (BossCount >= BossMax)
        {
            IsBoss = true;
            BossGauge.SetActive(false);
            BossCount = 0;
            BossGaugeBar.fillAmount = (float)BossCount / (float)BossMax;
            CancelEnemies();
            SpawnBoss();
        }
    }

    public void AddBossCount()
    {
        if (!IsBoss)
        {
            BossCount++;

            BossGaugeBar.fillAmount = (float)BossCount / (float)BossMax;
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
        EnemyB enemy = GameManager.Inst().ObjManager.MakeObj("EnemyB").gameObject.GetComponent<EnemyB>();
        Vector3 pos = Vector3.zero;
        pos.y = 13.0f;
        enemy.transform.position = pos;

        HPBarCanvas.SetActive(true);
    }

    public void RestartStage()
    {
        HPBarCanvas.SetActive(false);

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
}
