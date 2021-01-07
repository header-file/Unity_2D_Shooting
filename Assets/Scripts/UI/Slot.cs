using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject Selected;
    public Button DetailBtn;
    public Image Icon;
    public Text Name;
    public Text Level;

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
    }

    public void Show(int index)
    {
        Index = index;

        Selected.SetActive(false);
        DetailBtn.gameObject.SetActive(false);
        Icon.sprite = GameManager.Inst().UiManager.WeaponImages[index];
        Name.text = GameManager.Inst().TxtManager.GetBNames(index);
        Level.text = GameManager.Inst().TxtManager.GetBLevels(index);
    }
}
