using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject Selected;
    public GameObject Locked;
    public Text StageName;
    public Button SelectBtn;
    public Image Icon;
    public Text Name;
    public Text Level;
    public GameObject[] Grades;

    int Index;

    RectTransform Parent;
    RectTransform RectT;
    Vector3 ParentInitPos;

    public int GetIndex() { return Index; }

    public void SetIndex(int i) { Index = i; }

    void Awake()
    {
        RectT = GetComponent<RectTransform>();
        Parent = transform.GetComponentInParent<RectTransform>();
        ParentInitPos = Parent.position;
        Locked.SetActive(true);
    }

    public void Show(int index)
    {
        Index = index;

        Selected.SetActive(false);
        SelectBtn.gameObject.SetActive(false);
        Icon.sprite = GameManager.Inst().UiManager.WeaponImages[index];
        Name.text = GameManager.Inst().TxtManager.BulletTypeNames[index];
        Level.text = "Lv." + GameManager.Inst().UpgManager.BData[index].GetPowerLevel().ToString();

        for (int i = 0; i < Constants.MAXRARITY; i++)
            Grades[i].SetActive(false);
        Grades[GameManager.Inst().UpgManager.BData[index].GetRarity()].SetActive(true);

        if (Locked.gameObject.activeSelf == true &&
            GameManager.Inst().StgManager.UnlockBulletStages[index] < GameManager.Inst().StgManager.Stage)
            Locked.gameObject.SetActive(false);
    }

    public void OnClickSelectBtn()
    {
        GameManager.Inst().UiManager.MainUI.Bottom.OnClickBulletEquipBtn();
        //GameManager.Inst().UiManager.MainUI.Bottom.OnClickSelectBullet(Index);
    }
}
