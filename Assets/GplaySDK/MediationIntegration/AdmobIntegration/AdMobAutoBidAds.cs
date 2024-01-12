// Filename: AdMobAutoBidAds.cs
// Purpose: Control Auto bid of admob ads. Which will use in last scenario
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 5:59 PM 22/09/2023
// 
// Notes:
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using GoogleMobileAds.Api;
using GplaySDK.BaseLib;
using GplaySDK.BaseLib.RequireProperty;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.MediationIntegration.BaseLib;
using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer

namespace GplaySDK.AdmobIntegration
{
    public sealed class AdMobAutoBidAds : BaseAdsIntegration
    {
        public override AdIntegrationType IntegrationType => AdIntegrationType.Admob;

        private bool _isLoadedBanner = false;

        public override bool IsLoadedBanner => _isLoadedBanner;
        public override bool IsLoadedInterstitial => _interstitialAd != null && _interstitialAd.CanShowAd();
        public override bool IsLoadedRewardedVideo => _rewardedAd != null && _rewardedAd.CanShowAd();
        public override bool IsLoadedOpenAds => _appOpenAd != null && _appOpenAd.CanShowAd();
        public override bool IsLoadedNativeAds => _nativeAd != null;

        private AppOpenAd _appOpenAd = null;

        private BannerView _bannerView = null;

        private InterstitialAd _interstitialAd = null;

        private RewardedAd _rewardedAd = null;

        private NativeAd _nativeAd = null;

        private int _interstitialRetryCount = 0;

        private Action _onInterstitialClose;

        private string _interstitialPlacement;

        private int _rewardedRetryCount = 0;

        private Action _onRewardedClose;

        private Action _onRewardedFail;

        private string _rewardedPlacement;


        protected override void _Initialize(Action onSuccess = null)
        {
            if (onInitializing)
            {
#if LOG_VERBOSE
                "Admob AutoBid already on initializing".Log();
#endif
                return;
            }

            onInitializing = true;
#if LOG_VERBOSE
            "Admob AutoBid initialize".Log();
#endif
            Cmp.ShowConsentForm(consent =>
            {
                GdprValueAttribute.Value = consent;
                MobileAds.Initialize(status =>
                {
#if G_LOG_LOW_LEVEL
                    $"Admob AutoBid initialize status: {status.ToJson(true)}".Log();
#endif
                    onInitializing = false;
                    isInitialized = true;
                    LoadInterstitial();
                    LoadRewardedVideo();
                    _SetupBanner();
                    onSuccess?.Invoke();
                });
            });
        }

        private void _SetupBanner()
        {
            if (_bannerView != null)
            {
                _bannerView.Destroy();
                _bannerView = null;
            }

            _bannerView = new BannerView(AdMobConfig.Instance.Auto_BannerAdsId, AdSize.Banner, AdPosition.Bottom);
            var adRequest = new AdRequest();
            if (AdMobConfig.Instance.useCollapsedBanner) adRequest.Extras.Add("collapsible", "bottom");
            _bannerView.LoadAd(adRequest);
            _bannerView.Hide();

            _bannerView.OnBannerAdLoaded += () =>
            {
#if LOG_VERBOSE
                $"{IntegrationName} banner loaded".Log();
#endif
                _isLoadedBanner = true;
                InvokeBannerUpdated();
            };

            _bannerView.OnBannerAdLoadFailed += err =>
            {
#if LOG_ERROR
                $"{IntegrationName} banner load failed: {err.GetMessage()}".LogError();
#endif
            };

            _bannerView.OnAdClicked += () =>
            {
#if LOG_VERBOSE
                $"{IntegrationName} banner clicked".Log();
#endif
                LogAdView(AdType.Banner, IntegrationName, "Banner");
            };

            _bannerView.OnAdPaid += adValue =>
            {
#if LOG_VERBOSE
                $"{IntegrationName} banner paid: {adValue.Value / 1000000} {adValue.CurrencyCode}".Log();
#endif
                BaseAdsConfig.Internal.InvokeAdPaid(AdType.Banner, IntegrationName, (double) adValue.Value / 1000000,
                    adValue.CurrencyCode);
                LogAdRevenue(AdType.Banner, IntegrationName, (double) adValue.Value / 1000000, adValue.CurrencyCode,
                    AdMobConfig.Instance.useAsSubMediation);
            };


            
        }


        protected override void _ShowBanner()
        {
            _bannerView.Show();
        }

        protected override void _HideBanner()
        {
            _bannerView.Hide();
        }

