// Filename: MaxAds.cs
// Purpose: Max Integration for GplaySDK.
// 
// Author: Axolotl | lamanh.w@gmail.com
// Created: 4:46 PM 03/10/2023
// 
// Notes: Only Integration level. No business logic here. 
// 
// All rights reserved to Global Play Studio| gplayjsc.com

using System;
using GplaySDK.BaseLib;
using GplaySDK.BaseLib.RequireProperty;
using GplaySDK.Core.BaseLib.Utils;
using GplaySDK.MediationIntegration.BaseLib;
using UnityEngine;
using Debug = UnityEngine.Debug;

// For Lower Unity Version Compatibility
// ReSharper disable UnusedParameter.Local

// ReSharper disable RedundantDefaultMemberInitializer

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

// ReSharper disable AccessToStaticMemberViaDerivedType

namespace GplaySDK.MaxIntegration
{
    public class MaxAds : BaseAdsIntegration
    {
        public override AdIntegrationType IntegrationType => AdIntegrationType.Max;


        private bool _bannerReady = false;

        public override bool IsLoadedBanner => _bannerReady;

        public override bool IsLoadedInterstitial =>
            MaxSdk.IsInterstitialReady(MaxConfig.Instance.InterstitialAdUnitId);

        public override bool IsLoadedRewardedVideo => MaxSdk.IsRewardedAdReady(MaxConfig.Instance.RewardedAdUnitId);
        public override bool IsLoadedOpenAds => false;
        public override bool IsLoadedNativeAds => false;

        protected override void _Initialize(Action onSuccess = null)
        {
            if (onInitializing)
            {
#if LOG_VERBOSE
                "MaxSDK already on initializing".Log();
#endif
                return;
            }

            onInitializing = true;
            
#if G_LOG_LOW_LEVEL
            Debug.Log("Start initialize MaxAds");
            var tracer = new System.Diagnostics.StackTrace();
#endif

            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfig =>
            {
#if LOG_VERBOSE
#if G_LOG_LOW_LEVEL
                Debug.Log($"MaxSdk initialized {sdkConfig.IsSuccessfullyInitialized}\n{tracer}");
#else
                Debug.Log($"MaxSdk initialized {sdkConfig.IsSuccessfullyInitialized}");
#endif
#endif
                if (!sdkConfig.IsSuccessfullyInitialized)
                {
                    MaxSdk.InitializeSdk();
                    onInitializing = false;
                    return;
                }
#if G_DEBUG
                if (MaxConfig.Instance.enableInitDebugMode)
                {
                    MaxSdk.ShowMediationDebugger();
                }
#endif
                Initialize_Banner();
                Initialize_Interstitial();
                Initialize_Reward();
                onInitializing = false;
                isInitialized = true;
                LoadInterstitial();
                LoadRewardedVideo();
                onSuccess?.Invoke();
            };
            MaxSdk.SetSdkKey(MaxConfig.Instance.SDKKey);
            MaxSdk.SetHasUserConsent(GdprValueAttribute.Value);
            MaxSdk.InitializeSdk();
        }
        

        #region Banner

        private void Initialize_Banner()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += (_, __) =>
            {
#if LOG_VERBOSE
                Debug.Log("Banner loaded");
#endif
                _bannerReady = true;
                InvokeBannerUpdated();
            };

            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += (_, err) =>
            {
#if LOG_ERROR
                $"{IntegrationType} Banner failed to load with error: {err.Message}".LogError();
#endif
            };
            MaxSdkCallbacks.Banner.OnAdClickedEvent += (_, __) =>
            {
#if LOG_VERBOSE
                Debug.Log("Banner clicked");
#endif
                LogAdView(AdType.Banner, IntegrationName, "Banner");
            };
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (_, info) =>
            {
#if LOG_VERBOSE
                Debug.Log("Banner revenue paid");
#endif
                BaseAdsConfig.Internal.InvokeAdPaid(AdType.Banner, IntegrationName, info.Revenue, "USD");
                LogAdRevenue(AdType.Banner, IntegrationName, info.Revenue, "USD", false);
            };
            MaxSdk.CreateBanner(MaxConfig.Instance.BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
            MaxSdk.SetBannerBackgroundColor(MaxConfig.Instance.BannerAdUnitId, Color.clear);
            MaxSdk.SetBannerWidth(MaxConfig.Instance.BannerAdUnitId, MaxSdkUtils.IsTablet() ? 728 : 320);
            MaxSdk.LoadBanner(MaxConfig.Instance.BannerAdUnitId);
        }

        protected override void _ShowBanner()
        {
            MaxSdk.ShowBanner(MaxConfig.Instance.BannerAdUnitId);
        }

        protected override void _HideBanner()
        {
            MaxSdk.HideBanner(MaxConfig.Instance.BannerAdUnitId);
        }

        #endregion

        #region Interstitial

        private int _interstitialRetryCount = 0;


        private Action _onInterstitialClose;

