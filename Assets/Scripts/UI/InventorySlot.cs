using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public GameObject EMark;
    public GameObject Selected;
    public GameObject Exist;
    public GameObject NotExist;
    public GameObject Icon;

    int Index = 0;
    int Type = 0;

    public int GetIndex() { return Index; }
    public GameObject GetIcon() { return Icon; }
    public GameObject GetExist() { return Exist; }
    public GameObject GetNotExist() { return NotExist; }

    public void SetIndex(int i) { Index = i; }
    public void SetType(int t) { Type = t; }
    public void SetEmark(bool b) { EMark.SetActive(b); }
    public void SetSelected(bool b) { Selected.SetActive(b); }
    public void SetIcon(Sprite icon) { Icon.GetComponent<Image>().sprite = icon; }

    public void OnClick()
    {
        switch(Type)
        {
            case 0:
                GameManager.Inst().UiManager.OnClickInventoryDetailBtn(Index);
                break;
            case 1:
                GameManager.Inst().UiManager.OnClickEquipSelectBtn(Index);
                break;
            case 2:
                GameManager.Inst().UiManager.OnClickSynthesisSelectBtn(Index);
                break;
        }
    }
}
