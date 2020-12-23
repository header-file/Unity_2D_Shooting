using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Synthesis : MonoBehaviour
{
    public GameObject[] Buttons;
    public GameObject Lines;
    public GameObject Inventories;
    public Material[] Materials;
    public GameObject SuccessRate;
    public Text RateText;
    public GameObject ConfirmWindow;
    public GameObject EquipDetail;
    public GameObject SelectDetail;

    Player Player;
    Sprite OriginalSprite;
    Sprite QuestionSprite;
    Color[] Colors;
    Color TextColor;

    int Grade;
    int CurrentIndex;
    int[] InputTypes;
    int[] SelectedIndex;
    bool IsAbleSynthesize;
    int Rate;
    float Timer;
    int SynthType;

    public int GetGrade() { return Grade; }
    public int GetCurrentIndex() { return CurrentIndex; }

    public void SetCurrentIndex(int index) { CurrentIndex = index; }

    void Start()
    {
        Player = GameManager.Inst().Player;
        OriginalSprite = Buttons[0].transform.GetChild(0).GetComponent<Image>().sprite;
        QuestionSprite = Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite;
        SuccessRate.SetActive(false);
        ConfirmWindow.SetActive(false);
        EquipDetail.SetActive(false);
        SelectDetail.SetActive(false);

        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            GameObject inventorySlot = GameManager.Inst().ObjManager.MakeObj("InventorySlot");
            inventorySlot.transform.SetParent(Inventories.transform, false);
            InventorySlot slot = inventorySlot.GetComponent<InventorySlot>();
            slot.SetIndex(i);
            slot.SetType(2);
        }

        Colors = new Color[3];
        Colors[0] = Color.red;
        Colors[1] = new Color(0.5f, 0.0f, 1.0f);
        Colors[2] = new Color(0.1882353f, 0.8862746f, 0.7372549f);
        TextColor = Color.white;

        SelectedIndex = new int[3];
        for (int i = 0; i < 3; i++)
            SelectedIndex[i] = -1;

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
        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq != null)
            {
                Inventories.transform.GetChild(i).gameObject.SetActive(true);

                Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

                Sprite icon = eq.Icon;
                InventorySlot slot = Inventories.transform.GetChild(i).GetComponent<InventorySlot>();
                slot.SetIcon(icon);

                switch (eq.Type)
                {
                    case 0:
                        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
                        break;
                    case 1:
                        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        break;
                    case 2:
                        slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
                        break;
                }
            }
            else
            {
                Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void SortAsGrade(int grade)
    {
        if (grade < 0)
            return;

        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq != null)
            {
                if (eq.Rarity == grade)
                {
                    Inventories.transform.GetChild(i).gameObject.SetActive(true);

                    Inventories.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                    Inventories.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

                    InventorySlot slot = Inventories.transform.GetChild(i).GetComponent<InventorySlot>();
                    slot.SetIcon(eq.Icon);

                    switch (eq.Type)
                    {
                        case 0:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 60.0f);
                            break;
                        case 1:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                            break;
                        case 2:
                            slot.GetIcon().transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60.0f);
                            break;
                    }
                }
                else
                    Inventories.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
                break;

        }

        //Inventories.transform.GetChild(0).gameObject.SetActive(true);
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
        Player.EqData eq = Player.GetItem(index);
        if (eq != null)
        {
            //첫번째 버튼일 때
            if (CurrentIndex == 0)
            {
                Grade = eq.Rarity;

                if((SelectedIndex[1] > -1 && eq.Rarity != Player.GetItem(SelectedIndex[1]).Rarity) || 
                    (SelectedIndex[2] > -1 && eq.Rarity != Player.GetItem(SelectedIndex[2]).Rarity))
                {
                    for(int i = 1; i < 2; i++)
                    {
                        Buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = OriginalSprite;
                        Lines.transform.GetChild(i).GetComponent<Image>().material.SetColor("_GlowColor", Color.black);
                        if (SelectedIndex[i] > -1)
                            Inventories.transform.GetChild(SelectedIndex[i]).GetComponent<InventorySlot>().Selected.SetActive(false);
                        SelectedIndex[i] = -1;
                    }
                    
                    Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = QuestionSprite;
                }
            }
            else
            {
                if (SelectedIndex[0] == -1)
                    return;
            }

            //ui
            SuccessRate.SetActive(false);
            Buttons[CurrentIndex].transform.GetChild(0).GetComponent<Image>().sprite = eq.Icon;
            InputTypes[CurrentIndex] = eq.Type;

            ShowSelectDetail(eq);

            if(SelectedIndex[CurrentIndex] != -1)
                Inventories.transform.GetChild(SelectedIndex[CurrentIndex]).GetComponent<InventorySlot>().Selected.SetActive(false);
            SelectedIndex[CurrentIndex] = index;
            Inventories.transform.GetChild(index).GetComponent<InventorySlot>().Selected.SetActive(true);

            if(InputTypes[0] > -1 && InputTypes[1] > -1)
            {
                if (InputTypes[0] == InputTypes[1])
                    Lines.transform.GetChild(0).GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[0]]);
                else
                    Lines.transform.GetChild(0).GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
            }
            if(InputTypes[0] > -1 && InputTypes[2] > -1)
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

            InputTypes[CurrentIndex] = eq.Type;
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

        int rarity = Player.GetItem(SelectedIndex[0]).Rarity;
        
        for (int i = 0; i < 3; i++)
            GameManager.Inst().Player.RemoveItem(SelectedIndex[i]);
        GameManager.Inst().Player.DragItem(3);

        int rand = Random.Range(0, 100);
        if (rand >= Rate)
            return;

        rarity++;
        //GameManager.Inst().MakeEquipment(SynthType, rarity, Player.transform);
        int add = Player.AddItem(GameManager.Inst().MakeEuipData(SynthType, rarity));
        ShowInventory();

        //결과창
        ConfirmWindow.SetActive(false);
        ResetSprites();

        EquipDetail.SetActive(true);
        EquipDetail.GetComponent<InventoryDetail>().ShowDetail(add);
    }

    public void ResetSprites()
    {
        for (int i = 0; i < 3; i++)
        {
            Buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = OriginalSprite;
            Lines.transform.GetChild(i).GetComponent<Image>().material.SetColor("_GlowColor", Color.black);
            if(SelectedIndex[i] > -1)
                Inventories.transform.GetChild(SelectedIndex[i]).GetComponent<InventorySlot>().Selected.SetActive(false);
            SelectedIndex[i] = -1;
            InputTypes[i] = -1;
        }

        SuccessRate.SetActive(false);
        SelectDetail.SetActive(false);
        EquipDetail.SetActive(false);
        Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = QuestionSprite;
    }

    void ShowSelectDetail(Player.EqData eqData)
    {
        SelectDetail.SetActive(true);

        int type = eqData.Type;
        int value = (int)eqData.Value;

        SelectDetail.GetComponent<SelectDetail>().SetDatas(type, value);
    }

    public void CloseResult()
    {
        EquipDetail.SetActive(false);
    }

    public void CloseDetail()
    {
        SelectDetail.SetActive(false);
    }
}
