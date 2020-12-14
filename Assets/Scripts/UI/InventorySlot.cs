using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    int Index = 0;
    int Type = 0;

    public int GetIndex() { return Index; }

    public void SetIndex(int i) { Index = i; }
    public void SetType(int t) { Type = t; }

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
        }
        
    }
}
