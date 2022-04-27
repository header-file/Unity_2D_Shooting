using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cheat : MonoBehaviour
{
    interface PageType
    {
        void ShowPage();
    }

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

    PageType Page;
    PageType[] PageTypes;

    bool[] IsQstUps;
    bool[] IsQstDowns;

    int CurrentBulletType = 0;

    int ReinforceType = 0;
    int ReinforceRarity = 0;
    int EquipType = 0;
    int EquipRarity = 0;

    bool IsStarted;


    void Awake()
    {
        IsQstUps = new bool[4];
        IsQstDowns = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            IsQstUps[i] = false;
            IsQstDowns[i] = false;
        }

        PageTypes = new PageType[6];
        PageTypes[0] = new CharacterPage();
        PageTypes[1] = new WeaponPage();
        PageTypes[2] = new ResourcePage();
        PageTypes[3] = new StagePage();
        PageTypes[4] = new ItemPage();
        PageTypes[5] = new DataPage();
        Page = PageTypes[0];
    }

    void Start()
    {
        IsStarted = true;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!Toggles[3].isOn)
            return;

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

        if(IsStarted)
            for (int i = 0; i < Pages.Length; i++)
                Pages[i].SetActive(false);

        Pages[index].SetActive(true);
        Page = GameManager.Inst().UiManager.MainUI.Center.Cheat.PageTypes[index];
        ShowPage();
    }

    public void ShowPage()
    {
        Page.ShowPage();
    }

    void QuestUp(int index)
    {
        int id = QuestSlots[index].QuestID;

        GameManager.Inst().QstManager.QuestProgress(id % 10000 / 1000, id % 1000 / 100, 1);
        Page.ShowPage();
    }

    void QuestDown(int index)
    {
        int id = QuestSlots[index].QuestID;

        GameManager.Inst().QstManager.QuestProgress(id % 10000 / 1000, id % 1000 / 100, -1);
        Page.ShowPage();
    }

    class CharacterPage : PageType
    {
        public void ShowPage()
        {
            for (int i = 0; i < Constants.MAXSUBWEAPON; i++)
            {
                if (GameManager.Inst().GetSubweapons(i) == null)
                    CharacterOffButtons(i);
                else
                    CharacterOnButtons(i);
            }
        }

        void CharacterOnButtons(int i)
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CharCreateBtns[i].interactable = false;
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CharDeleteBtns[i].interactable = true;
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CharReviveBtns[i + 1].interactable = true;
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CharGodModeBtns[i + 1].interactable = true;
        }

        void CharacterOffButtons(int i)
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CharCreateBtns[i].interactable = true;
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CharDeleteBtns[i].interactable = false;
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CharReviveBtns[i + 1].interactable = false;
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CharGodModeBtns[i + 1].interactable = false;
        }
    }

    class WeaponPage : PageType
    {
        public void ShowPage()
        {
            ShowWeaponData();
        }

        void ShowWeaponData()
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.BulletName.text = GameManager.Inst().TxtManager.BulletTypeNames[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType];
            GameManager.Inst().UiManager.MainUI.Center.Cheat.BulletLevelText.text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetPowerLevel().ToString();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.RarityText.text = GameManager.Inst().TxtManager.RarityNames[GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetRarity()];

            GameManager.Inst().UiManager.MainUI.Center.Cheat.StatusText[0].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetDamage().ToString();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.StatusText[1].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetSpeed().ToString();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.StatusText[2].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetHealth().ToString();

            GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceText[0].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetAtk().ToString();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceText[1].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetSpd().ToString();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceText[2].text = GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetHp().ToString();

            GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceImage[0].fillAmount = (float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetAtk() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetMaxAtk();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceImage[1].fillAmount = (float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetSpd() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetMaxSpd();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceImage[2].fillAmount = (float)GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetHp() / GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetMaxHp();

            if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetRarity() == Constants.MAXRARITY - 1 &&
                GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetPowerLevel() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetMaxBulletLevel())
                GameManager.Inst().UiManager.MainUI.Center.Cheat.PlusLevelBtn.interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.PlusLevelBtn.interactable = true;

            if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetRarity() == 0 &&
                GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetPowerLevel() <= 1)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.MinusLevelBtn.interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.MinusLevelBtn.interactable = true;

            if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetAtk() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetMaxAtk())
                GameManager.Inst().UiManager.MainUI.Center.Cheat.PlusReinforceBtn[0].interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.PlusReinforceBtn[0].interactable = true;

            if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetAtk() <= 0)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.MinusReinforceBtn[0].interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.MinusReinforceBtn[0].interactable = true;

            if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetSpd() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetMaxSpd())
                GameManager.Inst().UiManager.MainUI.Center.Cheat.PlusReinforceBtn[1].interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.PlusReinforceBtn[1].interactable = true;

            if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetSpd() <= 0)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.MinusReinforceBtn[1].interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.MinusReinforceBtn[1].interactable = true;

            if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetHp() >= GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetMaxHp())
                GameManager.Inst().UiManager.MainUI.Center.Cheat.PlusReinforceBtn[2].interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.PlusReinforceBtn[2].interactable = true;

            if (GameManager.Inst().UpgManager.BData[GameManager.Inst().UiManager.MainUI.Center.Cheat.CurrentBulletType].GetHp() <= 0)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.MinusReinforceBtn[2].interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.MinusReinforceBtn[2].interactable = true;
        }
    }

    class ResourcePage : PageType
    {
        public void ShowPage()
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.CoinText.text = GameManager.Inst().Player.GetCoin().ToString();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.JewelText.text = GameManager.Inst().Jewel.ToString();
            for (int i = 0; i < Constants.MAXRESOURCETYPES; i++)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.ResourceTexts[i].text = GameManager.Inst().Resources[i].ToString();
        }
    }

    class StagePage : PageType
    {
        public void ShowPage()
        {
            for (int i = 0; i < GameManager.Inst().StgManager.ReachedStage; i++)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.StageBtns[i].interactable = false;

            int count = 0;
            for (int i = 0; i < GameManager.Inst().QstManager.Quests.Count; i++)
            {
                if (GameManager.Inst().QstManager.Quests[i].QuestId / 10000 == GameManager.Inst().StgManager.ReachedStage)
                {
                    GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestSlots[count].Desc.text = 
                        GameManager.Inst().QstManager.Quests[i].QuestDesc;
                    GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestSlots[count].Count.text = 
                        GameManager.Inst().QstManager.Quests[i].CurrentCount.ToString() + " / " + GameManager.Inst().QstManager.Quests[i].GoalCount.ToString();
                    GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestSlots[count].ProgressBar.fillAmount = 
                        (float)GameManager.Inst().QstManager.Quests[i].CurrentCount / GameManager.Inst().QstManager.Quests[i].GoalCount;
                    GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestSlots[count].QuestID = 
                        GameManager.Inst().QstManager.Quests[i].QuestId;

                    if (GameManager.Inst().QstManager.Quests[i].CurrentCount <= 0)
                        GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestBtns[count * 2 + 1].interactable = false;
                    else
                        GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestBtns[count * 2 + 1].interactable = true;

                    if (GameManager.Inst().QstManager.Quests[i].CurrentCount >= GameManager.Inst().QstManager.Quests[i].GoalCount)
                    {
                        GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestSlots[count].Check.SetActive(true);
                        GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestBtns[count * 2].interactable = false;
                    }
                    else
                    {
                        GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestSlots[count].Check.SetActive(false);
                        GameManager.Inst().UiManager.MainUI.Center.Cheat.QuestBtns[count * 2].interactable = true;
                    }

                    count++;
                }
            }
        }
    }

    class ItemPage : PageType
    {
        public void ShowPage()
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.MaxInventoryText.text = GameManager.Inst().Player.MaxInventory.ToString();

            for (int i = 0; i < 2; i++)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.InventoryBtns[i].interactable = true;

            if (GameManager.Inst().Player.MaxInventory >= Constants.MAXINVENTORY)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.InventoryBtns[0].interactable = false;
            else if (GameManager.Inst().Player.MaxInventory <= Constants.MININVENTORY)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.InventoryBtns[1].interactable = false;

            ShowReinforceDetail();
            ShowEquipDetail();
        }

        void ShowReinforceDetail()
        {
            string str = GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceTypes[GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceType];
            str += " + 1";
            GameManager.Inst().UiManager.MainUI.Center.Cheat.ReinforceValueText.text = str;
        }

        void ShowEquipValue()
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text = GameManager.Inst().EquipDatas[GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipType, GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipRarity, 1].ToString();
            GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueBtns[1].interactable = false;
            if (GameManager.Inst().EquipDatas[GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipType, GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipRarity, 2] <= 0)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueBtns[0].interactable = false;
            else
                GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueBtns[0].interactable = true;
        }

        void ShowEquipDetail()
        {
            ShowEquipValue();

            string detail = "";
            if (GameManager.Inst().EquipDatas[GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipType, GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipRarity, 0] > 0)
                detail += GameManager.Inst().EquipDatas[GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipType, GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipRarity, 0].ToString();
            detail += GameManager.Inst().TxtManager.EquipDetailFront[GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipType];
            if (int.Parse(GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text) > 0)
                detail += GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text;
            detail += GameManager.Inst().TxtManager.EquipDetailBack[GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipType];

            GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipDetailText.text = detail;
        }
    }

    class DataPage : PageType
    {
        public void ShowPage()
        {

        }
    }

    //Button Interaction
    //Character
    public void OnClickCreate(int index)
    {
        GameManager.Inst().UpgManager.AddSW(index);
        GameManager.Inst().UpgManager.AfterWork(index);

        ShowPage();
    }

    public void OnClickDelete(int index)
    {
        GameManager.Inst().GetSubweapons(index).Disable();

        ShowPage();
    }

    public void OnClickRevive(int index)
    {
        if (index == 0)
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

    //Weapon
    public void OnClickNextWeaponBtn()
    {
        CurrentBulletType++;
        if (CurrentBulletType >= Constants.MAXBULLETS)
            CurrentBulletType = 0;

        ShowPage();
    }

    public void OnClickPrevWeaponBtn()
    {
        CurrentBulletType--;
        if (CurrentBulletType <= 0)
            CurrentBulletType = Constants.MAXBULLETS - 1;

        ShowPage();
    }

    public void OnClickPlusWeaponLevelBtn()
    {
        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() < GameManager.Inst().UpgManager.BData[CurrentBulletType].GetMaxBulletLevel())
            GameManager.Inst().UpgManager.LevelUp(CurrentBulletType);
        else
            GameManager.Inst().UpgManager.RarityUp(CurrentBulletType);

        ShowPage();
    }

    public void OnClickMinusWeaponLevelBtn()
    {
        if (GameManager.Inst().UpgManager.BData[CurrentBulletType].GetPowerLevel() > 1)
            GameManager.Inst().UpgManager.LevelDown(CurrentBulletType);
        else
            GameManager.Inst().UpgManager.RarityDown(CurrentBulletType);

        ShowPage();
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

        ShowPage();
    }

    public void OnClickPlusWeaponReinforceBtn(int index)
    {
        switch (index)
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

        ShowPage();
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

        ShowPage();
    }

    //Resource
    public void OnClickInputBtn(int type)
    {
        string str = "";
        switch (type)
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

    //Stage
    public void OnClickStageBtn(int index)
    {
        for (int i = GameManager.Inst().StgManager.ReachedStage - 1; i < index; i++)
            GameManager.Inst().QstManager.OpenNextStage(index + 1);

        ShowPage();
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

    //Item
    public void OnSelectReinforceType(int type)
    {
        ReinforceType = type;

        ShowPage();
    }

    public void OnSelectReinforceRarity(int rarity)
    {
        ReinforceRarity = rarity;

        ShowPage();
    }

    public void OnClickMakeReinforceBtn()
    {
        GameManager.Inst().MakeReinforceData(ReinforceType, ReinforceRarity);
    }

    public void OnSelectEquipType(int type)
    {
        EquipType = type;

        ShowPage();
    }

    public void OnSelectEquipRarity(int rarity)
    {
        EquipRarity = rarity;

        ShowPage();
    }

    public void OnClickEquipValueBtn(int index)
    {
        if (index == 0)
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueBtns[1].interactable = true;

            GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text =
                (int.Parse(GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text) + 1).ToString();
            if (int.Parse(GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text) >=
                GameManager.Inst().EquipDatas[EquipType, EquipRarity, 2])
                GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueBtns[0].interactable = false;
        }
        else
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueBtns[0].interactable = true;

            GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text =
                (int.Parse(GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text) - 1).ToString();
            if (int.Parse(GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueText.text) <=
                GameManager.Inst().EquipDatas[EquipType, EquipRarity, 1])
                GameManager.Inst().UiManager.MainUI.Center.Cheat.EquipValueBtns[1].interactable = false;
        }

        ShowPage();
    }

    public void OnClickMakeEquipBtn()
    {
        GameManager.Inst().MakeEquipData(EquipType, EquipRarity);
    }

    public void OnClickInventoryBtn(int index)
    {
        if (index == 0)
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.InventoryBtns[1].interactable = true;

            GameManager.Inst().Player.MaxInventory += 10;
            GameManager.Inst().AddInventory(10);
            GameManager.Inst().UiManager.MainUI.Center.Cheat.MaxInventoryText.text = GameManager.Inst().Player.MaxInventory.ToString();

            if (GameManager.Inst().Player.MaxInventory >= Constants.MAXINVENTORY)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.InventoryBtns[0].interactable = false;
        }
        else
        {
            GameManager.Inst().UiManager.MainUI.Center.Cheat.InventoryBtns[0].interactable = true;

            GameManager.Inst().Player.MaxInventory -= 10;
            GameManager.Inst().UiManager.MainUI.Center.Cheat.MaxInventoryText.text = GameManager.Inst().Player.MaxInventory.ToString();

            if (GameManager.Inst().Player.MaxInventory <= Constants.MININVENTORY)
                GameManager.Inst().UiManager.MainUI.Center.Cheat.InventoryBtns[1].interactable = false;
        }
    }

    //Data
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
