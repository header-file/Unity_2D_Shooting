using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Synthesis : MonoBehaviour
{
    public GameObject[] Buttons;
    public GameObject Lines;
    public GameObject InventoryArea;
    public Material[] Materials;
    public GameObject SuccessRate;
    public Text RateText;
    public GameObject ConfirmWindow;
    public GameObject EquipDetail;
    public GameObject SelectDetail;
    public GameObject UnequipConfirmWindow;

    InventoryScroll Inventories;
    Sprite OriginalSprite;
    Sprite QuestionSprite;
    Color[] Colors;
    Color TextColor;

    int Grade;
    int CurrentIndex;
    int LastIndex;
    int[] InputTypes;
    int[] SelectedIndex;
    int[] SelectedUIDs;
    bool IsAbleSynthesize;
    int Rate;
    float Timer;
    int SynthType;
    int UnequipIndex;

    public int GetGrade() { return Grade; }
    public int GetCurrentIndex() { return CurrentIndex; }
    public int GetLastIndex() { return LastIndex; }
    public int GetUnequipIndex() { return UnequipIndex; }

    public void SetCurrentIndex(int index) { CurrentIndex = index; }

    void Start()
    {
        OriginalSprite = Buttons[0].transform.GetChild(0).GetComponent<Image>().sprite;
        QuestionSprite = Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite;
        SuccessRate.SetActive(false);
        ConfirmWindow.SetActive(false);
        EquipDetail.SetActive(false);
        SelectDetail.SetActive(false);
        UnequipConfirmWindow.SetActive(false);

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
        CurrentIndex = -1;
        InputTypes = new int[3];
        for (int i = 0; i < 3; i++)
            InputTypes[i] = -1;

        for(int i = 0; i < 3; i++)
        {
            GameObject line = GameManager.Inst().ObjManager.MakeObj("Line");
            Image lineImg = line.GetComponent<Image>();
            lineImg.material = Materials[i];
            lineImg.material.SetColor("_GlowColor", Color.black);

            line.transform.SetParent(Lines.transform, false);
            if (i == 0)
            {
                line.GetComponent<RectTransform>().anchoredPosition = new Vector3(-80.0f, 17.0f, 0.0f);
                line.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
            }
            else if (i == 1)
            {
                line.GetComponent<RectTransform>().anchoredPosition = new Vector3(80.0f, 17.0f, 0.0f);
                line.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
            }
            else
                line.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f, -100.0f, 0.0f);
        }

        IsAbleSynthesize = false;
        Rate = 0;
        Timer = 0.0f;
        SynthType = -1;

        gameObject.SetActive(false);
    }

    void Update()
    {
        if(SuccessRate.activeSelf)
        {
            Timer += Time.deltaTime * 10.0f;
            TextColor.a = Mathf.Abs(Mathf.Sin(Timer));
            RateText.color = TextColor;
        }
    }

    public void ShowInventory()
    {
        Inventories = GameManager.Inst().Inventory.GetComponent<InventoryScroll>();
        Inventories.transform.SetParent(InventoryArea.transform, false);
        Inventories.SetSlotType(2);

        Inventories.ResetInventory();
        Inventories.ShowInventory();
    }

    public void DiscardMaxGrade()
    {
        Inventories.ResetInventory();

        for (int i = 0; i < Constants.MAXINVENTORY; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            Inventories.GetSlot(i).Selected.SetActive(false);

            if (eq != null)
            {
                if (eq.Rarity < Constants.MAXRARITY - 1)
                {
                    Sprite icon = eq.Icon;
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);

                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(false);
                    slot.SetGradeSprite(eq.Rarity);

                    //for (int j = 0; j < 3; j++)
                    //{
                    //    if (eq.UID == SelectedUIDs[j])
                    //        slot.Selected.SetActive(true);
                    //}

                    //if (slot.Selected.activeSelf)
                    //    SelectedIndex[0] = i;
                }
                else
                {
                    Sprite icon = eq.Icon;
                    //InventorySlot slot = Inventories.GetSlot(Inventories.GetSwitchedIndex(i));
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);

                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(true);
                    slot.SetGradeSprite(eq.Rarity);
                }
            }
        }

        GameManager.Inst().Player.SortOption = (int)InventorySlot.SortOption.DISCRADMAXRARITY;

        Inventories.Sort();

        LastIndex = CurrentIndex;
    }

    public void SortAsGrade(int grade)
    {
        if (grade < 0)
            return;

        Inventories.ResetInventory();

        for (int i = 0; i < Constants.MAXINVENTORY; i++)
        {
            Player.EqData eq = GameManager.Inst().Player.GetItem(i);
            //Inventories.GetSlot(i).Selected.SetActive(false);

            if (eq != null)
            {
                if (eq.Rarity == grade)
                {
                    Sprite icon = eq.Icon;
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);

                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(false);
                    slot.SetGradeSprite(eq.Rarity);

                    //for (int j = 0; j < 3; j++)
                    //{
                    //    if (eq.UID == SelectedUIDs[j])
                    //        slot.Selected.SetActive(true);
                    //}

                    //if (slot.Selected.activeSelf)
                    //    SelectedIndex[0] = i;
                }
                else
                {
                    Sprite icon = eq.Icon;
                    InventorySlot slot = Inventories.GetSlot(i);
                    slot.gameObject.SetActive(true);

                    slot.GetNotExist().SetActive(false);
                    slot.GetExist().SetActive(true);
                    slot.SetIcon(icon);
                    slot.SetDisable(true);
                    slot.SetGradeSprite(eq.Rarity);
                }
                    
            }
        }
        
        GameManager.Inst().Player.SortOption = grade + (int)InventorySlot.SortOption.SAMERARITY;

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

        //UID
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
                if (SelectedUIDs[i] == eq.UID)
                    count++;

            if (count > 0)
            {
                if (count + 1 > eq.Quantity)
                    return;
            }
            else if (count == 0)
                Grade = eq.Rarity;

            SelectedUIDs[CurrentIndex] = eq.UID;
            SelectedIndex[CurrentIndex] = index;
        }

        //ui
        SuccessRate.SetActive(false);
        Buttons[CurrentIndex].transform.GetChild(0).GetComponent<Image>().sprite = eq.Icon;
        Inventories.GetSlot(Inventories.GetSwitchedIndex(index)).Checked.SetActive(true);

        InputTypes[CurrentIndex] = eq.Type;


        //LineColor
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
                if (InputTypes[i] > -1)
                    count++;

            if (count > 1)
            {
                if (InputTypes[0] > -1 && InputTypes[1] > -1)
                {
                    if (InputTypes[0] == InputTypes[1])
                        Lines.transform.GetChild(0).GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[0]]);
                    else
                        Lines.transform.GetChild(0).GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
                }
                if (InputTypes[0] > -1 && InputTypes[2] > -1)
                {
                    if (InputTypes[0] == InputTypes[2])
                        Lines.transform.GetChild(1).GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[0]]);
                    else
                        Lines.transform.GetChild(1).GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
                }
                if (InputTypes[1] > -1 && InputTypes[2] > -1)
                {
                    if (InputTypes[1] == InputTypes[2])
                        Lines.transform.GetChild(2).GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[1]]);
                    else
                        Lines.transform.GetChild(2).GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
                }
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
            SuccessRate.SetActive(true);
            Rate = 100 - eq.Rarity * 10;
            RateText.text = Rate.ToString();

            if (InputTypes[0] == InputTypes[1] && InputTypes[0] == InputTypes[2])
            {
                Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = eq.Icon;
                SynthType = InputTypes[0];
            }
            else
            {
                Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = QuestionSprite;
                SynthType = -1;
            }
        }
    }

    public void Synthesize()
    {
        if (!IsAbleSynthesize)
            return;

        int rarity = GameManager.Inst().Player.GetItem(SelectedIndex[0]).Rarity;
        
        for (int i = 0; i < 3; i++)
        {
            GameManager.Inst().Player.GetItem(SelectedIndex[i]).Quantity--;

            if (GameManager.Inst().Player.GetItem(SelectedIndex[i]).Quantity <= 0)
                GameManager.Inst().Player.RemoveItem(SelectedIndex[i]);
        }

        int rand = Random.Range(0, 100);
        if (rand < Rate)
            rarity++;

        int add = GameManager.Inst().Player.AddItem(GameManager.Inst().MakeEuipData(SynthType, rarity));
        EquipDetail.GetComponent<InventoryDetail>().ShowDetail(add);
        ShowInventory();

        //결과창
        ConfirmWindow.SetActive(false);
        ResetSprites();

        EquipDetail.SetActive(true);
    }

    public void ResetSprites()
    {
        for (int i = 0; i < 3; i++)
        {
            Buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = OriginalSprite;
            Lines.transform.GetChild(i).GetComponent<Image>().material.SetColor("_GlowColor", Color.black);
            if(SelectedIndex[i] > -1)
            {
                //Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex[i])).SetSelected(false);
                Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex[i])).Checked.SetActive(false);
            }
                
            SelectedIndex[i] = -1;
            InputTypes[i] = -1;
        }

        SuccessRate.SetActive(false);
        SelectDetail.SetActive(false);
        EquipDetail.SetActive(false);
        Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = QuestionSprite;

        Inventories.ResetInventory();
        ResetDisable();
    }

    public void ResetDisable()
    {
        for (int i = 0; i < Constants.MAXINVENTORY; i++)
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

        SelectDetail.GetComponent<SelectDetail>().SetDatas(type, value, Colors[InputTypes[CurrentIndex]]);
    }

    public void CloseResult()
    {
        EquipDetail.SetActive(false);
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
}



