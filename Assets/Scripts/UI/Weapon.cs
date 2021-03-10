using System.Collections;
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
    int[] SlotIndices;
    int CurBulletType;
    int ShowBulletType;
    bool IsMoving;
    bool IsFlickering;
    float MoveTimer;
    float TargetX;
    int[] TempCount;

    public int GetCurBulletType() { return CurBulletType; }

    public void SetCurBulletType(int type) { CurBulletType = type; }

    void Start()
    {
        IsMoving = false;
        IsFlickering = false;
        MoveTimer = 0.0f;

        SlotIndices = new int[3];
        SlotIndices[0] = Bullet.MAXBULLETS - 1;
        SlotIndices[1] = 0;
        SlotIndices[2] = 1;

        TargetX = 0.0f;

        ShowBulletType = 1;

        TempCount = new int[3];
        for(int i = 0; i < 3; i++)
            TempCount[i] = 0;

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
                    if (SlotIndices[i] >= Bullet.MAXBULLETS)
                        SlotIndices[i] -= Bullet.MAXBULLETS;
                    Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
                    InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
                    Show(SlotIndices[i], i);
                }

                if (SwitchWindows.transform.GetChild(i).position.x < -6.0f)
                {
                    Vector2 newPos = SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
                    newPos.x += 720.0f * 3;

                    SwitchWindows.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = newPos;

                    SlotIndices[i] -= 4;
                    if (SlotIndices[i] < 0)
                        SlotIndices[i] += Bullet.MAXBULLETS;
                    Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
                    InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
                    Show(SlotIndices[i], i);
                }
            }

        }
    }

    void Flickering()
    {
        Color col = GaugeColor;
        col.a -= 0.1f;
        if (col.a > 0.0f)
            col.a = 1.0f;

        for(int i = 0; i < 3; i++)
            EquipArea.FlickeringGauge(i, ShowBulletType, TempCount[i], col);
    }

    public void ShowUI()
    {
        SlotIndices[1] = CurBulletType;
        SlotIndices[0] = CurBulletType - 1;
        if (SlotIndices[0] < 0)
            SlotIndices[0] = Bullet.MAXBULLETS - 1;
        SlotIndices[2] = CurBulletType + 1;
        if (SlotIndices[2] >= Bullet.MAXBULLETS)
            SlotIndices[2] = 0;

        for (int i = 0; i < 3; i++)
        {
            Show(SlotIndices[i], i);
            Skins[i].SetCategoryAndLabel("Skin", GameManager.Inst().Player.Types[SlotIndices[i]]);
            InfoArea.Anim[i].SetInteger("Color", InfoArea.DefaultColor[SlotIndices[i]]);
        }

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
            InfoArea.PaintGauge(i, ShowBulletType);
    }

    void Show(int BulletType, int index)
    {
        for (int i = 0; i < 3; i++)
            PaintGauge(i, BulletType);
    }

    void ShowInventory()
    {
        Inventories = GameManager.Inst().Inventory.GetComponent<InventoryScroll>();
        Inventories.transform.SetParent(InventoryArea.transform, false);
        Inventories.SetSlotType(1);

        Inventories.ShowInventory();
    }

    public void Select(int index, int BulletType)
    {
        Player.EqData eq = GameManager.Inst().Player.GetItem(index);

        if (eq.Quantity > 0)
        {
            TempCount[eq.Type] += (int)eq.Value;
        }

        IsFlickering = true;
    }

    void PaintGauge(int type, int bulletType)
    {
        EquipArea.PaintGauge(type, bulletType);
    }

    public void Next(bool IsNext)
    {
        IsMoving = true;

        if (IsNext)
        {
            CurBulletType++;
            ShowBulletType++;
            TargetX -= 720.0f;

            if (CurBulletType >= Bullet.MAXBULLETS)
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
                CurBulletType = Bullet.MAXBULLETS - 1;

            if (ShowBulletType < 0)
                ShowBulletType = 2;
        }

        for (int i = 0; i < 2; i++)
            ArrowButtons.transform.GetChild(i).GetComponent<Button>().interactable = false;
    }
}
