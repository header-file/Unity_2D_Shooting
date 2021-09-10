using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class AdvertiseManager : MonoBehaviour
{
    public int AdLeft;
    public DateTime LastTime;

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
                    //Debug.Log("The ad was successfully shown.");

                    // 광고 시청이 완료되었을 때 처리
                    GameManager.Inst().UiManager.MainUI.Center.Shop.MinusAdLeft();
                    GameManager.Inst().AddJewel(5);

                    break;
                }
            case ShowResult.Skipped:
                {
                    //Debug.Log("The ad was skipped before reaching the end.");

                    // 광고가 스킵되었을 때 처리
                    GameManager.Inst().UiManager.MainUI.Center.Shop.MinusAdLeft();
                    GameManager.Inst().AddJewel(5);

                    break;
                }
            case ShowResult.Failed:
                {
                    //Debug.LogError("The ad failed to be shown.");

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
}