        private void Initialize_Interstitial()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (_, adInfo) =>
            {
#if LOG_VERBOSE
                Debug.Log($"{IntegrationType} Interstitial loaded| Value: {adInfo.Revenue}");
#endif
                _interstitialRetryCount = 0;
                onLoadingInterstitial = false;
            };
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (_, __) =>
            {
#if LOG_VERBOSE
                Debug.Log($"{IntegrationType} Interstitial failed to load| Retrying");
#endif
                onLoadingInterstitial = false;
                var time = Math.Pow(2, Math.Min(6, _interstitialRetryCount++));
                CoroutineUtils.StartDelayAction(LoadInterstitial, (float) time);
            };
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (_, error, __) =>
            {
#if LOG_ERROR
                Debug.LogError($"Interstitial failed to display with error: {error.Message}");
#endif
                _onInterstitialClose?.Invoke();
                _onInterstitialClose = null;
                LoadInterstitial();
            };
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += (_, info) =>
            {
                LogAdView(AdType.Interstitial, IntegrationName, info.Placement);
            };
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += (_, info) =>
            {
#if LOG_VERBOSE
                Debug.Log($"Interstitial paid| Value: {info.Revenue}");
#endif
                BaseAdsConfig.Internal.InvokeAdPaid(AdType.Interstitial, IntegrationName, info.Revenue, "USD");
                LogAdRevenue(AdType.Interstitial, IntegrationName, info.Revenue, "USD", false);
            };
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (_, __) =>
            {
                _onInterstitialClose?.Invoke();
                _onInterstitialClose = null;
                LoadInterstitial();
            };
        }

        #endregion

        #region Reward

        private int _rewardRetryCount = 0;

        private Action _onRewardRW;
        private Action _onCloseRW;
        private Action _onFailRW;

        private void Initialize_Reward()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += (_, adInfo) =>
            {
#if LOG_VERBOSE
                Debug.Log($"{IntegrationType} Rewarded video loaded| Value: {adInfo.Revenue}");
#endif
                _rewardRetryCount = 0;
                onLoadingRewardedVideo = false;
            };
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (_, __) =>
            {
                onLoadingRewardedVideo = false;
                var time = Math.Pow(2, Math.Min(6, _rewardRetryCount++));
                CoroutineUtils.StartDelayAction(LoadRewardedVideo, (float) time);
            };

            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (_, error, __) =>
            {
#if LOG_ERROR
                Debug.LogError("Rewarded video failed to display with error: " + error.Message);
#endif
                _onFailRW?.Invoke();
                _onFailRW = null;
                LoadRewardedVideo();
            };
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (_, __, info) =>
            {
#if LOG_VERBOSE
                Debug.Log($"Rewarded video received reward| Placement: {info.Placement}");
#endif
                LogAdView(AdType.Rewarded, IntegrationName, info.Placement);
                _onRewardRW?.Invoke();
                _onRewardRW = null;
            };
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (_, info) =>
            {
#if LOG_VERBOSE
                Debug.Log($"Rewarded video revenue paid| Value: {info.Revenue}");
#endif
                BaseAdsConfig.Internal.InvokeAdPaid(AdType.Rewarded, IntegrationName, info.Revenue, "USD");
                LogAdRevenue(AdType.Rewarded, IntegrationName, info.Revenue, "USD", false);
            };
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (_, __) =>
            {
                _onCloseRW?.Invoke();
                _onCloseRW = null;
                LoadRewardedVideo();
            };
        }

        #endregion


        protected override void _LoadInterstitial()
        {
            onLoadingInterstitial = true;
            MaxSdk.LoadInterstitial(MaxConfig.Instance.InterstitialAdUnitId);
        }

        protected override void _ShowInterstitial(Action onClose, string placement)
        {
            _onInterstitialClose = onClose;
            MaxSdk.ShowInterstitial(MaxConfig.Instance.InterstitialAdUnitId, placement);
        }

        protected override void _LoadRewardedVideo()
        {
            onLoadingRewardedVideo = true;
            MaxSdk.LoadRewardedAd(MaxConfig.Instance.RewardedAdUnitId);
        }

        protected override void _ShowRewardedVideo(Action actionReward, Action actionOnFail,
            Action actionClose,
            string placement)
        {
            _onRewardRW = actionReward;
            _onCloseRW = actionClose;
            _onFailRW = actionOnFail;
            MaxSdk.ShowRewardedAd(MaxConfig.Instance.RewardedAdUnitId, placement);
        }

        protected override void _LoadOpenAds(Action onSuccess)
        {
#if LOG_VERBOSE
            Debug.Log($"{IntegrationType} Open Ads is not supported");
#endif
        }

        protected override void _ShowOpenAds(string placementName, Action actionClose)
        {
#if LOG_VERBOSE
            Debug.Log($"{IntegrationType} Open Ads is not supported");
#endif
        }

        protected override void _LoadNativeAds(Action onSuccess)
        {
#if LOG_VERBOSE
            Debug.Log($"{IntegrationType} Native Ads is not supported");
#endif
        }

        protected override void _ShowNativeAds(string placementName, Action actionClose)
        {
#if LOG_VERBOSE
            Debug.Log($"{IntegrationType} Native Ads is not supported");
#endif
        }
    }
}