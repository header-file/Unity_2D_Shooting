using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Synthesis : MonoBehaviour
{
    public SynthesisSlot[] Buttons;
    //public GameObject Lines;
    public GameObject InventoryArea;
    //public Material[] Materials;
    public GameObject EnterItemText;
    public GameObject SuccessRate;
    public Text RateText;
    public GameObject ConfirmWindow;
    public InventoryDetail EquipDetail;
    public GameObject SelectDetail;
    public GameObject UnequipConfirmWindow;
    public Text Need;
    public Sprite[] Frames;
    public Button MergetBtn;
    public Animator Anim;
    public ParticleSystem Shine;

    InventoryScroll Inventories;
    Sprite OriginalSprite;
    Sprite QuestionSprite;
    Color[] Colors;
    Color TextColor;

    int Grade;
    int[] InputTypes;
    int[] SelectedIndex;
    int[] SelectedUIDs;
    bool IsAbleSynthesize;
    int Rate;
    float Timer;
    int SynthType;
    int UnequipIndex;
    bool IsSynthSuccess;

    public int GetGrade() { return Grade; }
    public int GetUnequipIndex() { return UnequipIndex; }

    void Start()
    {
        OriginalSprite = Buttons[0].Icon.sprite;
        QuestionSprite = Buttons[3].Icon.sprite;
        EnterItemText.SetActive(true);
        RateText.text = "0";
        Need.text = "0";
        SuccessRate.SetActive(false);
        ConfirmWindow.SetActive(false);
        EquipDetail.gameObject.SetActive(false);
        SelectDetail.SetActive(false);
        UnequipConfirmWindow.SetActive(false);
        MergetBtn.interactable = false;

        Colors = new Color[3];
        Colors[0] = Color.red;
        Colors[1] = new Color(0.5f, 0.0f, 1.0f);
        Colors[2] = new Color(0.1882353f, 0.8862746f, 0.7372549f);
        TextColor = Color.white;

        SelectedIndex = new int[3];
        for (int i = 0; i < 3; i++)
            SelectedIndex[i] = -1;

        SelectedUIDs = new int[3];
        for (int i = 0; i < 3; i++)
            SelectedUIDs[i] = -1;

        Grade = -1;
        //CurrentIndex = -1;
        InputTypes = new int[3];
        for (int i = 0; i < 3; i++)
            InputTypes[i] = -1;

        Anim.speed = 0.0f;

        IsAbleSynthesize = false;
        Rate = 0;
        Timer = 0.0f;
        SynthType = -1;

        IsSynthSuccess = false;

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (SuccessRate.activeSelf)
        {
            Timer += Time.deltaTime * 10.0f;
            TextColor.a = Mathf.Abs(Mathf.Sin(Timer));
            RateText.color = TextColor;
        }
    }

    public void ShowInventory()
    {
        Inventories = GameManager.Inst().UiManager.InventoryScroll.GetComponent<InventoryScroll>();
        Inventories.transform.SetParent(InventoryArea.transform, false);
        Inventories.SetSlotType(2);
        Inventories.ShowInventory();

        Sort();
    }

    public void Sort()
    {
        Inventories.ResetInventory();

        SortAsDefault();
    }

    public void SortAsDefault()
    {
        for (int i = 0; i < GameManager.Inst().Player.MaxInventory; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);

            if (eq != null)
            {
                if (eq.UID / 100 == 6)
                {
                    Sprite icon = eq.Icon;
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);

                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(false);
                    slot.SetGradeSprite(eq.Rarity);

                    for (int j = 0; j < Constants.MAXBULLETS; j++)
                    {
                        if (GameManager.Inst().UpgManager.BData[j].GetEquipIndex() == Inventories.GetSwitchedIndex(i))
                            slot.EMark.SetActive(true);
                    }
                }
            }
            else
                Inventories.HideSlot(i);
        }

        GameManager.Inst().Player.SortOption = (int)InventorySlot.SortOption.SYNTHESIS;

        Inventories.Sort();
    }

    public void SortAsGrade(int rarity)
    {
        Inventories.ResetInventory();

        for (int i = 0; i < GameManager.Inst().Player.MaxInventory; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);

            if (eq != null)
            {
                if (eq.UID / 100 == 6 && eq.Rarity == rarity)
                {
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.SetDisable(false);
                }
                else
                {
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.SetDisable(true);
                }

            }
        }

        GameManager.Inst().Player.SortOption = (int)InventorySlot.SortOption.SYNTHESIS_EQUIP + rarity;

        Inventories.Sort();
    }

    public void ShowConfirmWindow()
    {
        ConfirmWindow.SetActive(true);
    }

    public void CancelConfirm()
    {
        ConfirmWindow.SetActive(false);
    }

    public void SetButtons(int index)
    {
        Player.EqData eq = GameManager.Inst().Player.GetItem(index);
        if (eq.Rarity >= Constants.MAXRARITY - 1)
            return;

        if (eq.UID / 100 == 6)
        {
            for(int i = 0; i < 3; i++)
            {
                if (SelectedIndex[i] == index)
                {
                    UnSetButtons(index, i);
                    return;
                }
            }

            for(int i = 0; i < 3; i++)
            {
                if (SelectedIndex[i] != -1)
                    continue;

                if (Inventories.GetSlot(Inventories.GetSwitchedIndex(index)).EMark.activeSelf)
                    return;

                Buttons[i].Frame.sprite = Frames[eq.Rarity];
                Buttons[i].Icon.sprite = eq.Icon;
                InputTypes[i] = eq.Type;
                SelectedIndex[i] = index;
                SelectedUIDs[i] = eq.UID;

                Inventories.GetSlot(Inventories.GetSwitchedIndex(index)).Checked.SetActive(true);

                SortAsGrade(eq.Rarity);
                break;
            }
        }

        //합성 가능
        {
            for (int i = 0; i < 3; i++)
            {
                if (InputTypes[i] == -1)
                {
                    IsAbleSynthesize = false;
                    return;
                }
            }

            IsAbleSynthesize = true;
            MergetBtn.interactable = true;
            EnterItemText.SetActive(false);
            SuccessRate.SetActive(true);
            Rate = 100 - eq.Rarity * 10;
            RateText.text = Rate.ToString();
            int need = (int)Mathf.Pow(10, (eq.Rarity + 2));
            Need.text = need.ToString();

            if (eq.UID / 100 == 3)
                Buttons[3].Icon.sprite = GameManager.Inst().UiManager.FoodImages[eq.Type + (eq.Rarity + 1) * 3];
            else if (eq.UID / 100 == 6)
            {
                if (InputTypes[0] == InputTypes[1] && InputTypes[1] == InputTypes[2])
                {
                    Buttons[3].Icon.sprite = eq.Icon;
                    SynthType = InputTypes[0];
                }
                else
                {
                    Buttons[3].Icon.sprite = QuestionSprite;
                    SynthType = -1;
                }
            }
            Buttons[3].Frame.sprite = Frames[eq.Rarity + 1];
        }
    }

    void UnSetButtons(int index, int slotIndex)
    {
        Buttons[slotIndex].Frame.sprite = Frames[0];
        Buttons[slotIndex].Icon.sprite = OriginalSprite;
        InputTypes[slotIndex] = -1;
        SelectedIndex[slotIndex] = -1;
        SelectedUIDs[slotIndex] = -1;

        MergetBtn.interactable = false;
        EnterItemText.SetActive(true);
        SuccessRate.SetActive(false);
        Buttons[3].Icon.sprite = QuestionSprite;
        Buttons[3].Frame.sprite = Frames[0];

        Inventories.GetSlot(Inventories.GetSwitchedIndex(index)).Checked.SetActive(false);

        if (SelectedIndex[0] == SelectedIndex[1] && SelectedIndex[0] == SelectedIndex[2] && SelectedIndex[0] == -1)
            Sort();
    }

    //public void CancelSelect()
    //{
    //    if (InputTypes[0] == InputTypes[1] && InputTypes[0] == InputTypes[2] && InputTypes[0] == -1)
    //        return;

    //    ResetSprites();
    //    ShowInventory();
    //}

    public void Synthesize()
    {
        if (!IsAbleSynthesize)
            return;
        //if(GameManager.Inst().Player.GetItem(SelectedIndex[0]).Quantity > 1 &&
        //    GameManager.Inst().Player.CurInventory >= GameManager.Inst().Player.MaxInventory)
        //{
        //    GameManager.Inst().UiManager.MainUI.Center.PlayInventoryFull();
        //    return;
        //}
        if (GameManager.Inst().Player.GetCoin() < int.Parse(Need.text))
            return;
        else
            GameManager.Inst().Player.MinusCoin(int.Parse(Need.text));

        int rarity = GameManager.Inst().Player.GetItem(SelectedIndex[0]).Rarity;

        for (int i = 0; i < 3; i++)
            GameManager.Inst().Player.RemoveItem(SelectedIndex[i]);

        int rand = Random.Range(0, 100);
        if (rand < Rate)
        {
            rarity++;
            IsSynthSuccess = true;
        }
        else
            IsSynthSuccess = false;


        int add = -1;
        //if(SelectedUIDs[0] / 100 == 3)
        //    add = GameManager.Inst().Player.AddItem(GameManager.Inst().MakeReinforceData(SynthType, rarity));
        //else if(SelectedUIDs[0] / 100 == 6)
            add = GameManager.Inst().Player.AddItem(GameManager.Inst().MakeEquipData(SynthType, rarity));

        EquipDetail.GetComponent<InventoryDetail>().ShowDetail(add);
        ConfirmWindow.SetActive(false);

        GameManager.Inst().SodManager.PlayEffect("Synth try");
        Anim.speed = 1.0f;
        Shine.gameObject.SetActive(true);
        Shine.Play();
    }

    public void ShowResultWindow()
    {
        Anim.speed = 0.0f;
        Shine.Stop();
        Shine.gameObject.SetActive(false);

        //결과창
        if (IsSynthSuccess)
        {
            EquipDetail.Fail.SetActive(false);
            EquipDetail.Success.SetActive(true);
            GameManager.Inst().SodManager.PlayEffect("Synth success");
        }
        else
        {
            EquipDetail.Fail.SetActive(true);
            EquipDetail.Success.SetActive(false);
            GameManager.Inst().SodManager.PlayEffect("Synth fail");
        }

        ResetSprites();

        ShowInventory();

        EquipDetail.gameObject.SetActive(true);
        EquipDetail.ResultAnim.Play();
    }

    public void ResetSprites()
    {
        for (int i = 0; i < 3; i++)
        {
            Buttons[i].Icon.sprite = OriginalSprite;
            Buttons[i].Frame.sprite = Frames[0];
            
            if(SelectedIndex[i] > -1)
                Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex[i])).Checked.SetActive(false);
                
            SelectedIndex[i] = -1;
            SelectedUIDs[i] = -1;
            InputTypes[i] = -1;
        }

        SuccessRate.SetActive(false);
        SelectDetail.SetActive(false);
        EquipDetail.gameObject.SetActive(false);
        Buttons[3].Icon.sprite = QuestionSprite;
        Buttons[3].Frame.sprite = Frames[0];

        Inventories.ResetInventory();
        ResetDisable();
    }

    public void ResetDisable()
    {
        for (int i = 0; i < GameManager.Inst().Player.MaxInventory; i++)
        {
            if (GameManager.Inst().Player.GetItem(i) != null)
                Inventories.GetSlot(Inventories.GetSwitchedIndex(i)).SetDisable(false);
        }
    }

    void ShowSelectDetail(Player.EqData eqData)
    {
        SelectDetail.SetActive(true);

        int type = eqData.Type;
        int value = (int)eqData.Value;

        SelectDetail.GetComponent<SelectDetail>().SetDatas(type, value, Colors[InputTypes[0]]);
    }

    public void CloseResult()
    {
        EquipDetail.gameObject.SetActive(false);
    }

    public void CloseDetail()
    {
        SelectDetail.SetActive(false);

        for(int i = 0; i < 3; i++)
        {
            if (SelectedIndex[i] > -1)
                Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex[i])).SetSelected(false);
        }
    }

    public void ShowUnEquipConfirm(int index)
    {
        UnequipConfirmWindow.SetActive(true);

        UnequipIndex = index;
    }

    public void CloseUnequip()
    {
        UnequipConfirmWindow.SetActive(false);
    }

    public int CheckInputTypes()
    {
        int count = 0;

        for(int i = 0; i < 3; i++)
        {
            if (InputTypes[i] > -1)
                count++;
        }

        return count;
    }

    public void OnClickMergeBtn()
    {
        ConfirmWindow.SetActive(true);
    }

    public void OnClickCancelBtn()
    {
        ConfirmWindow.SetActive(false);
    }

    public void OnClickSynthesisConfirmBtn()
    {
        Synthesize();
    }

    public void OnClickSynthesisConfirmBackBtn()
    {
        CancelConfirm();
    }

    public void OnClickSynthesisSelectBtn(int index)
    {
        SetButtons(index);
    }

    public void OnClickSynthesisUnselectBtn(int index)
    {
        UnSetButtons(SelectedIndex[index], index);

        if (SceneManager.GetActiveScene().name == "Stage0" && GameManager.Inst().Tutorials.Step == 55)
            GameManager.Inst().Tutorials.Step++;
    }

    public void OnClickSynthesisResultBackBtn()
    {
        CloseResult();
    }

    public void OnClickSelectDetailBackBtn()
    {
        CloseDetail();
    }

    public void OnClickSynthesisUnequipBtn()
    {
        int index = UnequipIndex;
        int equipType = GameManager.Inst().Player.GetItem(index).Type;

        SetButtons(index);

        CloseUnequip();
    }

    public void OnClickUnequipConfirmBackBtn()
    {
        CloseUnequip();
    }
}



