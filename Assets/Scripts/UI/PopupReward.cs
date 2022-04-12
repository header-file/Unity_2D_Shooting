using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupReward : MonoBehaviour
{
    public enum RewardType
    {
        COIN = 0,
        CRYSTAL = 1,
        RESOURCE_1 = 2,
        RESOURCE_2 = 3,
        RESOURCE_3 = 4,
        RESOURCE_4 = 5,
        EQUIP_MAGNET = 6,
        EQUIP_HOMING = 7,
        EQUIP_HEAL = 8,
        EQUIP_VAMP = 9,
        EQUIP_SHIELD = 10,
        EQUIP_REVIVE = 11,
        EQUIP_REINFORCE = 12,
        EQUIP_KNOCKBACK = 13,
        EQUIP_SLOW = 14,
    }

    public Jun_TweenRuntime Tween;
    public GameObject Content;
    public Sprite[] SlotImages;

    List<InventorySlot> RewardSlots;


    void Awake()
    {
        RewardSlots = new List<InventorySlot>();
        Hide();
        gameObject.SetActive(false);
    }

    void Hide()
    {
        if (RewardSlots.Count <= 0)
            return;

        for (int i = RewardSlots.Count - 1; i >= 0; i--)
        {
            RewardSlots[i].Grade.SetActive(true);
            RewardSlots[i].Quantity.SetActive(false);
            RewardSlots[i].transform.SetParent(GameManager.Inst().ObjManager.UIPool.transform, false);
            RewardSlots[i].gameObject.SetActive(false);
            RewardSlots.Remove(RewardSlots[i]);
        }
    }

    public void Show(int Type, int Amount, int Grade = -1)
    {
        gameObject.SetActive(true);

        InventorySlot slot = GameManager.Inst().ObjManager.MakeObj("InventorySlot").GetComponent<InventorySlot>();
        slot.transform.SetParent(Content.transform, false);

        slot.SetIcon(SlotImages[Type]);

        slot.QuantityText.text = "";
        if (Amount != 0)
        {
            slot.Quantity.SetActive(true);
            slot.QuantityText.text = Amount.ToString();
        }
        else
            slot.Quantity.SetActive(false);

        if (Grade != -1)
        {
            slot.Grade.SetActive(true);
            slot.SetGradeSprite(Grade);
        }
        else
            slot.Grade.SetActive(false);

        RewardSlots.Add(slot);
    }

    public void OnClickExitBtn()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Tween.Play();
    }

    void OnDisable()
    {
        Hide();
    }
}
