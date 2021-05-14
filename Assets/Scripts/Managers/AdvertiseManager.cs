using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertiseManager : MonoBehaviour
{
    const string GooglePlay_ID = "4128875";
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
        if (!Advertisement.IsReady(Rewarded_Android))
            return;

        Advertisement.Show(Rewarded_Android);
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    Debug.Log("The ad was successfully shown.");

                    // 광고 시청이 완료되었을 때 처리

                    break;
                }
            case ShowResult.Skipped:
                {
                    Debug.Log("The ad was skipped before reaching the end.");

                    // 광고가 스킵되었을 때 처리

                    break;
                }
            case ShowResult.Failed:
                {
                    Debug.LogError("The ad failed to be shown.");

                    // 광고 시청에 실패했을 때 처리

                    break;
                }
        }
    }
}
