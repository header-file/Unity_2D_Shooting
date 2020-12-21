using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Synthesis : MonoBehaviour
{
    public GameObject[] Buttons;
    public GameObject[] Lines;
    public GameObject Inventories;

    Player Player;
    Sprite OriginalSprite;
    Sprite QuestionSprite;
    Color[] Colors;

    int Grade;
    int CurrentIndex;
    int[] InputTypes;
    

    public int GetGrade() { return Grade; }
    public int GetCurrentIndex() { return CurrentIndex; }

    public void SetCurrentIndex(int index) { CurrentIndex = index; }

    void Start()
    {
        Player = GameManager.Inst().Player;
        OriginalSprite = Buttons[0].transform.GetChild(0).GetComponent<Image>().sprite;
        QuestionSprite = Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite;

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

        Grade = -1;
        CurrentIndex = -1;
        InputTypes = new int[3];
        for (int i = 0; i < 3; i++)
            InputTypes[i] = -1;

        gameObject.SetActive(false);
    }

    public void ShowInventory()
    {
        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq.Icon != null)
            {
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
        }
    }

    public void SortAsGrade(int grade)
    {
        if (grade < 0)
            return;

        for (int i = 1; i <= Player.MAXINVENTORY; i++)
        {
            Player.EqData eq = Player.GetItem(i);
            if (eq.Icon != null)
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

        Inventories.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetButtons(int index)
    {
        Player.EqData eq = Player.GetItem(index);
        if (eq.Icon != null)
        {
            //data 처리
            if (CurrentIndex == 0)
                Grade = eq.Rarity;

            //ui
            Buttons[CurrentIndex].transform.GetChild(0).GetComponent<Image>().sprite = eq.Icon;
            InputTypes[CurrentIndex] = eq.Type;

            if(InputTypes[0] > -1 && InputTypes[1] > -1)
            {
                if (InputTypes[0] == InputTypes[1])
                    Lines[0].GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[0]]);
                else
                    Lines[0].GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
            }
            if(InputTypes[0] > -1 && InputTypes[2] > -1)
            {
                if (InputTypes[0] == InputTypes[2])
                    Lines[1].GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[0]]);
                else
                    Lines[1].GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
            }
            if (InputTypes[1] > -1 && InputTypes[2] > -1)
            {
                if (InputTypes[1] == InputTypes[2])
                    Lines[2].GetComponent<Image>().material.SetColor("_GlowColor", Colors[InputTypes[1]]);
                else
                    Lines[2].GetComponent<Image>().material.SetColor("_GlowColor", Color.white);
            }

            InputTypes[CurrentIndex] = eq.Type;
            for (int i = 0; i < 3; i++)
            {
                if (InputTypes[i] == -1)
                    return;
            }

            if (InputTypes[0] == InputTypes[1] && InputTypes[0] == InputTypes[2])
                Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = eq.Icon;
            else
                Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = QuestionSprite;
        }
    }

    public void ResetSprites()
    {
        for (int i = 0; i < 3; i++)
        {
            Buttons[i].transform.GetChild(0).GetComponent<Image>().sprite = OriginalSprite;
            Lines[2].GetComponent<Image>().material.SetColor("_GlowColor", Color.black);
        }
            
        Buttons[3].transform.GetChild(0).GetComponent<Image>().sprite = QuestionSprite;
    }
}
