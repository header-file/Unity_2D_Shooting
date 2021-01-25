using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SubWeapon : MonoBehaviour
{
    public GameObject[] NormalPos;
    public GameObject[] SpreadPos;
    public GameObject LaserPos;
    public GameObject ChargePos;
    public GameObject Arrow;
    public GameObject HPBarCanvas;
    public Image HPBar;

    public Sprite[] Sprites;

    SpriteRenderer SpriteRenderer;

    int BulletType;
    int DownCount;
    bool IsDown;
    bool IsEditMode;
    bool IsReload;
    bool IsAlive;
    bool IsBoss;
    bool IsMoving;
    int NumID;
    int CoolTime;

    const int COOLTIME = 5;
    int MaxHP;
    int CurHP;

    public int GetBulletType() { return BulletType; }
    public int GetCurHP() { return CurHP; }
    
    public void SetBulletType(int T) { BulletType = T; }
    public void SetNumID(int id) { NumID = id; }
    public void SetHP(int hp) { CurHP = MaxHP = hp; }

    public void BossMode()
    {
        IsBoss = true;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    public void EndBossMode()
    {
        IsBoss = false;
        IsMoving = true;

        int index = NumID;
        if (index > 2)
            index--;
        transform.position = Vector3.Lerp(transform.position, GameManager.Inst().UpgManager.SubPositions[index].transform.position, Time.deltaTime * 3.0f);

        if (Vector3.Distance(transform.position, GameManager.Inst().UpgManager.SubPositions[index].transform.position) > 0.001f)
            Invoke("EndBossMode", Time.deltaTime);
        else
            IsMoving = false;
    }

    public void Damage(int damage)
    {
        CurHP -= damage;

        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = transform.position;

        HPBarCanvas.SetActive(true);
        HPBar.fillAmount = (float)CurHP / (float)MaxHP * 0.415f;

        if (CurHP <= 0)
            Dead();
    }
    
    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        BulletType = 0;
        DownCount = 0;
        IsDown = false;
        IsEditMode = false;
        IsReload = true;
        IsMoving = false;
        Arrow.SetActive(false);
        IsAlive = true;
        CoolTime = 0;

        MaxHP = CurHP = 0;
        HPBar.fillAmount = 0.415f;
    }

    void Update()
    {
        if(!IsMoving)
            SetPosition();
        
         if (!IsAlive || !GameManager.Inst().IptManager.GetIsAbleSWControl())
            return;

        if (IsDown)
            DownCount++;

        if (!IsEditMode)
        {
            if( DownCount > 60)
                StartEditMode();                
        }

        if (IsEditMode)
            EditMode();
        else
            Fire();
    }

    void SetPosition()
    {
        if(IsBoss)
        {
            int index = NumID;
            if (index > 2)
                index--;
            transform.position = GameManager.Inst().Player.BossSubPoses[index].transform.position;
        } 
        else
        {
            int index = NumID;
            if (index > 2)
                index--;
            transform.position = GameManager.Inst().UpgManager.SubPositions[index].transform.position;
        }
        
    }

    void StartEditMode()
    {
        Time.timeScale = 0.1f;
        gameObject.transform.localScale = Vector3.one * 1.5f;
        IsEditMode = true;
        IsDown = false;
        Arrow.SetActive(true);
        GameManager.Inst().IptManager.SetIsAbleControl(false);

    }

    void EndEditMode()
    {
        Time.timeScale = 1.0f;
        gameObject.transform.localScale = Vector3.one * 1.0f;
        IsEditMode = false;
        DownCount = 0;
        Arrow.SetActive(false);
        GameManager.Inst().IptManager.SetIsAbleControl(true);
    }

    void EditMode()
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MPos = new Vector2(MousePos.x, MousePos.y);
        
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 norm = (MPos - pos) / Vector2.Distance(MPos, pos);
        float angle = Vector2.Angle(Vector2.up, norm);
        if (MousePos.x > transform.position.x)
            angle *= -1;
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, angle);
        transform.rotation = rot;
    }

    public void Fire()
    {
        if (!IsReload || BulletType < 0)
            return;

        IsReload = false;
        
        GameManager.Inst().ShtManager.Shoot((Bullet.BulletType)BulletType, gameObject, NumID);
        
        Invoke("Reload", GameManager.Inst().UpgManager.GetBData(BulletType).GetReloadTime());
    }

    public void Reload()
    {
        IsReload = true;
    }

    public void Dead()
    {
        if (!IsAlive)
            return;

        IsAlive = false;
        IsDown = false;
        SpriteRenderer.sprite = Sprites[1];

        CoolTime = COOLTIME;
        
        int id = NumID;
        if (id > 1)
            id--;
        GameManager.Inst().TxtManager.CoolTimes[id].SetActive(true);
        GameManager.Inst().TxtManager.SetCoolTimes(id, CoolTime);

        InvokeRepeating("CheckDead", 1.0f, 1.0f);
    }

    public void CheckDead()
    {
        CoolTime -= 1;

        int id = NumID;
        if (id > 1)
            id--;
        GameManager.Inst().TxtManager.SetCoolTimes(id, CoolTime);

        if (CoolTime <= 0)
        {
            IsAlive = true;
            CurHP = MaxHP;
            HPBar.fillAmount = (float)CurHP / (float)MaxHP * 0.415f;
            SpriteRenderer.sprite = Sprites[0];
            CoolTime = COOLTIME;
            CancelInvoke("CheckDead");

            GameManager.Inst().TxtManager.CoolTimes[id].SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        IsDown = true;
    }

    private void OnMouseUp()
    {
        IsDown = false;
        if (IsEditMode)
            EndEditMode();
        else
            GameManager.Inst().UiManager.OnClickManageBtn(NumID);
    }
}
