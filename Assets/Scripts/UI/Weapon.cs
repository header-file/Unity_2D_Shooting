﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.U2D.Animation;

public class Weapon : MonoBehaviour
{
    public GameObject InventoryArea;
    public GameObject SwitchWindows;
    public GameObject ArrowButtons;
    public InfoArea InfoArea;
    public SpriteResolver[] Skins;
    public SwitchWindow EquipArea;
    public Color GaugeColor;

    InventoryScroll Inventories;
    Player.EqData CurEquip;
    int[] SlotIndices;
    int CurBulletType;
    int ShowBulletType;
    bool IsMoving;
    bool IsFlickering;
    float MoveTimer;
    float TargetX;
    int[] TempCount;
    bool IsDelay;


    public int GetCurBulletType() { return CurBulletType; }
    public Player.EqData GetCurEquip() { return CurEquip; }

    public void SetCurBulletType(int type) { CurBulletType = type; }

    void Start()
    {
        IsMoving = false;
        IsFlickering = false;
        MoveTimer = 0.0f;

        SlotIndices = new int[3];
        SlotIndices[0] = Constants.MAXBULLETS - 1;
        SlotIndices[1] = 0;
        SlotIndices[2] = 1;

        TargetX = 0.0f;

        ShowBulletType = 1;

        TempCount = new int[3];
        for(int i = 0; i < 3; i++)
            TempCount[i] = 0;

        IsDelay = false;

        InfoArea.gameObject.SetActive(true);
        EquipArea.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (IsMoving)
            Moving();

        if (IsFlickering)
            Flickering();
    }