////기존에 선택된 장비랑 같은 장비일 때
//for (int i = 0; i < 3; i++)
//{
//    if (SelectedIndex[CurrentIndex] > -1)
//    {
//        if (CurrentIndex != i && index == SelectedIndex[i])
//            return;
//    }
//}

//Player.EqData eq = GameManager.Inst().Player.GetItem(index);
//if (eq.Rarity >= 4)
//    return;

//if (eq != null)
//{
//    //첫번째 버튼일 때
//    if (CurrentIndex == 0)
//    {
//        Grade = eq.Rarity;

//        if((SelectedIndex[1] > -1 && eq.Rarity != GameManager.Inst().Player.GetItem(SelectedIndex[1]).Rarity) || 
//            (SelectedIndex[2] > -1 && eq.Rarity != GameManager.Inst().Player.GetItem(SelectedIndex[2]).Rarity))
//        {
//            for(int i = 1; i < 2; i++)
//            {
//                Buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = OriginalSprite;
//                Lines.transform.GetChild(i).GetComponent<Image>().material.SetColor("_GlowColor", Color.black);
//                if (SelectedIndex[i] > -1)
//                    Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex[i])).SetChecked(false);
//                SelectedIndex[i] = -1;
//            }

//            Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = QuestionSprite;
//        }
//    }
//    else
//    {
//        if (SelectedIndex[0] <= -1)
//            return;
//    }

