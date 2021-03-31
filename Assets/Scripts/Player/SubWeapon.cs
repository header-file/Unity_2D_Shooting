using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.EventSystems;

public class SubWeapon : MonoBehaviour
{
    public SpriteResolver Skin;

    public GameObject[] NormalPos;
    public GameObject[] SpreadPos;
    public GameObject LaserPos;
    public GameObject ChargePos;
    public GameObject Arrow;
    public GameObject Shield;
    public GameObject Booster;
    public ObjectShake Shaker;

    int BulletType;
    int DownCount;
    bool IsDown;
    bool IsEditMode;
    bool IsReload;
    bool IsAlive;
    bool IsBoss;
    bool IsMoving;
    bool IsShield;
    int NumID;
    int CoolTime;
    bool IsInvincible;
    bool IsShaking;

    const int COOLTIME = 90;
    int MaxHP;
    int CurHP;

    Vector3 UIPos;

    public int GetBulletType() { return BulletType; }
    public int GetCurHP() { return CurHP; }
    public int GetMaxHP() { return MaxHP; }
    
    public void SetNumID(int id) { NumID = id; }
    public void SetHP(int hp) { CurHP = MaxHP = hp; }
    public void SetCurHP(int hp) { CurHP = hp; }
    public void SetMaxHP(int hp) { MaxHP = hp; }

    public void SetBulletType(int T)
    {
        BulletType = T;
        SetSkin();

        SetHPs();
    }

    public void SetHPs()
    {
        int dam = MaxHP - CurHP;
        MaxHP = GameManager.Inst().UpgManager.BData[BulletType].GetHealth() + GameManager.Inst().UpgManager.BData[BulletType].GetHp();
        CurHP = MaxHP - dam;
    }

    public void ShowShield()
    {
        IsShield = true;
        Shield.SetActive(true);
    }

    public void BossMode()
    {
        IsBoss = true;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        Booster.SetActive(true);
    }

    public void EndBossMode()
    {
        IsBoss = false;
        IsMoving = true;

        //int index = NumID;
        //if (index > 2)
        //    index--;
        //transform.position = Vector3.Lerp(transform.position, GameManager.Inst().UpgManager.SubPositions[index].transform.position, Time.deltaTime * 3.0f);
        transform.position = Vector3.Lerp(transform.position, GameManager.Inst().UpgManager.SubPositions[NumID].transform.position, Time.deltaTime * 3.0f);

        //if (Vector3.Distance(transform.position, GameManager.Inst().UpgManager.SubPositions[index].transform.position) > 0.001f)
        if (Vector3.Distance(transform.position, GameManager.Inst().UpgManager.SubPositions[NumID].transform.position) > 0.001f)
            Invoke("EndBossMode", Time.deltaTime);
        else
        {
            IsMoving = false;
            Booster.SetActive(false);
        }
    }

    public void Damage(int damage)
    {
        if (IsInvincible)
            return;
        else if(IsShield)
        {
            IsShield = false;
            Shield.SetActive(false);
            return;
        }

        CurHP -= damage;

        //Shake
        IsShaking = true;
        GameManager.Inst().ShkManager.Damage();

        IsInvincible = true;
        Invoke("ReturnInvincible", 0.1f);

        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = transform.position;

        GameManager.Inst().Turrets[NumID].HPUI.SetActive(true);
        GameManager.Inst().Turrets[NumID].HPBar.fillAmount = (float)CurHP / MaxHP * 0.415f;
        Invoke("HideHPUI", 1.0f);

        if (CurHP <= 0)
            Dead();
    }

    void HideHPUI()
    {
        GameManager.Inst().Turrets[NumID].HPUI.SetActive(false);
    }
    
    void Awake()
    {
        BulletType = -1;
        DownCount = 0;
        IsDown = false;
        IsEditMode = false;
        IsReload = true;
        IsMoving = false;
        Arrow.SetActive(false);
        IsAlive = true;
        CoolTime = 0;
        IsInvincible = false;
        IsShaking = false;

        MaxHP = CurHP = 0;
        GameManager.Inst().Turrets[NumID].HPBar.fillAmount = 0.415f;
    }

    void Update()
    {
        if(!IsMoving || !IsShaking)
            SetPosition();

        UIPos = gameObject.transform.position;
        UIPos.z = 90.0f;
        //if(NumID > 1)
        //    GameManager.Inst().Turrets[NumID - 1].transform.position = UIPos;
        //else
            GameManager.Inst().Turrets[NumID].transform.position = UIPos;

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
            //int index = NumID;
            //if (index > 2)
            //    index--;
            float speed = 5.0f;
            //if (index == 0 || index == 3)
            if(NumID == 0 || NumID == 3)
                speed = 4.0f;
            //transform.position = Vector3.Lerp(transform.position, GameManager.Inst().Player.BossSubPoses[index].transform.position, Time.deltaTime * speed);
            transform.position = Vector3.Lerp(transform.position, GameManager.Inst().Player.BossSubPoses[NumID].transform.position, Time.deltaTime * speed);
        } 
        else
        {
            //int index = NumID;
            //if (index > 2)
            //    index--;
            transform.position = GameManager.Inst().UpgManager.SubPositions[NumID].transform.position;
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
        
        Invoke("Reload", GameManager.Inst().UpgManager.BData[BulletType].GetReloadTime());
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

        HideHPUI();
        GetComponent<Animator>().SetInteger("Color", 0);

        CoolTime = COOLTIME;
        
        //int id = NumID;
        //if (id > 1)
        //    id--;
        GameManager.Inst().Turrets[NumID].CoolTime.gameObject.SetActive(true);
        GameManager.Inst().Turrets[NumID].SetCoolTime(CoolTime);

        InvokeRepeating("CheckDead", 1.0f, 1.0f);
    }

    public void CheckDead()
    {
        CoolTime -= 1;

        //int id = NumID;
        //if (id > 1)
        //    id--;
        GameManager.Inst().Turrets[NumID].SetCoolTime(CoolTime);

        if (CoolTime <= 0)
        {
            IsAlive = true;
            IsInvincible = true;
            CurHP = MaxHP;
            GameManager.Inst().Turrets[NumID].HPBar.fillAmount = (float)CurHP / MaxHP * 0.415f;

            GetComponent<Animator>().SetTrigger("Revive");
            Invoke("ReturnColor", 1.0f);
            Invoke("ReturnInvincible", 1.0f);

            CancelInvoke("CheckDead");

            GameManager.Inst().Turrets[NumID].CoolTime.gameObject.SetActive(false);
        }
    }

    void ReturnColor()
    {
        int id = NumID;
        if (id > 1)
            id++;

        GetComponent<Animator>().SetInteger("Color", GameManager.Inst().ShtManager.GetColorSelection(id) + 1);
    }

    void ReturnInvincible()
    {
        IsInvincible = false;
        IsShaking = false;
    }

    public void SetSkin()
    {
        Skin.SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[BulletType]);
    }

    public void SetSkinColor(int index)
    {
        GetComponent<Animator>().SetInteger("Color", ++index);
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
        {
            int id = NumID;
            if (id > 1)
                id++;

            GameManager.Inst().UiManager.OnClickManageBtn(id);
        }
    }
}
