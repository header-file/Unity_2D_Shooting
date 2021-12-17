using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdvertiseManager : MonoBehaviour
{
    public enum AdType
    {
        SHOP_JEWEL = 0,
        FLOATING = 1,
        SHOP_BUFF = 2,
    }

    public int AdLeft;
    public DateTime LastTime;
    public AdType AdvType;

    const string GooglePlay_ID = "4142451";
    const string Rewarded_Android = "Rewarded_Android";
    const string Interstitial_Android = "Interstitial_Android";
    const string Banner_Android = "Banner_Android";


    void Awake()
    {
        AdvertiseManager[] objs = FindObjectsOfType<AdvertiseManager>();
        if (objs.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        AdLeft = Constants.ADMAX;
    }

    void Start()
    {
#if UNITY_EDITOR
        Advertisement.Initialize(GooglePlay_ID, true);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        Advertisement.Initialize(GooglePlay_ID);
#endif
    }

    public void PlayAd()
    {
        if (AdLeft <= 0)
            return;

        if (GameManager.Inst().IsFullPrice)
        {
            Process();
            return;
        }

        ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };

        if (Advertisement.IsReady(Rewarded_Android))
            Advertisement.Show(Rewarded_Android, options);
        else if (Advertisement.IsReady(Interstitial_Android))
            Advertisement.Show(Interstitial_Android, options);
        else if (Advertisement.IsReady(Banner_Android))
            Advertisement.Show(Banner_Android, options);
        else
            return;

    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    // 광고 시청이 완료되었을 때 처리
                    Process();
                    break;
                }
            case ShowResult.Skipped:
                {
                    // 광고가 스킵되었을 때 처리
                    Process();
                    break;
                }
            case ShowResult.Failed:
                {
                    // 광고 시청에 실패했을 때 처리

                    break;
                }
        }
    }

    public void SetLastTime()
    {
        LastTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);
        LastTime = LastTime.AddDays(1);
    }

    void Process()
    {
        switch(AdvType)
        {
            case AdType.SHOP_JEWEL:
                GameManager.Inst().UiManager.MainUI.Center.Shop.MinusAdLeft();
                GameManager.Inst().AddJewel(5);
                break;
            case AdType.FLOATING:
                GameManager.Inst().UiManager.MainUI.Floating.Make();
                break;
            case AdType.SHOP_BUFF:
                GameManager.Inst().UiManager.MainUI.Buff.SubtractAdCount(1);
                GameManager.Inst().BufManager.StartBuff(GameManager.Inst().BufManager.CurrentBuffType);
                break;
        }
    }
}
