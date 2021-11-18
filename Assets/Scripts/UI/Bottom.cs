using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bottom : MonoBehaviour
{
    public Info Weapon;
    public BuySubWeapon BuySW;
    public GameObject Panel;

    public Slot[] Slots;
    public LoopScroll WeaponScroll;
    public Image[] Colors;
    public GameObject Arrows;
    public Text SWPrice;
    public Text SWName;

    int CurrentPlayerNum;


    public void ShowEquipBtn(int curBulletType)
    {
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            Slots[i].SelectBtn.gameObject.SetActive(false);

        Slots[curBulletType].SelectBtn.gameObject.SetActive(true);
    }

    public void SetBulletSelected()
    {
        for (int i = 0; i < Constants.MAXBULLETS; i++)
            Slots[i].Selected.SetActive(false);

        Slots[GameManager.Inst().UiManager.CurrentBulletType].Selected.SetActive(true);
    }

    //Button Interaction
    public void OnClickManageBtn(int Type)
    {
        //Time.timeScale = 0.0f;
        GameManager.Inst().IptManager.SetIsAbleControl(false);

        CurrentPlayerNum = Type;
        WeaponScroll.SetCurrentCharacter(CurrentPlayerNum);

        if (!GameManager.Inst().UiManager.GetIsMoveUp())
            GameManager.Inst().UiManager.SetTimer(0.0f);
        GameManager.Inst().UiManager.SetIsMoveUp(true);

        Weapon.gameObject.SetActive(true);
        BuySW.gameObject.SetActive(false);
        Weapon.SetColorSelected(GameManager.Inst().ShtManager.GetColorSelection(Type));

        for (int i = 0; i < 5; i++)
            Arrows.transform.GetChild(i).gameObject.SetActive(false);

        Arrows.transform.GetChild(Type).gameObject.SetActive(true);

        switch (CurrentPlayerNum)
        {
            case 2:
                GameManager.Inst().UiManager.CurrentBulletType = GameManager.Inst().Player.GetBulletType();
                break;

            case 0:
            case 1:
            case 3:
            case 4:
                if (CurrentPlayerNum > 1)
                    GameManager.Inst().UiManager.CurrentBulletType = GameManager.Inst().GetSubweapons(
                        CurrentPlayerNum - 1).GetBulletType();
                else
                    GameManager.Inst().UiManager.CurrentBulletType = GameManager.Inst().GetSubweapons(
                        CurrentPlayerNum).GetBulletType();
                break;
        }

        for (int i = 0; i < Constants.MAXBULLETS; i++)
            Slots[i].Show(i);

        ShowEquipBtn(GameManager.Inst().UiManager.CurrentBulletType);
        SetBulletSelected();

        WeaponScroll.MoveToSelected(GameManager.Inst().UiManager.CurrentBulletType);

        GameManager.Inst().Player.ShowEquipUI();
        for (int i = 0; i < Constants.MAXSUBWEAPON; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().GetSubweapons(i).ShowEquipUI();
        }

        if (SceneManager.GetActiveScene().name == "Stage0" && (GameManager.Inst().Tutorials.Step == 5 || GameManager.Inst().Tutorials.Step == 41))
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickManageCancel()
    {
        if (!GameManager.Inst().UiManager.GetIsMoveDown())
            GameManager.Inst().UiManager.SetTimer(0.0f);

        GameManager.Inst().UiManager.SetIsMoveDown(true);

        for (int i = 0; i < 5; i++)
            GameManager.Inst().UiManager.MainUI.Bottom.Arrows.transform.GetChild(i).gameObject.SetActive(false);

        GameManager.Inst().Player.EquipUI.SetActive(false);
        for (int i = 0; i < Constants.MAXSUBWEAPON; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) != null)
                GameManager.Inst().UiManager.MainUI.Center.Turrets[i].EquipUI.SetActive(false);
        }
    }

    public void OnClickSelectBullet(int BulletType)
    {
        switch (CurrentPlayerNum)
        {
            case 2:
                GameManager.Inst().Player.SetBulletType(BulletType);
                break;

            case 0:
            case 1:
            case 3:
            case 4:
                if (CurrentPlayerNum > 1)
                    GameManager.Inst().GetSubweapons(CurrentPlayerNum - 1).SetBulletType(BulletType);
                else
                    GameManager.Inst().GetSubweapons(CurrentPlayerNum).SetBulletType(BulletType);
                break;
        }

        GameManager.Inst().UiManager.CurrentBulletType = BulletType;
    }


    public void OnClickSubWeapon(int index)
    {
        BuySW.ShowBuy(index);
        if (!GameManager.Inst().UiManager.GetIsMoveUp())
            GameManager.Inst().UiManager.SetTimer(0.0f);
        GameManager.Inst().UiManager.SetIsMoveUp(true);

        Weapon.gameObject.SetActive(false);
        BuySW.gameObject.SetActive(true);

        for (int i = 0; i < 5; i++)
            Arrows.transform.GetChild(i).gameObject.SetActive(false);

        if (index > 1)
            index++;
        Arrows.transform.GetChild(index).gameObject.SetActive(true);

        CurrentPlayerNum = index;

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 8)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickBuySWBtn()
    {
        BuySW.Buy();

        OnClickManageBtn(CurrentPlayerNum);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 9)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickSubWeaponCancel()
    {
        if (!GameManager.Inst().UiManager.GetIsMoveDown())
            GameManager.Inst().UiManager.SetTimer(0.0f);
        GameManager.Inst().UiManager.SetIsMoveDown(true);

        for (int i = 0; i < 5; i++)
            Arrows.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void OnClickColorBtn(int index)
    {
        GameManager.Inst().ShtManager.SetColorSelection(CurrentPlayerNum, index);
        Weapon.SetColorSelected(index);

        if (CurrentPlayerNum == 2)
            GameManager.Inst().Player.SetSkinColor(index);
        else
        {
            int idx = CurrentPlayerNum;
            if (idx > 1)
                idx--;
            GameManager.Inst().GetSubweapons(idx).SetSkinColor(index);
        }

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 7)
            GameManager.Inst().Tutorials.Step++;
    }
}
