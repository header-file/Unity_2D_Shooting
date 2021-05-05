using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSwitch : MonoBehaviour
{
    public Text BeforeDetail;
    public Text AfterDetail;
    public int EquippedIndex;
    public int SelectedIndex;

    public void Show(int curBulletType, int selected)
    {
        gameObject.SetActive(true);
        EquippedIndex = GameManager.Inst().UpgManager.BData[curBulletType].GetEquipIndex();
        SelectedIndex = selected;

        if (GameManager.Inst().UpgManager.BData[curBulletType].GetEquipIndex() == -1)
            BeforeDetail.text = "";
        else
        {
            Player.EqData befEq = GameManager.Inst().Player.GetItem(GameManager.Inst().UpgManager.BData[curBulletType].GetEquipIndex());

            string befStr = "";
            if (GameManager.Inst().EquipDatas[befEq.Type, befEq.Rarity, 0] > 0)
                befStr = GameManager.Inst().EquipDatas[befEq.Type, befEq.Rarity, 0].ToString();
            befStr += GameManager.Inst().TxtManager.EquipDetailFront[befEq.Type];
            if (befEq.Value > 0)
                befStr += befEq.Value.ToString();
            befStr += GameManager.Inst().TxtManager.EquipDetailBack[befEq.Type];
            BeforeDetail.text = befStr;
        }

        Player.EqData aftEq = GameManager.Inst().Player.GetItem(selected);
        string aftStr = "";
        if (GameManager.Inst().EquipDatas[aftEq.Type, aftEq.Rarity, 0] > 0)
            aftStr = GameManager.Inst().EquipDatas[aftEq.Type, aftEq.Rarity, 0].ToString();
        aftStr += GameManager.Inst().TxtManager.EquipDetailFront[aftEq.Type];
        if (aftEq.Value > 0)
            aftStr += aftEq.Value.ToString();
        aftStr += GameManager.Inst().TxtManager.EquipDetailBack[aftEq.Type];
        AfterDetail.text = aftStr;
    }
}