        /// <summary>
        /// Load interstitial ads with retry mechanism. Retry time will be 2^n with n is the number of retry and max is 64
        /// </summary>
        protected override void _LoadInterstitial()
        {
            onLoadingInterstitial = true;
            CleanUp();
            var adRequest = new AdRequest();
            InterstitialAd.Load(AdMobConfig.Instance.Auto_InterstitialAdsId, adRequest,
                (interstitialAd, error) =>
                {
                    if (error != null)
                    {
#if LOG_ERROR
                        $"{IntegrationName} interstitial load failed: {error.GetMessage()}".LogError();
#endif
                        // Retry
                        onLoadingInterstitial = false;
                        var time = Math.Pow(2, Math.Min(6, _interstitialRetryCount++));
                        CoroutineUtils.StartDelayAction(LoadInterstitial, (float) time);
                        return;
                    }
                    //Success
#if LOG_VERBOSE
                    $"{IntegrationName} interstitial loaded".Log();
#endif
                    //Register event handler
                    interstitialAd.OnAdFullScreenContentFailed += displayError =>
                    {
#if LOG_ERROR
                        $"{IntegrationName} interstitial display failed: {displayError.GetMessage()}".LogError();
#endif
                        _onInterstitialClose?.Invoke();
                        _onInterstitialClose = null;
                        CleanUp();
                        //Use LoadInterstitial instead of _LoadInterstitial to load new interstitial.
                        LoadInterstitial();
                    };

                    interstitialAd.OnAdFullScreenContentOpened += () =>
                    {
#if LOG_VERBOSE
                        $"{IntegrationName} interstitial opened".Log();
#endif
                        LogAdView(AdType.Interstitial, IntegrationName, _interstitialPlacement);
                    };

                    interstitialAd.OnAdPaid += adValue =>
                    {
                        var revenue = (double) adValue.Value / 1000000;
#if LOG_VERBOSE
                        $"{IntegrationName} interstitial paid: {revenue} {adValue.CurrencyCode}".Log();
#endif
                        BaseAdsConfig.Internal.InvokeAdPaid(AdType.Interstitial, IntegrationName, revenue,
                            adValue.CurrencyCode);
                        LogAdRevenue(AdType.Interstitial, IntegrationName, revenue, adValue.CurrencyCode,
                            AdMobConfig.Instance.useAsSubMediation);
                    };

                    interstitialAd.OnAdFullScreenContentClosed += () =>
                    {
#if LOG_VERBOSE
                        $"{IntegrationName} interstitial closed".Log();
#endif
                        _onInterstitialClose?.Invoke();
                        _onInterstitialClose = null;
                        CleanUp();
                        //Use LoadInterstitial instead of _LoadInterstitial to load new interstitial.
                        LoadInterstitial();
                    };

                    //Final step - assign to current interstitial
                    CleanUp();
                    _interstitialAd = interstitialAd;
                    _interstitialRetryCount = 0;
                    onLoadingInterstitial = false;
                });
            return;

            //Clean up current interstitial ads
            void CleanUp()
            {
                if (_interstitialAd is null) return;
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }
        }

        protected override void _ShowInterstitial(Action onClose, string placement)
        {
            _onInterstitialClose = () => ThreadUtils.RunOnMainThread(onClose, true);
            _interstitialPlacement = placement;
            _interstitialAd.Show();
        }

        protected override void _LoadRewardedVideo()
        {
            onLoadingRewardedVideo = true;
            CleanUp();
            var adRequest = new AdRequest();
            RewardedAd.Load(AdMobConfig.Instance.Auto_RewardedVideoAdsId, adRequest,
                (rewardedAd, error) =>
                {
                    if (error != null)
                    {
#if LOG_ERROR
                        $"{IntegrationName} rewarded video load failed: {error.GetMessage()}".LogError();
#endif
                        // Retry
                        onLoadingRewardedVideo = false;
                        var time = Math.Pow(2, Math.Min(6, _rewardedRetryCount++));
                        CoroutineUtils.StartDelayAction(LoadRewardedVideo, (float) time);
                        return;
                    }
                    //Success
#if LOG_VERBOSE
                    $"{IntegrationName} rewarded video loaded".Log();
#endif
//Register event handler
                    rewardedAd.OnAdFullScreenContentFailed += displayError =>
                    {
#if LOG_VERBOSE
                        $"{IntegrationName} rewarded video display failed: {displayError.GetMessage()}".LogError();
#endif
                        _onRewardedClose?.Invoke();
                        _onRewardedFail?.Invoke();
                        _onRewardedClose = null;
                        _onRewardedFail = null;
                        CleanUp();
                        LoadRewardedVideo();
                    };
                    rewardedAd.OnAdFullScreenContentOpened += () =>
                    {
#if LOG_VERBOSE
                        $"{IntegrationName} rewarded video opened".Log();
#endif
                        LogAdView(AdType.Rewarded, IntegrationName, _rewardedPlacement);
                    };
                    rewardedAd.OnAdPaid += adValue =>
                    {
                        var revenue = (double) adValue.Value / 1000000;
#if LOG_VERBOSE
                        $"{IntegrationName} rewarded video paid: {revenue} {adValue.CurrencyCode}".Log();
#endif
                        BaseAdsConfig.Internal.InvokeAdPaid(AdType.Rewarded, IntegrationName, revenue,
                            adValue.CurrencyCode);
                        LogAdRevenue(AdType.Rewarded, IntegrationName, revenue, adValue.CurrencyCode,
                            AdMobConfig.Instance.useAsSubMediation);
                    };
                    rewardedAd.OnAdFullScreenContentClosed += () =>
                    {
#if LOG_VERBOSE
                        $"{IntegrationName} rewarded video closed".Log();
#endif
                        _onRewardedClose?.Invoke();
                        _onRewardedClose = null;
                        _onRewardedFail = null;
                        CleanUp();
                        LoadRewardedVideo();
                    };
                    //Final step - assign to current reward
                    CleanUp();
                    _rewardedAd = rewardedAd;
                    _rewardedRetryCount = 0;
                    onLoadingRewardedVideo = false;
                });


            return;

            void CleanUp()
            {
                if (_rewardedAd is null) return;
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }
        }

