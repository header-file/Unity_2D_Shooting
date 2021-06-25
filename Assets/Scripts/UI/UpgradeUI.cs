using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public Button BackBtn;
    public Canvas MainUI;
    public Image Background;
    
    public GameObject WeaponWindow;
    public GameObject SubWindow;
    public GameObject UpgradeWindow;

    public GameObject[] Bullets;

    RectTransform RectT;
    CanvasGroup BulletCanvasG;

    float time;
    float count;
    bool IsAppear;
    bool IsDisappear;
    string UIName;
    int NowBulletType;
    bool IsFadeIn;

    void Start()
    {
        gameObject.SetActive(false);
        Background.transform.localScale = Vector3.zero;
        IsAppear = false;
        IsDisappear = false;
        time = 0.0f;
        count = 1.0f / 20.0f;

        WeaponWindow.SetActive(false);
        SubWindow.SetActive(false);
        UpgradeWindow.SetActive(false);
        IsFadeIn = false;

        for (int i = 1; i < 5; i++)
            Bullets[i].SetActive(false);
    }

    void Update()
    {
        if (IsAppear)
            AppearUI(UIName);

        if (IsDisappear)
            DisAppearUI(UIName);

        if (IsFadeIn)
            FadeInBullets();
    }

    void AppearUI(string UIName)
    {
        switch(UIName)
        {
            case "Upgrade UI":
                Background.transform.localScale = Vector3.one * time;

                time += count;
                if (time >= 1.0f)
                    IsAppear = false;
                break;

            case "WeaponWindow":
            case "SubWindow": 
            case "UpgradeWindow":
                Vector3 scale = new Vector3(time, 1.0f, 1.0f);
                RectT.transform.localScale = scale;

                time += count;
                if(time >= 1.0f)
                {
                    IsAppear = false;
                    RectT = null;
                }
                break;

            default:
                break;
        }
        
    }

    void DisAppearUI(string UIName)
    {
        switch(UIName)
        {
            case "Upgrade UI":
                Background.transform.localScale = Vector3.one * time;

                time -= count;
                if (time <= 0.0f)
                {
                    IsDisappear = false;
                    gameObject.SetActive(false);
                    Time.timeScale = 1.0f;
                    GameManager.Inst().IptManager.SetIsAbleControl(true);
                }
                break;

            case "WeaponWindow":
            case "SubWindow":
            case "UpgradeWindow":
                Vector3 scale = new Vector3(time, 1.0f, 1.0f);
                RectT.transform.localScale = scale;

                time -= count;
                if (time <= 0.0f)
                {
                    IsDisappear = false;
                    RectT.gameObject.SetActive(false);
                }
                break;

            default:
                break;
        }
    }

    void FadeInBullets()
    {
        BulletCanvasG.alpha = time;
        time += count;
        if(time >= 1.0f)
            IsFadeIn = false;
    }

    public void AppearStart()
    {
        gameObject.SetActive(true);
        IsAppear = true;
        UIName = "Upgrade UI";
    }

    public void OnClickBackBtn()
    {
        IsDisappear = true;
        //MainUI.gameObject.GetComponent<MainUI>().UpgradeBtnInteractable();
        time = 1.0f;
        UIName = "Upgrade UI";
    }

    public void OnClickShopType(GameObject obj)
    {
        RectT = obj.gameObject.GetComponent<RectTransform>();
        UIName = obj.gameObject.name;
        obj.SetActive(true);
        IsAppear = true;
        time = 0.0f;
        BackBtn.interactable = false;
        NowBulletType = 0;
        Bullets[NowBulletType].SetActive(true);
    }

    public void OnClickShopTypeBack(GameObject obj)
    {
        RectT = obj.gameObject.GetComponent<RectTransform>();
        UIName = obj.gameObject.name;
        time = 1.0f;
        IsDisappear = true;
        BackBtn.interactable = true;
        Bullets[NowBulletType].SetActive(false);
    }

    public void OnClickNextBullet()
    {
        Bullets[NowBulletType].SetActive(false);

        if (NowBulletType >= 4)
            NowBulletType = 0;
        else
            NowBulletType++;

        IsFadeIn = true;
        time = 0.0f;
        Bullets[NowBulletType].SetActive(true);
        BulletCanvasG = Bullets[NowBulletType].GetComponent<CanvasGroup>();
        BulletCanvasG.alpha = 0.0f;
    }

    public void OnClickPreviousBullet()
    {
        Bullets[NowBulletType].SetActive(false);

        if (NowBulletType <= 0)
            NowBulletType = 4;
        else
            NowBulletType--;

        IsFadeIn = true;
        time = 0.0f;
        Bullets[NowBulletType].SetActive(true);
        BulletCanvasG = Bullets[NowBulletType].GetComponent<CanvasGroup>();
        BulletCanvasG.alpha = 0.0f;
    }
}