    void Moving()
    {
        MoveTimer += Time.deltaTime * 5.0f;
        Vector3 pos = SwitchWindows.GetComponent<RectTransform>().anchoredPosition;
        pos.x = Mathf.Lerp(pos.x, TargetX, MoveTimer);

        SwitchWindows.GetComponent<RectTransform>().anchoredPosition = pos;

        if (MoveTimer <= 0.5f)
            InfoArea.SetAlpha(1.0f - MoveTimer * 2.0f);
        else if (MoveTimer <= 1.0f)
            InfoArea.SetAlpha(MoveTimer * 2.0f - 1.0f);

        if (MoveTimer - 0.5f <= 0.001f)
            ShowInfoArea();

        if (MoveTimer >= 1.0f)
        {
            IsMoving = false;
            MoveTimer = 0.0f;

            for (int i = 0; i < 2; i++)
                ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = true;

            InfoArea.SetWeaponName(SlotIndices[ShowBulletType]);
            InfoArea.ShowDetail(SlotIndices[ShowBulletType]);

            for (int i = 0; i < 3; i++)
            {
                if (SwitchWindows.transform.GetChild(i).position.x > 6.0f)
                {
                    Vector2 newPos = SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                    newPos.x -= 720.0f * 3;

                    SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] += 4;
                    if (SlotIndices[i] >= Constants.MAXBULLETS)
                        SlotIndices[i] -= Constants.MAXBULLETS;
                    Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
                    InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
                    Show(SlotIndices[i]);
                }

                if (SwitchWindows.transform.GetChild(i).position.x < -6.0f)
                {
                    Vector2 newPos = SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                    newPos.x += 720.0f * 3;

                    SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] -= 4;
                    if (SlotIndices[i] < 0)
                        SlotIndices[i] += Constants.MAXBULLETS;
                    Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
                    InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
                    Show(SlotIndices[i]);
                }
            }

        }
    }

    void Flickering()
    {
        GaugeColor.a -= 0.1f;
        if (GaugeColor.a < 0.0f)
            GaugeColor.a = 1.0f;

        for(int i = 0; i < 3; i++)
        {
            switch(i)
            {
                case 0:
                    if (TempCount[i] + GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk())
                        EquipArea.FlickeringGauge(i, CurBulletType, GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk() - GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk(), GaugeColor);
                    else
                        EquipArea.FlickeringGauge(i, CurBulletType, TempCount[i], GaugeColor);
                    break;
                case 1:
                    if (TempCount[i] + GameManager.Inst().UpgManager.BData[CurBulletType].GetHp() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp())
                        EquipArea.FlickeringGauge(i, CurBulletType, GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp() - GameManager.Inst().UpgManager.BData[CurBulletType].GetHp(), GaugeColor);
                    else
                        EquipArea.FlickeringGauge(i, CurBulletType, TempCount[i], GaugeColor);
                    break;
                case 2:
                    if (TempCount[i] + GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd())
                        EquipArea.FlickeringGauge(i, CurBulletType, GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd() - GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd(), GaugeColor);
                    else
                        EquipArea.FlickeringGauge(i, CurBulletType, TempCount[i], GaugeColor);
                    break;
            }
        }
            
    }

    public void ShowUI()
    {
        SlotIndices[1] = CurBulletType;
        SlotIndices[0] = CurBulletType - 1;
        if (SlotIndices[0] < 0)
            SlotIndices[0] = Constants.MAXBULLETS - 1;
        SlotIndices[2] = CurBulletType + 1;
        if (SlotIndices[2] >= Constants.MAXBULLETS)
            SlotIndices[2] = 0;

        for (int i = 0; i < 3; i++)
        {
            Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
            InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
        }
        Show(SlotIndices[1]);

        ShowInventory();

        ShowBulletType = 1;
        ShowInfoArea();
    }

    void ShowInfoArea()
    {
        InfoArea.SetWeaponName(SlotIndices[ShowBulletType]);
        InfoArea.UpgradeBtn.interactable = GameManager.Inst().UpgManager.BData[SlotIndices[ShowBulletType]].GetActive();
        InfoArea.EquipAreaBtn.interactable = GameManager.Inst().UpgManager.BData[SlotIndices[ShowBulletType]].GetActive();

        for (int i = 0; i < 3; i++)
            InfoArea.PaintGauge(i, CurBulletType);
    }

    public void ShowEquipArea()
    {
        InfoArea.gameObject.SetActive(false);
        EquipArea.gameObject.SetActive(true);
        EquipArea.InfoWindow.SetActive(false);

        for (int i = 0; i < 3; i++)
            Show(CurBulletType);
    }

    void Show(int BulletType)
    {
        for (int i = 0; i < 3; i++)
            PaintGauge(i, BulletType);
    }

    void ShowInventory()
    {
        Inventories = GameManager.Inst().Inventory.GetComponent<InventoryScroll>();
        Inventories.transform.SetParent(InventoryArea.transform, false);
        Inventories.SetSlotType(1);

        Inventories.ResetInventory();
        Inventories.ShowInventory();
    }

    public void Select(int index, int BulletType)
    {
        CurEquip = GameManager.Inst().Player.GetItem(index);

        if (CurEquip.Quantity > 0)
            TempCount[CurEquip.Type] += (int)CurEquip.Value;

        EquipArea.WindowOn(CurEquip, TempCount[CurEquip.Type] / (int)CurEquip.Value);

        IsFlickering = true;
    }

    public void AddQuantity()
    {
        if (IsDelay)
            return;

        IsDelay = true;
        Invoke("ResetDelay", 0.1f);

        if (!IsFlickering)
            IsFlickering = true;

        if(CurEquip.Quantity > TempCount[CurEquip.Type] / (int)CurEquip.Value)
        {
            switch(CurEquip.Type)
            {
                case 0:
                    if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk() < GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk())
                    {
                        TempCount[CurEquip.Type] += (int)CurEquip.Value;
                        if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk())
                            EquipArea.SetInfo(GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxAtk() - GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk(), TempCount[CurEquip.Type] / (int)CurEquip.Value);
                        else
                            EquipArea.SetInfo(TempCount[CurEquip.Type], TempCount[CurEquip.Type] / (int)CurEquip.Value);
                    }
                    break;
                case 1:
                    if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetHp() < GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp())
                    {
                        TempCount[CurEquip.Type] += (int)CurEquip.Value;
                        if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetHp() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp())
                            EquipArea.SetInfo(GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxHp() - GameManager.Inst().UpgManager.BData[CurBulletType].GetHp(), TempCount[CurEquip.Type] / (int)CurEquip.Value);
                        else
                            EquipArea.SetInfo(TempCount[CurEquip.Type], TempCount[CurEquip.Type] / (int)CurEquip.Value);
                    }
                    break;
                case 2:
                    if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd() < GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd())
                    {
                        TempCount[CurEquip.Type] += (int)CurEquip.Value;
                        if (TempCount[CurEquip.Type] + GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd() > GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd())
                            EquipArea.SetInfo(GameManager.Inst().UpgManager.BData[CurBulletType].GetMaxSpd() - GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd(), TempCount[CurEquip.Type] / (int)CurEquip.Value);
                        else
                            EquipArea.SetInfo(TempCount[CurEquip.Type], TempCount[CurEquip.Type] / (int)CurEquip.Value);
                    }
                    break;
            }
        }
    }

    public void SubTQuantity()
    {
        if (IsDelay)
            return;

        IsDelay = true;
        Invoke("ResetDelay", 0.1f);

        if (TempCount[CurEquip.Type] / (int)CurEquip.Value > 1)
        {
            TempCount[CurEquip.Type] -= (int)CurEquip.Value;
            EquipArea.SetInfo(TempCount[CurEquip.Type], TempCount[CurEquip.Type] / (int)CurEquip.Value);
        }
    }

    public void SetStatChange()
    {
        switch(CurEquip.Type)
        {
            case 0:
                GameManager.Inst().UpgManager.BData[CurBulletType].SetAtk(GameManager.Inst().UpgManager.BData[CurBulletType].GetAtk() + TempCount[CurEquip.Type]);
                break;
            case 1:
                GameManager.Inst().UpgManager.BData[CurBulletType].SetHp(GameManager.Inst().UpgManager.BData[CurBulletType].GetHp() + TempCount[CurEquip.Type]);
                break;
            case 2:
                GameManager.Inst().UpgManager.BData[CurBulletType].SetSpd(GameManager.Inst().UpgManager.BData[CurBulletType].GetSpd() + TempCount[CurEquip.Type]);
                break;
        }

        CurEquip.Quantity -= (TempCount[CurEquip.Type] / (int)CurEquip.Value);
        TempCount[CurEquip.Type] = 0;
        IsFlickering = false;

        EquipArea.PaintGauge(CurEquip.Type, CurBulletType, TempCount[CurEquip.Type]);
        ShowInventory();
    }

    void ResetDelay()
    {
        IsDelay = false;
    }

    public void ResetData()
    {
        CurEquip = null;

        for(int i = 0; i < 3; i++)
            TempCount[i] = 0;

        IsFlickering = false;
        IsDelay = false;

        Inventories.ResetInventory();
    }

    void PaintGauge(int type, int bulletType)
    {
        EquipArea.PaintGauge(type, bulletType, TempCount[type]);
    }

    public void Next(bool IsNext)
    {
        IsMoving = true;

        if (IsNext)
        {
            CurBulletType++;
            ShowBulletType++;
            TargetX -= 720.0f;

            if (CurBulletType >= Constants.MAXBULLETS)
                CurBulletType = 0;

            if (ShowBulletType > 2)
                ShowBulletType = 0;
        }
        else
        {
            CurBulletType--;
            ShowBulletType--;
            TargetX += 720.0f;

            if (CurBulletType < 0)
                CurBulletType = Constants.MAXBULLETS - 1;

            if (ShowBulletType < 0)
                ShowBulletType = 2;
        }

        for (int i = 0; i < 2; i++)
            ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = false;
    }
}