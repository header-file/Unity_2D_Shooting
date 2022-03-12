using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cheat : MonoBehaviour
{
    public Toggle[] Toggles;
    public GameObject[] Pages;
    public NumInput NumInput;

    public Button[] CharReviveBtns;
    public Button[] CharGodModeBtns;
    public Button[] CharCreateBtns;
    public Button[] CharDeleteBtns;
    public Text[] GodModeBtnTexts;

    public Button PlusLevelBtn;
    public Button MinusLevelBtn;
    public Text BulletName;
    public Text BulletLevelText;
    public Text RarityText;
    public Text[] StatusText;
    public Button[] PlusReinforceBtn;
    public Button[] MinusReinforceBtn;
    public Text[] ReinforceText;
    public Image[] ReinforceImage;

    public Text CoinText;
    public Text JewelText;
    public Text[] ResourceTexts;

    public Text ReinforceValueText;
    public Text EquipValueText;
    public Button[] EquipValueBtns;
    public Text EquipDetailText;
    public string[] ReinforceTypes;
    public Text MaxInventoryText;
    public Button[] InventoryBtns;

    public Button[] StageBtns;
    public QuestSlot[] QuestSlots;
    public Button[] QuestBtns;

    int CurrentBulletType;
    int ReinforceType;
    int ReinforceRarity;
    int EquipType;
    int EquipRarity;
    bool[] IsQstUps;
    bool[] IsQstDowns;

    void Start()
    {
        CurrentBulletType = 0;

        IsQstUps = new bool[4];
        IsQstDowns = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            IsQstUps[i] = false;
            IsQstDowns[i] = false;
        }

        gameObject.SetActive(false);
    }

    void Update()
    {
        for(int i = 0; i < 4; i++)
        {
            if (IsQstUps[i])
                QuestUp(i);
            else if (IsQstDowns[i])
                QuestDown(i);
        }
    }

    public void SwitchPage(int index)
    {
        if (Toggles[index].isOn == false)
            return;

        for (int i = 0; i < Pages.Length; i++)
            Pages[i].SetActive(false);

        Toggles[index].Select();
        Pages[index].SetActive(true);
        ShowPage(index);
    }

    void ShowPage(int index)
    {
        switch(index)
        {
            case 0:
                ShowCharacterPage();
                break;
            case 1:
                ShowWeaponPage();
                break;
            case 2:
                ShowResourcePage();
                break;
            case 3:
                ShowStagePage();
                break;
            case 4:
                ShowItemPage();
                break;
            case 5:
                ShowDataPage();
                break;
        }
    }

    void ShowCharacterPage()
    {
        for(int i = 0; i < Constants.MAXSUBWEAPON; i++)
        {
            if (GameManager.Inst().GetSubweapons(i) == null)
                CharacterOffButtons(i);
            else
                CharacterOnButtons(i);
        }
    }

    void CharacterOnButtons(int i)
    {
        CharCreateBtns[i].interactable = false;
        CharDeleteBtns[i].interactable = true;
        CharReviveBtns[i + 1].interactable = true;
        CharGodModeBtns[i + 1].interactable = true;
    }

    void CharacterOffButtons(int i)
    {
        CharCreateBtns[i].interactable = true;
        CharDeleteBtns[i].interactable = false;
        CharReviveBtns[i + 1].interactable = false;
        CharGodModeBtns[i + 1].interactable = false;
    }

    void ShowWeaponPage()
    {
        CurrentBulletType = 0;

        ShowWeaponData();
    }

    void ShowWeaponData()
    {
        BulletName.text = GameManager.Inst().TxtManager.BulletTypeNames[CurrentBulletType];
        BulletLevelText.text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel().ToString();
        RarityText.text = GameManager.Inst().TxtManager.RarityNames[GameManager.Inst().UpgManager.BData[CurrentBulletType].GetRarity()];

        StatusText[0].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetDamage().ToString();
        StatusText[1].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpeed().ToString();
        StatusText[2].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHealth().ToString();

        ReinforceText[0].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk().ToString();
        ReinforceText[1].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd().ToString();
        ReinforceText[2].text = GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp().ToString();

        ReinforceImage[0].fillAmount = (float)GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() / GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxAtk();
        ReinforceImage[1].fillAmount = (float)GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() / GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxSpd();
        ReinforceImage[2].fillAmount = (float)GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() / GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxHp();

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetRarity() == Constants.MAXRARITY - 1 &&
            GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() >= GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxBulletLevel())
            PlusLevelBtn.interactable = false;
        else
            PlusLevelBtn.interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetRarity() == 0 &&
            GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() <= 1)
            MinusLevelBtn.interactable = false;
        else
            MinusLevelBtn.interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() >= GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxAtk())
            PlusReinforceBtn[0].interactable = false;
        else
            PlusReinforceBtn[0].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() <= 0)
            MinusReinforceBtn[0].interactable = false;
        else
            MinusReinforceBtn[0].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() >= GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxSpd())
            PlusReinforceBtn[1].interactable = false;
        else
            PlusReinforceBtn[1].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() <= 0)
            MinusReinforceBtn[1].interactable = false;
        else
            MinusReinforceBtn[1].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() >= GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxHp())
            PlusReinforceBtn[2].interactable = false;
        else
            PlusReinforceBtn[2].interactable = true;

        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() <= 0)
            MinusReinforceBtn[2].interactable = false;
        else
            MinusReinforceBtn[2].interactable = true;
    }

    void ShowResourcePage()
    {
        CoinText.text = GameManager.Inst().Player.GetCoin().ToString();
        JewelText.text = GameManager.Inst().Jewel.ToString();
        for (int i = 0; i < Constants.MAXSTAGES; i++)
            ResourceTexts[i].text = GameManager.Inst().Resources[i].ToString();
    }

    void ShowStagePage()
    {
        for (int i = 0; i < GameManager.Inst().StgManager.ReachedStage; i++)
            StageBtns[i].interactable = false;

        int count = 0;
        for(int i = 0; i < GameManager.Inst().QstManager.Quests.Count; i++)
        {
            if(GameManager.Inst().QstManager.Quests[i].QuestId / 10000 == GameManager.Inst().StgManager.ReachedStage)
            {
                QuestSlots[count].Desc.text = GameManager.Inst().QstManager.Quests[i].QuestDesc;
                QuestSlots[count].Count.text = GameManager.Inst().QstManager.Quests[i].CurrentCount.ToString() + " / " + GameManager.Inst().QstManager.Quests[i].GoalCount.ToString();
                QuestSlots[count].ProgressBar.fillAmount = (float)GameManager.Inst().QstManager.Quests[i].CurrentCount / GameManager.Inst().QstManager.Quests[i].GoalCount;
                QuestSlots[count].QuestID = GameManager.Inst().QstManager.Quests[i].QuestId;

                if (GameManager.Inst().QstManager.Quests[i].CurrentCount <= 0)
                    QuestBtns[count * 2 + 1].interactable = false;
                else
                    QuestBtns[count * 2 + 1].interactable = true;

                if (GameManager.Inst().QstManager.Quests[i].CurrentCount >= GameManager.Inst().QstManager.Quests[i].GoalCount)
                {
                    QuestSlots[count].Check.SetActive(true);
                    QuestBtns[count * 2].interactable = false;
                }
                else
                {
                    QuestSlots[count].Check.SetActive(false);
                    QuestBtns[count * 2].interactable = true;
                }                    

                count++;
            }
        }
    }

    void QuestUp(int index)
    {
        int id = QuestSlots[index].QuestID;

        GameManager.Inst().QstManager.QuestProgress(id % 10000 / 1000, id % 1000 / 100, 1);
        ShowStagePage();
    }

    void QuestDown(int index)
    {
        int id = QuestSlots[index].QuestID;

        GameManager.Inst().QstManager.QuestProgress(id % 10000 / 1000, id % 1000 / 100, -1);
        ShowStagePage();
    }

    void ShowItemPage()
    {
        MaxInventoryText.text = GameManager.Inst().Player.MaxInventory.ToString();

        for (int i = 0; i < 2; i++)
            InventoryBtns[i].interactable = true;

        if (GameManager.Inst().Player.MaxInventory >= Constants.MAXINVENTORY)
            InventoryBtns[0].interactable = false;
        else if (GameManager.Inst().Player.MaxInventory <= Constants.MININVENTORY)
            InventoryBtns[1].interactable = false;
    }

    void ShowDataPage()
    {

    }

    //Button
    public void OnClickRevive(int index)
    {
        if(index == 0)
            GameManager.Inst().Player.Revive();
        else
        {
            int i = index - 1;
            GameManager.Inst().GetSubweapons(i).Revive();
        }
    }

    public void OnClickInvincible(int index)
    {
        if (index == 0)
        {
            if (GameManager.Inst().Player.IsGodMode == true)
            {
                GameManager.Inst().Player.IsGodMode = false;
                GodModeBtnTexts[index].color = Color.white;
            }
            else
            {
                GameManager.Inst().Player.IsGodMode = true;
                GodModeBtnTexts[index].color = Color.yellow;
            }
        }
            
        else
        {
            int i = index - 1;

            if (GameManager.Inst().GetSubweapons(i).IsGodMode == true)
            {
                GameManager.Inst().GetSubweapons(i).IsGodMode = false;
                GodModeBtnTexts[index].color = Color.white;
            }
            else
            {
                GameManager.Inst().GetSubweapons(i).IsGodMode = true;
                GodModeBtnTexts[index].color = Color.yellow;
            }
        }
    }

    public void OnClickCreate(int index)
    {
        GameManager.Inst().UpgManager.AddSW(index);
        GameManager.Inst().UpgManager.AfterWork(index);

        CharacterOnButtons(index);
    }

    public void OnClickDelete(int index)
    {
        GameManager.Inst().GetSubweapons(index).Disable();

        CharacterOffButtons(index);
    }

    public void OnClickNextWeaponBtn()
    {
        CurrentBulletType++;
        if (CurrentBulletType >= Constants.MAXBULLETS)
            CurrentBulletType = 0;

        ShowWeaponData();
    }

    public void OnClickPrevWeaponBtn()
    {
        CurrentBulletType--;
        if (CurrentBulletType <= 0)
            CurrentBulletType = Constants.MAXBULLETS - 1;

        ShowWeaponData();
    }

    public void OnClickPlusWeaponLevelBtn()
    {
        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() < GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxBulletLevel())
            GameManager.Inst().UpgManager.LevelUp(CurrentBulletType);
        else
            GameManager.Inst().UpgManager.RarityUp(CurrentBulletType);

        ShowWeaponData();
    }

    public void OnClickMinusWeaponLevelBtn()
    {
        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() > 1)
            GameManager.Inst().UpgManager.LevelDown(CurrentBulletType);
        else
            GameManager.Inst().UpgManager.RarityDown(CurrentBulletType);

        ShowWeaponData();
    }

    public void OnClickReinforceGauge(int index)
    {
        Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int val = (int)(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxAtk() * (0.5f + mPos.x * 0.5f));
        if (val < 0)
            val = 0;

        switch (index)
        {
            case 0:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetAtk(val);
                break;
            case 1:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetSpd(val);
                break;
            case 2:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetHp(val);
                break;
        }

        ShowWeaponData();
    }

    public void OnClickPlusWeaponReinforceBtn(int index)
    {
        switch(index)
        {
            case 0:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetAtk(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() + 1);
                break;
            case 1:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetSpd(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() + 1);
                break;
            case 2:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetHp(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() + 1);
                break;
        }

        ShowWeaponData();
    }

    public void OnClickMinusWeaponReinforceBtn(int index)
    {
        switch (index)
        {
            case 0:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetAtk(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetAtk() - 1);
                break;
            case 1:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetSpd(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetSpd() - 1);
                break;
            case 2:
                GameManager.Inst().UpgManager.BData[CurrentBulletType].SetHp(GameManager.Inst().UpgManager.BData[CurrentBulletType].GetHp() - 1);
                break;
        }

        ShowWeaponData();
    }

    void ShowReinforceDetail()
    {
        string str = ReinforceTypes[ReinforceType];
        str += " + ";
        switch (ReinforceRarity)
        {
            case 0:
                str += "1";
                break;

            case 1:
                str += "5";
                break;

            case 2:
                str += "20";
                break;

            case 3:
                str += "75";
                break;

            case 4:
                str += "250";
                break;
        }
        ReinforceValueText.text = str;
    }

    void ShowEquipValue()
    {
        EquipValueText.text = GameManager.Inst().EquipDatas[EquipType, EquipRarity, 1].ToString();
        EquipValueBtns[1].interactable = false;
        if (GameManager.Inst().EquipDatas[EquipType, EquipRarity, 2] <= 0)
            EquipValueBtns[0].interactable = false;
        else
            EquipValueBtns[0].interactable = true;
    }

    void ShowEquipDetail()
    {
        string detail = "";
        if (GameManager.Inst().EquipDatas[EquipType, EquipRarity, 0] > 0)
            detail += GameManager.Inst().EquipDatas[EquipType, EquipRarity, 0].ToString();
        detail += GameManager.Inst().TxtManager.EquipDetailFront[EquipType];
        if (int.Parse(EquipValueText.text) > 0)
            detail += EquipValueText.text;
        detail += GameManager.Inst().TxtManager.EquipDetailBack[EquipType];

        EquipDetailText.text = detail;
    }

   public void OnClickInputBtn(int type)
    {
        string str = "";
        switch(type)
        {
            case 0:
                str = CoinText.text;
                break;

            case 1:
                str = JewelText.text;
                break;

            case 2:
                str = ResourceTexts[0].text;
                break;

            case 3:
                str = ResourceTexts[1].text;
                break;

            case 4:
                str = ResourceTexts[2].text;
                break;

            case 5:
                str = ResourceTexts[3].text;
                break;
        }

        NumInput.gameObject.SetActive(true);
        NumInput.Show(str, type);
    }

    public void OnSelectReinforceType(int type)
    {
        ReinforceType = type;

        ShowReinforceDetail();
    }

    public void OnSelectReinforceRarity(int rarity)
    {
        ReinforceRarity = rarity;

        ShowReinforceDetail();
    }

    public void OnClickMakeReinforceBtn()
    {
        GameManager.Inst().MakeReinforceData(ReinforceType, ReinforceRarity);
    }

    public void OnSelectEquipType(int type)
    {
        EquipType = type;

        ShowEquipValue();
        ShowEquipDetail();
    }

    public void OnSelectEquipRarity(int rarity)
    {
        EquipRarity = rarity;

        ShowEquipValue();
        ShowEquipDetail();
    }

    public void OnClickEquipValueBtn(int index)
    {
        if (index == 0)
        {
            EquipValueBtns[1].interactable = true;

            EquipValueText.text = (int.Parse(EquipValueText.text) + 1).ToString();
            if (int.Parse(EquipValueText.text) >= GameManager.Inst().EquipDatas[EquipType, EquipRarity, 2])
                EquipValueBtns[0].interactable = false;
        }
        else
        {
            EquipValueBtns[0].interactable = true;

            EquipValueText.text = (int.Parse(EquipValueText.text) - 1).ToString();
            if (int.Parse(EquipValueText.text) <= GameManager.Inst().EquipDatas[EquipType, EquipRarity, 1])
                EquipValueBtns[1].interactable = false;
        }

        ShowEquipDetail();
    }

    public void OnClickMakeEquipBtn()
    {
        GameManager.Inst().MakeEquipData(EquipType, EquipRarity);
    }

    public void OnClickInventoryBtn(int index)
    {
        if(index == 0)
        {
            InventoryBtns[1].interactable = true;

            GameManager.Inst().Player.MaxInventory += 10;
            GameManager.Inst().AddInventory(10);
            MaxInventoryText.text = GameManager.Inst().Player.MaxInventory.ToString();

            if (GameManager.Inst().Player.MaxInventory >= Constants.MAXINVENTORY)
                InventoryBtns[0].interactable = false;
        }
        else
        {
            InventoryBtns[0].interactable = true;

            GameManager.Inst().Player.MaxInventory -= 10;
            MaxInventoryText.text = GameManager.Inst().Player.MaxInventory.ToString();

            if (GameManager.Inst().Player.MaxInventory <= Constants.MININVENTORY)
                InventoryBtns[1].interactable = false;
        }
    }

    public void OnClickStageBtn(int index)
    {
        for(int i = GameManager.Inst().StgManager.ReachedStage - 1; i < index; i++)
            GameManager.Inst().QstManager.OpenNextStage(index + 1);

        ShowStagePage();
    }

    public void OnClickQuestUpBtn(int index)
    {
        IsQstUps[index] = true;
    }

    public void OnReleaseQuestUpBtn(int index)
    {
        IsQstUps[index] = false;
    }

    public void OnClickQuestDownBtn(int index)
    {
        IsQstDowns[index] = true;
    }

    public void OnReleaseQuestDownBtn(int index)
    {
        IsQstDowns[index] = false;
    }

    public void OnClickDateDeleteBtn()
    {
        GameManager.Inst().DatManager.GameData.ResetData();
        GameManager.Inst().DatManager.GameData.IsEraseData = true;

        Application.Quit();
    }

    public void OnClickBossShowBtn()
    {
        GameManager.Inst().StgManager.SetBossCount(GameManager.Inst().StgManager.Stage, 1000);
    }
}
