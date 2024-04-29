using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class AdmobAdManager : MonoBehaviour
{
    public static AdmobAdManager Instance;

    private readonly TimeSpan APPOPEN_TIMEOUT = TimeSpan.FromHours(4);
    private DateTime appOpenExpireTime;
    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private RewardedInterstitialAd rewardedInterstitialAd;
    private float deltaTime;
    //public UnityEvent OnAdLoadedEvent;
    //public UnityEvent OnAdFailedToLoadEvent;
    //public UnityEvent OnAdOpeningEvent;
    //public UnityEvent OnAdFailedToShowEvent;
    //public UnityEvent OnUserEarnedRewardEvent;
    //public UnityEvent OnAdClosedEvent;
    public bool showFpsMeter = true;


    public string bannerId;
    public string interstitialId;
    public string rewardId;
    public string appOpenId;

    public int rewardNum = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        // Add some test device IDs (replace with your own device IDs).
#if UNITY_IPHONE
        deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
        deviceIds.Add("75EF8D155528C04DACBBA6F36F433035");
#endif

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);

        // Listen to application foreground / background events.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

        Load_Banner_Admob();
        Load_Reward_Admob();
        Load_Interstitial_Admob();
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        Debug.Log("Initialization complete.");

        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // the main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            Debug.Log("Initialization complete.");
        });
    }

    private void Update()
    {
        if (showFpsMeter)
        {

            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
        }
        else
        {
        }
    }

    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    #endregion

    #region BANNER ADS

    public void Load_Banner_Admob()
    {
        //MainAdManager.Instance.noAdNoBanner = 1;
        Debug.Log("Requesting Banner ad.");

        // These ad units are configured to always serve test ads.
        //#if UNITY_EDITOR
        //        string adUnitId = "unused";
        //#elif UNITY_ANDROID
        //        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        //#elif UNITY_IPHONE
        //        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        //#else
        //        string adUnitId = "unexpected_platform";
        //#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);

        // Add Event Handlers
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded.");
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log("Banner ad failed to load with error: " + error.GetMessage());
            /*if (MainAdManager.Instance.mainKey == "Admob")
            {
                FacebookAdManager.Instance.Load_Banner_Facebook();
            }*/
        };
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner ad recorded an impression.");
        };
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner ad recorded a click.");
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner ad opening.");
        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner ad closed.");
        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Banner ad received a paid event.",
                                        adValue.CurrencyCode,
                                        adValue.Value);
            Debug.Log(msg);
        };

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void Destory_Banner_Admob()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void Load_Interstitial_Admob()
    {
        //MainAdManager.Instance.noAdNoInterstitial = 1;
        Debug.Log("Requesting Interstitial ad.");

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        // Load an interstitial ad
        InterstitialAd.Load(interstitialId, CreateAdRequest(),
            (InterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Debug.Log("Interstitial ad failed to load with error: " +
                        loadError.GetMessage());
                    //MainAdManager.Instance.ChangeAdIDAdmob_Interstitial();
                    return;
                }
                else if (ad == null)
                {
                    Debug.Log("Interstitial ad failed to load.");
                    //inAdManager.Instance.ChangeAdIDAdmob_Interstitial();
                    return;
                }

                Debug.Log("Interstitial ad loaded.");
                interstitialAd = ad;
                //MainAdManager.Instance.ChangeAdIDAdmob_Interstitial();
                ad.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Interstitial ad opening.");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Interstitial ad closed.");
                    /*if (MainAdManager.Instance.mainKey == "Admob")
                    {
                        Load_Interstitial_Admob();
                    }
                    else
                    {
                        FacebookAdManager.Instance.Load_Interstitial_Facebook();
                    }*/
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Debug.Log("Interstitial ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    Debug.Log("Interstitial ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.Log("Interstitial ad failed to show with error: " +
                                error.GetMessage());
                    //MainAdManager.Instance.ChangeAdIDAdmob_Interstitial();
                    /*if (MainAdManager.Instance.mainKey == "Admob")
                    {
                        FacebookAdManager.Instance.Load_Interstitial_Facebook();
                    }*/
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}",
                                               "Interstitial ad received a paid event.",
                                               adValue.CurrencyCode,
                                               adValue.Value);
                    Debug.Log(msg);
                };
            });
    }

    public void Show_Interstitial_Admob()
    {
        if (interstitialAd != null)
        {
            if (interstitialAd.CanShowAd())
            {
                interstitialAd.Show();
                interstitialAd.OnAdFullScreenContentClosed += () => { Load_Interstitial_Admob(); };
            }
        }
        else
        {
            Debug.Log("Interstitial ad is not ready yet.");
            Load_Interstitial_Admob();
        }
    }

    public void Destory_Interstitial_Admob()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void Load_Reward_Admob()
    {
        //MainAdManager.Instance.noAdNoReward = 1;
        Debug.Log("Requesting Rewarded ad.");
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        string adUnitId = "unexpected_platform";
#endif

        // create new rewarded ad instance
        RewardedAd.Load(rewardId, CreateAdRequest(),
            (RewardedAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Debug.Log("Rewarded ad failed to load with error: " +
                                loadError.GetMessage());
                    return;
                }
                else if (ad == null)
                {
                    Debug.Log("Rewarded ad failed to load.");
                    return;
                }

                Debug.Log("Rewarded ad loaded.");
                rewardedAd = ad;

                ad.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Rewarded ad opening.");

                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Rewarded ad closed.");
                    /*if (MainAdManager.Instance.mainKey == "Admob")
                    {
                        Load_Reward_Admob();
                    }
                    else
                    {
                        FacebookAdManager.Instance.Load_Reward_Facebook();
                    }*/
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Debug.Log("Rewarded ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    Debug.Log("Rewarded ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.Log("Rewarded ad failed to show with error: " +
                               error.GetMessage());
                    /*if (MainAdManager.Instance.mainKey == "Admob")
                    {
                        FacebookAdManager.Instance.Load_Reward_Facebook();
                    }*/
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}",
                                               "Rewarded ad received a paid event.",
                                               adValue.CurrencyCode,
                                               adValue.Value);
                    Debug.Log(msg);
                };
            });
    }

    public void Show_Reward_Admob()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show((Reward reward) =>
            {
                //MainAdManager.Instance.UserReward();
                GiveReward();
                Debug.Log("Rewarded ad granted a reward: " + reward.Amount);
                Load_Reward_Admob();
            });
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
            Load_Reward_Admob();
        }
    }

    public void RequestAndLoadRewardedInterstitialAd()
    {
        Debug.Log("Requesting Rewarded Interstitial ad.");

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a rewarded interstitial.
        RewardedInterstitialAd.Load(adUnitId, CreateAdRequest(),
            (RewardedInterstitialAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Debug.Log("Rewarded interstitial ad failed to load with error: " +
                                loadError.GetMessage());
                    return;
                }
                else if (ad == null)
                {
                    Debug.Log("Rewarded interstitial ad failed to load.");
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded.");
                rewardedInterstitialAd = ad;

                ad.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Rewarded interstitial ad opening.");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Rewarded interstitial ad closed.");
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Debug.Log("Rewarded interstitial ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    Debug.Log("Rewarded interstitial ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.Log("Rewarded interstitial ad failed to show with error: " +
                                error.GetMessage());
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}",
                                                "Rewarded interstitial ad received a paid event.",
                                                adValue.CurrencyCode,
                                                adValue.Value);
                    Debug.Log(msg);
                };
            });
    }

    public void ShowRewardedInterstitialAd()
    {
        if (rewardedInterstitialAd != null)
        {
            rewardedInterstitialAd.Show((Reward reward) =>
            {
                Debug.Log("Rewarded interstitial granded a reward: " + reward.Amount);
            });
        }
        else
        {
            Debug.Log("Rewarded interstitial ad is not ready yet.");
        }
    }

    #endregion

    #region APPOPEN ADS

    public bool IsAppOpenAdAvailable
    {
        get
        {
            return (appOpenAd != null
                    && appOpenAd.CanShowAd()
                    && DateTime.Now < appOpenExpireTime);
        }
    }

    public void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        UnityEngine.Debug.Log("App State is " + state);

        // OnAppStateChanged is not guaranteed to execute on the Unity UI thread.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (state == AppState.Foreground)
            {
                ShowAppOpenAd();
            }
        });
    }

    public void RequestAndLoadAppOpenAd()
    {
        Debug.Log("Requesting App Open ad.");
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/3419835294";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/5662855259";
#else
        string adUnitId = "unexpected_platform";
#endif

        // destroy old instance.
        if (appOpenAd != null)
        {
            DestroyAppOpenAd();
        }

        // Create a new app open ad instance.
        AppOpenAd.Load(appOpenId, ScreenOrientation.Portrait, CreateAdRequest(),
            (AppOpenAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    Debug.Log("App open ad failed to load with error: " +
                        loadError.GetMessage());
                    //MainAdManager.Instance.ChangeAdIDAdmob_AppOpen();
                    return;
                }
                else if (ad == null)
                {
                    Debug.Log("App open ad failed to load.");

                    //MainAdManager.Instance.ChangeAdIDAdmob_AppOpen();
                    return;
                }


                //MainAdManager.Instance.ChangeAdIDAdmob_AppOpen();
                Debug.Log("App Open ad loaded. Please background the app and return.");
                this.appOpenAd = ad;
                this.appOpenExpireTime = DateTime.Now + APPOPEN_TIMEOUT;

                ad.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("App open ad opened.");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("App open ad closed.");
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    Debug.Log("App open ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    Debug.Log("App open ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    Debug.Log("App open ad failed to show with error: " +
                        error.GetMessage());
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    string msg = string.Format("{0} (currency: {1}, value: {2}",
                                               "App open ad received a paid event.",
                                               adValue.CurrencyCode,
                                               adValue.Value);
                    Debug.Log(msg);
                };
            });
    }

    public void DestroyAppOpenAd()
    {
        if (this.appOpenAd != null)
        {
            this.appOpenAd.Destroy();
            this.appOpenAd = null;
        }
    }

    public void ShowAppOpenAd()
    {
        if (!IsAppOpenAdAvailable)
        {
            return;
        }
        appOpenAd.Show();
    }

    #endregion


    #region AD INSPECTOR

    public void OpenAdInspector()
    {
        Debug.Log("Opening Ad inspector.");

        MobileAds.OpenAdInspector((error) =>
        {
            if (error != null)
            {
                Debug.Log("Ad inspector failed to open with error: " + error);
            }
            else
            {
                Debug.Log("Ad inspector opened successfully.");
            }
        });
    }

    #endregion
    public bool Reward_Check_Admob()
    {
        if (rewardedAd != null)
        {
            return rewardedAd.CanShowAd();
        }
        else
        {
            return false;
        }
    }

    public void GiveReward()
    {
        switch (rewardNum)
        {
            case 1:
                EndingPanelUI.Instance.ConvertMoney3xAndDestroyPanel();
                break;

            case 2:
                InventoryUI.Instance.UnlockStanGunReward();
                break;

            case 3:
                OnScreenPanelUI.Instance.AddRandomNumberOfBullet(5 , 10);
                break;
                case 4:
                OnScreenPanelUI.Instance.AddMoneyByRewardedAd();
                break;
        }
    }
}