//    //UID
//    SelectedUIDs[CurrentIndex] = eq.UID;

//    //ui
//    SuccessRate.SetActive(false);
//    Buttons[CurrentIndex].transform.GetChild(0).GetComponent<Image>().sprite = eq.Icon;
//    InputTypes[CurrentIndex] = eq.Type;

//    ShowSelectDetail(eq);

//    if(SelectedIndex[CurrentIndex] != -1)
//        Inventories.GetSlot(Inventories.GetSwitchedIndex(SelectedIndex[CurrentIndex])).SetChecked(false);
//    SelectedIndex[CurrentIndex] = index;
//    Inventories.GetSlot(Inventories.GetSwitchedIndex(index)).SetChecked(true);

//    if (InputTypes[0] > -1 && InputTypes[1] > -1)
//    {
//        if (InputTypes[0] == InputTypes[1])
//            Lines.transform.GetChild(0).GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[0]]);
//        else
//            Lines.transform.GetChild(0).GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
//    }
//    if(InputTypes[0] > -1 && InputTypes[2] > -1)
//    {
//        if (InputTypes[0] == InputTypes[2])
//            Lines.transform.GetChild(1).GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[0]]);
//        else
//            Lines.transform.GetChild(1).GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
//    }
//    if (InputTypes[1] > -1 && InputTypes[2] > -1)
//    {
//        if (InputTypes[1] == InputTypes[2])
//            Lines.transform.GetChild(2).GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[1]]);
//        else
//            Lines.transform.GetChild(2).GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
//    }

//    InputTypes[CurrentIndex] = eq.Type;
//    for (int i = 0; i < 3; i++)
//    {
//        if (InputTypes[i] == -1)
//        {
//            IsAbleSynthesize = false;
//            return;
//        }
//    }
//    IsAbleSynthesize = true;
//    SuccessRate.SetActive(true);
//    Rate = 100 - eq.Rarity * 10;
//    RateText.text = Rate.ToString();

//    if (InputTypes[0] == InputTypes[1] && InputTypes[0] == InputTypes[2])
//    {
//        Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = eq.Icon;
//        SynthType = InputTypes[0];
//    }
//    else
//    {
//        Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = QuestionSprite;
//        SynthType = -1;
//    }
//}