        protected override void _ShowRewardedVideo(Action actionReward,
            Action actionOnFail,
            Action actionClose,
            string placement)
        {
            _onRewardedClose = () => ThreadUtils.RunOnMainThread(actionClose, true);
            _onRewardedFail = () => ThreadUtils.RunOnMainThread(actionOnFail, true);
            _rewardedPlacement = placement;
            _rewardedAd.Show(_ => ThreadUtils.RunOnMainThread(actionReward, true));
        }

        /// <summary>
        /// Load open ads without retry mechanism
        /// </summary>
        /// <param name="onSuccess"></param>
        protected override void _LoadOpenAds(Action onSuccess)
        {
            onLoadingOpenAds = true;
            CleanUp();
            var adRequest = new AdRequest();
            AppOpenAd.Load(AdMobConfig.Instance.Auto_OpenAppId, ScreenOrientation.Portrait, adRequest,
                (ad, error) =>
                {
                    if (error != null)
                    {
#if LOG_ERROR
                        Debug.LogError($"{IntegrationName} load open ad failed: " + error.GetMessage());
#endif
                        onLoadingOpenAds = false;
                        return;
                    }

                    ad.OnAdFullScreenContentClosed += () =>
                    {
                        CleanUp();
                        LoadOpenAds(null);
                    };
                    ad.OnAdPaid += value =>
                    {
                        //value is in micros
#if G_LOG_LOW_LEVEL
                        Debug.Log($"Admob auto bid open ad paid: {value.Value / 1000000} {value.CurrencyCode}");
#endif
                        BaseAdsConfig.Internal.InvokeAdPaid(AdType.OpenAds, IntegrationName,
                            (double) value.Value / 1000000,
                            value.CurrencyCode);
                        LogAdRevenue(AdType.OpenAds, IntegrationName, (double) value.Value / 1000000,
                            value.CurrencyCode,
                            AdMobConfig.Instance.useAsSubMediation);
                    };
                    ad.OnAdFullScreenContentFailed += err =>
                    {
#if LOG_ERROR
                        Debug.LogError("Admob auto bid open ad failed: " + err.GetMessage());
#endif
                        CleanUp();
                        LoadOpenAds(null);
                    };
                    _appOpenAd = ad;
                    onLoadingOpenAds = false;
                    onSuccess?.Invoke();
                });
            return;

            void CleanUp()
            {
                if (_appOpenAd is null) return;
                _appOpenAd.Destroy();
                _appOpenAd = null;
            }
        }

        protected override void _ShowOpenAds(string placementName, Action actionClose)
        {
            _appOpenAd.OnAdFullScreenContentClosed += () => { actionClose?.Invoke(); };
            LogAdView(AdType.OpenAds, IntegrationName, placementName);
            _appOpenAd.Show();
        }

        protected override void _LoadNativeAds(Action onSuccess)
        {
            var adLoader = new AdLoader.Builder(AdMobConfig.Instance.Auto_NativeAdsId)
                .ForNativeAd().Build();

            adLoader.OnNativeAdLoaded += (_, args) =>
            {
                _nativeAd = args.nativeAd;
                _nativeAd.OnPaidEvent += (_, paidArgs) =>
                {
                    var value = paidArgs.AdValue;
                    //value is in micros
#if G_LOG_LOW_LEVEL
                    Debug.Log($"{IntegrationName} native ads paid: {value.Value / 1000000} {value.CurrencyCode}");
#endif
                    BaseAdsConfig.Internal.InvokeAdPaid(AdType.NativeAds, IntegrationName,
                        (double) value.Value / 1000000,
                        value.CurrencyCode);
                    LogAdRevenue(AdType.NativeAds, IntegrationName, (double) value.Value / 1000000, value.CurrencyCode,
                        AdMobConfig.Instance.useAsSubMediation);
                };
                onSuccess?.Invoke();
            };

            adLoader.OnAdFailedToLoad += (_, args) =>
            {
#if LOG_ERROR
                $"{IntegrationName} native ad failed: {args.LoadAdError.GetMessage()}".LogError();
#endif
            };

            adLoader.LoadAd(new AdRequest.Builder().Build());
        }

        protected override void _ShowNativeAds(string placementName, Action actionClose)
        {
        }
    }
}