using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cheat : MonoBehaviour
{
    public Toggle[] Toggles;
    public GameObject[] Pages;

    public Button[] CharReviveBtns;
    public Button[] CharGodModeBtns;
    public Button[] CharCreateBtns;
    public Button[] CharDeleteBtns;
    public Text[] GodModeBtnTexts;


    void Start()
    {
        //Toggles[0].isOn = true;

        gameObject.SetActive(false);
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
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }

    void ShowCharacterPage()
    {
        for(int i = 0; i < Constants.MAXSUBWEAPON; i++)
        {
            if(GameManager.Inst().GetSubweapons(i) == null)
            {
                CharCreateBtns[i].interactable = true;
                CharDeleteBtns[i].interactable = false;
                CharReviveBtns[i + 1].interactable = false;
                CharGodModeBtns[i + 1].interactable = false;
            }
            else
            {
                CharCreateBtns[i].interactable = false;
                CharDeleteBtns[i].interactable = true;
                CharReviveBtns[i + 1].interactable = true;
                CharGodModeBtns[i + 1].interactable = true;
            }
        }
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

        CharCreateBtns[index].interactable = false;
        CharDeleteBtns[index].interactable = true;
        CharReviveBtns[index + 1].interactable = true;
        CharGodModeBtns[index + 1].interactable = true;
    }

    public void OnClickDelete(int index)
    {
        GameManager.Inst().GetSubweapons(index).Disable();

        CharCreateBtns[index].interactable = true;
        CharDeleteBtns[index].interactable = false;
        CharReviveBtns[index + 1].interactable = false;
        CharGodModeBtns[index + 1].interactable = false;
    }
}