//if (IsAbleSynthesize)
//{
//    IsAbleSynthesize = false;
//    MergetBtn.interactable = false;
//    EnterItemText.SetActive(true);
//    SuccessRate.SetActive(false);
//}

//Player.EqData eq = GameManager.Inst().Player.GetItem(index);
//if (eq.UID / 100 == 3)
//{
//    CurrentIndex = 0;
//    Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex[0])).Checked.SetActive(false);

//    for (int i = 0; i < 3; i++)
//    {
//        Buttons[i].Icon.sprite = QuestionSprite;
//        Buttons[i].Frame.sprite = Frames[0];
//        InputTypes[i] = -1;
//        SelectedIndex[i] = -1;
//        SelectedUIDs[i] = -1;
//    }

//    Inventories.ResetInventory();
//    ResetDisable();
//}
//else if (eq.UID / 100 == 6)
//{
//    CurrentIndex = index;
//    Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex[index])).Checked.SetActive(false);

//    Buttons[index].Icon.sprite = QuestionSprite;
//    Buttons[index].Frame.sprite = Frames[0];
//    InputTypes[index] = -1;
//    SelectedUIDs[index] = -1;
//    SelectedIndex[index] = -1;

//    for (int i = 0; i < 3; i++)
//        if (SelectedIndex[i] > -1)
//            return;

//    Inventories.ResetInventory();
//    ResetDisable();
//}