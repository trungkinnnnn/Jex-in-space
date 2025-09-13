using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static Action OnHandleX2Coin;

    public static AdsManager Instance;

    private RewardedAd _rewardedAd;
    private const string AD_UNIT_ID_REWARDED = "ca-app-pub-3940256099942544/5224354917";
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        MobileAds.Initialize(iniStatus => { });
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg = "Rewarded ad rewarded the user, Type: {0}, amount: {1}";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            AudioBGMManager.Instance.SetActive(false);
            _rewardedAd.Show((Reward reward) =>
            {
                OnHandleX2Coin?.Invoke();
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }    

    public void LoadRewardedAd()
    {
        Debug.Log("Loading Ads");
        var adRequest = new AdRequest();

        RewardedAd.Load(AD_UNIT_ID_REWARDED, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if(error != null || ad == null)
                {
                    Debug.Log("Rewarded failed, Error : " + error);
                    return;
                }    

                Debug.Log("Ads with Response : " + ad.GetResponseInfo());   
                _rewardedAd = ad;
                RegisterReloadHandle(_rewardedAd);
            });
    }    

    private void RegisterReloadHandle(RewardedAd rewardedAd)
    {
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {

            Time.timeScale = 0f;
            AudioBGMManager.Instance.SetActive(true);

            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
        };

        rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Time.timeScale = 0f;
            AudioBGMManager.Instance.SetActive(true);

            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
        };
    }    

}
