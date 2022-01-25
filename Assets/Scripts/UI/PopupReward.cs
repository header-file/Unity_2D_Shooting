using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupReward : MonoBehaviour
{
    public enum RewardType
    {
        COIN = 0,
        CRYSTAL = 1,
        EQUIPMENT = 2,
        RESOURCE_1 = 3,
        RESOURCE_2 = 4,
        RESOURCE_3 = 5,
        RESOURCE_4 = 6,
    }

    public InventorySlot[] RewardSlots;
    public Jun_TweenRuntime Tween;

    void Start()
    {
        for (int i = 0; i < RewardSlots.Length; i++)
        {
            RewardSlots[i].gameObject.SetActive(false);
            RewardSlots[i].SetType(-1);
        }

        gameObject.SetActive(false);
    }

    void Hide()
    {
        for (int i = 0; i < RewardSlots.Length; i++)
            RewardSlots[i].gameObject.SetActive(false);
    }

    public void Show(int Type, int Amount, int Grade = -1)
    {
        RewardSlots[Type].gameObject.SetActive(true);
        RewardSlots[Type].QuantityText.text = Amount.ToString();

        if (Grade != -1)
            RewardSlots[Type].SetGradeSprite(Grade);
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
