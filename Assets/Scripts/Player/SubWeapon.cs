using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SubWeapon : MonoBehaviour
{
    public SpriteResolver Skin;

    public GameObject[] NormalPos;
    public GameObject[] SpreadPos;
    public GameObject LaserPos;
    public GameObject ChargePos;
    public GameObject[] GatlingPoses;
    public GameObject Arrow;

    public GameObject Shield;

    public ObjectShake Shaker;

    public ShieldPart[] ShieldParts;

    //Cheat
    public bool IsGodMode;

    public int ShootCount;
    public int CoolTime;

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
    bool IsInvincible;
    bool IsShaking;
    float GatlingGyesu;

    const int COOLTIME = 300;
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
        GameManager.Inst().UpgManager.SetHPData(BulletType);
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
        if (!IsAlive)
            return;

        IsShield = true;
        Shield.SetActive(true);
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

        for (int i = 0; i < ShieldParts.Length; i++)
            ShieldParts[i].gameObject.SetActive(false);

        GatlingGyesu = 1.0f;
    }

    void Start()
    {
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].HPBar.fillAmount = 0.415f;
    }

    void Update()
    {
        if (!IsMoving || !IsShaking)
            SetPosition();

        SetUIPos();
        EquipCount();

        if (BulletType == (int)Bullet.BulletType.GATLING)
            GatlingMove();

        if (!IsAlive || !GameManager.Inst().IptManager.GetIsAbleSWControl())
            return;

        if (IsDown)
            DownCount++;

        if (!IsEditMode)
        {
            if (DownCount > 60)
                StartEditMode();
        }

        if (IsEditMode)
            EditMode();
        else
            Fire();
    }

    void SetUIPos()
    {
        UIPos = gameObject.transform.position;
        UIPos.z = 90.0f;
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].transform.position = UIPos;
    }

    void EquipCount()
    {
        if (!IsAlive || GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex() == -1 || CheckPassive())
            return;

        GameManager.Inst().EquManager.Count(gameObject, GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex(), NumID);

        if (GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].EquipUI.activeSelf)
            SetEquipUI();
    }

    void GatlingMove()
    {
        Vector2 gatPos = GatlingPoses[0].transform.localPosition;
        gatPos.x -= Time.fixedDeltaTime * GatlingGyesu;

        GatlingPoses[0].transform.localPosition = gatPos;
        GatlingPoses[1].transform.localPosition = -gatPos;

        if (Mathf.Abs(gatPos.x) >= 0.5f)
            GatlingGyesu *= -1.0f;
    }

    bool CheckPassive()
    {
        int type = GameManager.Inst().Player.GetItem(GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex()).Type;

        if (type == (int)Item_ZzinEquipment.EquipType.REINFORCE ||
            type == (int)Item_ZzinEquipment.EquipType.VAMP)
            return true;
        else
            return false;
    }

    public void BossMode()
    {
        IsBoss = true;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        //Booster.SetActive(true);
    }

    public void EndBossMode()
    {
        IsBoss = false;
        IsMoving = true;

        //int index = NumID;
        //if (index > 2)
        //    index--;
        //transform.position = Vector3.Lerp(transform.position, GameManager.Inst().UpgManager.SubPositions[index].transform.position, Time.deltaTime * 3.0f);
        transform.position = Vector3.Lerp(transform.position, GameManager.Inst().UiManager.SubPositions[NumID].transform.position, Time.deltaTime * 3.0f);

        //if (Vector3.Distance(transform.position, GameManager.Inst().UpgManager.SubPositions[index].transform.position) > 0.001f)
        if (Vector3.Distance(transform.position, GameManager.Inst().UiManager.SubPositions[NumID].transform.position) > 0.001f)
            Invoke("EndBossMode", Time.deltaTime);
        else
        {
            IsMoving = false;
            //Booster.SetActive(false);
        }
    }

    public void Damage(int damage)
    {
        if (IsGodMode)
            return;
        else if (IsInvincible)
            return;
        else if (!IsAlive)
            return;
        else if (IsShield)
        {
            IsShield = false;
            Shield.SetActive(false);
            return;
        }
        else
        {
            for (int i = 0; i < ShieldParts.Length; i++)
            {
                if (ShieldParts[i].gameObject.activeSelf)
                {
                    ShieldParts[i].gameObject.SetActive(false);
                    return;
                }
            }
        }

        CurHP -= damage;

        //DamageText
        GameManager.Inst().TxtManager.ShowDmgText(gameObject.transform.position, damage, (int)TextManager.DamageType.BYENEMY, false);

        //Shake
        IsShaking = true;
        GameManager.Inst().ShkManager.Damage();

        GameManager.Inst().SodManager.PlayEffect("Player hit");

        IsInvincible = true;
        Invoke("ReturnInvincible", 0.1f);

        GameObject hit = GameManager.Inst().ObjManager.MakeObj("Hit");
        hit.transform.position = transform.position;

        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].HPUI.SetActive(true);
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].HPBar.fillAmount = (float)CurHP / MaxHP * 0.415f;
        Invoke("HideHPUI", 1.0f);

        if (CurHP <= 0)
            Dead();
    }

    public void Heal(int heal)
    {
        if (heal < 1)
            heal = 1;

        CurHP += heal;
        if (CurHP > MaxHP)
            CurHP = MaxHP;

        //DamageText
        GameManager.Inst().TxtManager.ShowDmgText(gameObject.transform.position, heal, (int)TextManager.DamageType.PLAYERHEAL, false);

        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].HPUI.SetActive(true);
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].HPBar.fillAmount = (float)CurHP / MaxHP * 0.415f;
        Invoke("HideHPUI", 1.0f);
    }

    void HideHPUI()
    {
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].HPUI.SetActive(false);
    }

    void SetPosition()
    {
        if(IsBoss)
        {
            float speed = 5.0f;
            if(NumID == 0 || NumID == 3)
                speed = 4.0f;
            transform.position = Vector3.Lerp(transform.position, GameManager.Inst().Player.BossSubPoses[NumID].transform.position, Time.deltaTime * speed);
        } 
        else
        {
            transform.position = GameManager.Inst().UiManager.SubPositions[NumID].transform.position;
        }
    }

    public void ShowEquipUI()
    {
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].EquipUI.SetActive(true);

        SetEquipUI();
    }

    public void SetEquipUI()
    {
        if (GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex() > -1)
        {
            Player.EqData e = GameManager.Inst().Player.GetItem(GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex());

            GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].EquipIcon.sprite = e.Icon;

            if (GameManager.Inst().EquipDatas[e.Type, e.Rarity, 0] > 0)
            {
                GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].EquipBar.fillAmount = 1.0f - (e.CoolTime / GameManager.Inst().EquipDatas[e.Type, e.Rarity, 0]);
            }
            else
            {
                if (e.Type == 3)
                    GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].EquipBar.fillAmount = 1.0f;
                else if (e.Type == 6)
                    GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].EquipBar.fillAmount = 1.0f - (ShootCount / e.Value);
            }
        }
        else
        {
            GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].EquipIcon.sprite = GameManager.Inst().Player.QuestionMark;
            GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].EquipBar.fillAmount = 0.0f;
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

        if (GameManager.Inst().UpgManager.BData[BulletType].GetIsReinforce() &&
            ShootCount >= GameManager.Inst().Player.GetItem(GameManager.Inst().UpgManager.BData[BulletType].GetEquipIndex()).Value)
        {
            ShootCount = 0;

            //장비 효과 발동 이펙트
            GameObject EquipAction = GameManager.Inst().ObjManager.MakeObj("EquipAction");
            EquipAction.transform.position = transform.position;
            EquipAction.GetComponent<ActivationTimer>().IsStart = true;

            GameManager.Inst().ShtManager.Shoot((Bullet.BulletType)BulletType, gameObject, NumID, false, true, -1);
        }
        else
            GameManager.Inst().ShtManager.Shoot((Bullet.BulletType)BulletType, gameObject, NumID, GameManager.Inst().UpgManager.BData[BulletType].GetIsVamp(), false, -1);

        Invoke("Reload", GameManager.Inst().UpgManager.BData[BulletType].GetReloadTime());
    }

    public void Reload()
    {
        IsReload = true;
    }

    public void Dead()
    {
        if(GameManager.Inst().UpgManager.BData[BulletType].GetIsRevive())
        {
            GameManager.Inst().UpgManager.BData[BulletType].SetIsRevive(false);
            Revive();
            return;
        }

        IsAlive = false;
        IsDown = false;

        if (IsShield)
            Shield.SetActive(false);

        HideHPUI();
        GetComponent<Animator>().SetInteger("Color", 0);

        CoolTime = COOLTIME + 300 * GameManager.Inst().UpgManager.BData[GetBulletType()].GetRarity();
        
        //int id = NumID;
        //if (id > 1)
        //    id--;
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].CoolTime.gameObject.SetActive(true);
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].SetCoolTime(CoolTime);

        InvokeRepeating("CheckDead", 1.0f, 1.0f);
    }

    public void CheckDead()
    {
        CoolTime -= 1;

        //int id = NumID;
        //if (id > 1)
        //    id--;
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].SetCoolTime(CoolTime);

        if (CoolTime <= 0)
        {
            Revive();

            CancelInvoke("CheckDead");
        }
    }

    public void Revive()
    {
        IsAlive = true;
        IsInvincible = true;
        CurHP = MaxHP;
        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].HPBar.fillAmount = (float)CurHP / MaxHP * 0.415f;

        GetComponent<Animator>().SetTrigger("Revive");
        Invoke("ReturnColor", 1.0f);
        Invoke("ReturnInvincible", 1.0f);

        GameManager.Inst().UiManager.MainUI.Center.Turrets[NumID].CoolTime.gameObject.SetActive(false);
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

    public void RestoreShield(int amount)
    {
        if (!IsAlive)
            return;

        for (int i = 0; i < amount; i++)
            ShieldParts[i].gameObject.SetActive(true);
    }

    public void Disable()
    {
        GameManager.Inst().SetSubWeapons(null, NumID);
        GameManager.Inst().UpgManager.SetSubWeaponLevel(GameManager.Inst().StgManager.Stage, NumID, 0);
        int id = NumID;
        if (id > 1)
            id++;
        GameManager.Inst().ShtManager.SetColorSelection(id, 0);

        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        IsDown = true;
    }

    private void OnMouseUp()
    {
        IsDown = false;
        if (IsEditMode)
        {
            EndEditMode();

            if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 11)
                GameManager.Inst().Tutorials.Step++;
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 11)
                return;

            int id = NumID;
            if (id > 1)
                id++;

            if (IsAlive)
                GameManager.Inst().UiManager.MainUI.Bottom.OnClickManageBtn(id);
            else
                GameManager.Inst().UiManager.ShowReviveAlert(NumID);
        }
    }
}
