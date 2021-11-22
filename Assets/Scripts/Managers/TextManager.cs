using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public enum DamageType
    {
        BYPLYAER = 0,
        BYENEMY = 1,
        PLAYERHEAL = 2,
        REINFORCED = 3,
    }

    public string[] EquipName;
    public string[] EquipDetailFront;
    public string[] EquipDetailBack;
    public string[] EquipDetailSimple;
    public string[] BulletTypeNames;
    public string[] RarityNames;

    string[] BPrices;

    string[] SubNames;

    public string GetSubNames(int index) { return SubNames[index]; }

    public void SetBPrices(int index, int price) { BPrices[index] = price.ToString(); }
    public void SetSPrice(int price) { GameManager.Inst().UiManager.MainUI.Bottom.SWPrice.text = price.ToString(); }
    public void SetSName(int index) { GameManager.Inst().UiManager.MainUI.Bottom.SWName.text = SubNames[index]; }
    

    void Awake()
    {
        GameManager.Inst().TxtManager = gameObject.GetComponent<TextManager>();

        BPrices = new string[Constants.MAXBULLETS];

        SubNames = new string[4];
        for(int i = 0; i < Constants.MAXSUBWEAPON; i++)
           SubNames[i] = "Turret - 0" + (i + 1).ToString();

        for (int i = 0; i < Constants.MAXBULLETS; i++)
            BPrices[i] = "0";
    }

    void FixedUpdate()
    {
        if (!GameManager.Inst().StgManager.IsBoss)
            return;

        float time = GameManager.Inst().StgManager.BossTimer;
        time = (float)System.Math.Truncate((double)time * 100) / 100;
        GameManager.Inst().UiManager.MainUI.BossTimer.text = time.ToString();

        GameManager.Inst().StgManager.BossTimer -= Time.deltaTime;

        if(GameManager.Inst().StgManager.BossTimer <= 0)
        {
            GameManager.Inst().StgManager.Boss.Die();

            GameManager.Inst().StgManager.IsBoss = false;
            GameManager.Inst().StgManager.RestartStage();
        }
    }

    public void ShowDmgText(Vector3 pos, float dmg, int type, bool isReinforced)
    {
        GameObject text = GameManager.Inst().ObjManager.MakeObj("DamageText");
        text.transform.position = pos;
        text.SetActive(true);
        DamageText dmgText = text.GetComponent<DamageText>();
        if (type == (int)DamageType.PLAYERHEAL)
            dmgText.SetPlusText(dmg);
        else
            dmgText.SetText(dmg);
        dmgText.SetSize(dmgText.DefaultSize);
        dmgText.SetColor(type);

        if (isReinforced)
        {
            //text.transform.localScale = Vector3.one * 2.0f;
            dmgText.SetSize(48);
            dmgText.SetColor((int)DamageType.REINFORCED);
        }

        ActivationTimer timer = text.GetComponent<ActivationTimer>();
        timer.IsStart = true;
    }
}
